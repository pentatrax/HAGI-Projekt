using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastermind
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[,] gameBoard = new int[10, 4]
            /*{
                {1,2,3,4 },
                {5,6,1,2 },
                {3,4,5,6 },
                {0,0,0,0 },
                {1,2,3,4 },
                {5,6,1,2 },
                {3,4,5,6 },
                {0,0,0,0 },
                {1,2,3,4 },
                {5,6,1,2 }
            }*/;

            
            Console.WriteLine("Welcome to Mastermind, I am the Codemaster and your objective is to guess the code i will come up with." + //Introduction to the game
                "\n\nBefore we begin do you want to view the tutorial?");

            #region Tutorial (prompt & guide)
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
            for (int turn = 1; turn <= 10; turn++)
            {
                //Console.Clear(); //Clears the board before the next round

                Console.WriteLine($"Turn {turn}");

                #region Print game board
                for (int x = 0; x < gameBoard.GetLength(0); x++)
                {
                    #region Displays turn number
                    switch (x)
                    {
                        case 0:
                            {
                                Console.Write(" 1. ");
                                break;
                            }
                        case 1:
                            {
                                Console.Write(" 2. ");
                                break;
                            }
                        case 2:
                            {
                                Console.Write(" 3. ");
                                break;
                            }
                        case 3:
                            {
                                Console.Write(" 4. ");
                                break;
                            }
                        case 4:
                            {
                                Console.Write(" 5. ");
                                break;
                            }
                        case 5:
                            {
                                Console.Write(" 6. ");
                                break;
                            }
                        case 6:
                            {
                                Console.Write(" 7. ");
                                break;
                            }
                        case 7:
                            {
                                Console.Write(" 8. ");
                                break;
                            }
                        case 8:
                            {
                                Console.Write(" 9. ");
                                break;
                            }
                        case 9:
                            {
                                Console.Write("10. ");
                                break;
                            }
                    }
                    #endregion

                    for (int y = 0; y < gameBoard.GetLength(1); y++) //Changing background color according to the values in gameBoard array
                    {
                        ColorSwitch(gameBoard[x, y]);
                        Console.Write("  ");
                        

                        Console.BackgroundColor = ConsoleColor.Black; //"Resets" background color
                    }

                    #region Display available colors - Maybe change this
                    switch (x)
                    {
                        case 1:
                            {
                                Console.Write("   Colors you can guess:");
                                break;
                            }
                        case 2:
                            {
                                Console.Write("   Red - Green - Blue");
                                break;
                            }
                        case 3:
                            {
                                Console.Write("   Yellow - Magenta - Cyan");
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

                #region Guess
                Console.WriteLine("\nPlease input your guess using this format:" + //Doesn't check for correct input format yet!!! (maybe use Regex, somehow)
                    "\n[1st color]-[2nd color]-[3rd color]-[4th color]");

                string wholeGuess = Console.ReadLine().ToLower(); //Reads input and makes it all lowercase

                string[] splitGuess = wholeGuess.Split('-'); //Splits string at every '-' and saves individual strings to splitGuess array

                #region Convert string to byte - Make this smarter!!!
                byte[] byteGuess = new byte[4];

                for (byte i = 0; i <= 3; i++) //"Copies" and converts splitGuess to byteGuess byte-array
                {
                    switch (splitGuess[i])
                    {
                        case "red":
                            {
                                byteGuess[i] = 1;
                                break;
                            }

                        case "green":
                            {
                                byteGuess[i] = 2;
                                break;
                            }

                        case "blue":
                            {
                                byteGuess[i] = 3;
                                break;
                            }

                        case "yellow":
                            {
                                byteGuess[i] = 4;
                                break;
                            }

                        case "magenta":
                            {
                                byteGuess[i] = 5;
                                break;
                            }

                        case "cyan":
                            {
                                byteGuess[i] = 6;
                                break;
                            }
                    }
                }

                foreach (byte digit in byteGuess)
                {
                    Console.Write(digit);
                }
                Console.WriteLine();
                #endregion

                #endregion
            }
            #endregion
        }
        static void ColorSwitch(int input) //Changes background color based on int value input
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
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    break;

                case 6:
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    break;

                default:
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
            }
        }
    }
}