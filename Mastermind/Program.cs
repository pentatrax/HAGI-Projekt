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
            int[,] gameBoard = new int[4, 10]
            {
                {1,2,3,4,5,6,0,0,0,0 },
                {1,2,3,4,5,6,0,0,0,0 },
                {1,2,3,4,5,6,0,0,0,0 },
                {1,2,3,4,5,6,0,0,0,0 }
            };

            for (int x = 0; x < gameBoard.GetLength(0); x++)
            {
                for (int y = 0; y < gameBoard.GetLength(1); y++)
                {
                    switch (gameBoard[x, y])
                    {
                        case 0:
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write("  ");
                            break;

                        case 1:
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.Write("  ");
                            break;

                        case 2:
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.Write("  ");
                            break;

                        case 3:
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.Write("  ");
                            break;

                        case 4:
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.Write("  ");
                            break;

                        case 5:
                            Console.BackgroundColor = ConsoleColor.Cyan;
                            Console.Write("  ");
                            break;

                        case 6:
                            Console.BackgroundColor = ConsoleColor.Magenta;
                            Console.Write("  ");
                            break;

                        default:
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write("  ");
                            break;
                    }
                }
                Console.WriteLine();
            }
            
            Console.ReadKey();
        }
    }
}
