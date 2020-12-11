using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2020
{
    internal class Day11 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetOccupiedSeats(LoadInput("practice")));
            Console.WriteLine(GetOccupiedSeats(LoadInput()));
        }

        private static int GetOccupiedSeats(string[] map, bool useSightNeighbours = false)
        {
            while (true)
            {
                var nextMap = new string[map.Length];
                var hasChanges = false;
                for (var y = 0; y < map.Length; y++)
                {
                    nextMap[y] = "";
                    for (var x = 0; x < map[0].Length; x++)
                    {
                        var neighbours = useSightNeighbours ? GetSightNeighbours(map, x, y).ToArray() : GetNeighbours(map, x, y).ToArray();
                        if (map[y][x] == 'L' && neighbours.Count(c => c == '#') == 0)
                        {
                            nextMap[y] +='#';
                            hasChanges = true;
                        }
                        else if (map[y][x] == '#' && neighbours.Count(c => c == '#') >= (useSightNeighbours ? 5 : 4))
                        {
                            nextMap[y] += 'L';
                            hasChanges = true;
                        }
                        else
                        {
                            nextMap[y] += map[y][x];
                        }
                    }
                }
                if (!hasChanges) break;
                map = nextMap;
            }
            return map.Sum(l => l.Count(c => c == '#'));
        }

        private static IEnumerable<char> GetNeighbours(string[] map, int x, int y)
        {
            for (var ny = y - 1; ny <= y + 1; ny++)
            {
                for (var nx = x - 1; nx <= x + 1; nx++)
                {
                    if ((ny == y && nx == x) || ny < 0 || ny >= map.Length || nx < 0 || nx >= map[0].Length) continue;
                    yield return map[ny][nx];
                }
            }
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetOccupiedSeats(LoadInput("practice"), true));
            Console.WriteLine(GetOccupiedSeats(LoadInput(), true));
        }

        private static IEnumerable<char> GetSightNeighbours(string[] map, int x, int y)
        {
            for (var dy = - 1; dy <= 1; dy++)
            {
                for (var dx = - 1; dx <= 1; dx++)
                {
                    if (dy == 0 && dx == 0) continue;
                    var cx = x;
                    var cy = y;
                    var found = false;
                    while (!found) {
                        cx += dx;
                        cy += dy;
                        if (cy < 0 || cy >= map.Length || cx < 0 || cx >= map[0].Length) {
                            found = true;
                        } else if (map[cy][cx] == '#' || map[cy][cx] == 'L') {
                            found = true;
                            yield return map[cy][cx];
                        }
                    }
                }
            }
        }
    }
}