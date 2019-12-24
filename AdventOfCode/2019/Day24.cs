using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day24 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetBiodiversityOfRepeatedState(LoadInput("practice")));
            Console.WriteLine(GetBiodiversityOfRepeatedState(LoadInput()));
        }

        private static int GetBiodiversityOfRepeatedState(string[] mapString)
        {
            var map = new bool[5, 5];
            for (var y = 0; y < mapString.Length; y++)
            for (var x = 0; x < mapString[y].Length; x++)
                map[x, y] = mapString[y][x] == '#';

            var seen = new HashSet<int> {GetBiodiversity(map)};
            while (true)
            {
                map = Evolve(map);
                if (seen.Contains(GetBiodiversity(map))) break;
                seen.Add(GetBiodiversity(map));
            }

            return GetBiodiversity(map);
        }

        private static void PrintMap(bool[,] map)
        {
            for (var y = 0; y < map.GetLength(1); y++)
            {
                for (var x = 0; x < map.GetLength(0); x++)
                    Console.Write(map[x, y] ? '#' : '.');

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        private static bool[,] Evolve(bool[,] map)
        {
            var nextMap = new bool[map.GetLength(0), map.GetLength(1)];
            for (var y = 0; y < map.GetLength(1); y++)
            for (var x = 0; x < map.GetLength(0); x++)
            {
                var neighbours = 0;
                if (x > 0 && map[x - 1, y]) neighbours++;
                if (x < map.GetLength(0) - 1 && map[x + 1, y]) neighbours++;
                if (y > 0 && map[x, y - 1]) neighbours++;
                if (y < map.GetLength(1) - 1 && map[x, y + 1]) neighbours++;

                if (map[x, y] && neighbours == 1 || !map[x, y] && (neighbours == 1 || neighbours == 2))
                    nextMap[x, y] = true;
                else
                    nextMap[x, y] = false;
            }

            return nextMap;
        }

        private static int GetBiodiversity(bool[,] map)
        {
            var sum = 0;
            for (var y = 0; y < map.GetLength(1); y++)
            for (var x = 0; x < map.GetLength(0); x++)
                if (map[x, y])
                    sum += 1 << (x + y * 5);

            return sum;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetBugsAfterTime(LoadInput("practice"), 10));
            Console.WriteLine(GetBugsAfterTime(LoadInput(), 200));
        }

        private static int GetBugsAfterTime(string[] mapString, int time)
        {
            var map = new bool[5, 5, time + 1];
            for (var y = 0; y < mapString.Length; y++)
            for (var x = 0; x < mapString[y].Length; x++)
                map[x, y, time / 2] = mapString[y][x] == '#';

            for (var t = 0; t < time; t++)
                map = Evolve(map);

            return map.Cast<bool>().Count(b => b);
        }

        private static void PrintMap(bool[,,] map)
        {
            for (var z = 0; z < map.GetLength(2); z++)
            {
                Console.WriteLine($"z: {z}");
                for (var y = 0; y < map.GetLength(1); y++)
                {
                    for (var x = 0; x < map.GetLength(0); x++)
                        Console.Write(map[x, y, z] ? '#' : '.');

                    Console.WriteLine();
                }

                Console.WriteLine();
            }
        }

        private static bool[,,] Evolve(bool[,,] map)
        {
            var nextMap = new bool[map.GetLength(0), map.GetLength(1), map.GetLength(2)];

            var width = map.GetLength(0);
            var height = map.GetLength(1);
            var depth = map.GetLength(2);
            var midWidth = width / 2;
            var midHeight = height / 2;

            for (var z = 0; z < depth; z++)
            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
            {
                if (x == midWidth && y == midHeight) continue;

                var neighbours = 0;
                //left x
                if (x == 0)
                {
                    if (z > 0 && map[midWidth - 1, midHeight, z - 1])
                        neighbours++;
                }
                else if (x == midWidth + 1 && y == midHeight)
                {
                    for (var y2 = 0; y2 < height; y2++)
                        if (z < depth - 1 && map[width - 1, y2, z + 1])
                            neighbours++;
                }
                else if (map[x - 1, y, z])
                {
                    neighbours++;
                }

                //right x
                if (x == width - 1)
                {
                    if (z > 0 && map[midWidth + 1, midHeight, z - 1])
                        neighbours++;
                }
                else if (x == midWidth - 1 && y == midHeight)
                {
                    for (var y2 = 0; y2 < height; y2++)
                        if (z < depth - 1 && map[0, y2, z + 1])
                            neighbours++;
                }
                else if (map[x + 1, y, z])
                {
                    neighbours++;
                }

                //top y
                if (y == 0)
                {
                    if (z > 0 && map[midWidth, midHeight - 1, z - 1])
                        neighbours++;
                }
                else if (x == midWidth && y == midHeight + 1)
                {
                    for (var x2 = 0; x2 < width; x2++)
                        if (z < depth - 1 && map[x2, height - 1, z + 1])
                            neighbours++;
                }
                else if (map[x, y - 1, z])
                {
                    neighbours++;
                }

                //bottom y
                if (y == height - 1)
                {
                    if (z > 0 && map[midWidth, midHeight + 1, z - 1])
                        neighbours++;
                }
                else if (x == midWidth && y == midHeight - 1)
                {
                    for (var x2 = 0; x2 < width; x2++)
                        if (z < depth - 1 && map[x2, 0, z + 1])
                            neighbours++;
                }
                else if (map[x, y + 1, z])
                {
                    neighbours++;
                }

                if (map[x, y, z] && neighbours == 1 || !map[x, y, z] && (neighbours == 1 || neighbours == 2))
                    nextMap[x, y, z] = true;
                else
                    nextMap[x, y, z] = false;
            }

            return nextMap;
        }
    }
}