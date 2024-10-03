using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;

namespace ClassicGameClient
{
    internal class Program
    {

        static void Main(string[] args)
        {
            #region Initiator
            bool appRunning = true;
            GameState appState = GameState.MainMenu;
            while (appRunning)
            {
                switch (appState)
                {
                    case GameState.MainMenu:
                        MainMenu(out appState, out appRunning);
                        break;
                    case GameState.Chess:
                        Chess(out appState);
                        break;
                    case GameState.SinkAShip:
                        SinkAShip(out appState);
                        break;
                    case GameState.Jeopardy:
                        Jeopardy(out appState);
                        break;
                    case GameState.Mastermind:
                        Mastermind(out appState);
                        break;
                }
            }
            #endregion
        }
        #region Chess Data
        private enum GameState : byte
        {
            MainMenu = 0,
            Chess = 1,
            Jeopardy = 2,
            SinkAShip = 3,
            Mastermind = 4,
            MineSweaper = 5,
        }
        private enum ChessPieces : byte
        {
            None = 0,
            WhiteKing = 1,
            WhiteQueen = 2,
            WhiteBishop = 3,
            WhiteRook = 4,
            WhiteKnight = 5,
            WhitePawn = 6,
            BlackKing = 7,
            BlackQueen = 8,
            BlackBishop = 9,
            BlackRook = 10,
            BlackKnight = 11,
            BlackPawn = 12,
        }
        #region BattleshipEnums
        //Enums for the Battleship Game
        private enum BattleshipLogistics
        {
            Water = 0,
            PatrolBoat = 1,
            Submarine = 2,
            Destroyer = 3,
            Battleship = 4,
            Carrier = 5,
            Bombed = 6,
        }
        #endregion
        private static void Log(Object input)
        {
            Console.Write(input);
        }
        private static void Log(Object input, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(input);
            Console.ForegroundColor = ConsoleColor.White;
        }
        private static void Color(ConsoleColor c)
        {
            Console.ForegroundColor = c;
        }
        private static void BackColor(ConsoleColor c)
        {
            Console.BackgroundColor = c;
        }
        #region BattleshipFunctions
        //Functions for Battleship Game
        /// <summary>
        /// Computer generated attacks to the enemy in battleships
        /// </summary>
        /// <param name="playerField"></param>
        private static void BSEnemyFire(int[,] playerField)
        {
            bool shotChecked = false;
            Random rnd = new Random();
            while (shotChecked == false)
            {
                int x = rnd.Next(0, 10);
                int y = rnd.Next(0, 10);
                if (playerField[x, y] == (int)BattleshipLogistics.Bombed)
                {
                    continue;
                }
                else if (playerField[x, y] == (int)BattleshipLogistics.Water)
                {
                    playerField[x, y] = 6;
                    ShowBSField(playerField);
                    Color(ConsoleColor.Blue);
                    Console.WriteLine($"\nSplash! Enemy hit water at {x + 1},{y + 1}!");
                    shotChecked = true;
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    string hitShip = Enum.GetName(typeof(BattleshipLogistics), playerField[x, y]);
                    playerField[x, y] = 6;
                    ShowBSField(playerField);
                    Color(ConsoleColor.Red);
                    Console.WriteLine($"\nBOOM! Enenmy has hit a/an {hitShip} at {x + 1},{y + 1}!");
                    shotChecked = true;
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
        /// <summary>
        /// Using user input to attack an enemy in Battleship
        /// </summary>
        /// <param name="enemyField"></param>
        /// <param name="attackField"></param>
        private static void BSPlayerFire(int[,] enemyField, int[,] attackField) //
        {
            bool checkedShot = false;
            bool intXChecker = false;
            bool intYChecker = false;
            int x = 0;
            int y = 0;
            while (checkedShot == false)
            {
                while (intXChecker == false)
                {
                    ShowBSField(attackField);
                    Console.Write("\nWhere on the x axis, do you wanna hit? (1 -> 10): ");
                    intXChecker = int.TryParse(Console.ReadLine().Trim(), out x);
                    if (intXChecker == true && x < 11 && x > 0)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Wrong number or not a number! Try again!");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }
                }
                Console.Clear();
                while (intYChecker == false)
                {
                    ShowBSField(attackField);
                    Console.Write("\nWhere on the Y axis, do you wanna hit? (1 -> 10): ");
                    intYChecker = int.TryParse(Console.ReadLine().Trim(), out y);
                    if (intYChecker == true && y < 11 && y > 0)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Wrong number or not a number! Try again!");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }
                }
                Console.Clear();
                y -= 1;
                x -= 1;
                if (enemyField[x, y] == (int)BattleshipLogistics.Water)
                {
                    enemyField[x, y] = 6;
                    attackField[x, y] = 6;
                    checkedShot = true;
                    ShowBSField(attackField);
                    Color(ConsoleColor.Blue);
                    Console.WriteLine("\nSPLASH! You hit the water!");
                    Console.ReadKey();
                    Console.ResetColor();
                    Console.Clear();
                    break;
                }
                else if (enemyField[x, y] == (int)BattleshipLogistics.Bombed)
                {
                    Console.WriteLine("\nThe place you chose has already been hit... try again!");
                    intXChecker = false;
                    intYChecker = false;
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                else
                {
                    checkedShot = true;
                    string shipName = Enum.GetName(typeof(BattleshipLogistics), enemyField[x, y]);
                    enemyField[x, y] = 6;
                    attackField[x, y] = 6;
                    ShowBSField(attackField);
                    Color(ConsoleColor.Red);
                    Console.WriteLine($"\nBOOM! You hit a/an {shipName}!");
                    Console.ReadKey();
                    Console.ResetColor();
                    Console.Clear();
                    break;
                }
            }
        }
        /// <summary>
        /// Show the battlefield with an user-friendly UI
        /// </summary>
        /// <param name="field"></param>
        private static void ShowBSField(int[,] field) //
        {
            Console.Write("    ");
            for (int i = 0; i < field.GetLength(0); i++)
            {
                Console.Write("     ");
            }
            Color(ConsoleColor.DarkRed);
            Console.Write("  X:\n");
            Console.ResetColor();
            Console.Write("   +");
            for (int i = 0; i < field.GetLength(0); i++)
            {
                Console.Write("----+");
            }
            Console.Write("\n");
            for (int x = 0; x < field.GetLength(0); x++)
            {
                Console.Write("   |");
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    switch (field[x, y])
                    {
                        case 0:
                            Color(ConsoleColor.White);
                            BackColor(ConsoleColor.Blue);
                            Console.Write("    ");
                            Console.ResetColor();
                            break;
                        case 1:
                            Color(ConsoleColor.White);
                            BackColor(ConsoleColor.Green);
                            Console.Write("PPPP");
                            Console.ResetColor();
                            break;
                        case 2:
                            Color(ConsoleColor.White);
                            BackColor(ConsoleColor.DarkYellow);
                            Console.Write("SSSS");
                            Console.ResetColor();
                            break;
                        case 3:
                            Color(ConsoleColor.White);
                            BackColor(ConsoleColor.DarkMagenta);
                            Console.Write("DDDD");
                            Console.ResetColor();
                            break;
                        case 4:
                            Color(ConsoleColor.White);
                            BackColor(ConsoleColor.DarkGreen);
                            Console.Write("BBBB");
                            Console.ResetColor();
                            break;
                        case 5:
                            Color(ConsoleColor.White);
                            BackColor(ConsoleColor.DarkGray);
                            Console.Write("CCCC");
                            Console.ResetColor();
                            break;
                        case 6:
                            Color(ConsoleColor.White);
                            BackColor(ConsoleColor.Red);
                            Console.Write("!!!!");
                            Console.ResetColor();
                            break;
                    }
                    Console.Write("|");
                }
                Color(ConsoleColor.Red);
                Console.Write($"  {x + 1}");
                Console.ResetColor();
                Console.Write("\n   |");
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    switch (field[x, y])
                    {
                        case 0:
                            Color(ConsoleColor.White);
                            BackColor(ConsoleColor.Blue);
                            Console.Write("    ");
                            Console.ResetColor();
                            break;
                        case 1:
                            Color(ConsoleColor.White);
                            BackColor(ConsoleColor.Green);
                            Console.Write("PPPP");
                            Console.ResetColor();
                            break;
                        case 2:
                            Color(ConsoleColor.White);
                            BackColor(ConsoleColor.DarkYellow);
                            Console.Write("SSSS");
                            Console.ResetColor();
                            break;
                        case 3:
                            Color(ConsoleColor.White);
                            BackColor(ConsoleColor.DarkMagenta);
                            Console.Write("DDDD");
                            Console.ResetColor();
                            break;
                        case 4:
                            Color(ConsoleColor.White);
                            BackColor(ConsoleColor.DarkGreen);
                            Console.Write("BBBB");
                            Console.ResetColor();
                            break;
                        case 5:
                            Color(ConsoleColor.White);
                            BackColor(ConsoleColor.DarkGray);
                            Console.Write("CCCC");
                            Console.ResetColor();
                            break;
                        case 6:
                            Color(ConsoleColor.White);
                            BackColor(ConsoleColor.Red);
                            Console.Write("!!!!");
                            Console.ResetColor();
                            break;
                    }
                    Console.Write("|");
                }

                Console.Write("\n   +----+----+----+----+----+----+----+----+----+----+\n");
            }
            Console.WriteLine("");
            Color(ConsoleColor.DarkRed);
            Console.Write("Y:");
            Color(ConsoleColor.Red);
            Console.WriteLine("   1    2    3    4    5    6    7    8    9    10");
            Console.ResetColor();
        }
        /// <summary>
        /// Generates a random battleship setup (Mainly used for Computer)
        /// </summary>
        /// <param name="enemyField"></param>
        private static void GenerateBSEnemyField(int[,] enemyField)
        {
            bool placementCheck = false;
            Random rnd = new Random();
            while (true)
            {
                while (placementCheck == false)
                {
                retry:
                    int x = rnd.Next(0, 10);
                    int y = rnd.Next(0, 10);
                    if (enemyField[x, y] == 0)
                    {
                        int direction = rnd.Next(1, 4);
                        switch (direction)
                        {
                            case 1:
                                if (x - 1 >= 0 && enemyField[x - 1, y] == 0)
                                {
                                    enemyField[x, y] = 1;
                                    enemyField[x - 1, y] = 1;
                                    placementCheck = true;
                                    break;
                                }
                                else
                                {
                                    goto retry;
                                }
                            case 2:
                                if (x + 1 <= 9 && enemyField[x + 1, y] == 0)
                                {
                                    enemyField[x, y] = 1;
                                    enemyField[x + 1, y] = 1;
                                    placementCheck = true;
                                    break;
                                }
                                else
                                {
                                    goto retry;
                                }
                            case 3:
                                if (y - 1 >= 0 && enemyField[x, y - 1] == 0)
                                {
                                    enemyField[x, y] = 1;
                                    enemyField[x, y - 1] = 1;
                                    placementCheck = true;
                                    break;
                                }
                                else
                                {
                                    goto retry;
                                }
                            case 4:
                                if (y + 1 <= 9 && enemyField[x, y + 1] == 0)
                                {
                                    enemyField[x, y] = 1;
                                    enemyField[x, y + 1] = 1;
                                    placementCheck = true;
                                    break;
                                }
                                else
                                {
                                    goto retry;
                                }
                        }
                    }
                    else
                    {
                        goto retry;
                    }
                }
                placementCheck = false;
                while (placementCheck == false)
                {
                retry:
                    int x = rnd.Next(0, 10);
                    int y = rnd.Next(0, 10);
                    if (enemyField[x, y] == 0)
                    {
                        int direction = rnd.Next(1, 4);
                        switch (direction)
                        {
                            case 1:
                                if (x - 1 >= 0 && enemyField[x - 1, y] == 0)
                                {
                                    enemyField[x, y] = 1;
                                    enemyField[x - 1, y] = 1;
                                    placementCheck = true;
                                    break;
                                }
                                else
                                {
                                    goto retry;
                                }
                            case 2:
                                if (x + 1 <= 9 && enemyField[x + 1, y] == 0)
                                {
                                    enemyField[x, y] = 1;
                                    enemyField[x + 1, y] = 1;
                                    placementCheck = true;
                                    break;
                                }
                                else
                                {
                                    goto retry;
                                }
                            case 3:
                                if (y - 1 >= 0 && enemyField[x, y - 1] == 0)
                                {
                                    enemyField[x, y] = 1;
                                    enemyField[x, y - 1] = 1;
                                    placementCheck = true;
                                    break;
                                }
                                else
                                {
                                    goto retry;
                                }
                            case 4:
                                if (y + 1 <= 9 && enemyField[x, y + 1] == 0)
                                {
                                    enemyField[x, y] = 1;
                                    enemyField[x, y + 1] = 1;
                                    placementCheck = true;
                                    break;
                                }
                                else
                                {
                                    goto retry;
                                }
                        }
                    }
                    else
                    {
                        goto retry;
                    }
                }
                placementCheck = false;
                while (placementCheck == false)
                {
                retry:
                    int x = rnd.Next(0, 10);
                    int y = rnd.Next(0, 10);
                    if (enemyField[x, y] == 0)
                    {
                        int direction = rnd.Next(1, 4);
                        switch (direction)
                        {
                            case 1:
                                if (x - 2 >= 0 && enemyField[x - 1, y] == 0 && enemyField[x - 2, y] == 0)
                                {
                                    enemyField[x, y] = 2;
                                    enemyField[x - 1, y] = 2;
                                    enemyField[x - 2, y] = 2;
                                    placementCheck = true;
                                    break;
                                }
                                else
                                {
                                    goto retry;
                                }
                            case 2:
                                if (x + 2 <= 9 && enemyField[x + 1, y] == 0 && enemyField[x + 2, y] == 0)
                                {
                                    enemyField[x, y] = 2;
                                    enemyField[x + 1, y] = 2;
                                    enemyField[x + 2, y] = 2;
                                    placementCheck = true;
                                    break;
                                }
                                else
                                {
                                    goto retry;
                                }
                            case 3:
                                if (y - 2 >= 0 && enemyField[x, y - 1] == 0 && enemyField[x, y - 2] == 0)
                                {
                                    enemyField[x, y] = 2;
                                    enemyField[x, y - 1] = 2;
                                    enemyField[x, y - 2] = 2;
                                    placementCheck = true;
                                    break;
                                }
                                else
                                {
                                    goto retry;
                                }
                            case 4:
                                if (y + 2 <= 9 && enemyField[x, y + 1] == 0 && enemyField[x, y + 2] == 0)
                                {
                                    enemyField[x, y] = 2;
                                    enemyField[x, y + 1] = 2;
                                    enemyField[x, y + 2] = 2;
                                    placementCheck = true;
                                    break;
                                }
                                else
                                {
                                    goto retry;
                                }
                        }
                    }
                    else
                    {
                        goto retry;
                    }
                }
                placementCheck = false;
                while (placementCheck == false)
                {
                retry:
                    int x = rnd.Next(0, 10);
                    int y = rnd.Next(0, 10);
                    if (enemyField[x, y] == 0)
                    {
                        int direction = rnd.Next(1, 4);
                        switch (direction)
                        {
                            case 1:
                                if (x - 2 >= 0 && enemyField[x - 1, y] == 0 && enemyField[x - 2, y] == 0)
                                {
                                    enemyField[x, y] = 3;
                                    enemyField[x - 1, y] = 3;
                                    enemyField[x - 2, y] = 3;
                                    placementCheck = true;
                                    break;
                                }
                                else
                                {
                                    goto retry;
                                }
                            case 2:
                                if (x + 2 <= 9 && enemyField[x + 1, y] == 0 && enemyField[x + 2, y] == 0)
                                {
                                    enemyField[x, y] = 3;
                                    enemyField[x + 1, y] = 3;
                                    enemyField[x + 2, y] = 3;
                                    placementCheck = true;
                                    break;
                                }
                                else
                                {
                                    goto retry;
                                }
                            case 3:
                                if (y - 2 >= 0 && enemyField[x, y - 1] == 0 && enemyField[x, y - 2] == 0)
                                {
                                    enemyField[x, y] = 3;
                                    enemyField[x, y - 1] = 3;
                                    enemyField[x, y - 2] = 3;
                                    placementCheck = true;
                                    break;
                                }
                                else
                                {
                                    goto retry;
                                }
                            case 4:
                                if (y + 2 <= 9 && enemyField[x, y + 1] == 0 && enemyField[x, y + 2] == 0)
                                {
                                    enemyField[x, y] = 3;
                                    enemyField[x, y + 1] = 3;
                                    enemyField[x, y + 2] = 3;
                                    placementCheck = true;
                                    break;
                                }
                                else
                                {
                                    goto retry;
                                }
                        }
                    }
                    else
                    {
                        goto retry;
                    }
                }
                placementCheck = false;
                while (placementCheck == false)
                {
                retry:
                    int x = rnd.Next(0, 10);
                    int y = rnd.Next(0, 10);
                    if (enemyField[x, y] == 0)
                    {
                        int direction = rnd.Next(1, 4);
                        switch (direction)
                        {
                            case 1:
                                if (x - 3 >= 0 && enemyField[x - 1, y] == 0 && enemyField[x - 2, y] == 0 && enemyField[x - 3, y] == 0)
                                {
                                    enemyField[x, y] = 4;
                                    enemyField[x - 1, y] = 4;
                                    enemyField[x - 2, y] = 4;
                                    enemyField[x - 3, y] = 4;
                                    placementCheck = true;
                                    break;
                                }
                                else
                                {
                                    goto retry;
                                }
                            case 2:
                                if (x + 3 <= 9 && enemyField[x + 1, y] == 0 && enemyField[x + 2, y] == 0 && enemyField[x + 3, y] == 0)
                                {
                                    enemyField[x, y] = 4;
                                    enemyField[x + 1, y] = 4;
                                    enemyField[x + 2, y] = 4;
                                    enemyField[x + 3, y] = 4;
                                    placementCheck = true;
                                    break;
                                }
                                else
                                {
                                    goto retry;
                                }
                            case 3:
                                if (y - 3 >= 0 && enemyField[x, y - 1] == 0 && enemyField[x, y - 2] == 0 && enemyField[x, y - 3] == 0)
                                {
                                    enemyField[x, y] = 4;
                                    enemyField[x, y - 1] = 4;
                                    enemyField[x, y - 2] = 4;
                                    enemyField[x, y - 3] = 4;
                                    placementCheck = true;
                                    break;
                                }
                                else
                                {
                                    goto retry;
                                }
                            case 4:
                                if (y + 3 <= 9 && enemyField[x, y + 1] == 0 && enemyField[x, y + 2] == 0 && enemyField[x, y - 3] == 0)
                                {
                                    enemyField[x, y] = 4;
                                    enemyField[x, y + 1] = 4;
                                    enemyField[x, y + 2] = 4;
                                    enemyField[x, y + 3] = 4;
                                    placementCheck = true;
                                    break;
                                }
                                else
                                {
                                    goto retry;
                                }
                        }
                    }
                    else
                    {
                        goto retry;
                    }
                }
                placementCheck = false;
                while (placementCheck == false)
                {
                retry:
                    int x = rnd.Next(0, 10);
                    int y = rnd.Next(0, 10);
                    if (enemyField[x, y] == 0)
                    {
                        int direction = rnd.Next(1, 4);
                        switch (direction)
                        {
                            case 1:
                                if (x - 4 >= 0 && enemyField[x - 1, y] == 0 && enemyField[x - 2, y] == 0 && enemyField[x - 3, y] == 0 && enemyField[x - 4, y] == 0)
                                {
                                    enemyField[x, y] = 5;
                                    enemyField[x - 1, y] = 5;
                                    enemyField[x - 2, y] = 5;
                                    enemyField[x - 3, y] = 5;
                                    enemyField[x - 4, y] = 5;
                                    placementCheck = true;
                                    break;
                                }
                                else
                                {
                                    goto retry;
                                }
                            case 2:
                                if (x + 4 <= 9 && enemyField[x + 1, y] == 0 && enemyField[x + 2, y] == 0 && enemyField[x + 3, y] == 0 && enemyField[x + 4, y] == 0)
                                {
                                    enemyField[x, y] = 5;
                                    enemyField[x + 1, y] = 5;
                                    enemyField[x + 2, y] = 5;
                                    enemyField[x + 3, y] = 5;
                                    enemyField[x + 4, y] = 5;
                                    placementCheck = true;
                                    break;
                                }
                                else
                                {
                                    goto retry;
                                }
                            case 3:
                                if (y - 4 >= 0 && enemyField[x, y - 1] == 0 && enemyField[x, y - 2] == 0 && enemyField[x, y - 3] == 0 && enemyField[x, y - 4] == 0)
                                {
                                    enemyField[x, y] = 5;
                                    enemyField[x, y - 1] = 5;
                                    enemyField[x, y - 2] = 5;
                                    enemyField[x, y - 3] = 5;
                                    enemyField[x, y - 4] = 5;
                                    placementCheck = true;
                                    break;
                                }
                                else
                                {
                                    goto retry;
                                }
                            case 4:
                                if (y + 4 <= 9 && enemyField[x, y + 1] == 0 && enemyField[x, y + 2] == 0 && enemyField[x, y - 3] == 0 && enemyField[x, y - 4] == 0)
                                {
                                    enemyField[x, y] = 5;
                                    enemyField[x, y + 1] = 5;
                                    enemyField[x, y + 2] = 5;
                                    enemyField[x, y + 3] = 5;
                                    enemyField[x, y + 4] = 5;
                                    placementCheck = true;
                                    break;
                                }
                                else
                                {
                                    goto retry;
                                }
                        }
                    }
                    else
                    {
                        goto retry;
                    }
                }
                placementCheck = false;
                break;
            }
        }
        /// <summary>
        /// Using user input to place different boats on the battlefield in Battleship.
        /// </summary>
        /// <param name="playerField"></param>
        private static void UserFieldGeneration(int[,] playerField)
        {
            int shipID;
            bool placementCheck = false;
            while (placementCheck == false)
            {
                shipID = 1;
            retry:
                ShowBSField(playerField);
                Console.WriteLine("");
                Console.Write($"Which x coordinate do you wanna place a {Enum.GetName(typeof(BattleshipLogistics), shipID)}? (Needs 2 spaces) (1 - 10): ");
                if (int.TryParse(Console.ReadLine().Trim(), out int x) && x < 11 && x > 0)
                {
                    x--;
                    Console.Write($"Which y coordinate do you wanna place a {Enum.GetName(typeof(BattleshipLogistics), shipID)}? (Needs 2 spaces) (1 - 10): ");
                    if (int.TryParse(Console.ReadLine().Trim(), out int y) && y < 11 && y > 0)
                    {
                        y--;
                        if (playerField[x, y] == 0)
                        {
                            Console.WriteLine("1 for North!\n2 for South!\n3 for West\n4 for East!");
                            Console.Write($"Which direction do you wanna go for the {Enum.GetName(typeof(BattleshipLogistics), shipID)}? (Needs 2 spaces): ");
                            if (int.TryParse(Console.ReadLine().Trim(), out int direction) && direction < 5 && direction > 0)
                            {
                                switch (direction)
                                {
                                    case 1:
                                        if (x - 1 >= 0 && playerField[x - 1, y] == 0)
                                        {
                                            playerField[x, y] = 1;
                                            playerField[x - 1, y] = 1;
                                            placementCheck = true;
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} has been placed!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} can't be placed here! Try again!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            goto retry;
                                        }
                                    case 2:
                                        if (x + 1 <= 9 && playerField[x + 1, y] == 0)
                                        {
                                            playerField[x, y] = 1;
                                            playerField[x + 1, y] = 1;
                                            placementCheck = true;
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} has been placed!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} can't be placed here! Try again!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            goto retry;
                                        }
                                    case 3:
                                        if (y - 1 >= 0 && playerField[x, y - 1] == 0)
                                        {
                                            playerField[x, y] = 1;
                                            playerField[x, y - 1] = 1;
                                            placementCheck = true;
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} has been placed!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} can't be placed here! Try again!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            goto retry;
                                        }
                                    case 4:
                                        if (y + 1 <= 9 && playerField[x, y + 1] == 0)
                                        {
                                            playerField[x, y] = 1;
                                            playerField[x, y + 1] = 1;
                                            placementCheck = true;
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} has been placed!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} can't be placed here! Try again!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            goto retry;
                                        }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Unkown command! Try again!");
                                Console.ReadKey();
                                Console.Clear();
                                goto retry;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Something is already here! Try again!");
                            Console.ReadKey();
                            Console.Clear();
                            goto retry;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Is outside the battlefield or is not an number! Try again!");
                        Console.ReadKey();
                        Console.Clear();
                        goto retry;
                    }
                }
                else
                {
                    Console.WriteLine("Is outside the battlefield or is not an number! Try again!");
                    Console.ReadKey();
                    Console.Clear();
                    goto retry;
                }

            }
            placementCheck = false;
            while (placementCheck == false)
            {
                shipID = 1;
            retry:
                ShowBSField(playerField);
                Console.WriteLine("");
                Console.Write($"Which x coordinate do you wanna place a {Enum.GetName(typeof(BattleshipLogistics), shipID)}? (Needs 2 spaces) (1 - 10): ");
                if (int.TryParse(Console.ReadLine().Trim(), out int x) && x < 11 && x > 0)
                {
                    x--;
                    Console.Write($"Which y coordinate do you wanna place a {Enum.GetName(typeof(BattleshipLogistics), shipID)}? (Needs 2 spaces) (1 - 10): ");
                    if (int.TryParse(Console.ReadLine().Trim(), out int y) && y < 11 && y > 0)
                    {
                        y--;
                        if (playerField[x, y] == 0)
                        {
                            Console.WriteLine("1 for North!\n2 for South!\n3 for West\n4 for East!");
                            Console.Write($"Which direction do you wanna go for the {Enum.GetName(typeof(BattleshipLogistics), shipID)}? (Needs 2 spaces): ");
                            if (int.TryParse(Console.ReadLine().Trim(), out int direction) && direction < 5 && direction > 0)
                            {
                                switch (direction)
                                {
                                    case 1:
                                        if (x - 1 >= 0 && playerField[x - 1, y] == 0)
                                        {
                                            playerField[x, y] = 1;
                                            playerField[x - 1, y] = 1;
                                            placementCheck = true;
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} has been placed!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} can't be placed here! Try again!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            goto retry;
                                        }
                                    case 2:
                                        if (x + 1 <= 9 && playerField[x + 1, y] == 0)
                                        {
                                            playerField[x, y] = 1;
                                            playerField[x + 1, y] = 1;
                                            placementCheck = true;
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} has been placed!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} can't be placed here! Try again!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            goto retry;
                                        }
                                    case 3:
                                        if (y - 1 >= 0 && playerField[x, y - 1] == 0)
                                        {
                                            playerField[x, y] = 1;
                                            playerField[x, y - 1] = 1;
                                            placementCheck = true;
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} has been placed!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} can't be placed here! Try again!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            goto retry;
                                        }
                                    case 4:
                                        if (y + 1 <= 9 && playerField[x, y + 1] == 0)
                                        {
                                            playerField[x, y] = 1;
                                            playerField[x, y + 1] = 1;
                                            placementCheck = true;
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} has been placed!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} can't be placed here! Try again!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            goto retry;
                                        }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Unkown command! Try again!");
                                Console.ReadKey();
                                Console.Clear();
                                goto retry;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Something is already here! Try again!");
                            Console.ReadKey();
                            Console.Clear();
                            goto retry;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Is outside the battlefield or is not an number! Try again!");
                        Console.ReadKey();
                        Console.Clear();
                        goto retry;
                    }
                }
                else
                {
                    Console.WriteLine("Is outside the battlefield or is not an number! Try again!");
                    Console.ReadKey();
                    Console.Clear();
                    goto retry;
                }
            }
            placementCheck = false;
            while (placementCheck == false)
            {
                shipID = 2;
            retry:
                ShowBSField(playerField);
                Console.WriteLine("");
                Console.Write($"Which x coordinate do you wanna place a {Enum.GetName(typeof(BattleshipLogistics), shipID)}? (Needs 3 spaces) (1 - 10): ");
                if (int.TryParse(Console.ReadLine().Trim(), out int x) && x < 11 && x > 0)
                {
                    x--;
                    Console.Write($"Which y coordinate do you wanna place a {Enum.GetName(typeof(BattleshipLogistics), shipID)}? (Needs 3 spaces) (1 - 10): ");
                    if (int.TryParse(Console.ReadLine().Trim(), out int y) && y < 11 && y > 0)
                    {
                        y--;
                        if (playerField[x, y] == 0)
                        {
                            Console.WriteLine("1 for North!\n2 for South!\n3 for West\n4 for East!");
                            Console.Write($"Which direction do you wanna go for the {Enum.GetName(typeof(BattleshipLogistics), shipID)}? (Needs 3 spaces): ");
                            if (int.TryParse(Console.ReadLine().Trim(), out int direction) && direction < 5 && direction > 0)
                            {
                                switch (direction)
                                {
                                    case 1:
                                        if (x - 2 >= 0 && playerField[x - 1, y] == 0 && playerField[x - 2, y] == 0)
                                        {
                                            playerField[x, y] = 2;
                                            playerField[x - 1, y] = 2;
                                            playerField[x - 2, y] = 2;
                                            placementCheck = true;
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} has been placed!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} can't be placed here! Try again!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            goto retry;
                                        }
                                    case 2:
                                        if (x + 2 <= 9 && playerField[x + 1, y] == 0 && playerField[x + 2, y] == 0)
                                        {
                                            playerField[x, y] = 2;
                                            playerField[x + 1, y] = 2;
                                            playerField[x + 2, y] = 2;
                                            placementCheck = true;
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} has been placed!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} can't be placed here! Try again!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            goto retry;
                                        }
                                    case 3:
                                        if (y - 2 >= 0 && playerField[x, y - 1] == 0 && playerField[x, y - 2] == 0)
                                        {
                                            playerField[x, y] = 2;
                                            playerField[x, y - 1] = 2;
                                            playerField[x, y - 2] = 2;
                                            placementCheck = true;
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} has been placed!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} can't be placed here! Try again!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            goto retry;
                                        }
                                    case 4:
                                        if (y + 2 <= 9 && playerField[x, y + 1] == 0 && playerField[x, y + 2] == 0)
                                        {
                                            playerField[x, y] = 2;
                                            playerField[x, y + 1] = 2;
                                            playerField[x, y + 2] = 2;
                                            placementCheck = true;
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} has been placed!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} can't be placed here! Try again!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            goto retry;
                                        }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Unkown command! Try again!");
                                Console.ReadKey();
                                Console.Clear();
                                goto retry;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Something is already here! Try again!");
                            Console.ReadKey();
                            Console.Clear();
                            goto retry;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Is outside the battlefield or is not an number! Try again!");
                        Console.ReadKey();
                        Console.Clear();
                        goto retry;
                    }
                }
                else
                {
                    Console.WriteLine("Is outside the battlefield or is not an number! Try again!");
                    Console.ReadKey();
                    Console.Clear();
                    goto retry;
                }
            }
            placementCheck = false;
            while (placementCheck == false)
            {
                shipID = 3;
            retry:
                ShowBSField(playerField);
                Console.WriteLine("");
                Console.Write($"Which x coordinate do you wanna place a {Enum.GetName(typeof(BattleshipLogistics), shipID)}? (Needs 3 spaces) (1 - 10): ");
                if (int.TryParse(Console.ReadLine().Trim(), out int x) && x < 11 && x > 0)
                {
                    x--;
                    Console.Write($"Which y coordinate do you wanna place a {Enum.GetName(typeof(BattleshipLogistics), shipID)}? (Needs 3 spaces) (1 - 10): ");
                    if (int.TryParse(Console.ReadLine().Trim(), out int y) && y < 11 && y > 0)
                    {
                        y--;
                        if (playerField[x, y] == 0)
                        {
                            Console.WriteLine("1 for North!\n2 for South!\n3 for West\n4 for East!");
                            Console.Write($"Which direction do you wanna go for the {Enum.GetName(typeof(BattleshipLogistics), shipID)}? (Needs 3 spaces): ");
                            if (int.TryParse(Console.ReadLine().Trim(), out int direction))
                            {
                                switch (direction)
                                {
                                    case 1:
                                        if (x - 2 >= 0 && playerField[x - 1, y] == 0 && playerField[x - 2, y] == 0)
                                        {
                                            playerField[x, y] = 3;
                                            playerField[x - 1, y] = 3;
                                            playerField[x - 2, y] = 3;
                                            placementCheck = true;
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} has been placed!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} can't be placed here! Try again!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            goto retry;
                                        }
                                    case 2:
                                        if (x + 2 <= 9 && playerField[x + 1, y] == 0 && playerField[x + 2, y] == 0)
                                        {
                                            playerField[x, y] = 3;
                                            playerField[x + 1, y] = 3;
                                            playerField[x + 2, y] = 3;
                                            placementCheck = true;
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} has been placed!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} can't be placed here! Try again!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            goto retry;
                                        }
                                    case 3:
                                        if (y - 2 >= 0 && playerField[x, y - 1] == 0 && playerField[x, y - 2] == 0)
                                        {
                                            playerField[x, y] = 3;
                                            playerField[x, y - 1] = 3;
                                            playerField[x, y - 2] = 3;
                                            placementCheck = true;
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} has been placed!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} can't be placed here! Try again!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            goto retry;
                                        }
                                    case 4:
                                        if (y + 2 <= 9 && playerField[x, y + 1] == 0 && playerField[x, y + 2] == 0)
                                        {
                                            playerField[x, y] = 3;
                                            playerField[x, y + 1] = 3;
                                            playerField[x, y + 2] = 3;
                                            placementCheck = true;
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} has been placed!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} can't be placed here! Try again!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            goto retry;
                                        }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Unkown command! Try again!");
                                Console.ReadKey();
                                Console.Clear();
                                goto retry;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Something is already here! Try again!");
                            Console.ReadKey();
                            Console.Clear();
                            goto retry;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Is outside the battlefield or is not an number! Try again!");
                        Console.ReadKey();
                        Console.Clear();
                        goto retry;
                    }
                }
                else
                {
                    Console.WriteLine("Is outside the battlefield or is not an number! Try again!");
                    Console.ReadKey();
                    Console.Clear();
                    goto retry;
                }

            }
            placementCheck = false;
            while (placementCheck == false)
            {
                shipID = 4;
            retry:
                ShowBSField(playerField);
                Console.WriteLine("");
                Console.Write($"Which x coordinate do you wanna place a {Enum.GetName(typeof(BattleshipLogistics), shipID)}? (Needs 4 spaces) (1 - 10): ");
                if (int.TryParse(Console.ReadLine().Trim(), out int x) && x < 11 && x > 0)
                {
                    x--;
                    Console.Write($"Which y coordinate do you wanna place a {Enum.GetName(typeof(BattleshipLogistics), shipID)}? (Needs 4 spaces) (1 - 10): ");
                    if (int.TryParse(Console.ReadLine().Trim(), out int y) && y < 11 && y > 0)
                    {
                        y--;
                        if (playerField[x, y] == 0)
                        {
                            Console.WriteLine("1 for North!\n2 for South!\n3 for West\n4 for East!");
                            Console.Write($"Which direction do you wanna go for the {Enum.GetName(typeof(BattleshipLogistics), shipID)}? (Needs 4 spaces): ");
                            if (int.TryParse(Console.ReadLine().Trim(), out int direction))
                            {
                                switch (direction)
                                {
                                    case 1:
                                        if (x - 3 >= 0 && playerField[x - 1, y] == 0 && playerField[x - 2, y] == 0 && playerField[x - 3, y] == 0)
                                        {
                                            playerField[x, y] = 4;
                                            playerField[x - 1, y] = 4;
                                            playerField[x - 2, y] = 4;
                                            playerField[x - 3, y] = 4;
                                            placementCheck = true;
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} has been placed!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} can't be placed here! Try again!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            goto retry;
                                        }
                                    case 2:
                                        if (x + 3 <= 9 && playerField[x + 1, y] == 0 && playerField[x + 2, y] == 0 && playerField[x + 3, y] == 0)
                                        {
                                            playerField[x, y] = 4;
                                            playerField[x + 1, y] = 4;
                                            playerField[x + 2, y] = 4;
                                            playerField[x + 3, y] = 4;
                                            placementCheck = true;
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} has been placed!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} can't be placed here! Try again!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            goto retry;
                                        }
                                    case 3:
                                        if (y - 3 >= 0 && playerField[x, y - 1] == 0 && playerField[x, y - 2] == 0 && playerField[x, y - 3] == 0)
                                        {
                                            playerField[x, y] = 4;
                                            playerField[x, y - 1] = 4;
                                            playerField[x, y - 2] = 4;
                                            playerField[x, y - 3] = 4;
                                            placementCheck = true;
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} has been placed!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} can't be placed here! Try again!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            goto retry;
                                        }
                                    case 4:
                                        if (y + 3 <= 9 && playerField[x, y + 1] == 0 && playerField[x, y + 2] == 0 && playerField[x, y + 3] == 0)
                                        {
                                            playerField[x, y] = 4;
                                            playerField[x, y + 1] = 4;
                                            playerField[x, y + 2] = 4;
                                            playerField[x, y + 3] = 4;
                                            placementCheck = true;
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} has been placed!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} can't be placed here! Try again!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            goto retry;
                                        }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Unkown command! Try again!");
                                Console.ReadKey();
                                Console.Clear();
                                goto retry;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Something is already here! Try again!");
                            Console.ReadKey();
                            Console.Clear();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Is outside the battlefield or is not an number! Try again!");
                        Console.ReadKey();
                        Console.Clear();
                        goto retry;
                    }
                }
                else
                {
                    Console.WriteLine("Is outside the battlefield or is not an number! Try again!");
                    Console.ReadKey();
                    Console.Clear();
                    goto retry;
                }

            }
            placementCheck = false;
            while (placementCheck == false)
            {
                shipID = 5;
            retry:
                ShowBSField(playerField);
                Console.WriteLine("");
                Console.Write($"Which x coordinate do you wanna place a {Enum.GetName(typeof(BattleshipLogistics), shipID)}? (Needs 5 spaces) (1 - 10): ");
                if (int.TryParse(Console.ReadLine().Trim(), out int x) && x < 11 && x > 0)
                {
                    x--;
                    Console.Write($"Which y coordinate do you wanna place a {Enum.GetName(typeof(BattleshipLogistics), shipID)}? (Needs 5 spaces) (1 - 10): ");
                    if (int.TryParse(Console.ReadLine().Trim(), out int y) && y < 11 && y > 0)
                    {
                        y--;
                        if (playerField[x, y] == 0)
                        {
                            Console.WriteLine("1 for North!\n2 for South!\n3 for West\n4 for East!");
                            Console.Write($"Which direction do you wanna go for the {Enum.GetName(typeof(BattleshipLogistics), shipID)}? (Needs 5 spaces): ");
                            if (int.TryParse(Console.ReadLine().Trim(), out int direction) && direction < 5 && direction > 0)
                            {
                                switch (direction)
                                {
                                    case 1:
                                        if (x - 4 >= 0 && playerField[x - 1, y] == 0 && playerField[x - 2, y] == 0 && playerField[x - 3, y] == 0 && playerField[x - 4, y] == 0)
                                        {
                                            playerField[x, y] = 5;
                                            playerField[x - 1, y] = 5;
                                            playerField[x - 2, y] = 5;
                                            playerField[x - 3, y] = 5;
                                            playerField[x - 4, y] = 5;
                                            placementCheck = true;
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} has been placed!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} can't be placed here! Try again!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            goto retry;
                                        }
                                    case 2:
                                        if (x + 4 <= 9 && playerField[x + 1, y] == 0 && playerField[x + 2, y] == 0 && playerField[x + 3, y] == 0 && playerField[x + 4, y] == 0)
                                        {
                                            playerField[x, y] = 5;
                                            playerField[x + 1, y] = 5;
                                            playerField[x + 2, y] = 5;
                                            playerField[x + 3, y] = 5;
                                            playerField[x + 4, y] = 5;
                                            placementCheck = true;
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} has been placed!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} can't be placed here! Try again!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            goto retry;
                                        }
                                    case 3:
                                        if (y - 4 >= 0 && playerField[x, y - 1] == 0 && playerField[x, y - 2] == 0 && playerField[x, y - 3] == 0 && playerField[x, y - 4] == 0)
                                        {
                                            playerField[x, y] = 5;
                                            playerField[x, y - 1] = 5;
                                            playerField[x, y - 2] = 5;
                                            playerField[x, y - 3] = 5;
                                            playerField[x, y - 4] = 5;
                                            placementCheck = true;
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} has been placed!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} can't be placed here! Try again!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            goto retry;
                                        }
                                    case 4:
                                        if (y + 4 <= 9 && playerField[x, y + 1] == 0 && playerField[x, y + 2] == 0 && playerField[x, y + 3] == 0 && playerField[x, y + 4] == 0)
                                        {
                                            playerField[x, y] = 5;
                                            playerField[x, y + 1] = 5;
                                            playerField[x, y + 2] = 5;
                                            playerField[x, y + 3] = 5;
                                            playerField[x, y + 4] = 5;
                                            placementCheck = true;
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} has been placed!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{Enum.GetName(typeof(BattleshipLogistics), shipID)} can't be placed here! Try again!");
                                            Console.ReadKey();
                                            Console.Clear();
                                            goto retry;
                                        }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Unkown command! Try again!");
                                Console.ReadKey();
                                Console.Clear();
                                goto retry;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Something is already here! Try again!");
                            Console.ReadKey();
                            Console.Clear();
                            goto retry;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Is outside the battlefield or is not an number! Try again!");
                        Console.ReadKey();
                        Console.Clear();
                        goto retry;
                    }
                }
                else
                {
                    Console.WriteLine("Is outside the battlefield or is not an number! Try again!");
                    Console.ReadKey();
                    Console.Clear();
                    goto retry;
                }
                Log("Final Placement:\n", ConsoleColor.DarkRed);
                ShowBSField(playerField);
                Console.ReadKey();
                Console.Clear();

            }
        }
        /// <summary>
        /// A boolean check to check if an player is still alive on the battlefield in Battleships.
        /// </summary>
        /// <param name="Field"></param>
        /// <returns></returns>
        private static bool IsPlayerAlive(int[,] Field)
        {
            bool enemyAlive = false;
            foreach (int field in Field)
            {
                if (field > 0 && field < 6)
                {
                    enemyAlive = true;
                }
                else
                {
                    continue;
                }
            }
            if (enemyAlive)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        /// <summary>
        /// Translates [a1] coordinates to array coordinates.
        /// </summary>
        /// <param name="instruction">Takes any string using the format [a2].</param>
        /// <param name="error">Writes error if instruction doesnt fit coordinates format.</param>
        /// <returns></returns>
        private static byte[] InstructionToCordinates(string instruction, out string error)
        {
            byte[] cords = new byte[2];
            string[] boardCords = {
                "ABCDEFGH",
                "87654321"
            };
            Regex singleInstructCheck = new Regex("^([A-H][1-8])+");


            error = "";
            if (singleInstructCheck.IsMatch(instruction))
            {
                cords[0] = (byte)boardCords[0].IndexOf(instruction[0]);
                cords[1] = (byte)boardCords[1].IndexOf(instruction[1]);
            }
            else
            {
                error = "Instrcution doesn't fit the format!";
            }

            return cords;
        }
        #region Main Menu
        /// <summary>
        /// The main menu that allows transference between the different games through the change of GameState (An enum)
        /// </summary>
        /// <param name="gameState"></param>
        /// <param name="appRunning"></param>
        private static void MainMenu(out GameState gameState, out bool appRunning)
        {
            Console.Clear();
            appRunning = true;
            gameState = GameState.MainMenu;
            while (true)
            {
                Log(" _______  _        _______  _______  _______ _________ _______    _______  _______  _______  _______  _______ \r\n(  ____ \\( \\      (  ___  )(  ____ \\(  ____ \\\\__   __/(  ____ \\  (  ____ \\(  ___  )(       )(  ____ \\(  ____ \\\r\n| (    \\/| (      | (   ) || (    \\/| (    \\/   ) (   | (    \\/  | (    \\/| (   ) || () () || (    \\/| (    \\/\r\n| |      | |      | (___) || (_____ | (_____    | |   | |        | |      | (___) || || || || (__    | (_____ \r\n| |      | |      |  ___  |(_____  )(_____  )   | |   | |        | | ____ |  ___  || |(_)| ||  __)   (_____  )\r\n| |      | |      | (   ) |      ) |      ) |   | |   | |        | | \\_  )| (   ) || |   | || (            ) |\r\n| (____/\\| (____/\\| )   ( |/\\____) |/\\____) |___) (___| (____/\\  | (___) || )   ( || )   ( || (____/\\/\\____) |\r\n(_______/(_______/|/     \\|\\_______)\\_______)\\_______/(_______/  (_______)|/     \\||/     \\|(_______/\\_______)\r\n                                                                                                              \n", ConsoleColor.DarkGreen);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("1");
                Console.ResetColor();
                Console.Write(" for ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Chess\n");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("2");
                Console.ResetColor();
                Console.Write(" for ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Mastermind\n");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("3");
                Console.ResetColor();
                Console.Write(" for ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Battleship\n");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("4");
                Console.ResetColor();
                Console.Write(" for ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Jeopardy\n");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("5");
                Console.ResetColor();
                Console.Write(" for ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("Credits\n");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("6");
                Console.ResetColor();
                Console.Write(" to ");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("Exit program\n");
                Console.ResetColor();
                Console.Write("So what do you wanna do?: ");
                if (int.TryParse(Console.ReadLine().Trim(), out int choice) && choice < 7 && choice > 0)
                {
                    switch (choice)
                    {
                        case 1:
                            gameState = GameState.Chess;
                            Console.Clear();
                            return;
                        case 2:
                            gameState = GameState.Mastermind;
                            Console.Clear();
                            return;
                        case 3:
                            gameState = GameState.SinkAShip;
                            Console.Clear();
                            return;
                        case 4:
                            gameState = GameState.Jeopardy;
                            Console.Clear();
                            return;
                        case 5:
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write("The creators of the different game are as following:\n");
                            Console.ResetColor();
                            Console.Write("Chess made by: ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("Philip Nord Nielsen\n");
                            Console.ResetColor();
                            Console.Write("Mastermind made by: ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("Thomas Mortensen\n");
                            Console.ResetColor();
                            Console.Write("Battleship made by: ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("Lasse Handberg Gohlke\n");
                            Console.ResetColor();
                            Console.Write("Jeopardy made by: ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("Silas Megård Opstrup\n");
                            Console.ResetColor();
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        case 6:
                            appRunning = false;
                            Console.Clear();
                            return;
                    }
                    break;
                }
                else
                {
                    Log("\nInvalid number or Not a number", ConsoleColor.DarkRed);
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
        #endregion
        /// <summary>
        /// Contains the entire Chess game loop.
        /// </summary>
        /// <param name="gameState">Writes to gameState to change the state of the application.</param>
        private static void Chess(out GameState gameState)
        {
            byte[,] ChangePawn(byte[,] board, string input, byte[] PawnCoords, bool player, out string error)
            {
                byte[,] temp = CopyChessBoard(board);
                bool PawnChanged = true;
                error = "";
                switch (input)
                {
                    case "queen":
                        temp[PawnCoords[1], PawnCoords[0]] = player ?
                            (byte)ChessPieces.WhiteQueen : (byte)ChessPieces.BlackQueen;
                        break;
                    case "bishop":
                        temp[PawnCoords[1], PawnCoords[0]] = player ?
                            (byte)ChessPieces.WhiteBishop : (byte)ChessPieces.BlackBishop;
                        break;
                    case "Rook":
                        temp[PawnCoords[1], PawnCoords[0]] = player ?
                            (byte)ChessPieces.WhiteRook : (byte)ChessPieces.BlackRook;
                        break;
                    case "Knight":
                        temp[PawnCoords[1], PawnCoords[0]] = player ?
                            (byte)ChessPieces.WhiteKnight : (byte)ChessPieces.BlackKnight;
                        break;
                    default:
                        error = "Pawn can't change into that!";
                        PawnChanged = false;
                        break;
                }
                return PawnChanged ? temp : board;
            }
            /// <summary>
            /// Checks both ends of the board to see if a Pawn has moved to an edge.
            /// </summary>
            /// <param name="board">Takes a byte array that is the current state ofthe board.</param>
            /// <param name="player">Takes a bool value that determines if check white player or not.</param>
            /// <param name="PawnCoords">Outputs the coordinates of the Pawn to this variable.</param>
            bool PawnReachedOtherSide(byte[,] board, bool player, out byte[] PawnCoords)
            {
                bool reachedOtherSide = false;
                PawnCoords = new byte[2];
                ChessPieces currentPiece = ChessPieces.None;
                ChessPieces pieceToCheckFor = player ? ChessPieces.WhitePawn : ChessPieces.BlackPawn;
                byte[] coords = new byte[2];
                coords[1] = player ? (byte)0 : (byte)(board.GetLength(0) - 1);

                // loops through either white or black rows to check if a Pawn made it to the other side.
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    coords[0] = (byte)i;
                    currentPiece = GetBoardPiece(board, coords);
                    if (currentPiece == pieceToCheckFor)
                    {
                        reachedOtherSide = true;
                        PawnCoords[0] = coords[0];
                        PawnCoords[1] = coords[1];
                        break;
                    }
                }
                return reachedOtherSide;
            }
            /// <summary>
            /// Returns a new instance of a given board state.
            /// </summary>
            /// <param name="board">The board that should be copied.</param>
            byte[,] CopyChessBoard(byte[,] board)
            {
                int depth = board.Length / board.GetLength(0);
                int length = board.GetLength(0);
                byte[,] temp = new byte[depth, length];

                for (int i = 0; i < depth; i++)
                {
                    for (int j = 0; j < length; j++)
                    {
                        temp[i, j] = board[i, j];
                    }
                }

                return temp;
            }
            /// <summary>
            /// Checks to see if the spot on the board is empty.
            /// </summary>
            /// <param name="board">A multi byte array containing the current state of the chess board.</param>
            /// <param name="to">A byte array containing the coordinates that should be checked if the spot is empty.</param>
            bool BoardSpotIsEmpty(byte[,] board, byte[] to)
            {
                bool empty = false;
                int depth = board.GetLength(0);
                int width = board.GetLength(1);
                if (to[0] < depth && to[1] < width)
                {
                    empty = board[to[1], to[0]] == 0;
                }

                return empty;
            }
            /// <summary>
            /// Returns a chess piece according to the board state and the given coordinates.
            /// </summary>
            /// <param name="board">Takes a multi byte array containing the current board state.</param>
            /// <param name="coords">Takes a byte array that contains coordinates.</param>
            ChessPieces GetBoardPiece(byte[,] board, byte[] coords)
            {
                ChessPieces piece = ChessPieces.None;
                int depth = board.GetLength(0);
                int width = board.GetLength(1);
                if (coords[0] < depth && coords[1] < width)
                {
                    piece = (ChessPieces)board[coords[1], coords[0]];
                }
                return piece;
            }
            /// <summary>
            /// Checks if the current move made by the player and returns a bool in case the move is valid or not.
            /// </summary>
            /// <param name="board">The version of the board before the move is made.</param>
            /// <param name="to">A byte array containing the move the player wants to make.</param>
            /// <param name="from">A byte array containing where the player wants to move from.</param>
            /// <param name="chessPiece">What chess piece is being moved.</param>
            /// <param name="player">A bool conveying if the current move is made by the first player or not.</param>
            bool IsValidChessMove(byte[,] board, byte[] to, byte[] from, ChessPieces chessPiece, bool player)
            {
                bool isValidMove = false;
                byte piece = (byte)(player ? chessPiece : chessPiece - 6);
                byte[] possibleMove = new byte[2];
                bool opponentBlocking = false;
                bool ownPieceBlocking = false;
                bool possibleMoveIsMove = false;
                int[,] moveSet;
                int moves;

                switch ((ChessPieces)piece)
                {
                    case ChessPieces.WhitePawn:
                        if (player)
                        {
                            possibleMove[0] = (byte)(from[0]);
                            possibleMove[1] = (byte)(from[1] - 2);
                            if ((to[0] == possibleMove[0] && to[1] == possibleMove[1]) && from[1] == 6 && BoardSpotIsEmpty(board, possibleMove))
                            {
                                isValidMove = true;
                            }
                            possibleMove[1] = (byte)(from[1] - 1);
                            if ((to[0] == possibleMove[0] && to[1] == possibleMove[1]) && BoardSpotIsEmpty(board, possibleMove))
                            {
                                isValidMove = true;
                            }
                            possibleMove[0] = (byte)(from[0] - 1);
                            if ((to[0] == possibleMove[0] && to[1] == possibleMove[1]) && !BoardSpotIsEmpty(board, possibleMove))
                            {
                                isValidMove = true;
                            }
                            possibleMove[0] = (byte)(from[0] + 1);
                            if ((to[0] == possibleMove[0] && to[1] == possibleMove[1]) && !BoardSpotIsEmpty(board, possibleMove))
                            {
                                isValidMove = true;
                            }
                        }
                        else
                        {
                            possibleMove[0] = (byte)(from[0]);
                            possibleMove[1] = (byte)(from[1] + 2);
                            if ((to[0] == possibleMove[0] && to[1] == possibleMove[1]) && from[1] == 1 && BoardSpotIsEmpty(board, possibleMove))
                            {
                                isValidMove = true;
                            }
                            possibleMove[1] = (byte)(from[1] + 1);
                            if ((to[0] == possibleMove[0] && to[1] == possibleMove[1]) && BoardSpotIsEmpty(board, possibleMove))
                            {
                                isValidMove = true;
                            }
                            possibleMove[0] = (byte)(from[0] - 1);
                            if ((to[0] == possibleMove[0] && to[1] == possibleMove[1]) && !BoardSpotIsEmpty(board, possibleMove))
                            {
                                isValidMove = true;
                            }
                            possibleMove[0] = (byte)(from[0] + 1);
                            if ((to[0] == possibleMove[0] && to[1] == possibleMove[1]) && !BoardSpotIsEmpty(board, possibleMove))
                            {
                                isValidMove = true;
                            }
                        }
                        break;
                    case ChessPieces.WhiteKnight:
                        possibleMove = new byte[2];
                        moveSet = new int[8, 2]{
                            {1,-2},
                            {2, -1},
                            {2, 1},
                            {1, 2},
                            {-1, 2},
                            {-2, 1},
                            {-2, -1},
                            {-1, -2},
                        };
                        moves = moveSet.GetLength(0);
                        for (int i = 0; i < moves; i++)
                        {
                            possibleMove[1] = (byte)(from[1] + moveSet[i, 0]);
                            possibleMove[0] = (byte)(from[0] + moveSet[i, 1]);
                            if ((to[0] == possibleMove[0] && to[1] == possibleMove[1]))
                            {
                                isValidMove = true;
                            }
                        }
                        break;
                    case ChessPieces.WhiteKing:
                        possibleMove = new byte[2];
                        moveSet = new int[8, 2]{
                            {0,-1},
                            {1,-1},
                            {1, 0},
                            {1, 1},
                            {0, 1},
                            {-1, 1},
                            {-1, 0},
                            {-1, -1},
                        };
                        moves = moveSet.GetLength(0);
                        for (int i = 0; i < moves; i++)
                        {
                            possibleMove[1] = (byte)(from[1] + moveSet[i, 0]);
                            possibleMove[0] = (byte)(from[0] + moveSet[i, 1]);
                            if ((to[0] == possibleMove[0] && to[1] == possibleMove[1]))
                            {
                                isValidMove = true;
                            }
                        }

                        break;
                    case ChessPieces.WhiteRook:
                        moveSet = new int[4, 2]
                        {
                            {-1,0},
                            {1,0},
                            {0,-1},
                            {0,1},
                        };
                        moves = moveSet.GetLength(0);
                        for (int i = 0; i < moves; i++)
                        {
                            possibleMove[1] = (byte)(from[1] + moveSet[i, 0]);
                            possibleMove[0] = (byte)(from[0] + moveSet[i, 1]);
                            for (int j = 0; j < board.GetLength(0); j++)
                            {
                                if (j == 0)
                                {
                                    possibleMove[1] = (byte)(possibleMove[1] + moveSet[i, 0] * j);
                                    possibleMove[0] = (byte)(possibleMove[0] + moveSet[i, 1] * j);
                                }
                                else
                                {
                                    possibleMove[1] = (byte)(possibleMove[1] + moveSet[i, 0]);
                                    possibleMove[0] = (byte)(possibleMove[0] + moveSet[i, 1]);
                                }
                                opponentBlocking = player ?
                                    (!BoardSpotIsEmpty(board, possibleMove) && (byte)GetBoardPiece(board, possibleMove) >= (byte)ChessPieces.BlackKing) :
                                    (!BoardSpotIsEmpty(board, possibleMove) && (byte)GetBoardPiece(board, possibleMove) <= (byte)ChessPieces.WhitePawn && (byte)GetBoardPiece(board, possibleMove) > (byte)ChessPieces.None);
                                ownPieceBlocking = player ?
                                    (!BoardSpotIsEmpty(board, possibleMove) && (byte)GetBoardPiece(board, possibleMove) <= (byte)ChessPieces.WhitePawn && (byte)GetBoardPiece(board, possibleMove) > (byte)ChessPieces.None) :
                                    (!BoardSpotIsEmpty(board, possibleMove) && (byte)GetBoardPiece(board, possibleMove) >= (byte)ChessPieces.BlackKing);
                                possibleMoveIsMove = (to[0] == possibleMove[0] && to[1] == possibleMove[1]);

                                if (ownPieceBlocking) break;
                                if (opponentBlocking && !possibleMoveIsMove) break;
                                if (opponentBlocking && possibleMoveIsMove)
                                {
                                    isValidMove = true;
                                    break;
                                }
                                else if (possibleMoveIsMove)
                                {
                                    isValidMove = true;
                                    break;
                                }

                            }
                            if (isValidMove) break;
                        }
                        break;
                    case ChessPieces.WhiteBishop:
                        moveSet = new int[4, 2]
                        {
                            {-1,-1},
                            {1,-1},
                            {1,1},
                            {-1,1},
                        };
                        moves = moveSet.GetLength(0);
                        for (int i = 0; i < moves; i++)
                        {
                            possibleMove[1] = (byte)(from[1] + moveSet[i, 0]);
                            possibleMove[0] = (byte)(from[0] + moveSet[i, 1]);
                            for (int j = 0; j < board.GetLength(0); j++)
                            {
                                if (j == 0)
                                {
                                    possibleMove[1] = (byte)(possibleMove[1] + moveSet[i, 0] * j);
                                    possibleMove[0] = (byte)(possibleMove[0] + moveSet[i, 1] * j);
                                }
                                else
                                {
                                    possibleMove[1] = (byte)(possibleMove[1] + moveSet[i, 0]);
                                    possibleMove[0] = (byte)(possibleMove[0] + moveSet[i, 1]);
                                }
                                opponentBlocking = player ?
                                    (!BoardSpotIsEmpty(board, possibleMove) && (byte)GetBoardPiece(board, possibleMove) >= (byte)ChessPieces.BlackKing) :
                                    (!BoardSpotIsEmpty(board, possibleMove) && (byte)GetBoardPiece(board, possibleMove) <= (byte)ChessPieces.WhitePawn && (byte)GetBoardPiece(board, possibleMove) > (byte)ChessPieces.None);
                                ownPieceBlocking = player ?
                                    (!BoardSpotIsEmpty(board, possibleMove) && (byte)GetBoardPiece(board, possibleMove) <= (byte)ChessPieces.WhitePawn && (byte)GetBoardPiece(board, possibleMove) > (byte)ChessPieces.None) :
                                    (!BoardSpotIsEmpty(board, possibleMove) && (byte)GetBoardPiece(board, possibleMove) >= (byte)ChessPieces.BlackKing);
                                possibleMoveIsMove = (to[0] == possibleMove[0] && to[1] == possibleMove[1]);

                                if (ownPieceBlocking) break;
                                if (opponentBlocking && !possibleMoveIsMove) break;
                                if (opponentBlocking && possibleMoveIsMove)
                                {
                                    isValidMove = true;
                                    break;
                                }
                                else if (possibleMoveIsMove)
                                {
                                    isValidMove = true;
                                    break;
                                }

                            }
                            if (isValidMove) break;
                        }
                        break;
                    case ChessPieces.WhiteQueen:
                        moveSet = new int[8, 2]
                        {
                            {-1,0},
                            {1,0},
                            {0,-1},
                            {0,1},
                            {-1,-1},
                            {1,-1},
                            {1,1},
                            {-1,1},
                        };
                        moves = moveSet.GetLength(0);
                        for (int i = 0; i < moves; i++)
                        {
                            possibleMove[1] = (byte)(from[1] + moveSet[i, 0]);
                            possibleMove[0] = (byte)(from[0] + moveSet[i, 1]);
                            for (int j = 0; j < board.GetLength(0); j++)
                            {
                                if (j == 0)
                                {
                                    possibleMove[1] = (byte)(possibleMove[1] + moveSet[i, 0] * j);
                                    possibleMove[0] = (byte)(possibleMove[0] + moveSet[i, 1] * j);
                                }
                                else
                                {
                                    possibleMove[1] = (byte)(possibleMove[1] + moveSet[i, 0]);
                                    possibleMove[0] = (byte)(possibleMove[0] + moveSet[i, 1]);
                                }
                                opponentBlocking = player ?
                                    (!BoardSpotIsEmpty(board, possibleMove) && (byte)GetBoardPiece(board, possibleMove) >= (byte)ChessPieces.BlackKing) :
                                    (!BoardSpotIsEmpty(board, possibleMove) && (byte)GetBoardPiece(board, possibleMove) <= (byte)ChessPieces.WhitePawn && (byte)GetBoardPiece(board, possibleMove) > (byte)ChessPieces.None);
                                ownPieceBlocking = player ?
                                    (!BoardSpotIsEmpty(board, possibleMove) && (byte)GetBoardPiece(board, possibleMove) <= (byte)ChessPieces.WhitePawn && (byte)GetBoardPiece(board, possibleMove) > (byte)ChessPieces.None) :
                                    (!BoardSpotIsEmpty(board, possibleMove) && (byte)GetBoardPiece(board, possibleMove) >= (byte)ChessPieces.BlackKing);
                                possibleMoveIsMove = (to[0] == possibleMove[0] && to[1] == possibleMove[1]);

                                if (ownPieceBlocking) break;
                                if (opponentBlocking && !possibleMoveIsMove) break;
                                if (opponentBlocking && possibleMoveIsMove)
                                {
                                    isValidMove = true;
                                    break;
                                }
                                else if (possibleMoveIsMove)
                                {
                                    isValidMove = true;
                                    break;
                                }

                            }
                            if (isValidMove) break;
                        }
                        break;
                }

                return isValidMove;
            }
            /// <summary>
            /// Checks to see if the players move is valid and returns an updated board with the new version. 
            /// </summary>
            /// <param name="board">Takes current version of the board.</param>
            /// <param name="instructions">Takes instructions and throws an error if instructions arent formatted correctly.</param>
            /// <param name="playerTurn">Boolean to check if the turn is the first or second players.</param>
            /// <param name="error">Writes any error message to this parameter.</param>
            /// <returns></returns>
            byte[,] TryMovePiece(byte[,] board, string instructions, bool isPlayerTurn, out string error)
            {
                Regex inputCheckPattern = new Regex("^([A-H][1-8]-[A-H][1-8])+");
                string player = (isPlayerTurn) ? "White" : "Black";
                bool errorHappened = false;
                byte[,] temp = CopyChessBoard(board);
                error = "";
                string[] arr_instructions = instructions.ToUpper().Split('-');
                if (inputCheckPattern.IsMatch(instructions.ToUpper()))
                {
                    byte[] from = InstructionToCordinates(arr_instructions[0], out error);
                    byte[] to = InstructionToCordinates(arr_instructions[1], out error);
                    byte movingPiece = board[from[1], from[0]];
                    byte fieldToMove = board[to[1], to[0]];

                    switch (isPlayerTurn)
                    {
                        case true:
                            if (!(fieldToMove > 0 && fieldToMove <= 6))
                            {
                                temp[to[1], to[0]] = movingPiece;
                                temp[from[1], from[0]] = 0;
                            }
                            else
                            {
                                error = $"{player} can't capture their own pieces!";
                            }
                            if (!IsValidChessMove(board, to, from, (ChessPieces)movingPiece, isPlayerTurn))
                            {
                                error = "That isn't a valid move!";
                            }
                            if (!(movingPiece > 0 && movingPiece <= 6))
                            {
                                error = $"{player} can't move opponents pieces!";
                            }
                            break;
                        case false:
                            if (!(fieldToMove > 0 && fieldToMove >= 7))
                            {
                                temp[to[1], to[0]] = movingPiece;
                                temp[from[1], from[0]] = 0;
                            }
                            else
                            {
                                error = $"{player} can't capture their own pieces!";
                            }
                            if (!IsValidChessMove(board, to, from, (ChessPieces)movingPiece, isPlayerTurn))
                            {
                                error = "That isn't a valid move!";
                            }
                            if (!(movingPiece > 0 && movingPiece >= 7))
                            {
                                error = $"{player} can't move opponents pieces!";
                            }
                            break;
                    }
                }
                else
                {
                    error = "Input did not match instruction example!";
                }
                if (error != "")
                {
                    errorHappened = true;
                }

                return (!errorHappened) ? temp : board;
            }
            /// <summary>
            /// Converts string to chess piece values.
            /// </summary>
            /// <param name="setup">The current row of chess pieces to be converted to values</param>
            /// <param name="index">What piece in the string to convert.</param>
            /// <param name="isPlayerChessPiece">A bool value determining if its a white piece or not.</param>
            ChessPieces GetChessPiece(string setup, int index, bool isPlayerChessPieces)
            {
                ChessPieces value = ChessPieces.None;
                char piece = setup.ToUpper()[index];
                switch (piece)
                {
                    case 'R':
                        value = (isPlayerChessPieces) ? ChessPieces.WhiteRook : ChessPieces.BlackRook;
                        break;
                    case 'N':
                        value = (isPlayerChessPieces) ? ChessPieces.WhiteKnight : ChessPieces.BlackKnight;
                        break;
                    case 'B':
                        value = (isPlayerChessPieces) ? ChessPieces.WhiteBishop : ChessPieces.BlackBishop;
                        break;
                    case 'Q':
                        value = (isPlayerChessPieces) ? ChessPieces.WhiteQueen : ChessPieces.BlackQueen;
                        break;
                    case 'K':
                        value = (isPlayerChessPieces) ? ChessPieces.WhiteKing : ChessPieces.BlackKing;
                        break;
                    case 'P':
                        value = (isPlayerChessPieces) ? ChessPieces.WhitePawn : ChessPieces.BlackPawn;
                        break;
                    case ' ':
                        value = ChessPieces.None;
                        break;
                    default:
                        value = ChessPieces.None;
                        break;
                }

                return value;
            }
            /// <summary>
            /// Takes in the board variable and populates it with chess pieces acording to standard chess.
            /// Returns the populated multi byte array.
            /// </summary>
            /// <param name="board">The multi byte array you want to populate with chess pieces.</param>
            byte[,] PopulateBoard(byte[,] board)
            {
                int boardDepth = board.Length / board.GetLength(0);
                string setup1 = "RNBQKBNR";
                string setup2 = "PPPPPPPP";
                string setup3 = "        ";
                for (int depth = 0; depth < boardDepth; depth++)
                {
                    switch (depth)
                    {
                        case 0:
                            for (int i = 0; i < setup1.Length; i++)
                            {
                                board[depth, i] = (byte)GetChessPiece(setup1, i, false);
                            }
                            break;
                        case 1:
                            for (int i = 0; i < setup2.Length; i++)
                            {
                                board[depth, i] = (byte)GetChessPiece(setup2, i, false);
                            }
                            break;
                        case 6:
                            for (int i = 0; i < setup2.Length; i++)
                            {
                                board[depth, i] = (byte)GetChessPiece(setup2, i, true);
                            }
                            break;
                        case 7:
                            for (int i = 0; i < setup1.Length; i++)
                            {
                                board[depth, i] = (byte)GetChessPiece(setup1, i, true);
                            }
                            break;
                        default:
                            for (int i = 0; i < setup1.Length; i++)
                            {
                                board[depth, i] = (byte)GetChessPiece(setup3, i, false);
                            }
                            break;
                    }
                }
                return board;
            }
            /// <summary>
            /// Draws the entire chess board according to the given game state variables.
            /// </summary>
            /// <param name="board">Takes the current multi byte array containing chess data.</param>
            /// <param name="movesMade">Takes a int that gets displayed as moves made.</param>
            /// <param name="lastMove">Take a string that gets displayed as the last move played.</param>
            /// <param name="checkMated">Takes a boolean determining if the game is in a checkmate or not.</param>
            void DrawChessBoard(byte[,] board, int movesMade, string lastMove, bool checkMated)
            {
                void LogoPixel(string row, ConsoleColor color, ConsoleColor backColor)
                {
                    for (int i = 0; i<row.Length; i++)
                    {
                        switch (row[i])
                        {
                            case ' ':
                                BackColor(backColor);
                                Log("  ");
                                break;
                            default:
                                BackColor(color);
                                Log("  ");
                                break;
                        }
                    }
                }
                ConsoleColor logoColor = ConsoleColor.White;
                ConsoleColor logoBackColor = ConsoleColor.DarkBlue;
                string[] logo =
                {
                    " #### #  # #### #### #### ",
                    " #  # #  # #    #    #    ",
                    " #    #### ###  #### #### ",
                    " #  # #  # #       #    # ",
                    " #### #  # #### #### #### "
                };
                int boardDepth = board.Length / board.GetLength(0);
                ChessPieces character = ChessPieces.None;
                string characterAsci = " KQBRNP";
                string[] boardCords = {
                "ABCDEFGH",
                "87654321"
            };
                for (int i = 0; i < boardDepth; i++)
                {
                    for (int j = 0; j < boardDepth; j++)
                    {
                        if (i % 2 == 0)
                        {
                            if (j % 2 == 0)
                            {
                                BackColor(ConsoleColor.Black);
                            }
                            else
                            {
                                BackColor(ConsoleColor.White);
                            }
                        }
                        else
                        {
                            if (j % 2 != 0)
                            {
                                BackColor(ConsoleColor.Black);
                            }
                            else
                            {
                                BackColor(ConsoleColor.White);
                            }
                        }
                        character = (ChessPieces)board[i, j];
                        if ((byte)character > 0 && (byte)character > 6)
                        {
                            Color(ConsoleColor.Red);
                            Log(" " + characterAsci[(byte)character - 6]);
                        }
                        else if ((byte)character > 0 && (byte)character <= 6)
                        {
                            Color(ConsoleColor.Green);
                            Log(" " + characterAsci[(byte)character]);
                        }
                        else
                        {
                            Log("  ");
                        }
                        if (j == boardDepth - 1)
                        {
                            BackColor(ConsoleColor.Black);
                            Log(" " + boardCords[1][i], ConsoleColor.Blue);
                        }
                    }
                    switch (i)
                    {
                        case 0:
                            Log("   ");
                            Log("Moves: ", ConsoleColor.Blue);
                            Log(movesMade, ConsoleColor.White);
                            break;
                        case 1:
                            Log("   ");
                            Log("Last Move: ", ConsoleColor.Blue);
                            Log(lastMove, ConsoleColor.White);
                            break;
                        //case 2:
                        //    Log("   ");
                        //    Log("Checkmated: ", ConsoleColor.Blue);
                        //    if (checkMated)
                        //    {
                        //        Log("Check Mate!");
                        //    } else
                        //    {
                        //        Log("N/A");
                        //    }
                        //    break;
                        case 3:
                            Log("  ");
                            LogoPixel(logo[0], logoColor, logoBackColor);
                            break;
                        case 4:
                            Log("  ");
                            LogoPixel(logo[1], logoColor, logoBackColor);
                            break;
                        case 5:
                            Log("  ");
                            LogoPixel(logo[2], logoColor, logoBackColor);
                            break;
                        case 6:
                            Log("  ");
                            LogoPixel(logo[3], logoColor, logoBackColor);
                            break;
                        case 7:
                            Log("  ");
                            LogoPixel(logo[4], logoColor, logoBackColor);
                            break;
                    }
                    Log("\n");
                }
                BackColor(ConsoleColor.Black);
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    Log(" " + boardCords[0][i], ConsoleColor.Blue);
                }
                Log("", logoColor);
                Log("\n");
            }
            gameState = GameState.Chess;
            string errorMsg = "";
            int boardSize = 8;
            byte[,] gameBoard = new byte[boardSize, boardSize];
            string movesHistory = "";
            byte[] coordsOfPawnThatReachedOtherSide = new byte[2];
            bool inGame = true;
            bool playerTurn = true;
            bool checkMate = false;
            string playerInput = "";
            gameBoard = PopulateBoard(gameBoard);
            while (inGame)
            {
                Console.Clear();
                DrawChessBoard(gameBoard, movesHistory.Split(',').Length - 1, movesHistory.Split(',').Last().ToUpper(), checkMate);
                if (errorMsg != "") Log(errorMsg + "\n", ConsoleColor.Red);
                Log("You can write Restart or Quit to do each respectively.\n", ConsoleColor.Yellow);
                Log("It's ", ConsoleColor.Yellow);
                if (playerTurn)
                {
                    Log("Whites ", ConsoleColor.White);
                }
                else
                {
                    Log("Blacks ", ConsoleColor.Gray);
                }
                Log("turn.\n", ConsoleColor.Yellow);
                Log("What's your move [A1-H8]?: ", ConsoleColor.Yellow);
                playerInput = Console.ReadLine();
                if (playerInput != "")
                {
                    switch (playerInput.ToLower())
                    {
                        case "restart":
                            playerTurn = true;
                            movesHistory = "";
                            checkMate = false;
                            errorMsg = "";
                            gameBoard = PopulateBoard(gameBoard);
                            break;
                        case "quit":
                            errorMsg = "";
                            inGame = false;
                            gameState = GameState.MainMenu;
                            break;
                        default:
                            gameBoard = TryMovePiece(gameBoard, playerInput, playerTurn, out errorMsg);
                            if (errorMsg == "")
                            {
                                movesHistory += playerTurn ? ",[W]" : ",[B]";
                                movesHistory += playerInput;
                            }
                            while (PawnReachedOtherSide(gameBoard, playerTurn, out coordsOfPawnThatReachedOtherSide))
                            {
                                Console.Clear();
                                DrawChessBoard(gameBoard, movesHistory.Split(',').Length - 1, movesHistory.Split(',').Last().ToUpper(), checkMate);
                                if (errorMsg != "") Log(errorMsg + "\n", ConsoleColor.Red);
                                Log("Congrats ", ConsoleColor.Green);
                                if (playerTurn)
                                {
                                    Log("White", ConsoleColor.White);
                                }
                                else
                                {
                                    Log("Black", ConsoleColor.Gray);
                                }
                                Log(", you managed to get a Pawn to the other side!\n", ConsoleColor.Green);
                                Log("What piece do you want to change it to?\n", ConsoleColor.Yellow);
                                Log("Choice [Queen / Bishop / Rook / Knight]: ", ConsoleColor.Yellow);
                                playerInput = Console.ReadLine().ToLower();
                                gameBoard = ChangePawn(gameBoard, playerInput, coordsOfPawnThatReachedOtherSide, playerTurn, out errorMsg);
                            }
                            playerTurn = (errorMsg == "") ? !playerTurn : playerTurn;
                            break;
                    }
                }
                else
                {
                    errorMsg = "You need to input an instruction!";
                }
            }
        }
        private static void Mastermind(out GameState appState)
        {
            throw new NotImplementedException();
        }
        #endregion

        private static void Jeopardy(out GameState appState)
        {
            //Quiz arrays and variable initialisers
            #region _Variables
            int[,] pointsArray = new int[5,5]
            {
                {100, 100, 100, 100, 100},
                {200, 200, 200, 200, 200},
                {300, 300, 300, 300, 300},
                {400, 400, 400, 400, 400},
                {500, 500, 500, 500, 500}
            };

            string[,] questionsArray = new string[5,5]
            {
                {"The first name of Germany's dictator during World War 2.", "A flat, round Italian food that is generally served with cheese, tomato and a variety of other toppings.", "This band is popular for hits such as \"Mama Mia\" and \"Dancing Queen\".", "Tech A4", "A Chinese social media platform that is currently popular for sharing and creating short videos."},
                {"The last name of the man who directed the Manhattan Project. He is said to be the \"father of the atomic bomb\".", "This fruit is associated with the discovery of gravity when it fell and hit Isaac Newton in the head.", "Real name Tim Bergling, this Swedish DJ goes by this artist name.", "Tech B4", "A frog muppet who is commonly shown in a variety of internet memes."},
                {"The Declaration of Independence was signed during this year.", "The name of a celebrity chef who is famous for his aggressive behaviour towards other chefs which he deems bad at cooking.", "This musician is well known for his song that is commonly used to trick people online into listening to it.", "Tech C4", "Created in June 2012, this social media platform became popular for sharing and creating short, comedic videos."},
                {"This female aviation pioneer disappeared over the Pacific Ocean on her journey to become the first female pilot to circumnavigate the world.", "Beside sushi, this is one of Japan's most popular and globally known foods.", "This term refers to the fact that an unusual amount of musicians have all died at the same specific age.", "Tech D4", "This song and music video is known for being re-created both audibly and visually in unusual ways, often using tools that are not designed for it."},
                {"This peace contract written by \"The big three\" was signed in June 1919 and played a key role in ending World War 1.", "The popular fast-food, French fries, originates from this country.", "First and last name of the original lead singer of Linkin Park.", "Tech E4", "This videogame is commonly referred to in the question \"But can it run ...\" due to its high hardware requirements for its time."}
            };

            string[,] answersArray =
            {
                {"Adolf", "Pizza", "ABBA", "Tech A4", "TikTok"},
                {"Oppenheimer", "Apple", "Avicii", "Tech B4", "Pepe"},
                {"1776", "Gordon Ramsay", "Rick Astley", "Tech C4", "Vine"},
                {"Amelia Earhart", "Ramen", "27 Club", "Tech D4", "Bad Apple"},
                {"Treaty of Versaille", "Belgium", "Chester Bennington", "Tech E4", "Crysis"}
            };

            string[] categories = {"History", "Food", "Music", "Tech", "Memes"};
            string[] playerNames = { };
            int[] playerScores = { };
            int currentPlayer = 0;
            int intChoiceX = 0;
            int intChoiceY = 0;
            int currentGuesses;
            string answer;
            bool quit = false;
            #endregion

            //Start game
            #region _StartGame
            appState = GameState.Jeopardy;
            AsciiScroll();
            int playerCount = PlayerCount(out appState);
            Array.Resize<string>(ref playerNames, playerCount);
            Array.Resize<int>(ref playerScores, playerCount);
            AsciiScroll();
            for (int x = 0; x < playerScores.GetLength(0); x++)
            {
                playerScores[x] = 0;
            }
            if (quit == false)
            {
                PlayerNames();
            }
            Console.Clear();
            if (quit == false)
            {
                WriteBoard();
            }
            #endregion

            //Game end conditions
            #region _End
            Console.WriteLine("Game ended. Returning to main menu.");
            Console.ReadKey();
            Console.Clear();
            appState = GameState.MainMenu;
            return;
            #endregion


            

            //--------------------------------------------------FUNCTIONS--------------------------------------------------
            #region PlayerCount
            int PlayerCount(out GameState state)
            {
                state = GameState.Jeopardy;
                Console.Write("Player count: ");
                int value2;
                while (quit == false)
                {
                    string value1 = Console.ReadLine();
                    if (value1 == "/quit")
                    {
                        quit = true;
                        break;
                    }
                    if (int.TryParse(value1, out value2))
                    {
                        if (value2 < 1 || value2 > 10)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Out of range! Max 10 players.");
                            Console.ResetColor();
                            Console.ReadKey();
                            Console.Clear();
                            AsciiScroll();
                            Console.Write("Player count: ");
                        }
                        else
                        {
                            Console.Clear();
                            return value2;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: Input is not valid a number. Please try again.", true);
                        Console.ResetColor();
                        Console.ReadKey();
                        Console.Clear();
                        AsciiScroll();
                        Console.Write("Player count: ");
                    }
                }
                return 0;
            }
            #endregion

            #region PlayerNames
            void PlayerNames()
            {
                for (int i = 0; i < playerNames.Length; i++)
                {
                    Console.Write($"Player {i + 1}'s name: ");
                    playerNames[i] = (Console.ReadLine());
                    if (playerNames[i] == "/quit")
                    {
                        quit = true;
                        break;
                    }
                    Console.Clear();
                    AsciiScroll();
                }
                Console.WriteLine("Confirm players? Type yes/no to confirm or redo player names.");
                if (quit == false)
                {
                    string redoAnswer = Console.ReadLine();
                    if (redoAnswer == "/quit")
                    {
                        quit = true;
                    }
                    while ((redoAnswer != "yes") && (redoAnswer != "no") && (redoAnswer != "") && (quit == false))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input.");
                        Console.ResetColor();
                        Console.ReadKey();
                        Console.Clear();
                        AsciiScroll();
                        Console.WriteLine("Confirm players? Type yes/no to confirm or redo player names.");
                        redoAnswer = Console.ReadLine();
                    }
                    if (redoAnswer == "no")
                    {
                        Console.Clear();
                        AsciiScroll();
                        PlayerNames();
                    }
                    else if (quit == false)
                    {
                        Console.Clear();
                        AsciiScroll();
                        Console.WriteLine("Great, lets play! Press any key to continue.");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }
            #endregion

            //Sets up the board of questions, displays player scores and calls SelectQuestion.
            #region WriteBoard
            void WriteBoard(bool proceed = true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Topics:   History, Food, Music,");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("        _______________________X____________________");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("              Tech, Memes");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("             |     HIS 1   FD  2   MUS 3   TCH 4   ME  5");
                Console.WriteLine("                                      |    +-----+ +-----+ +-----+ +-----+ +-----+");
                for (int x = 0; x < questionsArray.GetLength(0); x++)
                {
                    if (x == 2)
                    {
                        Console.Write("                                      Y    ");
                    }
                    else
                    {
                        Console.Write("                                      |    ");
                    }
                    for (int y = 0; y < questionsArray.GetLength(1); y++)
                    {
                        if (pointsArray[x, y] != 0)
                        {
                            Console.Write($"| ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(pointsArray[x, y]);
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write(" | ");
                        }
                        else
                        {
                            Console.Write($"|     | ");
                        }
                    }
                    Console.WriteLine();
                    Console.WriteLine("                                      |    +-----+ +-----+ +-----+ +-----+ +-----+");
                }
                Console.ResetColor();
                Console.WriteLine();
                for (int x = 0; x < playerNames.GetLength(0); x++)
                {
                    if (currentPlayer == x)
                    {
                        Console.WriteLine($"-> {playerNames[x]}: {playerScores[x]} points");
                    }
                    else
                    {
                        Console.WriteLine($"   {playerNames[x]}: {playerScores[x]} points");
                    }
                }
                Console.WriteLine("                                            Which question do you want to choose?   ");
                if (proceed == true)
                {
                    SelectQuestion();
                }
            }
            #endregion

            //Asks the player for input and points to the corresponding question on the board.
            //Writes an error if the input is not a number, or if it is out of array range.
            //Writes another error if the selected question is equal to zero on the pointsArray.
            //Calls WriteQuestion on succesful selection.
            #region SelectQuestion
            void SelectQuestion()
            {
                //int checkValue;
                string stringChoiceX;
                string stringChoiceY;
                Console.WriteLine();
                Console.Write("                                            Category: ");
                while (quit == false)
                {
                    stringChoiceX = (Console.ReadLine());
                    if (stringChoiceX == "/quit")
                    {
                        quit = true;
                        break;
                    }
                    if (int.TryParse(stringChoiceX, out intChoiceX))
                    {
                        if (intChoiceX >= 1 && intChoiceX <= pointsArray.GetLength(0))
                        {
                            intChoiceX--;
                            break;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"                                    Not within range. Please select between 1 and {pointsArray.GetLength(0)}.");
                            Console.ResetColor();
                            Console.ReadKey();
                            Console.Clear();
                            WriteBoard(false);
                            Console.WriteLine();
                            Console.Write("                                            Category: ");
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("                                  Error: Input is not valid a number. Please try again.");
                        Console.WriteLine("                 Note: Category is selected by entering the category's corresponding number. Fx '2' for Food.");
                        Console.ResetColor();
                        Console.ReadKey();
                        Console.Clear();
                        WriteBoard(false);
                        Console.WriteLine();
                        Console.Write("                                            Category: ");
                    }
                }

                Console.Clear();
                WriteBoard(false);
                Console.WriteLine();
                Console.WriteLine($"                                            Category: {categories[intChoiceX]}");
                Console.Write("                                            Points: ");

                while (quit == false)
                {
                    stringChoiceY = (Console.ReadLine());
                    if (stringChoiceY == "/quit")
                    {
                        quit = true;
                        break;
                    }
                    if (int.TryParse(stringChoiceY, out intChoiceY))
                    {
                        //if (intChoiceY >= 1 && intChoiceY <= (pointsArray.Length / pointsArray.GetLength(0)))
                        if (intChoiceY == 100 || intChoiceY == 200 || intChoiceY == 300 || intChoiceY == 400 || intChoiceY == 500)
                        {
                            intChoiceY--;
                            break;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("                                 Not within range. Please select 100, 200, 300, 400 or 500.");
                            Console.ResetColor();
                            Console.ReadKey();
                            Console.Clear();
                            WriteBoard(false);
                            Console.WriteLine();
                            Console.WriteLine($"                                            Category: {categories[intChoiceX]}");
                            Console.Write("                                            Points: ");
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("                                  Error: Input is not valid a number. Please try again.");
                        Console.ResetColor();
                        Console.ReadKey();
                        Console.Clear();
                        WriteBoard(false);
                        Console.WriteLine();
                        Console.WriteLine($"                                            Category: {categories[intChoiceX]}");
                        Console.Write("                                            Points: ");
                    }
                }
                intChoiceY /= 100;
                if ((pointsArray[intChoiceY, intChoiceX] == 0) && (quit == false))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("                            That question is already answered! Please choose a new one.");
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.Clear();
                    WriteBoard();
                }
                currentGuesses = 3;
                Console.Clear();
                if (quit == false)
                {
                    WriteQuestion();
                }
            }
            #endregion

            //Writes the selected question, displays the number of guesses left and calls ReadAnswer.
            #region WriteQuestion
            void WriteQuestion()
            {
                //Console.WriteLine(questionsArray[intChoiceY, intChoiceX]);
                QuestionFormat(questionsArray[intChoiceY, intChoiceX], pointsArray[intChoiceY, intChoiceX]);
                
                Console.WriteLine();
                if (currentGuesses != 0)
                {
                    Console.WriteLine($"You have {currentGuesses} guesses left.");
                }
                ReadAnswer();
                
            }
            #endregion

            //Reads the players answer and checks if it is equal to the string in the answers array.
            //Deducts guesses on incorrect answer until none are left, and gives points to active player on correct answer.
            //Calls WriteBoard on correct answer or when out of guesses.
            //Sets value of current question to 0 in pointsArray, marking it as unavailable.
            #region ReadAnswer
            void ReadAnswer()
            {
                while (quit == false)
                {
                    if (currentGuesses != 0)
                    {
                        Console.Write("Answer: ");
                        answer = Console.ReadLine().ToLower();
                        if (answer == "/quit")
                        {
                            quit = true;
                            break;
                        }
                        if (answer.Contains(answersArray[intChoiceY, intChoiceX].ToLower()))
                        {
                            Console.WriteLine();
                            Console.WriteLine($"Correct! The answer was {answersArray[intChoiceY, intChoiceX]}.");
                            Console.WriteLine($"Awarded {pointsArray[intChoiceY, intChoiceX]} points to {playerNames[currentPlayer]}.");
                            playerScores[currentPlayer] += pointsArray[intChoiceY, intChoiceX];
                            pointsArray[intChoiceY, intChoiceX] = 0;
                            if (currentPlayer < (playerNames.GetLength(0) - 1))
                            {
                                currentPlayer++;
                            }
                            else
                            {
                                currentPlayer = 0;
                            }
                            Console.ReadKey();
                            Console.Clear();
                            WriteBoard();
                        }
                        else
                        {
                            Console.WriteLine("Wrong answer!");
                            currentGuesses--;
                            if (currentGuesses == 0)
                            {
                                pointsArray[intChoiceY, intChoiceX] = 0;
                            }
                            Console.ReadKey();
                            Console.Clear();
                            WriteQuestion();
                        }
                    }
                    else
                    {
                        Console.WriteLine("You have run out of guesses. Press to continue.");
                        if (currentPlayer < (playerNames.GetLength(0) - 1))
                        {
                            currentPlayer++;
                        }
                        else
                        {
                            currentPlayer = 0;
                        }
                        Console.ReadKey();
                        Console.Clear();
                        WriteBoard();
                    }
                }
            }
            #endregion

            #region Ascii Scroll
            void AsciiScroll()
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Recommended playing in full screen.");
                Console.WriteLine("You may type '/quit' at any time during the game to return to the main menu.");
                Console.ResetColor();
                Console.WriteLine("   __________________________________");
                Console.WriteLine(" / \\                                 \\.");
                Console.WriteLine("|   |      Welcome to Jeopardy!      |.");
                Console.WriteLine(" \\_/| Enter a classic quiz game of   |.");
                Console.WriteLine("    | Jeopardy, answering a range of |.");
                Console.WriteLine("    |  questions posed as answers.   |.");
                Console.WriteLine("    |                                |.");
                Console.WriteLine("    | How many are playing? Max 10.  |.");
                Console.WriteLine("    |                                |.");

                for (int x = 0; x < playerNames.Length; x++)
                {
                    if (playerNames[x] == null)
                    {
                        Console.Write("    | - ");
                        Console.WriteLine("                             |.");
                    }
                    else
                    {
                        Console.Write("    | - ");
                        Console.Write(playerNames[x]);
                        for (int y = 0; y < 29 - playerNames[x].Length; y++)
                        {
                            Console.Write(" ");
                        }
                        Console.WriteLine("|.");
                    }
                }


                Console.WriteLine("    |                                |.");
                Console.WriteLine("    |   _____________________________|___");
                Console.WriteLine("    |  /      Made by Silas Opstrup     /.");
                Console.WriteLine("    \\_/________________________________/.");
                Console.WriteLine();
            }

            #endregion

            #region QuestionFormat
            void QuestionFormat(string x, int y)
            {
                Console.Write(" ____");
                for (int i = 0; i < x.Length; i++)
                {
                    Console.Write("_");
                }
                Console.WriteLine("____ ");

                Console.Write("|  __");
                for (int i = 0; i < x.Length; i++)
                {
                    Console.Write("_");
                }
                Console.WriteLine("__  |");

                Console.Write("| |  ");
                for (int i = 0; i < x.Length; i++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine("  | |");


                Console.WriteLine($"| |  {x}  | |");
                Console.Write($"| |     - {y} points");
                for (int i = 0; i < x.Length-15; i++)
                {
                    Console.Write(" ");
                }
                if (y == 0)
                {
                    Console.Write("  ");
                }
                Console.WriteLine("  | |");


                Console.Write("| |__");
                for (int i = 0; i < x.Length; i++)
                {
                    Console.Write("_");
                }
                Console.WriteLine("__| |");

                Console.Write("|____");
                for (int i = 0; i < x.Length; i++)
                {
                    Console.Write("_");
                }
                Console.WriteLine("____|");
            }
            #endregion
        }
        #region BattleShipGame
        /// <summary>
        /// Battleships game programmed by Lasse Handberg Gohlke. (Player vs. Computer)
        /// 
        /// </summary>
        /// <param name="appState"></param>
        private static void SinkAShip(out GameState appState) // Made by Lasse Handberg Gohlke
        {
            appState = GameState.SinkAShip;
            Random rnd = new Random();
            int[,] playerField = new int[10, 10] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } }; ;
            int[,] enemyField = new int[10, 10] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } }; ;
            int[,] attackField = new int[10, 10] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } }; ;
            bool gameEnd = false;
            bool gameReady = false;
            bool playerAlive = false;
            bool enemyAlive = false;
            GenerateBSEnemyField(enemyField);
            ConsoleColor userColor = ConsoleColor.Green;
            Color(ConsoleColor.DarkBlue);
            Console.WriteLine("    ____          __   __   __             __     _      \r\n   / __ ) ____ _ / /_ / /_ / /___   _____ / /_   (_)____ \r\n  / __  |/ __ `// __// __// // _ \\ / ___// __ \\ / // __ \\\r\n / /_/ // /_/ // /_ / /_ / //  __/(__  )/ / / // // /_/ /\r\n/_____/ \\__,_/ \\__/ \\__//_/ \\___//____//_/ /_//_// .___/ \r\n                                                /_/      ");
            Console.ResetColor();
            Console.Write("Please enter your name?: ");
            string userName = Console.ReadLine().Trim();
            #region IntroductionLoop
            while (gameReady == false) // Start loop to set up game first time
            {
                Console.Write("Welcome ");
                Color(userColor);
                Console.Write($"{userName}");
                Console.ResetColor();
                Console.WriteLine("! Ready to play Battleship?");
                Console.Write("Please write YES or NO, if you want to play Battleship?: ");
                string answer = Console.ReadLine().Trim().ToUpper();
                if (answer == "YES")
                {
                    Console.WriteLine("Well... Let's start the game! But first...");
                redo:
                    Console.Write("Do you wanna place your OWN ships or allow the COMPUTER?: ");
                    answer = Console.ReadLine().Trim().ToUpper();
                    if (answer == "OWN")
                    {
                        Console.WriteLine("Well let's get to it!");
                        Console.ReadKey();
                        Console.Clear();
                        UserFieldGeneration(playerField);
                        gameReady = true;
                    }
                    else if (answer == "COMPUTER")
                    {
                        Console.Clear();
                        Color(ConsoleColor.DarkRed);
                        Console.WriteLine("COMPUTER GENERATING PLAYERFIELD!!!!\n");
                        Console.ReadKey();
                        GenerateBSEnemyField(playerField);
                        Console.WriteLine("FINAL SETUP!");
                        Console.ResetColor();
                        ShowBSField(playerField);
                        Console.ReadKey();
                        Console.Clear();
                        gameReady = true;
                    }
                    else
                    {
                        Color(ConsoleColor.Red);
                        Console.WriteLine("Didn't understand?");
                        Console.ReadKey();
                        Console.ResetColor();
                        Console.Clear();
                        goto redo;
                    }
                }
                else if (answer == "NO")
                {
                    appState = GameState.MainMenu;
                    Color(ConsoleColor.Red);
                    Console.WriteLine("Returning to Main menu!");
                    Console.ReadKey();
                    Console.ResetColor();
                    Console.Clear();
                    return;
                }
                else
                {
                    Color(ConsoleColor.Red);
                    Console.WriteLine("Didn't understand?");
                    Console.ReadKey();
                    Console.ResetColor();
                    Console.Clear();
                }
            }
            #endregion
            #region Random Initiativ
            int playerInitiativ = rnd.Next(1, 3); // Randomly made initiativ
            if (playerInitiativ == 1) 
            {
                Console.ForegroundColor = userColor;
                Console.Write(userName);
                Console.ResetColor();
                Console.Write(" starts!");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Computer");
                Console.ResetColor();
                Console.Write(" starts!");
                Console.ReadKey();
                Console.Clear();
            }
            #endregion
            #region Main Loop
            while (gameEnd == false) // Main Loop for game
            {
                if (playerInitiativ == 1)
                {
                    BSPlayerFire(enemyField, attackField);
                    enemyAlive = IsPlayerAlive(enemyField);
                    if (enemyAlive)
                    {
                        ShowBSField(attackField);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("\nEnemy");
                        Console.ResetColor();
                        Console.Write(" is stil alive! Game continues!");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else
                    {
                    redo:
                        ShowBSField(playerField);
                        Console.ForegroundColor = userColor;
                        Console.WriteLine();
                        Console.Write(userName);
                        Console.ResetColor();
                        Console.Write(" has defeated The Computer! Congratulations!\nDo you wanna PLAY again or do you wanna EXIT?: ");
                        string userInput = Console.ReadLine().Trim().ToUpper();
                        if (userInput == "PLAY")
                        {
                            Console.WriteLine("Resetting the playing field!");
                            for (int x = 0; x < enemyField.GetLength(0); x++) // Resets all the field arrays!
                            {
                                for (int y = 0; y < enemyField.GetLength(1); y++)
                                {
                                    enemyField[x, y] = 0;
                                    playerField[x, y] = 0;
                                    attackField[x, y] = 0;
                                }
                            }
                        redo2:
                            Console.Write("Do you wanna place your OWN ships or allow the COMPUTER?: ");
                            string answer = Console.ReadLine().Trim().ToUpper();
                            if (answer == "OWN")
                            {
                                Console.WriteLine("Well let's get to it!");
                                Console.ReadKey();
                                Console.Clear();
                                UserFieldGeneration(playerField);
                            }
                            else if (answer == "COMPUTER")
                            {
                                Console.Clear();
                                Color(ConsoleColor.DarkRed);
                                Console.WriteLine("COMPUTER GENERATING PLAYERFIELD!!!!\n");
                                GenerateBSEnemyField(playerField);
                                Console.WriteLine("FINAL SETUP!");
                                Console.ResetColor();
                                ShowBSField(playerField);
                                Console.ReadKey();
                                Console.Clear();
                            }
                            else
                            {
                                Color(ConsoleColor.Red);
                                Console.WriteLine("Didn't understand?");
                                Console.ReadKey();
                                Console.ResetColor();
                                Console.Clear();
                                goto redo2;
                            }
                            playerInitiativ = rnd.Next(1, 3);
                            if (playerInitiativ == 1)
                            {
                                Console.ForegroundColor = userColor;
                                Console.Write(userName);
                                Console.ResetColor();
                                Console.Write(" starts next game!\n");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("Computer");
                                Console.ResetColor();
                                Console.Write(" starts next game!\n");
                            }
                        }
                        else if (userInput == "EXIT")
                        {
                            gameEnd = true;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Returning to main menu!");
                            Console.ReadKey();
                            Console.Clear();
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Didn't understand! Try Again!");
                            Console.ResetColor();
                            Console.ReadKey();
                            Console.Clear();
                            goto redo;
                        }
                    }
                    BSEnemyFire(playerField);
                    playerAlive = IsPlayerAlive(playerField);
                    if (playerAlive)
                    {
                        ShowBSField(playerField);
                        Console.ForegroundColor = userColor;
                        Console.WriteLine();
                        Console.Write(userName);
                        Console.ResetColor();
                        Console.Write(" is still alive! Game continues!");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else
                    {
                    redo:
                        ShowBSField(playerField);
                        Console.WriteLine();
                        Console.ForegroundColor = userColor;
                        Console.Write(userName);
                        Console.ResetColor();
                        Console.Write(" has been defeated! Better luck next time!\nDo you wanna PLAY again or do you wanna EXIT?: ");
                        string userInput = Console.ReadLine().Trim().ToUpper();
                        if (userInput == "PLAY")
                        {
                            Console.WriteLine("Resetting the playing field!");
                            for (int x = 0; x < enemyField.GetLength(0); x++) // Resets all the field arrays!
                            {
                                for (int y = 0; y < enemyField.GetLength(1); y++)
                                {
                                    enemyField[x, y] = 0;
                                    playerField[x, y] = 0;
                                    attackField[x, y] = 0;
                                }
                            }
                            GenerateBSEnemyField(enemyField);
                        redo2:
                            Console.Write("Do you wanna place your OWN ships or allow the COMPUTER?: ");
                            string answer = Console.ReadLine().Trim().ToUpper();
                            if (answer == "OWN")
                            {
                                Console.WriteLine("Well let's get to it!");
                                Console.ReadKey();
                                Console.Clear();
                                UserFieldGeneration(playerField);
                            }
                            else if (answer == "COMPUTER")
                            {
                                Console.Clear();
                                Color(ConsoleColor.DarkRed);
                                Console.WriteLine("COMPUTER GENERATING PLAYERFIELD!!!!\n");
                                GenerateBSEnemyField(playerField);
                                Console.WriteLine("FINAL SETUP!");
                                Console.ResetColor();
                                ShowBSField(playerField);
                                Console.ReadKey();
                                Console.Clear();
                            }
                            else
                            {
                                Color(ConsoleColor.Red);
                                Console.WriteLine("Didn't understand?");
                                Console.ReadKey();
                                Console.ResetColor();
                                Console.Clear();
                                goto redo2;
                            }
                            playerInitiativ = rnd.Next(1, 3);
                            if (playerInitiativ == 1)
                            {
                                Console.ForegroundColor = userColor;
                                Console.Write(userName);
                                Console.ResetColor();
                                Console.Write(" starts next game!\n");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("Computer");
                                Console.ResetColor();
                                Console.Write(" starts next game!\n");
                            }
                        }
                        else if (userInput == "EXIT")
                        {
                            gameEnd = true;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Returning to main menu!");
                            Console.ReadKey();
                            Console.Clear();
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Didn't understand! Try Again!");
                            Console.ResetColor();
                            Console.ReadKey();
                            Console.Clear();
                            goto redo;
                        }
                    }
                }
                else
                {
                    BSEnemyFire(playerField);
                    playerAlive = IsPlayerAlive(playerField);
                    if (playerAlive)
                    {
                        ShowBSField(playerField);
                        Console.WriteLine();
                        Console.ForegroundColor = userColor;
                        Console.Write(userName);
                        Console.ResetColor();
                        Console.Write(" is still alive! Game continues!");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else
                    {
                    redo:
                        ShowBSField(playerField);
                        Console.WriteLine();
                        Console.ForegroundColor = userColor;
                        Console.Write(userName);
                        Console.ResetColor();
                        Console.Write(" has been defeated! Better luck next time!\nDo you wanna PLAY again or do you wanna EXIT?: ");
                        string userInput = Console.ReadLine().Trim().ToUpper();
                        if (userInput == "PLAY")
                        {
                            Console.WriteLine("Resetting the playing field!");
                            for (int x = 0; x < enemyField.GetLength(0); x++) // Resets all the field arrays!
                            {
                                for (int y = 0; y < enemyField.GetLength(1); y++)
                                {
                                    enemyField[x, y] = 0;
                                    playerField[x, y] = 0;
                                    attackField[x, y] = 0;
                                }
                            }
                            GenerateBSEnemyField(enemyField);
                        redo2:
                            Console.Write("Do you wanna place your OWN ships or allow the COMPUTER?: ");
                            string answer = Console.ReadLine().Trim().ToUpper();
                            if (answer == "OWN")
                            {
                                Console.WriteLine("Well let's get to it!");
                                Console.ReadKey();
                                Console.Clear();
                                UserFieldGeneration(playerField);
                            }
                            else if (answer == "COMPUTER")
                            {
                                Console.Clear();
                                Color(ConsoleColor.DarkRed);
                                Console.WriteLine("COMPUTER GENERATING PLAYERFIELD!!!!\n");
                                GenerateBSEnemyField(playerField);
                                Console.WriteLine("FINAL SETUP!");
                                Console.ResetColor();
                                ShowBSField(playerField);
                                Console.ReadKey();
                                Console.Clear();
                            }
                            else
                            {
                                Color(ConsoleColor.Red);
                                Console.WriteLine("Didn't understand?");
                                Console.ReadKey();
                                Console.ResetColor();
                                Console.Clear();
                                goto redo2;
                            }
                            playerInitiativ = rnd.Next(1, 3);
                            if (playerInitiativ == 1)
                            {
                                Console.ForegroundColor = userColor;
                                Console.Write(userName);
                                Console.ResetColor();
                                Console.Write(" starts next game!\n");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("Computer");
                                Console.ResetColor();
                                Console.Write(" starts next game!\n");
                            }
                        }
                        else if (userInput == "EXIT")
                        {
                            gameEnd = true;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Returning to main menu!");
                            Console.ReadKey();
                            Console.Clear();
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Didn't understand! Try Again!");
                            Console.ResetColor();
                            Console.ReadKey();
                            Console.Clear();
                            goto redo;
                        }
                    }
                    BSPlayerFire(enemyField, attackField);
                    enemyAlive = IsPlayerAlive(enemyField);
                    if (enemyAlive)
                    {
                        ShowBSField(attackField);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("\nEnemy");
                        Console.ResetColor();
                        Console.Write(" is stil alive! Game continues!");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else
                    {
                    redo:
                        ShowBSField(playerField);
                        Console.WriteLine();
                        Console.ForegroundColor = userColor;
                        Console.Write(userName);
                        Console.ResetColor();
                        Console.Write(" has defeated The Computer! Congratulations!\nDo you wanna PLAY again or do you wanna EXIT?: ");
                        string userInput = Console.ReadLine().Trim().ToUpper();
                        if (userInput == "PLAY")
                        {
                            Console.WriteLine("Resetting the playing field!");
                            for (int x = 0; x < enemyField.GetLength(0); x++) // Resets all the field arrays!
                            {
                                for (int y = 0; y < enemyField.GetLength(1); y++)
                                {
                                    enemyField[x, y] = 0;
                                    playerField[x, y] = 0;
                                    attackField[x, y] = 0;
                                }
                            }
                            GenerateBSEnemyField(enemyField);
                        redo2:
                            Console.Write("Do you wanna place your OWN ships or allow the COMPUTER?: ");
                            string answer = Console.ReadLine().Trim().ToUpper();
                            if (answer == "OWN")
                            {
                                Console.WriteLine("Well let's get to it!");
                                Console.ReadKey();
                                Console.Clear();
                                UserFieldGeneration(playerField);
                            }
                            else if (answer == "COMPUTER")
                            {
                                Color(ConsoleColor.DarkRed);
                                Console.WriteLine("COMPUTER GENERATING PLAYERFIELD!!!!\n");
                                GenerateBSEnemyField(playerField);
                                Console.WriteLine("FINAL SETUP!");
                                Console.ResetColor();
                                ShowBSField(playerField);
                                Console.ReadKey();
                                Console.Clear();
                            }
                            else
                            {
                                Color(ConsoleColor.Red);
                                Console.WriteLine("Didn't understand?");
                                Console.ReadKey();
                                Console.ResetColor();
                                Console.Clear();
                                goto redo2;
                            }
                            playerInitiativ = rnd.Next(1, 3);
                            if (playerInitiativ == 1)
                            {
                                Console.ForegroundColor = userColor;
                                Console.Write(userName);
                                Console.ResetColor();
                                Console.Write("Player starts next game!\n");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("Computer");
                                Console.ResetColor();
                                Console.Write(" starts next game!");
                            }
                        }
                        else if (userInput == "EXIT")
                        {
                            gameEnd = true;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Returning to main menu!");
                            Console.ReadKey();
                            Console.Clear();
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Didn't understand! Try Again!");
                            Console.ResetColor();
                            Console.ReadKey();
                            Console.Clear();
                            goto redo;
                        }
                    }
                }
                if (enemyAlive && playerAlive)
                {
                redo:
                    Console.Write("Both sides are still alive. Do you wanna continue playing?\nWrite ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("PLAY");
                    Console.ResetColor();
                    Console.Write(" or press enter without writing anything to continue playing! Write ");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("EXIT");
                    Console.ResetColor();
                    Console.Write(" to end the game: ");
                    string userInput = Console.ReadLine().Trim().ToUpper();
                    if (userInput == "PLAY" || userInput == "")
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("The Game continues!");
                        playerAlive = false;
                        enemyAlive = false;
                        Console.ReadKey();
                        Console.Clear();
                        Console.ResetColor();
                    }
                    else if (userInput == "EXIT")
                    {
                        gameEnd = true;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Returning to main menu!");
                        Console.ReadKey();
                        Console.Clear();
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Didn't understand! Try Again!");
                        Console.ResetColor();
                        Console.ReadKey();
                        Console.Clear();
                        goto redo;
                    }
                }
            }
            #endregion
            appState = GameState.MainMenu;
        }
        #endregion
    }
}


//Editing changes: Added regions outside of Jeopardy function for easier reading.
//Editing changes: Set game-state to always be jeopardy for easier testing.