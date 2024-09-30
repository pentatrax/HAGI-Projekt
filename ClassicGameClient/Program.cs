using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
                {"Question A1", "Question A2", "Question A3", "Question A4", "Question A5"},
                {"Question B1", "Question B2", "Question B3", "Question B4", "Question B5"},
                {"Question C1", "Question C2", "Question C3", "Question C4", "Question C5"},
                {"Question D1", "Question D2", "Question D3", "Question D4", "Question D5"},
                {"Question E1", "Question E2", "Question E3", "Question E4", "Question E5"}
            };

            string[,] answersArray =
            {
                {"Answer A1", "Answer A2", "Answer A3", "Answer A4", "Answer A5"},
                {"Answer B1", "Answer B2", "Answer B3", "Answer B4", "Answer B5"},
                {"Answer C1", "Answer C2", "Answer C3", "Answer C4", "Answer C5"},
                {"Answer D1", "Answer D2", "Answer D3", "Answer D4", "Answer D5"},
                {"Answer E1", "Answer E2", "Answer E3", "Answer E4", "Answer E5"}
            };

            string[] playerNames = { };
            int[] playerScores = { };
            int currentPlayer = 0;
            int intChoiceX = 0;
            int intChoiceY = 0;
            int currentGuesses;
            string answer;
            #endregion

            //Start game
            #region _StartGame
            appState = GameState.Jeopardy;
            AsciiScroll();
            int playerCount = PlayerCount();
            Array.Resize<string>(ref playerNames, playerCount);
            Array.Resize<int>(ref playerScores, playerCount);
            AsciiScroll();
            for (int x = 0; x < playerScores.GetLength(0); x++)
            {
                playerScores[x] = 0;
            }
            PlayerNames();
            #endregion

            //Starts the main function cycle
            WriteBoard();

            //Game end conditions
            #region _End
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("APPLICATION END: PRESS ANY KEY");
            Console.ReadKey();
            Environment.Exit(0);
            #endregion




            //--------------------------------------------------FUNCTIONS--------------------------------------------------
            #region PlayerCount
            int PlayerCount()
            {
                Console.Write("Player count: ");
                int value2;
                while (true)
                {
                    string value1 = Console.ReadLine();
                    if (int.TryParse(value1, out value2))
                    {
                        if (value2 < 1 || value2 > 5)
                        {
                            Console.WriteLine("Out of range!");
                        }
                        else
                        {
                            Console.Clear();
                            return value2;
                        }
                    }
                    else
                    { 
                        Console.WriteLine("Error: Input is not valid a number. Please try again.", true);
                        Console.ReadKey();
                        Console.Clear();
                        AsciiScroll();
                        Console.Write("Player count: ");
                    }
                }
            }
            #endregion

            #region PlayerNames
            void PlayerNames()
            {
                for (int i = 0; i < playerNames.Length; i++)
                {
                    Console.Write($"Player {i + 1}'s name: ");
                    playerNames[i] = (Console.ReadLine());
                    Console.Clear();
                    AsciiScroll();
                }
                Console.WriteLine("Confirm players? Type yes/no to confirm or redo player names.");
                string redoAnswer = Console.ReadLine();
                while ((redoAnswer != "yes") && (redoAnswer != "no") && (redoAnswer != ""))
                {
                    Console.WriteLine("Invalid input.");
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
                else
                {
                    Console.Clear();
                    AsciiScroll();
                    Console.WriteLine("Great, lets play! Press any key to continue.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            #endregion

            //Sets up the board of questions, displays player scores and calls SelectQuestion.
            #region WriteBoard
            void WriteBoard(bool proceed = true)
            {
                Console.WriteLine("   _____________X_______________");
                for (int x = 0; x < questionsArray.GetLength(0); x++)
                {
                    if (x == 2)
                    {
                        Console.Write(" Y ");
                    }
                    else
                    {
                        Console.Write(" | ");
                    }
                    for (int y = 0; y < questionsArray.GetLength(1); y++)
                    {
                        if (pointsArray[x, y] != 0)
                        {
                            Console.Write($"{pointsArray[x, y]}   ");
                        }
                        else
                        {
                            Console.Write($" {pointsArray[x, y]}    ");
                        }
                    }
                    Console.WriteLine();
                    Console.WriteLine(" | ");
                }
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
                Console.WriteLine("Which question do you want to choose?   ");
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
                Console.Write("X axis: ");
                while (true)
                {
                    stringChoiceX = (Console.ReadLine());
                    if (int.TryParse(stringChoiceX, out intChoiceX))
                    {
                        if (intChoiceX >= 1 && intChoiceX <= pointsArray.GetLength(0))
                        {
                            intChoiceX--;
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"Not within range. Please select between 1 and {pointsArray.GetLength(0)}.");
                            Console.ReadKey();
                            Console.Clear();
                            WriteBoard(false);
                            Console.WriteLine();
                            Console.Write("X axis: ");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Input is not valid a number. Please try again.");
                        Console.ReadKey();
                        Console.Clear();
                        WriteBoard(false);
                        Console.WriteLine();
                        Console.Write("X axis: ");
                    }
                }
                Console.Write("Y axis: ");
                while (true)
                {
                    stringChoiceY = (Console.ReadLine());
                    if (int.TryParse(stringChoiceY, out intChoiceY))
                    {
                        if (intChoiceY >= 1 && intChoiceY <= (pointsArray.Length / pointsArray.GetLength(0)))
                        {
                            intChoiceY--;
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"Not within range. Please select between 1 and {pointsArray.Length / pointsArray.GetLength(0)}.");
                            Console.ReadKey();
                            Console.Clear();
                            WriteBoard(false);
                            Console.WriteLine();
                            Console.WriteLine($"X axis: {intChoiceX + 1}");
                            Console.Write("Y axis: ");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Input is not valid a number. Please try again.");
                        Console.ReadKey();
                        Console.Clear();
                        WriteBoard(false);
                        Console.WriteLine();
                        Console.WriteLine($"X axis: {intChoiceX + 1}");
                        Console.Write("Y axis: ");
                    }
                }
                if (pointsArray[intChoiceY, intChoiceX] == 0)
                {
                    Console.WriteLine("That question is already answered! Please choose a new one.");
                    Console.ReadKey();
                    Console.Clear();
                    WriteBoard();
                }
                currentGuesses = 3;
                Console.Clear();
                WriteQuestion();
            }
            #endregion

            //Writes the selected question, displays the number of guesses left and calls ReadAnswer.
            #region WriteQuestion
            void WriteQuestion()
            {
                Console.WriteLine(questionsArray[intChoiceY, intChoiceX]);
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
                while (true)
                {
                    if (currentGuesses != 0)
                    {
                        Console.Write("Answer: ");
                        answer = Console.ReadLine().ToLower();
                        if (answer == answersArray[intChoiceY, intChoiceX].ToLower())
                        {
                            Console.WriteLine($"Correct! Awarded {pointsArray[intChoiceY, intChoiceX]} points to {playerNames[currentPlayer]}.");
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
                        WriteBoard();
                    }
                }
            }
            #endregion

            #region Ascii Scroll
            void AsciiScroll()
            {
                Console.WriteLine("   __________________________________");
                Console.WriteLine(" / \\                                 \\.");
                Console.WriteLine("|   |      Welcome to Jeopardy!      |.");
                Console.WriteLine(" \\_/| Enter a classic quiz game of   |.");
                Console.WriteLine("    | Jeopardy, answering a range of |.");
                Console.WriteLine("    |  questions posed as answers.   |.");
                Console.WriteLine("    |                                |.");
                Console.WriteLine("    | How many are playing?  Max 5.  |.");
                Console.WriteLine("    |                                |.");

                for (int x = 0; x < playerNames.Length; x++)
                {
                    if (playerNames[x] == null)
                    {
                        Console.Write("    | - ");
                    }
                    else
                    {
                        Console.Write("    | - ");
                        Console.Write(playerNames[x]);
                    }

                    //if (writeNames == true)
                    //{
                    //    Console.Write(playerNames[x]);
                    //    //int repeatCount = playerNames[x].Length;


                    //    for (int y = 0; y < playerNames[x].Length; y++)
                    //    {
                    //        Console.WriteLine(" -");
                    //    }
                    //    //Console.WriteLine();
                    //}
                    //else
                    //{
                    Console.WriteLine("                             |.");
                    //}
                }


                Console.WriteLine("    |                                |.");
                Console.WriteLine("    |   _____________________________|___");
                Console.WriteLine("    |  /      Made by Silas Opstrup     /.");
                Console.WriteLine("    \\_/________________________________/.");
                Console.WriteLine();
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