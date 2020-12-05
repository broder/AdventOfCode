using System;
using System.Linq;

namespace AdventOfCode._2020
{
    internal class Day05 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetSeatId("FBFBBFFRLR"));
            Console.WriteLine(GetSeatId("BFFFBBFRRR"));
            Console.WriteLine(GetSeatId("FFFBBBFRRR"));
            Console.WriteLine(GetSeatId("BBFFBBFRLL"));
            Console.WriteLine(LoadInput().Select(GetSeatId).Max());
        }

        private static int GetSeatId(string pass)
        {
            int row = 0, col = 0;

            int rowStart = 0, rowWidth = 128;
            foreach (var c in pass.Substring(0, 7))
            {
                rowWidth /= 2;
                if (c == 'B')
                    rowStart += rowWidth;
            }
            row = rowStart;

            int colStart = 0, colWidth = 8;
            foreach (var c in pass.Substring(7, 3))
            {
                colWidth /= 2;
                if (c == 'R')
                    colStart += colWidth;
            }
            col = colStart;

            return row * 8 + col;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(FindSeatId(LoadInput()));
        }

        private static int FindSeatId(string[] passes)
        {
            var seatIds = passes.Select(GetSeatId).ToArray();
            Array.Sort(seatIds);
            for (var i = 1; i < seatIds.Length; i++)
                if (seatIds[i - 1] == seatIds[i] - 2)
                    return seatIds[i] - 1;
            return -1;
        }
    }
}