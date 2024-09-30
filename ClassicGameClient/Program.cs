using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClassicGameClient
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
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
        }
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
            WhiteTower = 4,
            WhiteHorse = 5,
            WhiteRook = 6,
            BlackKing = 7,
            BlackQueen = 8,
            BlackBishop = 9,
            BlackTower = 10,
            BlackHorse = 11,
            BlackRook = 12,
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
        private enum BattleshipDirection
        {
            Up = 1,
            Down = 2,
            Left = 3,
            Right = 4
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
                    Console.WriteLine($"Splash! Enemy hit water at {x + 1},{y + 1}!");
                    shotChecked = true;
                    Console.ResetColor();
                }
                else
                {
                    string hitShip = Enum.GetName(typeof(BattleshipLogistics), playerField[x, y]);
                    playerField[x, y] = 6;
                    ShowBSField(playerField);
                    Color(ConsoleColor.Red);
                    Console.WriteLine($"BOOM! Enenmy has hit a/an {hitShip} at {x + 1},{y + 1}!");
                    shotChecked = true;
                    Console.ResetColor();
                }
            }

        }
        private static void BSPlayerFire(int[,] enemyField, int[,] attackField)
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
                    Console.Write("Where on the x axis, do you wanna hit?: (1 -> 10)");
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
                    Console.Write("Where on the Y axis, do you wanna hit? (1 -> 10): ");
                    intYChecker = int.TryParse(Console.ReadLine().Trim(), out y);
                    if (intYChecker == true && y < 11 && y > 0)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("");
                    }
                }
                y -= 1;
                x -= 1;
                if (enemyField[x, y] == (int)BattleshipLogistics.Water)
                {
                    checkedShot = true;
                    Color(ConsoleColor.Blue);
                    Console.WriteLine("SPLASH! You hit the water!");
                    enemyField[x, y] = 6;
                    attackField[x, y] = 6;
                    Console.ReadKey();
                    Console.ResetColor();
                    Console.Clear();
                    break;
                }
                else if (enemyField[x, y] == (int)BattleshipLogistics.Bombed)
                {
                    Console.WriteLine("The place you chose has already been hit... try again!");
                    intXChecker = false;
                    intYChecker = false;
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                else
                {
                    checkedShot = true;
                    Color(ConsoleColor.Red);
                    Console.WriteLine($"BOOM! You hit a/an {Enum.GetName(typeof(BattleshipLogistics), enemyField[x, y])}!");
                    enemyField[x, y] = 6;
                    attackField[x, y] = 6;
                    Console.ReadKey();
                    Console.ResetColor();
                    Console.Clear();
                    break;
                }
            }
        }
        private static void ShowBSField(int[,] field)
        {
            for (int x = 0; x < field.GetLength(0); x++)
            {
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
                }
                Console.Write($" {x + 1} \n");
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
                }
                Console.Write("\n");
            }
            Console.WriteLine("");
            Console.WriteLine("1   2   3   4   5   6   7   8   9   10");
        }
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
                                if (x - 1 > 0 && enemyField[x - 1, y] == 0)
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
                                if (x + 1 < 9 && enemyField[x + 1, y] == 0)
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
                                if (y - 1 > 0 && enemyField[x, y - 1] == 0)
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
                                if (y + 1 < 9 && enemyField[x, y + 1] == 0)
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
                    if (enemyField[x,y] == 0)
                    {
                        int direction = rnd.Next(1, 4);
                        switch (direction)
                        {
                            case 1:
                                if (x - 1 > 0 && enemyField[x-1,y] == 0)
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
                                if (x + 1 < 9 && enemyField[x + 1, y] == 0)
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
                                if (y - 1 > 0 && enemyField[x, y - 1] == 0)
                                {
                                    enemyField[x, y] = 1;
                                    enemyField[x , y-1] = 1;
                                    placementCheck = true;
                                    break;
                                }
                                else
                                {
                                    goto retry;
                                }
                            case 4:
                                if (y + 1 < 9 && enemyField[x, y + 1] == 0)
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
                                if (x - 2 > 0 && enemyField[x - 1, y] == 0 && enemyField[x-2, y] == 0)
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
                                if (x + 2 < 9 && enemyField[x + 1, y] == 0 && enemyField[x + 2, y] == 0)
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
                                if (y - 2 > 0 && enemyField[x, y - 1] == 0 && enemyField[x, y - 2] == 0)
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
                                if (y + 2 < 9 && enemyField[x, y + 1] == 0 && enemyField[x, y + 2] == 0)
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
                                if (x - 2 > 0 && enemyField[x - 1, y] == 0 && enemyField[x - 2, y] == 0)
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
                                if (x + 2 < 9 && enemyField[x + 1, y] == 0 && enemyField[x + 2, y] == 0)
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
                                if (y - 2 > 0 && enemyField[x, y - 1] == 0 && enemyField[x, y - 2] == 0)
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
                                if (y + 2 < 9 && enemyField[x, y + 1] == 0 && enemyField[x, y + 2] == 0)
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
                                if (x - 3 > 0 && enemyField[x - 1, y] == 0 && enemyField[x - 2, y] == 0 && enemyField[x - 3, y] == 0)
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
                                if (x + 3 < 9 && enemyField[x + 1, y] == 0 && enemyField[x + 2, y] == 0 && enemyField[x + 3, y] == 0)
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
                                if (y - 3 > 0 && enemyField[x, y - 1] == 0 && enemyField[x, y - 2] == 0 && enemyField[x, y - 3] == 0)
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
                                if (y + 3 < 9 && enemyField[x, y + 1] == 0 && enemyField[x, y + 2] == 0 && enemyField[x, y - 3] == 0)
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
                                if (x - 4 > 0 && enemyField[x - 1, y] == 0 && enemyField[x - 2, y] == 0 && enemyField[x - 3, y] == 0 && enemyField[x - 4, y] == 0)
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
                                if (x + 4 < 9 && enemyField[x + 1, y] == 0 && enemyField[x + 2, y] == 0 && enemyField[x + 3, y] == 0 && enemyField[x + 4, y] == 0)
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
                                if (y - 4 > 0 && enemyField[x, y - 1] == 0 && enemyField[x, y - 2] == 0 && enemyField[x, y - 3] == 0 && enemyField[x, y - 4] == 0)
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
                                if (y + 4 < 9 && enemyField[x, y + 1] == 0 && enemyField[x, y + 2] == 0 && enemyField[x, y - 3] == 0 && enemyField[x, y - 4] == 0)
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
        private static void UserFieldGeneration(int[,] playerField)
        {

        }
        #endregion
        private static byte[] InstructionToCordinates(string instruction, out string error)
        {
            byte[] cords = { 50, 50 };
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
        private static byte[,] TryMovePiece(byte[,] board, string instructions, out string error)
        {
            Regex inputCheckPattern = new Regex("^([A-H][1-8]-[A-H][1-8])+");
            bool errorHappened = false;
            byte[,] temp = board;
            error = "";
            string[] arr_instructions = instructions.ToUpper().Split('-');
            if (inputCheckPattern.IsMatch(instructions.ToUpper()) && instructions.Length <= 5)
            {
                byte[] from = InstructionToCordinates(arr_instructions[0], out error);
                byte[] to = InstructionToCordinates(arr_instructions[1], out error);

                byte moovingPiece = board[from[1], from[0]];
                byte fieldToMove = board[to[1], to[0]];

                temp[to[1], to[0]] = moovingPiece;
                temp[from[1], from[0]] = 0;
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
        private static void MainMenu(out GameState gameState, out bool appRunning)
        {
            Random rnd = new Random();
            appRunning = true;
            gameState = GameState.MainMenu;
            int[,] test = new int[10, 10] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };
            GenerateBSEnemyField(test);
            ShowBSField(test);
            for (int i = 0; i < 10; i++)
            {
                BSEnemyFire(test);
            }
            ShowBSField(test);
            Log("Hi friens ^^.");
            Console.ReadKey();

        }
        private static void Chess(out GameState gameState)
        {
            ChessPieces GetChessPiece(string setup, int index, bool isPlayerChessPieces)
            {
                ChessPieces value = ChessPieces.None;
                char piece = setup.ToUpper()[index];
                switch (piece)
                {
                    case 'T':
                        value = (isPlayerChessPieces) ? ChessPieces.WhiteTower : ChessPieces.BlackTower;
                        break;
                    case 'H':
                        value = (isPlayerChessPieces) ? ChessPieces.WhiteHorse : ChessPieces.BlackHorse;
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
                    case 'R':
                        value = (isPlayerChessPieces) ? ChessPieces.WhiteRook : ChessPieces.BlackRook;
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
            byte[,] PopulateBoard(byte[,] board)
            {
                int boardDepth = board.Length / board.GetLength(0);
                string setup1 = "THBQKBHT";
                string setup2 = "RRRRRRRR";
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
            void DrawChessBoard(byte[,] board)
            {
                int boardDepth = board.Length / board.GetLength(0);
                ChessPieces character = ChessPieces.None;
                string characterAsci = " KQBTHRKQBTHR";
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
                            Log(" " + characterAsci[(byte)character]);
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
                    Log("\n");
                }
                BackColor(ConsoleColor.Black);
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    Log(" " + boardCords[0][i], ConsoleColor.Blue);
                }
                Log("\n");
            }
            gameState = GameState.Chess;
            string errorMsg = "";
            int boardSize = 8;
            byte[,] gameBoard = new byte[boardSize, boardSize];
            bool inGame = false;
            string playerInput = "";
                while (inGame)
                {
                    Console.Clear();
                    DrawChessBoard(gameBoard);
                    if (errorMsg != "") Log(errorMsg + "\n", ConsoleColor.Red);
                    Log("You can write Restart or Quit to do each respectively.\n", ConsoleColor.Yellow);
                    Log("What's your move [A2-B3]?: ", ConsoleColor.Yellow);
                    playerInput = Console.ReadLine();
                    if (playerInput != "")
                    {
                        switch (playerInput.ToLower())
                        {
                            case "restart":
                                errorMsg = "";
                                gameBoard = PopulateBoard(gameBoard);
                                break;
                            case "quit":
                                errorMsg = "";
                                inGame = false;
                                break;
                            default:
                                gameBoard = TryMovePiece(gameBoard, playerInput, out errorMsg);
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

        private static void Jeopardy(out GameState appState)
        {
            throw new NotImplementedException();
        }
        #region BattleShipGame
        private static void SinkAShip(out GameState appState) // Made by Lasse Handberg Gohlke
        {
            appState = GameState.Chess;
            Console.WriteLine();
            int[,] playerField = new int[10, 10] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } }; ;
            int[,] enemyField = new int[10, 10] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } }; ;
            int[,] attackField = new int[10, 10] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } }; ;
            bool gameEnd = false;
            bool playerAlive = false;
            bool enemyAlive = false;
            GenerateBSEnemyField(enemyField);
            ConsoleColor userColor = ConsoleColor.Green;
            Console.Write("Please enter your name?:");
            string userName = Console.ReadLine().Trim();
            while (true)
            {
                Console.Write("Welcome ");
                Color(userColor);
                Console.Write($"{userName}");
                Console.ResetColor();
                Console.WriteLine("! Ready to play Battleship?");
                Console.Write("Please write YES or NO, if you want to play Battleship? ");
                string answer = Console.ReadLine().Trim().ToUpper();
                if (answer == "YES")
                {
                    Console.WriteLine("Well... Let's start the game!");
                    Console.ReadKey();
                    Console.Clear();
                    break;
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
            while (gameEnd == false)
            {
                foreach (int field in enemyField)
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
                foreach (int field in playerField)
                {
                    if (field > 0 && field < 6)
                    {
                        playerAlive = true;
                    }
                    else
                    {
                        continue;
                    }
                }
                if (playerAlive == false)
                {

                }
                else if (enemyAlive == true)
                {

                }
                else
                {
                    Console.WriteLine("Both sides are still alive. The battle continues!");
                    playerAlive = false;
                    enemyAlive = false;
                    Console.ReadKey();
                }
            }
            appState = GameState.MainMenu;
        }
        #endregion
    }
}
