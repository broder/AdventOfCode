using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018
{
    internal class Day13 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(RunSimulation(LoadInput("practice.1"), true));
            Console.WriteLine(RunSimulation(LoadInput(), true));
        }

        private static string RunSimulation(IReadOnlyList<string> input, bool endOnFirstCrash)
        {
            var baseMap = new char[input[0].Length, input.Count];
            var map = new char[input[0].Length, input.Count];
            var cartIntersectionDirections = new Dictionary<string, int>();
            for (var y = 0; y < input.Count; y++)
            {
                for (var x = 0; x < input[y].Length; x++)
                {
                    if ("^v".Contains(input[y][x]))
                    {
                        baseMap[x, y] = '|';
                        map[x, y] = input[y][x];
                        cartIntersectionDirections[$"{x},{y}"] = 0;
                    }
                    else if ("><".Contains(input[y][x]))
                    {
                        baseMap[x, y] = '-';
                        map[x, y] = input[y][x];
                        cartIntersectionDirections[$"{x},{y}"] = 0;
                    }
                    else
                    {
                        baseMap[x, y] = input[y][x];
                        map[x, y] = input[y][x];
                    }
                }
            }

            while (true)
            {
                var moved = new HashSet<string>();
                for (var x = 0; x < map.GetLength(0); x++)
                for (var y = 0; y < map.GetLength(1); y++)
                {
                    if (!"^>v<".Contains(map[x, y]) || moved.Contains($"{x},{y}")) continue;
                    
                    int nextX, nextY;
                    switch (map[x, y])
                    {
                        case '^':
                            nextX = x;
                            nextY = y - 1;
                            break;
                        case 'v':
                            nextX = x;
                            nextY = y + 1;
                            break;
                        case '<':
                            nextX = x - 1;
                            nextY = y;
                            break;
                        default: // >
                            nextX = x + 1;
                            nextY = y;
                            break;
                    }

                    if ("^>v<".Contains(map[nextX, nextY]))
                    {
                        if (endOnFirstCrash) return $"{nextX},{nextY}";
                        
                        map[x, y] = baseMap[x, y];
                        cartIntersectionDirections.Remove($"{x},{y}");
                        map[nextX, nextY] = baseMap[nextX, nextY];
                        cartIntersectionDirections.Remove($"{nextX},{nextY}");
                        
                        continue;
                    }
                    
                    if ("/\\".Contains(map[nextX, nextY]))
                        map[nextX, nextY] = GetNextCornerDirection(map[x, y], map[nextX, nextY]);
                    else if (map[nextX, nextY] == '+')
                    {
                        map[nextX, nextY] =
                            GetNextIntersectionDirection(map[x, y], cartIntersectionDirections[$"{x},{y}"]);
                        cartIntersectionDirections[$"{x},{y}"] = (cartIntersectionDirections[$"{x},{y}"] + 1) % 3;
                    }
                    else
                        map[nextX, nextY] = map[x, y];

                    cartIntersectionDirections[$"{nextX},{nextY}"] = cartIntersectionDirections[$"{x},{y}"];
                    cartIntersectionDirections.Remove($"{x},{y}");

                    map[x, y] = baseMap[x, y];
                    moved.Add($"{nextX},{nextY}");
                }
                
                if (cartIntersectionDirections.Count == 1)
                    return cartIntersectionDirections.Keys.First();
            }
        }

        private static char GetNextIntersectionDirection(char cart, int cartIntersectionDirections)
        {
            switch (cart)
            {
                case '>' when cartIntersectionDirections == 0:
                case '^' when cartIntersectionDirections == 1:
                case '<' when cartIntersectionDirections == 2:
                    return '^';
                case '^' when cartIntersectionDirections == 0:
                case '<' when cartIntersectionDirections == 1:
                case 'v' when cartIntersectionDirections == 2:
                    return '<';
                case '<' when cartIntersectionDirections == 0:
                case 'v' when cartIntersectionDirections == 1:
                case '>' when cartIntersectionDirections == 2:
                    return 'v';
                case 'v' when cartIntersectionDirections == 0:
                case '>' when cartIntersectionDirections == 1:
                case '^' when cartIntersectionDirections == 2:
                default:
                    return '>';
            }
        }

        private static char GetNextCornerDirection(char cart, char nextTrack)
        {
            switch (cart)
            {
                case '^' when nextTrack == '/':
                case 'v' when nextTrack == '\\':
                    return '>';
                case '^' when nextTrack == '\\':
                case 'v' when nextTrack == '/':
                    return '<';
                case '<' when nextTrack == '/':
                case '>' when nextTrack == '\\':
                    return 'v';
                case '<' when nextTrack == '\\':
                case '>' when nextTrack == '/':
                default:
                    return '^';
            }
        }
        
        protected override void RunPartTwo()
        {
            Console.WriteLine(RunSimulation(LoadInput("practice.2"), false));
            Console.WriteLine(RunSimulation(LoadInput(), false));
        }
    }
}