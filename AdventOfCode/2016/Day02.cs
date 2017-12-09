using System;
using System.Linq;

namespace AdventOfCode._2016
{
    internal class Day02 : BaseDay
    {
        private readonly int[,] PartOneKeypad =
        {
            {1, 2, 3},
            {4, 5, 6},
            {7, 8, 9}
        };

        private readonly int[,] PartTwoKeypad =
        {
            {0, 0, 1, 0, 0},
            {0, 2, 3, 4, 0},
            {5, 6, 7, 8, 9},
            {0, 10, 11, 12, 0},
            {0, 0, 13, 0, 0}
        };

        protected override void RunPartOne()
        {
            Console.WriteLine(NavigateKeypad(PartOneKeypad, "practice"));
            Console.WriteLine(NavigateKeypad(PartOneKeypad));
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(NavigateKeypad(PartTwoKeypad, "practice"));
            Console.WriteLine(NavigateKeypad(PartTwoKeypad));
        }

        private string NavigateKeypad(int[,] keypad, string fileVariant = null)
        {
            var lines = LoadInput(fileVariant);
            var keyPadNumber = 5;
            var output = "";

            foreach (var directions in lines)
            {
                keyPadNumber = directions.Aggregate(keyPadNumber,
                    (current, direction) => GetNextKeypadNumber(keypad, direction, current));
                output += keyPadNumber.ToString("X");
            }

            return output;
        }

        private int GetNextKeypadNumber(int[,] keypad, char direction, int currentKeypadNumber)
        {
            int currentRow = -1, currentCol = -1;
            for (var row = 0; row < keypad.GetLength(0); row++)
            {
                for (var col = 0; col < keypad.GetLength(1); col++)
                {
                    if (keypad[row, col] != currentKeypadNumber) continue;

                    currentRow = row;
                    currentCol = col;
                    break;
                }
            }

            if (direction == 'U' && currentRow > 0)
                currentRow--;
            else if (direction == 'D' && currentRow < keypad.GetLength(0) - 1)
                currentRow++;
            else if (direction == 'L' && currentCol > 0)
                currentCol--;
            else if (direction == 'R' && currentCol < keypad.GetLength(1) - 1)
                currentCol++;

            return keypad[currentRow, currentCol] == 0 ? currentKeypadNumber : keypad[currentRow, currentCol];
        }
    }
}