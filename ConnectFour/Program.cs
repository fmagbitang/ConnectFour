﻿using System;

namespace ConnectFour
{
    enum GameMode
    {
        TwoPlayer,
        PlayerVsComputer
    }

    class Player
    {
        public string Name { get; }
        public string ID { get; }

        public Player(string name, string id)
        {
            Name = name;
            ID = id;
        }
    }

    class ConnectFourGame
    {
        private const int Rows = 6;
        private const int Columns = 7;
        private int[,] board;
        private Player[] _players;
        private int currentPlayerIndex;
        private GameMode gameMode;

        public ConnectFourGame()
        {
            board = new int[Rows, Columns];
            _players = new Player[2];
            currentPlayerIndex = 0;
            gameMode = GameMode.TwoPlayer;
        }

        public void StartGame()
        {
            Console.WriteLine("Welcome to Connect Four!");
            Console.WriteLine();

            for (int i = 0; i < _players.Length; i++)
            {
                Console.Write($"Enter Player {i + 1}'s name: ");
                string name = Console.ReadLine();
                string id = (i == 0) ? "X" : "O";
                _players[i] = new Player(name, id);
            }

            Console.WriteLine();
            Console.WriteLine("Choose a game mode:");
            Console.WriteLine();
            Console.WriteLine("1. 2-Player Mode");
            Console.WriteLine("2. User vs Computer Mode");
            Console.WriteLine();
            Console.Write("Enter your choice (1 or 2): ");
            string modeChoice = Console.ReadLine();

            if (modeChoice == "2")
            {
                gameMode = GameMode.PlayerVsComputer;
                Console.WriteLine("You have chosen Player vs Computer Mode.");
            }
            else
            {
                Console.WriteLine("You have chosen 2-Player Mode.");
            }

            bool is_ended = false;

            while (!is_ended)
            {
                Console.Clear();
                GameBoard();

                if (gameMode == GameMode.TwoPlayer || currentPlayerIndex == 0)
                {
                    Console.WriteLine($"Player {_players[currentPlayerIndex].Name}'s turn.");
                    Console.WriteLine("Enter number from ( 1 - 7 ) or \'r\' to restart or \'0\' to quit the game: ");
                    string input = Console.ReadLine();

                    if (input.ToLower() == "r")
                    {
                        // reset the game again.
                        ResetGame();
                        continue;
                    }

                    int quitOption = 0;
                    if (int.TryParse(input, out quitOption) && (quitOption == 0 || quitOption == 9))
                    {
                        bool validInput = false;
                        while (!validInput)
                        {
                            Console.WriteLine("Do you want to quit the game? (9 = Yes, 0 = No): ");
                            string restartInput = Console.ReadLine();

                            if (int.TryParse(restartInput, out quitOption) && (quitOption == 0 || quitOption == 9))
                                validInput = true;
                            else
                                Console.WriteLine("Invalid input. Please enter 9 for Yes or 0 for No.");
                        }

                        if (quitOption == 9)
                        {
                            // the game is quit by the player.
                            is_ended = true;
                            break;
                        }
                        else
                        {
                            // continue playing.
                            continue;
                        }
                    }

                    if (int.TryParse(input, out int column) && column >= 1 && column <= Columns)
                    {
                        // Subtract 1 to convert to zero-based index
                        column--;

                        if (PlayMove(column))
                        {
                            // restart the game and continue to play.
                            Console.Clear();
                            GameBoard();
                            Console.WriteLine($"Player {_players[currentPlayerIndex].Name} wins!");

                            if (AskForRestart())
                            {
                                // restart the game and continue to play.
                                ResetGame();
                                continue;
                            }
                            else
                            {
                                // end the game.
                                is_ended = true;
                                break;
                            }
                        }
                        else if (IsBoardFull())
                        {
                            // board is now full. The game will end.
                            Console.Clear();
                            GameBoard();
                            Console.WriteLine("It's a draw!");

                            if (AskForRestart())
                            {
                                // restart the game and continue to play.
                                ResetGame();
                                continue;
                            }
                            else
                            {
                                // end the game.
                                is_ended = true;
                                break;
                            }
                        }
                        else
                        {
                            currentPlayerIndex = (currentPlayerIndex + 1) % 2; // Switch player turns
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid column number. Please try again.");
                        Console.ReadLine();
                    }
                }
                else if (gameMode == GameMode.PlayerVsComputer && currentPlayerIndex == 1)
                {
                    Console.WriteLine("Computer's turn...");
                    int computerColumn = ComputerPlayer.GenerateMove(board);
                    PlayMove(computerColumn);

                    Console.Clear();
                    GameBoard();
                    Console.WriteLine("Computer has played.");

                    if (GameWinner(Rows - 1, computerColumn))
                    {
                        Console.WriteLine("Computer wins!");

                        if (AskForRestart())
                        {
                            // restart the game and continue to play.
                            ResetGame();
                            continue;
                        }
                        else
                        {
                            // end the game.
                            is_ended = true;
                            break;
                        }
                    }
                    else if (IsBoardFull())
                    {
                        // board is now full. The game will end.
                        Console.WriteLine("It's a draw!");

                        if (AskForRestart())
                        {
                            ResetGame();
                            continue;
                        }
                        else
                        {
                            is_ended = true;
                            break;
                        }
                    }
                    else
                    {
                        // Switch player for turns
                        currentPlayerIndex = (currentPlayerIndex + 1) % 2;
                        Console.WriteLine("Your turn...");
                    }
                }
            }
        }

        private bool PlayMove(int column)
        {
            // Column checker
            if (IsColumnFull(column))
            {
                Console.WriteLine("Column is full. Please choose another column.");
                Console.ReadLine();
                return false;
            }

            // looping to get if there is a player winner
            for (int row = Rows - 1; row >= 0; row--)
            {
                if (board[row, column] == 0)
                {
                    board[row, column] = currentPlayerIndex + 1;
                    if (GameWinner(row, column))
                        return true;
                    break;
                }
            }

            return false;
        }

        private bool IsColumnFull(int column)
        {
            // return column board
            return board[0, column] != 0;
        }

        private bool GameWinner(int row, int column)
        {
            int _player = board[row, column];

            // horizontal checking
            for (int col = Math.Max(0, column - 3); col <= Math.Min(Columns - 4, column); col++)
            {
                if (board[row, col] == _player &&
                    board[row, col + 1] == _player &&
                    board[row, col + 2] == _player &&
                    board[row, col + 3] == _player)
                    return true;
            }

            // vertical checking
            for (int rows = Math.Max(0, row - 3); rows <= Math.Min(Rows - 4, row); rows++)
            {
                if (board[rows, column] == _player &&
                    board[rows + 1, column] == _player &&
                    board[rows + 2, column] == _player &&
                    board[rows + 3, column] == _player)
                    return true;
            }

            // diagonal checking ( top left to bottom right)
            for (int rows = Math.Max(0, row - 3), col = Math.Max(0, column - 3); rows <= Math.Min(Rows - 4, row) && col <= Math.Min(Columns - 4, column); rows++, col++)
            {
                if (board[rows, col] == _player &&
                    board[rows + 1, col + 1] == _player &&
                    board[rows + 2, col + 2] == _player &&
                    board[rows + 3, col + 3] == _player)
                    return true;
            }

            // diagonal checking ( top right to bottom left)
            for (int rows = Math.Min(Rows - 1, row + 3), col = Math.Max(0, column - 3); rows >= Math.Max(3, row) && col <= Math.Min(Columns - 4, column); rows--, col++)
            {
                if (board[rows, col] == _player &&
                    board[rows - 1, col + 1] == _player &&
                    board[rows - 2, col + 2] == _player &&
                    board[rows - 3, col + 3] == _player)
                    return true;
            }

            return false;
        }

        private bool IsBoardFull()
        {
            // looping to check if board is full
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    if (board[row, col] == 0)
                        return false;
                }
            }

