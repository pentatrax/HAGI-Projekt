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
        private static void MainMenu(out GameState gameState, out bool appRunning)
        {
            appRunning = true;
            gameState = GameState.MainMenu;
            Log("Hi friens ^^.");
            Console.ReadKey();
            gameState = GameState.Chess;
        }
        /// <summary>
        /// Contains the entire Chess game loop.
        /// </summary>
        /// <param name="gameState">Writes to gameState to go back to menu.</param>
        private static void Chess(out GameState gameState)
        {
            byte[,] CopyChessBoard(byte[,] board)
            {
                int depth = board.Length / board.GetLength(0);
                int length = board.GetLength(0);
                byte[,] temp = new byte[depth,length];

                for (int i = 0; i < depth; i++)
                {
                    for (int j = 0; j < length; j++)
                    {
                        temp[i, j] = board[i, j];
                    }
                }

                return temp;
            }
            bool IsValidChessMove(byte[,] board, byte[] to, byte[] from, ChessPieces chessPiece, bool player)
            {
                bool isValidMove = false;
                byte piece = (byte)(player ? chessPiece : chessPiece - 6);
                byte[] possibleMove = new byte[2];
                bool fieldIsEmpty = false;

                switch ((ChessPieces)piece)
                {
                    case ChessPieces.WhiteRook:
                        if (player) {
                            possibleMove[0] = (byte)(from[0]);
                            possibleMove[1] = (byte)(from[1] - 2);
                            fieldIsEmpty = board[possibleMove[1], possibleMove[0]] == 0;
                            if ((to[0] == possibleMove[0] && to[1] == possibleMove[1]) && from[1] == 6 && fieldIsEmpty)
                            {
                                isValidMove = true;
                            }
                            possibleMove[1] = (byte)(from[1] - 1);
                            fieldIsEmpty = board[possibleMove[1], possibleMove[0]] == 0;
                            if ((to[0] == possibleMove[0] && to[1] == possibleMove[1]) && fieldIsEmpty)
                            {
                                isValidMove = true;
                            }
                            possibleMove[0] = (byte)(from[0] -1);
                            fieldIsEmpty = board[possibleMove[1], possibleMove[0]] == 0;
                            if ((to[0] == possibleMove[0] && to[1] == possibleMove[1]) && !fieldIsEmpty)
                            {
                                isValidMove = true;
                            }
                            possibleMove[0] = (byte)(from[0] + 1);
                            fieldIsEmpty = board[possibleMove[1], possibleMove[0]] == 0;
                            if ((to[0] == possibleMove[0] && to[1] == possibleMove[1]) && !fieldIsEmpty)
                            {
                                isValidMove = true;
                            }
                        } else
                        {
                            possibleMove[0] = (byte)(from[0]);
                            possibleMove[1] = (byte)(from[1] + 2);
                            fieldIsEmpty = board[possibleMove[1], possibleMove[0]] == 0;
                            if ((to[0] == possibleMove[0] && to[1] == possibleMove[1]) && from[1] == 1 && fieldIsEmpty)
                            {
                                isValidMove = true;
                            }
                            possibleMove[1] = (byte)(from[1] + 1);
                            fieldIsEmpty = board[possibleMove[1], possibleMove[0]] == 0;
                            if ((to[0] == possibleMove[0] && to[1] == possibleMove[1]) && fieldIsEmpty)
                            {
                                isValidMove = true;
                            }
                            possibleMove[0] = (byte)(from[0] - 1);
                            fieldIsEmpty = board[possibleMove[1], possibleMove[0]] == 0;
                            if ((to[0] == possibleMove[0] && to[1] == possibleMove[1]) && !fieldIsEmpty)
                            {
                                isValidMove = true;
                            }
                            possibleMove[0] = (byte)(from[0] + 1);
                            fieldIsEmpty = board[possibleMove[1], possibleMove[0]] == 0;
                            if ((to[0] == possibleMove[0] && to[1] == possibleMove[1]) && !fieldIsEmpty)
                            {
                                isValidMove = true;
                            }
                        }
                        break;
                    case ChessPieces.WhiteHorse:
                        possibleMove = new byte[2];
                        fieldIsEmpty = false;
                        int[,] moveSet = {
                            {1,-2},
                            {2, -1},
                            {2, 1},
                            {1, 2},
                            {-1, 2},
                            {-2, 1},
                            {-2, -1},
                            {-1, -2},
                        };
                        int moves = moveSet.GetLength(0);
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
                        break;
                    case ChessPieces.WhiteTower:
                        break;
                    case ChessPieces.WhiteBishop:
                        break;
                    case ChessPieces.WhiteQueen:
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
                            if (!(movingPiece > 0 && movingPiece <= 6))
                            {
                                error = $"{player} can't move opponents pieces!";
                            }
                            if (!IsValidChessMove(board, to, from, (ChessPieces)movingPiece, isPlayerTurn))
                            {
                                error = "That isn't a valid move!";
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
                            if (!(movingPiece > 0 && movingPiece >= 7))
                            {
                                error = $"{player} can't move opponents pieces!";
                            }
                            if (!IsValidChessMove(board, to, from, (ChessPieces)movingPiece, isPlayerTurn))
                            {
                                error = "That isn't a valid move!";
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
                string characterAsci = " KQBTHR";
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
                            Log(" " + characterAsci[(byte)character-6]);
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
            bool inGame = true;
            bool playerTurn = true;
            string playerInput = "";
            gameBoard = PopulateBoard(gameBoard);
            while (inGame)
            {
                Console.Clear();
                DrawChessBoard(gameBoard);
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
                            gameState = GameState.MainMenu;
                            break;
                        default:
                            gameBoard = TryMovePiece(gameBoard, playerInput, playerTurn, out errorMsg);
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

        private static void Jeopardy(out GameState appState)
        {
            throw new NotImplementedException();
        }

        private static void SinkAShip(out GameState appState)
        {
            throw new NotImplementedException();
        }
    }
}
