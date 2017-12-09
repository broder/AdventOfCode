using System;

namespace AdventOfCode._2015
{
    internal class Day25 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetValueFromGrid(3, 4, 1L, i => i + 1L));
            Console.WriteLine(GetValueFromGrid(3010, 3019, 20151125L, i => i * 252533 % 33554393));
        }

        public long GetValueFromGrid(int targetRow, int targetCol, long seed, Func<long, long> nextFunc)
        {
            var currentRow = 1;
            var currentCol = 1;
            var currentValue = seed;
            while (true)
            {
                if (currentRow == targetRow && currentCol == targetCol)
                    return currentValue;

                if (currentRow == 1)
                {
                    currentRow = currentCol + 1;
                    currentCol = 1;
                }
                else
                {
                    currentRow -= 1;
                    currentCol += 1;
                }
                currentValue = nextFunc(currentValue);
            }
        }

        protected override void RunPartTwo()
        {
        }
    }
}