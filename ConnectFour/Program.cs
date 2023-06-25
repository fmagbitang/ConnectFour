using System;

namespace ConnectFour
{
    enum GameMode
    {
        TwoPlayer,
        PlayerVsAI
    }

    abstract class ConnectFourPlayer
    {
        public string Name { get; protected set; }
        public string ID { get; protected set; }

    }

    class HumanPlayer : ConnectFourPlayer
    {
        public HumanPlayer(string name, string id)
        {
            Name = name;
            ID = id;
        }

    }

    class AIPlayer
    {
        public static int GenerateMove(int[,] board)
        {
            Random random = new Random();

            // Add a slight delay to simulate the AI's decision-making process
            System.Threading.Thread.Sleep(1500);

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

    public interface IPlayGame
    {
        void StartGame();
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