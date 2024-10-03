using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mastermind
{
    internal class Program
    {
        static void Main(string[] args)
        {
            byte[,] gameBoard = new byte[10, 4];
            byte[,] feedback = new byte[10, 4];

            //Intro screen
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(
                " __        __   _                            _        \n" +
                " \\ \\      / /__| | ___ ___  _ __ ___   ___  | |_ ___  \n" +
                "  \\ \\ /\\ / / _ \\ |/ __/ _ \\| '_ ` _ \\ / _ \\ | __/ _ \\ \n" +
                "   \\ V  V /  __/ | (_| (_) | | | | | |  __/ | || (_) |\n" +
                "    \\_/\\_/ \\___|_|\\___\\___/|_| |_| |_|\\___|  \\__\\___/\n");

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(
                "888b     d888                   888                                  d8b               888 \n" +
                "8888b   d8888                   888                                  Y8P               888 \n" +
                "88888b.d88888                   888                                                    888 \n" +
                "888Y88888P888  8888b.  .d8888b  888888 .d88b.  888d888 88888b.d88b.  888 88888b.   .d88888 \n" +
                "888 Y888P 888     \"88b 88K      888   d8P  Y8b 888P\"   888 \"888 \"88b 888 888 \"88b d88\" 888 \n" +
                "888  Y8P  888 .d888888 \"Y8888b. 888   88888888 888     888  888  888 888 888  888 888  888 \n" +
                "888   \"   888 888  888      X88 Y88b. Y8b.     888     888  888  888 888 888  888 Y88b 888 \n" +
                "888       888 \"Y888888  88888P'  \"Y888 \"Y8888  888     888  888  888 888 888  888  \"Y88888\n");
            
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Made by Thomas Mortensen\n");
            Console.BackgroundColor = ConsoleColor.Black;

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nGreetings, I am the Codemaster and your objective is to guess the code i will come up with." +
                "\n\nBefore we begin do you want to view the tutorial?");

            #region Tutorial
            bool tutorialPrompt = true;

            while (tutorialPrompt == true)
            {
                string answer = Console.ReadLine().ToLower();

                if (answer == "yes") //Tutorial
                {
                    Console.Clear();

                    Console.WriteLine("Mastermind tutorial:");

                    Console.WriteLine("Before the game begins, I will come up with a 4-digit code using 6 colors (the same color can appear more than once)" +
                        "\nand you have 10 turns to try and guess the code.");

                    Console.WriteLine("\n1. You will take a turn trying to guess the code" +
                        "\n\n2. I will give you feedback on your guess:" +
                        "\n   - If you guess the code correctly, you win!" +
                        "\n   - If you guess a color in the code correctly, but it is not in the correct position, you get a white peg" +
                        "\n   - If you guess a color and its position correctly, you get a colored peg" +
                        "\n\n3. If your guess was not correct, you take your next turn");

                    Console.WriteLine("\nPress any key to continue to the game");
                    Console.ReadKey();
                    break;
                }
                else if (answer == "no") //Skip tutorial
                {
                    tutorialPrompt = false;

                    Console.Clear();
                }
                else //Invalid input
                {
                    Console.Clear();

                    Console.WriteLine("Invalid input, try agian" +
                        "\n\nDo you want to view the tutorial?");
                }
            }
            #endregion
            
            Console.Clear(); //Clear the console to get ready for the game

            #region Generate secret code
            Random rand = new Random();

            byte[] secretCode = new byte[4];

            for (byte i = 0; i < secretCode.GetLength(0); i++)
            {
                secretCode[i] = Convert.ToByte(rand.Next(1,7)); //generates a number that is greater or equal to 1 and less than 7
            }
            #endregion

            Console.WriteLine("I have come up with a code, now your job is to try to guess it!");

            #region Gameloop
            byte turn = 1;
            bool gameWon = false;

            for (turn = 1; turn <= 11; turn++)
            {
                Console.Clear(); //Clears the board before the next round

                #region Display game board + Extra info
                //Displays turn number at the top of the screen
                Console.WriteLine($"Turn {turn}\n");

                for (byte x = 0; x < gameBoard.GetLength(0); x++)
                {
                    #region Displays turn number to the left of the game board
                    //Highlight turn on the left of the screen
                    if (x == turn - 1)
                    {
                        Console.Write(" > ");
                    }
                    else
                    {
                        Console.Write("   ");
                    }
                    
                    //Displays turn number to the left of the game board
                    if (x + 1 < 10)
                    {
                        Console.Write(" " + (x + 1) + ". ");
                    }
                    else
                    {
                        Console.Write(x + 1 +". ");
                    }
                    #endregion

                    #region Display gameboard
                    //Displays gameboard as cells of 2 spaces with background color corresponding to values in gameBoard array 
                    for (byte y = 0; y < gameBoard.GetLength(1) + feedback.GetLength(1); y++)
                    {
                        /*Console.ForegroundColor = ConsoleColor.Black;*/ //Unneccesary if i settle on spaces for pegs, useful if the pegs need to stand out

                        if (y < 4)
                        {
                            Console.Write('|');

                            ColorSwitch(gameBoard[x, y]);
                            Console.Write("  "); //Sets width (and look) of guess "pegs"

                            //Adds a space between gameboard and feedback pegs
                            if (y == 3)
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.Write("|-");
                            }
                        }
                        else
                        {
                            Console.Write('|');

                            ColorSwitch(feedback[x, y - 4]);
                            Console.Write(" "); //Sets width (and look) of feedback "pegs"

                            if (y == 7)
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.Write("| ");
                            }
                        }

                        //Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black; //"Resets" background color
                    }
                    #endregion

                    #region Display available colors - Maybe change this
                    string spacer = "   ";
                    switch (x)
                    {
                        case 1:
                            {
                                Console.Write(spacer + "Colors available for guess:");
                                break;
                            }
                        case 2:
                            {
                                Console.Write(spacer + "Red/R - Green/G - Blue/B");
                                break;
                            }
                        case 3:
                            {
                                Console.Write(spacer + "Yellow/Y - Magenta/M - Cyan/C");
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    };
                    #endregion

                    Console.WriteLine();
                }
                #endregion

                Console.WriteLine();

                bool guessPrompt = true;

                if (turn == 11 || gameWon == true)
                {
                    guessPrompt = false;

                    break;
                }

                #region Guessing
                string[] splitGuess = new string[4];

                while (guessPrompt == true)
                {
                    Console.WriteLine("Please write your guess using this format:" +
                        "\n1st color-2nd color-3rd color-4th color");

                    string wholeGuess = Console.ReadLine().ToLower(); //Reads input and makes it all lowercase

                    #region Guess format & specific color authentication
                    Regex guessFormat = new Regex(@"(^\w+-\w+-\w+-\w+$)");
                    bool validGuessFormat = guessFormat.IsMatch(wholeGuess);

                    
                    if (validGuessFormat == true)
                    {
                        //Split whole guess (at every '-') into seperate strings and saves in splitGuess string-array
                        splitGuess = wholeGuess.Split('-');
                        //for (int i = 0; i < splitGuess.Length; i++)
                        //{
                        //    splitGuess[i] = wholeGuess.Split('-')[i];
                        //}

                        foreach (string color in splitGuess)
                        {
                            //Color authentication
                            Regex availableColors = new Regex("red|r|green|g|blue|b|yellow|y|magenta|m|cyan|c");
                            bool validColor = availableColors.IsMatch(color);

                            if (validColor == true)
                            {
                                guessPrompt = false;
                            }
                            else
                            {
                                guessPrompt = true;
                                
                                Console.WriteLine("One or more of the input colors cannot be used, try again");

                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Guess does not follow correct format, try again");
                    }
                }
                #endregion

                #region Convert guess strings to bytes
                //Could have used Array.ConvertAll but it seemed way more complex when i looked it up
                byte[] splitGuessByte = new byte[splitGuess.GetLength(0)];

                for (byte i = 0; i < splitGuess.GetLength(0); i++) //"Copies" and converts splitGuess to splitGuessByte byte-array
                {
                    switch (splitGuess[i])
                    {
                        case string n when (n == "red" || n == "r"):
                            {
                                splitGuessByte[i] = 1;
                                break;
                            }

                        case string n when (n == "green" || n == "g"):
                            {
                                splitGuessByte[i] = 2;
                                break;
                            }

                        case string n when (n == "blue" || n == "b"):
                            {
                                splitGuessByte[i] = 3;
                                break;
                            }

                        case string n when (n == "yellow" || n == "y"):
                            {
                                splitGuessByte[i] = 4;
                                break;
                            }

                        case string n when (n == "magenta" || n == "m"):
                            {
                                splitGuessByte[i] = 5;
                                break;
                            }

                        case string n when (n == "cyan" || n == "c"):
                            {
                                splitGuessByte[i] = 6;
                                break;
                            }
                    }
                    
                    //Save guess to gameboard
                    gameBoard[turn - 1, i] = splitGuessByte[i];
                }
                #endregion

                #region Checking guess
                bool[] secretCodeMatched = new bool[4];
                bool[] guessMatched = new bool[4];

                int coloredPegs = 0;
                int whitePegs = 0;

                for (byte i = 0; i < gameBoard.GetLength(1); i++)
                {
                    if (splitGuessByte[i] == secretCode[i])
                    {
                        coloredPegs++;
                        secretCodeMatched[i] = true;
                        guessMatched[i] = true;
                    }
                }

                for (byte i = 0; i < gameBoard.GetLength(1); i++)
                {
                    if (!guessMatched[i])
                    {
                        for (byte j = 0; j < gameBoard.GetLength(1); j++)
                        {
                            if (!secretCodeMatched[j] && splitGuessByte[i] == secretCode[j])
                            {
                                whitePegs++;
                                secretCodeMatched[j] = true;
                                break;
                            }
                        }
                    }
                }

                if (coloredPegs > 0)
                {
                    for (byte i = 0; i < coloredPegs; i++)
                    {
                        feedback[turn - 1, i] = 7;
                    }

                    if (coloredPegs == 4)
                    {
                        gameWon = true;
                    }
                }

                if (whitePegs > 0)
                {
                    for (byte i = 0; i < whitePegs; i++)
                    {
                        feedback[turn - 1, i + coloredPegs] = 8;
                    }
                }
                #endregion
                #endregion
            }
            #endregion

            if (gameWon == true)
            {
                if (turn - 1 == 1)
                {
                    Console.WriteLine("You won in 1 turn!");
                }
                else
                {
                    Console.WriteLine($"You won in {turn - 1} turns!");
                }
            }
            else
            {
                Console.WriteLine("You lost :(");
            }

            Console.ReadKey();
        }
        static void ColorSwitch(byte input) //Changes background color based on byte value input
        {
            switch (input)
            {
                //Gameboard pegs
                case 1:
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    break;

                case 2:
                    Console.BackgroundColor = ConsoleColor.Green;
                    break;

                case 3:
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    break;

                case 4:
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    break;

                case 5:
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    break;

                case 6:
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    break;

                //Feedback pegs
                case 7:
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;

                case 8:
                    Console.BackgroundColor = ConsoleColor.White;
                    break;

                //"Empty" space
                default:
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
            }
        }
    }
}