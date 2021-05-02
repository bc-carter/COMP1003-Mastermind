using System;

namespace COMP1003_Mastermind
{
    public class Mastermind
    {
        static void Main(string[] args)
        {
            Mastermind mind = new Mastermind();
            string input = string.Empty;
            Console.WriteLine("Welcome to the command line Mastermind!" + Environment.NewLine +
                "May your Quest be filled with riches!" + Environment.NewLine);

            // Loops through the input and determines when the game should quit
            while (mind.active && mind.action != PlayerActions.QUIT)
            {
                Console.WriteLine("Your Command: ");
                input = mind.ReadUserInput();
                Console.WriteLine(Environment.NewLine);

                mind.ProcessUserInput(input);

                mind.GameLoop(mind.active);
            }
        }
    }
}

