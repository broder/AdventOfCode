using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2020
{
    internal class Day24 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetFlippedTiles(LoadInput("practice")));
            Console.WriteLine(GetFlippedTiles(LoadInput()));
        }

        private static int GetFlippedTiles(string[] input, int time = 0)
        {
            var s = 400;
            var tiles = new bool[s, s];
            foreach (var line in input)
            {
                int x = s / 2, y = s / 2;
                for (var i = 0; i < line.Length; i++)
                {
                    if (line[i] == 'e')
                        x += 2;
                    else if (line[i] == 'w')
                        x -= 2;
                    else if (line[i] == 'n' && line[i + 1] == 'e')
                    {
                        x += 1;
                        y += 1;
                        i += 1;
                    }
                    else if (line[i] == 'n' && line[i + 1] == 'w')
                    {
                        x -= 1;
                        y += 1;
                        i += 1;
                    }
                    else if (line[i] == 's' && line[i + 1] == 'e')
                    {
                        x += 1;
                        y -= 1;
                        i += 1;
                    }
                    else if (line[i] == 's' && line[i + 1] == 'w')
                    {
                        x -= 1;
                        y -= 1;
                        i += 1;
                    }
                }
                tiles[x, y] = !tiles[x, y];
            }


            for (var t = 0; t < time; t++)
            {
                var nextTiles = new bool[s, s];

                for (var x = 0; x < s; x++)
                {
                    for (var y = 0; y < s; y++)
                    {
                        if (!IsValidHexPosition(x, y)) continue;

                        var neighbours = GetNeighbours(tiles, x, y).Count(b => b);

                        if ((tiles[x, y] && (neighbours == 0 || neighbours > 2))
                            || (!tiles[x, y] && neighbours == 2))
                            nextTiles[x, y] = !tiles[x, y];
                        else
                            nextTiles[x, y] = tiles[x, y];
                    }
                }

                tiles = nextTiles;
            }

            return tiles.Cast<bool>().Count(b => b);
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetFlippedTiles(LoadInput("practice"), 100));
            Console.WriteLine(GetFlippedTiles(LoadInput(), 100));
        }

        private static bool IsValidHexPosition(int x, int y) => y % 2 == 0 ? x % 2 == 0 : x % 2 == 1;

        private static IEnumerable<bool> GetNeighbours(bool[,] tiles, int x, int y)
        {
            if (x >= 2)
                yield return tiles[x - 2, y];
            if (x < tiles.GetLength(0) - 2)
                yield return tiles[x + 2, y];
            if (x >= 1 && y < tiles.GetLength(0) - 1)
                yield return tiles[x - 1, y + 1];
            if (x < tiles.GetLength(0) - 1 && y < tiles.GetLength(0) - 1)
                yield return tiles[x + 1, y + 1];
            if (x >= 1 && y >= 1)
                yield return tiles[x - 1, y - 1];
            if (x < tiles.GetLength(0) - 1 && y >= 1)
                yield return tiles[x + 1, y - 1];
        }
    }
}