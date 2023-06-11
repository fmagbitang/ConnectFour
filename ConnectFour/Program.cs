using System;

namespace ConnectFour
{
    class ConnectFourPlayer
    {
        public string Name { get; set; }
        public string ID { get; set; }

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
            Console.WriteLine(_players);
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

