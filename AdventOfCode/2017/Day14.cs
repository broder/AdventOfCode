using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode._2017
{
    internal class Day14 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetUsedSquares("flqrgnkx"));
            Console.WriteLine(GetUsedSquares("hwlqcszp"));
        }

        private static int GetUsedSquares(string seed)
        {
            return GenerateDisk(seed).Cast<bool>().Count(b => b);
        }
        
        private static bool[,] GenerateDisk(string seed)
        {
            var disk = new bool[128, 128];
            for (var y = 0; y < 128; y++)
            {
                var hash = Day10.GetKnotHash($"{seed}-{y}")
                    .Aggregate(new StringBuilder(),
                        (sb, c) => sb.Append(Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')))
                    .ToString();
                for (var x = 0; x < hash.Length; x++)
                    if (hash[x] == '1')
                        disk[x, y] = true;
            }
            return disk;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetRegions("flqrgnkx"));
            Console.WriteLine(GetRegions("hwlqcszp"));
        }

        private static int GetRegions(string seed)
        {
            var disk = GenerateDisk(seed);
            var regions = 0;
            var seen = new bool[128, 128];
            for (var y = 0; y < 128; y++)
            {
                for (var x = 0; x < 128; x++)
                {
                    if (seen[x, y]) continue;
                    if (!disk[x, y]) continue;
                    regions++;
                    
                    // DFS
                    var dfs = new Queue<Point>();
                    dfs.Enqueue(new Point(x, y));
                    while (dfs.Any())
                    {
                        var p = dfs.Dequeue();
                        if (seen[p.X, p.Y]) continue;
                        if (!disk[p.X, p.Y]) continue;
                        
                        seen[p.X, p.Y] = true;
                        
                        if (p.X > 0) dfs.Enqueue(new Point(p.X - 1, p.Y));
                        if (p.X < 127) dfs.Enqueue(new Point(p.X + 1, p.Y));
                        if (p.Y > 0) dfs.Enqueue(new Point(p.X, p.Y - 1));
                        if (p.Y < 127) dfs.Enqueue(new Point(p.X, p.Y + 1));
                    }
                }
            }
            return regions;
        }
    }
}