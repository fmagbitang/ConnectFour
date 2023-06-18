using System;

namespace ConnectFour
{
    class ConnectFourPlayer
    {
        public string Name { get; }
        public string ID { get; }

        public ConnectFourPlayer(string name, string id)
        {
            Name = name;
            ID = id;
        }
    }

    class ConnectFourGame
    {
        private const int Rows = 6;
        private const int Column = 7;
        private int[,] board;
        private ConnectFourPlayer[] _players;
        private int currentPlayersIndex;

        public ConnectFourGame()
        {
            board = new int[Rows, Column];
            _players = new ConnectFourPlayer[2];
            currentPlayersIndex = 0;
        }

        public void StartGame()
        {
            Console.WriteLine("Welcome to Connect Four!");
            Console.WriteLine();

            // create the player’s name.
            for (int i= 0; i < _players.Length; i++)
            {
                Console.WriteLine($"Enter Player {i + 1}'s name: ");
                string name = Console.ReadLine();
                string id = (i == 0) ? "X" : "O";
                _players[i] = new ConnectFourPlayer(name, id);
            }
            //Console.WriteLine(_players);

            bool is_ended = false;

            while (!is_ended)
            {
                Console.Clear();
                GameBoard();

                Console.WriteLine($"Player {_players[currentPlayersIndex].Name}\'s turn.");
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

                if (int.TryParse(input, out int column) && column >= 1  && column <= Column)
                {
                    // Subtract 1 to convert to zero-based index
                    column--;

                    if (PlayMove(column))
                    {
                        Console.Clear();
                        GameBoard();
                        Console.WriteLine($"Player {_players[currentPlayersIndex].Name} wins!");

                        bool validInput = false;
                        int restartOption = 0;

                        while (!validInput)
                        {
                            Console.WriteLine("Do you want to restart the game? (1 = Yes, 0 = No): ");
                            string restartInput = Console.ReadLine();

                            if (int.TryParse(restartInput, out restartOption) && (restartOption == 0 || restartOption == 1))
                                validInput = true;
                            else
                                Console.WriteLine("Invalid input. Please enter 1 for Yes or 0 for No.");
                        }

                        if (restartOption == 1)
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
                        is_ended = true;
                        break;
                    }
                    else
                    {
                        // Switch player for turns
                        currentPlayersIndex = (currentPlayersIndex + 1) % 2;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid column number. Please try again.");
                    Console.ReadLine();
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
                    board[row, column] = currentPlayersIndex + 1;
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
            for (int col = Math.Max(0, column - 3); col <= Math.Min(Column - 4, column); col++)
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
            for (int rows = Math.Max(0, row - 3), col = Math.Max(0, column - 3); rows <= Math.Min(Rows - 4, row) && col <= Math.Min(Column - 4, column); rows++, col++)
            {
                if (board[rows, col] == _player &&
                    board[rows + 1, col + 1] == _player &&
                    board[rows + 2, col + 2] == _player &&
                    board[rows + 3, col + 3] == _player)
                    return true;
            }

            // diagonal checking ( top right to bottom left)
            for (int rows = Math.Min(Rows - 1, row + 3), col = Math.Max(0, column - 3); rows >= Math.Max(3, row) && col <= Math.Min(Column - 4, column); rows--, col++)
            {
                if (board[rows, col] == _player &&
                    board[rows - 1, col + 1] == _player &&
                    board[rows - 2, col + 2] == _player &&
                    board[rows - 3, col + 3] == _player)
                    return true;
            }

            return false;
        }

        private void GameBoard()
        {
            // looping for board rows and column
            for (int rows = 0; rows < Rows; rows++)
            {
                Console.Write("|");
                for (int col = 0; col < Column; col++)
                {
                    string id = (board[rows, col] == 0) ? " . " : $" {_players[board[rows, col] - 1].ID} ";
                    Console.Write(id);
                }
                Console.Write("| \n");
            }

            Console.WriteLine();
        }

        private bool IsBoardFull()
        {
            // looping to check if board is full
            for (int rows = 0; rows  < Rows; rows++)
            {
                for (int col = 0; col < Column; col++)
                {
                    if (board[rows, col] == 0)
                        return false;
                }
            }
            return true;
        }

        private void ResetGame()
        {
            // reset the game and modify the
            // player's index to 0.
            board = new int[Rows, Column];
            currentPlayersIndex = 0;
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

