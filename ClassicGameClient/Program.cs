using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClassicGameClient
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            #region Initiator
            bool appRunning = true;
            GameState appState = GameState.Jeopardy;
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
        #endregion
        #region MainMenu
        private static void MainMenu(out GameState gameState, out bool appRunning)
        {
            appRunning = true;
            gameState = GameState.MainMenu;
            Log("Hi friens ^^.");
            Console.ReadKey();

        }
        #endregion
        #region Chess
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
        #endregion
        #region Mastermind
        private static void Mastermind(out GameState appState)
        {
            throw new NotImplementedException();
        }
        #endregion

        private static void Jeopardy(out GameState appState)
        {
            //Quiz arrays

            //Start game
            appState = GameState.Jeopardy;
            Console.WriteLine("How many players/teams are playing?");
            int playerCount = PlayerCount();
            Console.WriteLine("What are your names?");
            string[] playerNames = { };
            Array.Resize<string>(ref playerNames, playerCount);
            PlayerNames();

            //Quiz board setup


            //Game end conditions
            Console.WriteLine("APPLICATION END: PRESS ANY KEY");
            Console.ReadKey();
            Environment.Exit(0);
            #region PlayerCount
            int PlayerCount()
            {
                int value2;
                while (true)
                {
                    string value1 = Console.ReadLine();
                    if (int.TryParse(value1, out value2))
                    {
                        // Input was a valid integer
                        return value2;
                    }
                    else
                    {
                        // Input was not a valid integer 
                        Console.WriteLine("Error: Input is not valid a number. Please try again.", true);
                    }
                }
            }
            #endregion

            #region PlayerNames
            void PlayerNames()
            {
                for (int i = 0; i < playerNames.Length; i++)
                {
                    Console.WriteLine($"Player {i + 1}:");
                    playerNames[i] = (Console.ReadLine());
                }
                Console.WriteLine();
                Console.Write("All players: ");
                for (int i = 0; i < playerNames.Length; i++)
                {
                    Console.Write($"{playerNames[i]} ");
                }

                Console.WriteLine();
                Console.WriteLine("Confirm players? Type yes/no to confirm or redo player names.");
                string redoAnswer = Console.ReadLine();
                while ((redoAnswer != "yes") && (redoAnswer != "no"))
                {
                    Console.WriteLine("Invalid input.");
                    redoAnswer = Console.ReadLine();
                }
                if (redoAnswer == "no")
                {
                    PlayerNames();
                }
                else
                {
                    Console.WriteLine("Great, lets play! Press any key to continue.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            #endregion
        }
        #region Battleship
        private static void SinkAShip(out GameState appState)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}


//Editing changes: Added regions outside of Jeopardy function for easier reading.
//Editing changes: Set game-state to always be jeopardy for easier testing.
//Editing changes: Added jeopardy game function