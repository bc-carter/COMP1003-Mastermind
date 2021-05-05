using System;

namespace COMP1003_Mastermind
{
    public class Mastermind
    {
        
        private bool active = true;

        public enum GameStatus { NULL, STARTED, POSITIONSSELECTED, PLAYING, QUIT }; //stores state of game
        private GameStatus action = GameStatus.NULL;

        private bool isPlaying = false;

        string[] colours = { "Blue", "Yellow", "Red", "Orange", "Green", "Pink", "Purple", "Brown", "White" };

        int N = 0;
        int M = 0;


        bool correctrange = false;

        private int[] currentCode;


        private int[] GenerateRandomNumbers(int Size, int maxRange)
        {
            int eachNumber;
            int[] randomNumber = new int[Size];
            Random rnd = new Random();

            for (int i = 0; i < Size; i++)
            {
                eachNumber = rnd.Next(1, maxRange);
                randomNumber[i] = eachNumber;

                //Console.Write(eachNumber); //used for testing
            }
            Console.WriteLine();
            return randomNumber;
        }

        public void GameLoop(bool active)
        {

            if (isPlaying == true)
            {
                if (action == GameStatus.STARTED)
                {
                    Console.WriteLine("How many positions would you like to play using?");
                    string positions = Console.ReadLine();
                    int.TryParse(positions, out N);
                    action = GameStatus.POSITIONSSELECTED;
                }
                if (action == GameStatus.POSITIONSSELECTED)
                {
                    while (correctrange == false)
                    {
                        Console.WriteLine("How many colours would you like to play using? (1-9)");
                        string colours = Console.ReadLine();
                        int.TryParse(colours, out M);
                        if (M >= 1 && M <= 9)
                        {
                            currentCode = GenerateRandomNumbers(N, M + 1);
                            action = GameStatus.PLAYING;
                            correctrange = true;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.WriteLine(Environment.NewLine + "You have chosen to play with " + N + " pegs.");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine("Please enter a number in the range 1-9");
                        }
                    }
                }
                while (action == GameStatus.PLAYING)
                {
                    Console.WriteLine("Enter your guess");
                    int[] userArray = GetUserGuess(N);
                    int hits = CountHits(currentCode, userArray);
                    if (hits == N)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("You have won!");
                        Console.WriteLine("The code was:");
                        for (int j = 0; j < N; j++)
                        {
                            Console.Write(currentCode[j] + " ");
                        }
                        action = GameStatus.QUIT;
                    }
                }

            }
        }

        public void ProcessInput(string input)
        {
            input = input.ToLower();
            switch (input)
            {
                case "yes":
                    action = GameStatus.STARTED;
                    isPlaying = true;
                    break;
                case "no":
                    action = GameStatus.QUIT;
                    break;
                case "quit":
                    action = GameStatus.QUIT;
                    break;
            }
        }

        public static int[] GetUserGuess(int userSize)
        {
            int number = 0;
            int[] userGuess = new int[userSize];
            for (int i = 0; i < userGuess.Length; i++)
            {
                Console.Write("Digit {0}: ", (i + 1));
                ConsoleKeyInfo info = Console.ReadKey();
                char inputtedKey = info.KeyChar;
                number = int.Parse(inputtedKey.ToString());
                userGuess[i] = number;
                Console.WriteLine();
                //Console.Write(number);
            }
            Console.WriteLine();
            Console.Clear();
            Console.Write("Your guess: ");
            for (int i = 0; i < userGuess.Length; i++)
            {
                Console.Write(userGuess[i] + " ");
            }
            Console.WriteLine();
           
            return userGuess;
        }
        public static int CountHits(int[] currentCode, int[] userArray)
        {
            int hit = 0;
            int miss = 0;
            int hits = 0;

            for (int i = 0; i < currentCode.Length; i++)
            {
                if (currentCode[i] == userArray[i])
                {
                    hit ++;
                    hits = hit;
                }
                else
                {
                    miss ++;
                }
            }
            Console.WriteLine("Results: {0} Hit(s), {1} Miss(es)", hit, miss);
            return hits;
        }
        static void Main(string[] args)
        {
            Mastermind mind = new Mastermind();
            string input = string.Empty;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("*******************************************" + Environment.NewLine + "* Welcome to the command line Mastermind! *" + Environment.NewLine +
                "*******************************************" + Environment.NewLine);
            Console.ResetColor();
            Console.WriteLine("Would you like to start a new game? (yes/no)");

            // Loops through the input and determines when the game should quit

            while (mind.active && mind.action != GameStatus.QUIT)
            {
                if (mind.action != GameStatus.PLAYING)
                {
                    Console.WriteLine("Your Command: ");
                }

                input = Console.ReadLine();
                Console.WriteLine(Environment.NewLine);
                mind.ProcessInput(input);
                mind.GameLoop(mind.active);

            }
        }
    }
}

