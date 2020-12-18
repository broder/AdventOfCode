using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2020
{
    internal class Day17 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetActiveCubes(LoadInput("practice"), 20, 6));
            Console.WriteLine(GetActiveCubes(LoadInput(), 30, 6));
        }

        private static int GetActiveCubes(string[] initialPlane, int size, int timeLimit)
        {
            var map = new string[size][];
            var half = size / 2;
            for (var z = 0; z < size; z++)
            {
                map[z] = new string[size];
                for (var y = 0; y < size; y++)
                {
                    if (z == half && y >= half && y < half + initialPlane.Length)
                    {
                        var p = initialPlane[y - half];
                        var pw = p.Length;
                        var lp = half - pw / 2;
                        var rp = half - pw / 2 + (pw % 2 == 1 ? 1 : 0);
                        map[z][y] = new string('.', lp) + p + new string('.', rp);
                    }
                    else
                    {
                        map[z][y] = new string('.', size);
                    }
                }
            }

            for (var t = 0; t < timeLimit; t++)
            {
                var nextMap = new string[size][];
                for (var z = 0; z < size; z++)
                {
                    nextMap[z] = new string[size];
                    for (var y = 0; y < size; y++)
                    {
                        nextMap[z][y] = "";
                        for (var x = 0; x < size; x++)
                        {
                            var activeNeighbours = GetNeighbours(map, size, x, y, z).Count(c => c == '#');
                            if (map[z][y][x] == '.' && activeNeighbours == 3)
                            {
                                nextMap[z][y] += '#';
                            }
                            else if (map[z][y][x] == '#' && !(activeNeighbours == 2 || activeNeighbours == 3))
                            {
                                nextMap[z][y] += '.';
                            }
                            else
                            {
                                nextMap[z][y] += map[z][y][x];
                            }
                        }
                    }
                }
                map = nextMap;
            }
            return map.Sum(p => p.Sum(l => l.Count(c => c == '#')));
        }

        private static IEnumerable<char> GetNeighbours(string[][] map, int size, int x, int y, int z)
        {
            for (var nz = z - 1; nz <= z + 1; nz++)
            {
                for (var ny = y - 1; ny <= y + 1; ny++)
                {
                    for (var nx = x - 1; nx <= x + 1; nx++)
                    {
                        if ((nz == z && ny == y && nx == x) || nz < 0 || nz >= size || ny < 0 || ny >= size || nx < 0 || nx >= size) continue;
                        yield return map[nz][ny][nx];
                    }
                }
            }
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetActiveHyperCubes(LoadInput("practice"), 20, 6));
            Console.WriteLine(GetActiveHyperCubes(LoadInput(), 30, 6));
        }

        private static int GetActiveHyperCubes(string[] initialPlane, int size, int timeLimit)
        {
            var map = new string[size][][];
            var half = size / 2;
            for (var w = 0; w < size; w++)
            {
                map[w] = new string[size][];
                for (var z = 0; z < size; z++)
                {
                    map[w][z] = new string[size];
                    for (var y = 0; y < size; y++)
                    {
                        if (w == half && z == half && y >= half && y < half + initialPlane.Length)
                        {
                            var p = initialPlane[y - half];
                            var pw = p.Length;
                            var lp = half - pw / 2;
                            var rp = half - pw / 2 + (pw % 2 == 1 ? 1 : 0);
                            map[w][z][y] = new string('.', lp) + p + new string('.', rp);
                        }
                        else
                        {
                            map[w][z][y] = new string('.', size);
                        }
                    }
                }
            }


            for (var t = 0; t < timeLimit; t++)
            {
                var nextMap = new string[size][][];
                for (var w = 0; w < size; w++)
                {
                    nextMap[w] = new string[size][];
                    for (var z = 0; z < size; z++)
                    {
                        nextMap[w][z] = new string[size];
                        for (var y = 0; y < size; y++)
                        {
                            nextMap[w][z][y] = "";
                            for (var x = 0; x < size; x++)
                            {
                                var activeNeighbours = GetHyperNeighbours(map, size, x, y, z, w).Count(c => c == '#');
                                if (map[w][z][y][x] == '.' && activeNeighbours == 3)
                                {
                                    nextMap[w][z][y] += '#';
                                }
                                else if (map[w][z][y][x] == '#' && !(activeNeighbours == 2 || activeNeighbours == 3))
                                {
                                    nextMap[w][z][y] += '.';
                                }
                                else
                                {
                                    nextMap[w][z][y] += map[w][z][y][x];
                                }
                            }
                        }
                    }
                }
                map = nextMap;
            }
            return map.Sum(c => c.Sum(p => p.Sum(l => l.Count(i => i == '#'))));
        }

        private static IEnumerable<char> GetHyperNeighbours(string[][][] map, int size, int x, int y, int z, int w)
        {
            for (var nw = w - 1; nw <= w + 1; nw++)
            {
                for (var nz = z - 1; nz <= z + 1; nz++)
                {
                    for (var ny = y - 1; ny <= y + 1; ny++)
                    {
                        for (var nx = x - 1; nx <= x + 1; nx++)
                        {
                            if ((nw == w && nz == z && ny == y && nx == x) || nw < 0 || nw >= size || nz < 0 || nz >= size || ny < 0 || ny >= size || nx < 0 || nx >= size) continue;
                            yield return map[nw][nz][ny][nx];
                        }
                    }
                }
            }
        }
    }
}