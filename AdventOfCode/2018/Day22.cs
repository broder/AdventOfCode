using System;
using System.Collections.Generic;
using System.Linq;
using Priority_Queue;

namespace AdventOfCode._2018
{
    internal class Day22 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetRiskLevel(LoadInput("practice")));
            Console.WriteLine(GetRiskLevel(LoadInput()));
        }

        private static int GetRiskLevel(IReadOnlyList<string> input)
        {
            var (depth, target) = ParseInput(input);

            var map = GenerateGeologicalMap(target, depth);

            var risk = 0;
            for (var x = 0; x <= target.X; x++)
            for (var y = 0; y <= target.Y; y++)
                risk += map[x, y] % 3;

            return risk;
        }

        private static (int depth, Point target) ParseInput(IReadOnlyList<string> input)
        {
            var depth = int.Parse(input[0].Remove(0, 7));
            var targetCoords = input[1].Remove(0, 8).Split(',').Select(int.Parse).ToArray();
            var target = new Point(targetCoords[0], targetCoords[1]);
            return (depth, target);
        }

        private static int[,] GenerateGeologicalMap(Point target, int depth)
        {
            var map = new int[target.X + 100, target.Y + 100];
            for (var x = 0; x < map.GetLength(0); x++)
            for (var y = 0; y < map.GetLength(1); y++)
            {
                int geologicalIndex;
                if (x == 0 && y == 0)
                    geologicalIndex = 0;
                else if (x == 0)
                    geologicalIndex = y * 48271;
                else if (y == 0)
                    geologicalIndex = x * 16807;
                else if (x == target.X && y == target.Y)
                    geologicalIndex = 0;
                else
                    geologicalIndex = map[x - 1, y] * map[x, y - 1];

                map[x, y] = (geologicalIndex + depth) % 20183;
            }

            return map;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetFastestRoute(LoadInput("practice")));
            Console.WriteLine(GetFastestRoute(LoadInput()));
        }

        private static int GetFastestRoute(IReadOnlyList<string> input)
        {
            var (depth, target) = ParseInput(input);

            var map = GenerateRegionMap(GenerateGeologicalMap(target, depth));

            var queue = new SimplePriorityQueue<Tuple<Point, Tool, int>>();
            var seen = new HashSet<Tuple<Point, Tool>>();
            queue.Enqueue(new Tuple<Point, Tool, int>(new Point(0, 0), Tool.Torch, 0), 0);
            while (queue.Count > 0)
            {
                var (currentPosition, currentTool, currentTime) = queue.Dequeue();
                if (!seen.Add(new Tuple<Point, Tool>(currentPosition, currentTool))) continue;

                if (currentPosition.Equals(target))
                {
                    var findTime = currentTime;
                    if (currentTool != Tool.Torch) findTime += 7;
                    return findTime;
                }

                foreach (var d in Point.ManhattanDirections)
                {
                    var nextPosition = currentPosition.Add(d);
                    if (nextPosition.X < 0 || nextPosition.X >= map.GetLength(0)
                                           || nextPosition.Y < 0 || nextPosition.Y >= map.GetLength(1)) continue;

                    var disallowedTools = new List<Tool>();
                    if (map[currentPosition.X, currentPosition.Y] == Region.Rocky
                        || map[nextPosition.X, nextPosition.Y] == Region.Rocky)
                        disallowedTools.Add(Tool.Neither);

                    if (map[currentPosition.X, currentPosition.Y] == Region.Wet
                        || map[nextPosition.X, nextPosition.Y] == Region.Wet)
                        disallowedTools.Add(Tool.Torch);

                    if (map[currentPosition.X, currentPosition.Y] == Region.Narrow
                        || map[nextPosition.X, nextPosition.Y] == Region.Narrow)
                        disallowedTools.Add(Tool.ClimbingGear);

                    foreach (var nextTool in Enum.GetValues(typeof(Tool)).Cast<Tool>())
                    {
                        if (disallowedTools.Contains(nextTool)) continue;

                        var nextTime = currentTime + 1;
                        if (nextTool != currentTool)
                            nextTime += 7;
                        queue.Enqueue(new Tuple<Point, Tool, int>(nextPosition, nextTool, nextTime), nextTime);
                    }
                }
            }

            return 0;
        }

        private static Region[,] GenerateRegionMap(int[,] geologicalMap)
        {
            var map = new Region[geologicalMap.GetLength(0), geologicalMap.GetLength(1)];
            for (var x = 0; x < map.GetLength(0); x++)
            for (var y = 0; y < map.GetLength(1); y++)
            {
                switch (geologicalMap[x, y] % 3)
                {
                    case 0:
                        map[x, y] = Region.Rocky;
                        break;
                    case 1:
                        map[x, y] = Region.Wet;
                        break;
                    default: // 2
                        map[x, y] = Region.Narrow;
                        break;
                }
            }

            return map;
        }

        private enum Region
        {
            Rocky,
            Wet,
            Narrow
        }

        private enum Tool
        {
            Neither,
            Torch,
            ClimbingGear
        }
    }
}