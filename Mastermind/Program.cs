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

            //Intro screen
            Console.WriteLine("Welcome to Mastermind, I am the Codemaster and your objective is to guess the code i will come up with." +
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
                        "\n   - If you guess a color correctly, but it is not in the correct position, you get a colored peg" + //Colored peg
                        "\n   - If you guess a color correctly and it is in the correct position, you get a white peg" + //White Peg
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

            foreach (byte codeDigit in secretCode)
            {
                secretCode[codeDigit] = Convert.ToByte(rand.Next(1,7)); //generates a number that is greater or equal to 1 and less than 7
            }
            #endregion

            Console.WriteLine("I have come up with a code, now your job is to try to guess it!");

            #region Gameloop
            for (byte turn = 1; turn <= 10; turn++)
            {
                Console.Clear(); //Clears the board before the next round

                #region Display game board + Extra info
                //Displays turn number at the top of the screen
                Console.WriteLine($"Turn {turn}\n");

                for (byte x = 0; x < gameBoard.GetLength(0); x++)
                {
                    #region Displays turn number to the left of the game board
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
                    for (byte y = 0; y < gameBoard.GetLength(1); y++)
                    {
                        ColorSwitch(gameBoard[x, y]);
                        Console.Write("  ");
                        
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

                #region Guessing
                Console.WriteLine("\nPlease write your guess using this format:" +
                    "\n1st color-2nd color-3rd color-4th color");

                bool guessPrompt = true;

                string[] splitGuess = new string[4];

                while (guessPrompt == true)
                {
                    string wholeGuess = Console.ReadLine().ToLower(); //Reads input and makes it all lowercase

                    #region Guess format & specific color authentication
                    Regex guessFormat = new Regex(@"(\w+-\w+-\w+-\w+)");
                    bool validGuessFormat = guessFormat.IsMatch(wholeGuess);

                    //Split whole guess (at every '-') into seperate strings and saves in splitGuess string-array
                    splitGuess = wholeGuess.Split('-');
                    
                    if (validGuessFormat == true)
                    {
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
                }
                #endregion

                //Save guess to gameboard
                for (byte y = 0; y < gameBoard.GetLength(1); y++)
                {
                    gameBoard[turn - 1, y] = splitGuessByte[y];
                }
                #endregion
            }
            #endregion
        }
        static void ColorSwitch(byte input) //Changes background color based on byte value input
        {
            switch (input)
            {
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

                default:
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
            }
        }
    }
}