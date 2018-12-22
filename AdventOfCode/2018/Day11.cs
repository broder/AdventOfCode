using System;

namespace AdventOfCode._2018
{
    internal class Day11 : BaseDay
    {
        private const int GridSize = 300;

        protected override void RunPartOne()
        {
            Console.WriteLine(GetPowerLevel(3, 5, 8));
            Console.WriteLine(GetPowerLevel(122, 79, 57));
            Console.WriteLine(GetPowerLevel(217, 196, 39));
            Console.WriteLine(GetPowerLevel(101, 153, 71));

            Console.WriteLine(GetLargestPower(18, 3, 4));
            Console.WriteLine(GetLargestPower(42, 3, 4));
            Console.WriteLine(GetLargestPower(5093, 3, 4));
        }

        private static int GetPowerLevel(int x, int y, int serialNumber)
        {
            var rackId = x + 10;
            var powerLevel = rackId * y;
            powerLevel += serialNumber;
            powerLevel *= rackId;
            powerLevel = powerLevel / 100 % 10;
            powerLevel -= 5;
            return powerLevel;
        }

        private static string GetLargestPower(int serialNumber, int lowerSquareSize = 0, int upperSquareSize = GridSize)
        {
            var grid = new int[upperSquareSize][,];

            var maxPower = int.MinValue;
            var maxX = -1;
            var maxY = -1;
            var maxSquareSize = -1;

            for (var squareSize = 1; squareSize < upperSquareSize; squareSize++)
            {
                grid[squareSize] = new int[GridSize - squareSize + 1, GridSize - squareSize + 1];
                for (var x = 0; x <= GridSize - squareSize; x++)
                {
                    for (var y = 0; y <= GridSize - squareSize; y++)
                    {
                        int squarePower;
                        if (squareSize == 1)
                        {
                            squarePower = GetPowerLevel(x + 1, y + 1, serialNumber);
                        }
                        else
                        {
                            squarePower = grid[squareSize - 1][x, y];
                            for (var diff = 0; diff < squareSize - 1; diff++)
                            {
                                squarePower += grid[1][x + diff, y + squareSize - 1];
                                squarePower += grid[1][x + squareSize - 1, y + diff];
                            }

                            squarePower += grid[1][x + squareSize - 1, y + squareSize - 1];
                        }

                        grid[squareSize][x, y] = squarePower;

                        if (lowerSquareSize > squareSize || squarePower <= maxPower)
                            continue;

                        maxPower = squarePower;
                        maxX = x;
                        maxY = y;
                        maxSquareSize = squareSize;
                    }
                }
            }

            return $"{maxX + 1},{maxY + 1},{maxSquareSize}";
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetLargestPower(18));
            Console.WriteLine(GetLargestPower(42));
            Console.WriteLine(GetLargestPower(5093));
        }
    }
}