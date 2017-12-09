using System;

namespace AdventOfCode._2016
{
    internal class Day18 : BaseDay
    {
        private string Input =
            ".^^^^^.^^^..^^^^^...^.^..^^^.^^....^.^...^^^...^^^^..^...^...^^.^.^.......^..^^...^.^.^^..^^^^^...^.";

        protected override void RunPartOne()
        {
            Console.WriteLine(GetSafeTiles("..^^.", 3));
            Console.WriteLine(GetSafeTiles(".^^.^.^^^^", 10));
            Console.WriteLine(GetSafeTiles(Input, 40));
        }

        private static int GetSafeTiles(string firstRow, int totalRows)
        {
            var safeTiles = 0;
            var trapRow = new bool[firstRow.Length];
            for (var i = 0; i < firstRow.Length; i++)
            {
                trapRow[i] = firstRow[i] == '^';
                safeTiles += !trapRow[i] ? 1 : 0;
            }

            for (var rowCount = 1; rowCount < totalRows; rowCount++)
            {
                var nextRow = new bool[trapRow.Length];
                for (var i = 0; i < firstRow.Length; i++)
                {
                    var left = i - 1 >= 0 && trapRow[i - 1];
                    var center = trapRow[i];
                    var right = i + 1 < trapRow.Length && trapRow[i + 1];
                    nextRow[i] = left && center && !right || !left && center && right || left && !center && !right ||
                                 !left && !center && right;
                    safeTiles += !nextRow[i] ? 1 : 0;
                }
                trapRow = nextRow;
            }
            return safeTiles;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetSafeTiles(Input, 400000));
        }
    }
}