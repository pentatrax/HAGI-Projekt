﻿using System;
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