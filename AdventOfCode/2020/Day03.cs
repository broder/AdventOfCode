using System;
using System.Linq;

namespace AdventOfCode._2020
{
    internal class Day03 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetTrees(LoadInput("practice"), 3, 1));
            Console.WriteLine(GetTrees(LoadInput(), 3, 1));
        }

        private int GetTrees(string[] map, int xDiff, int yDiff)
        {
            int x = 0, y = 0, trees = 0;
            while (y + yDiff < map.Length) {
                x += xDiff;
                x %= map[0].Length;
                y += yDiff;

                if (map[y][x] == '#')
                    trees++;
            }
            return trees;
        }

        protected override void RunPartTwo()
        {
            var slopes = new [] {
                new Point(1,1),
                new Point(3,1),
                new Point(5,1),
                new Point(7,1),
                new Point(1,2),
            };
            Console.WriteLine(slopes.Select(p => GetTrees(LoadInput("practice"), p.X, p.Y)).Aggregate(1L, (m, v) => m * v));
            Console.WriteLine(slopes.Select(p => GetTrees(LoadInput(), p.X, p.Y)).Aggregate(1L, (m, v) => m * v));
        }
    }
}