            return true;
        }

        private void GameBoard()
        {
            // looping for board rows and column
            for (int rows = 0; rows < Rows; rows++)
            {
                Console.Write("|");
                for (int col = 0; col < Columns; col++)
                {
                    string id = (board[rows, col] == 0) ? " * " : $" {_players[board[rows, col] - 1].ID} ";
                    Console.Write(id);
                }
                Console.Write("| \n");
            }

            Console.WriteLine();
        }

        private void ResetGame()
        {
            // reset the game and modify the
            // player's index to 0.
            board = new int[Rows, Columns];
            currentPlayerIndex = 0;
        }

        private bool AskForRestart()
        {
            while (true)
            {
                Console.Write("Do you want to restart the game? (1-Yes, 0-No): ");
                string input = Console.ReadLine();

                if (input == "1")
                {
                    return true;
                }
                else if (input == "0")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
            }
        }
    }

    class ComputerPlayer
    {
        public static int GenerateMove(int[,] board)
        {
            Random random = new Random();
            int column = random.Next(0, board.GetLength(1));

            while (IsColumnFull(board, column))
            {
                column = random.Next(0, board.GetLength(1));
            }

            return column;
        }

        private static bool IsColumnFull(int[,] board, int column)
        {
            return board[0, column] != 0;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ConnectFourGame playgame = new ConnectFourGame();
            playgame.StartGame();
        }
    }
}
