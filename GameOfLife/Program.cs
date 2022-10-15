using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
namespace GameOfLife
{
    class Program
    {
        const int Rows = 25;
        const int Columns = 50;

        static bool RUNSIMUULATION = true;
        static void Main(string[] args)
        {
            var grid = new Status[Rows, Columns];

            for (var row = 0; row < Rows;row++)
            {
                for (var colum = 0; colum < Columns; colum++)
                {
                    grid[row, colum] = (Status)RandomNumberGenerator.GetInt32(0,2);
                }
            }

            Console.CancelKeyPress += (sender, args) =>
            {
                RUNSIMUULATION = false;
                Console.WriteLine("\n Ending...");
            };

            Console.Clear();

            while (RUNSIMUULATION)
            {
                Print(grid);
                grid = NextGeneration(grid);
            }
        }
        private static Status[,] NextGeneration(Status[,] currentGrid)
        {
            var nextGeneration = new Status[Rows, Columns];

            // Loop through every cell
            for (var row = 1; row < Rows - 1; row++)

                for (var colum = 1; colum < Columns - 1; colum++)
                {
                    var aliveNeighbors = 0;
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            aliveNeighbors += currentGrid[row + i, colum + j] == Status.Alive ? 1 : 0;
                        }
                    }
                    var currentCell = currentGrid[row, colum];

                    // The cell need to be subtracted from its neighbors as it was counted before.

                    aliveNeighbors -= currentCell == Status.Alive ? 1 : 0;

                    // implimenting rules of life.

                    if (currentCell == Status.Alive && aliveNeighbors < 2)
                    {
                        nextGeneration[row, colum] = Status.Dead;
                    }
                    else if (currentCell == Status.Alive && aliveNeighbors > 3)
                    {
                        nextGeneration[row, colum] = Status.Dead;
                    }
                    else if (currentCell == Status.Dead && aliveNeighbors == 3)
                    {
                        nextGeneration[row, colum] = Status.Alive;
                    }
                    else
                    {
                        nextGeneration[row, colum] = currentCell;
                    }
                }
            return nextGeneration;
        }
        private static void Print(Status[,] future, int timeout = 500)
        {
            var stringBuilder = new StringBuilder();
            for (var row = 0; row < Rows; row++)
            {
                for (var colum = 0; colum < Columns; colum++)
                {
                    var cell = future[row, colum];
                    stringBuilder.Append(cell == Status.Alive ? ":D" : ":E");
                }
                stringBuilder.Append("\n");
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            Console.Write(stringBuilder.ToString());
            Thread.Sleep(timeout);
        }
    }
    public enum Status
    {
        Dead,
        Alive,
    }
}

