using System;

namespace COMP1003_Mastermind
{
    public class Mastermind
    {
        
        private bool active = true;

        public enum GameStatus { NULL, STARTED, POSITIONSSELECTED, PLAYING, QUIT, END }; //stores state of game
        private GameStatus action = GameStatus.NULL; // initially sets status to NULL

        private bool isPlaying = false;

        // string[] colours = { "", "Blue", "Yellow", "Red", "Orange", "Green", "Pink", "Purple", "Brown", "White" }; // used to tell user colours - unnescessary as game solely uses numbers

        int N = 0; // stores code length
        int M = 0; // stores code range (colours)

        int[,] queue = new int[150,150]; // initialises queue 2d array

        int guesses = 0; // stores current guess number

        bool correctrange = false; // used for error handling
        bool checkint = false; // used for error handling

        private int[] currentCode; // stores secret code


        // RandomNumberGenerator generates a secret code based on N and M parameters
        private int[] RandomNumberGenerator(int Size, int maxRange)
        {
            int eachNumber;
            int[] randomNumber = new int[Size];
            Random rnd = new Random();
            for (int i = 0; i < Size; i++)
            {
                eachNumber = rnd.Next(1, maxRange);
                randomNumber[i] = eachNumber;

                // Console.Write(eachNumber); //used for testing
            }
            Console.WriteLine();
            return randomNumber;
        }

        // GameLoop is the core of the game, it determines what happens 
        // next in the game depending on what the current GameStatus is
        public void GameLoop(bool active)
        {

            if (isPlaying == true)
            {
                if (action == GameStatus.STARTED)
                {
                    while (checkint == false)
                    {
                        Console.WriteLine("How many positions would you like to play with?"); 
                        string positions = Console.ReadLine();
                        int.TryParse(positions, out N);
                        if (N >= 1)
                        {
                            action = GameStatus.POSITIONSSELECTED;
                            checkint = true;
                        }
                        else // error handling
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Please enter a number equal to or greater than 1");
                            Console.ResetColor();
                        }
                    }
                }
                if (action == GameStatus.POSITIONSSELECTED)
                {
                    while (correctrange == false)
                    {
                        Console.WriteLine("How many colours would you like to play with? (1-9)");
                        string colours = Console.ReadLine();
                        int.TryParse(colours, out M);
                        if (M >= 1 && M <= 9)
                        {
                            currentCode = RandomNumberGenerator(N, M + 1);
                            action = GameStatus.PLAYING;
                            correctrange = true;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.WriteLine(Environment.NewLine + "You have chosen to play with " + N + " positions.");
                            Console.ResetColor();
                        }
                        else // error handling
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Please enter a number in the range 1-9");
                            Console.ResetColor();
                        }
                    }
                }
                while (action == GameStatus.PLAYING)
                {
                    Console.WriteLine("Enter your guess");
                    int[] userCode = ProcessGuess(N);
                    int hits = HitCalculator(currentCode, userCode);
                    if (hits == N)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("You have won!");
                        Console.WriteLine("The code was:");
                        for (int j = 0; j < N; j++)
                        {
                            Console.Write(currentCode[j] + " ");
                        }
                        //Console.Write("- ");
                        //for (int j = 0; j < N; j++)
                        //{
                        //    Console.Write(colours[currentCode[j]] + " ");  // can be used to associate colours to numbers
                        //}
                        Console.ResetColor();
                        action = GameStatus.END;
                    }
                }
                if (action == GameStatus.END) // replay feature
                {
                    guesses = 0;
                    correctrange = false;
                    checkint = false;
                    Console.WriteLine(Environment.NewLine + "Would you like to play again? (yes/no)");
                }
            }
        }
        
        // ProcessInput is used to determine the action of the game
        // based on the users input
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

        // ProcessGuess is used to ask the users' input, it also prints guess history
        private int[] ProcessGuess(int userSize) // asks for users guess'
        {
            int number = 0;
            int[] userGuess = new int[userSize];
            for (int i = 0; i < N; i++) // reads users guess'
            {
                try
                {
                    Console.Write("Digit {0}: ", (i + 1));
                    ConsoleKeyInfo info = Console.ReadKey();
                    char inputtedKey = info.KeyChar;
                    number = int.Parse(inputtedKey.ToString());
                    userGuess[i] = number;
                    queue[guesses, i] = number;
                    Console.WriteLine();
                }
                catch (System.FormatException) // if wrong format detected, throws error message
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(Environment.NewLine + "Please enter a number in the range of 1 - {0}!", M);
                    i--;
                    Console.ResetColor();
                }
            }
            Console.Clear();
            Console.WriteLine("******************************************************"); // prints guess history
            Console.Write("Guess History: ");
            Console.WriteLine();
            guesses++; // increment current guess number
            for (int c = 0; c < guesses; c++)
            {
                for (int i = 0; i < N; i++)
                {
                    Console.Write(queue[c,i] + " ");
                }
                //Console.Write("- ");
                //for (int i = 0; i < N; i++)
                //{
                //    Console.Write(colours[queue[c,i]] + " ");  // can be used to associate numbers with colours
                //}
                Console.WriteLine();
            }
            Console.WriteLine("******************************************************");
            return userGuess;
        }
        
        // HitCalculator is used to calculate hits based on the users previous guess
        private int HitCalculator(int[] currentCode, int[] userCode) // calculates hits / misses based on users guess and the generated code
        {
            int[] cloneCurrentCode = new int[N];
            int[] cloneUserCode = new int[N];

            int blackPeg = 0;
            int miss;
            int totalhits;
            int whitePeg = 0;

            Array.Copy(currentCode, cloneCurrentCode, N); // copy secret code to clone array
            Array.Copy(userCode, cloneUserCode, N); // copy user guess to clone array

            for (int a = 0; a < N; a++)
            {
                for (int b = 0; b < N; b++)
                {
                    if (cloneUserCode[a] == cloneCurrentCode[b]) // increments partial hits when conditions met (white pegs)
                    {
                        if (cloneUserCode[a] != 0)
                        {
                            whitePeg++;
                            cloneUserCode[a] = 0;
                            cloneCurrentCode[b] = 0;
                        }
                    }
                }
            }
            for (int i = 0; i < N; i++)
            {
                if (userCode[i] == currentCode[i]) //increments exact hits when conditions met (black pegs)
                {
                    blackPeg++;
                    whitePeg--;
                }
            }
            totalhits = blackPeg + whitePeg;
            miss = N - totalhits;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Results: {0} Black Peg(s), {1} White Peg(s), {2} Miss(es)", blackPeg, whitePeg, miss); // prints results
            Console.ResetColor();
            return blackPeg;
        }
        
        // main is used to call other methods when initially starting the game
        static void Main(string[] args)
        {
            Mastermind mind = new Mastermind();
            string input = string.Empty;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("*******************************************" + Environment.NewLine + "* Welcome to the command line Mastermind! *" + Environment.NewLine +
                "*******************************************" + Environment.NewLine);
            Console.ResetColor();
            Console.WriteLine("Would you like to start a new game? (yes/no)");

            // loops through user input and determines when to quit game

            while (mind.active && mind.action != GameStatus.QUIT)
            {
                if (mind.action != GameStatus.PLAYING) // asks user to enter command if gamestatus isn't PLAYING
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

