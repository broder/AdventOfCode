using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day20 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(FindShortestPath(LoadInput("practice.1")));
            Console.WriteLine(FindShortestPath(LoadInput("practice.2")));
            Console.WriteLine(FindShortestPath(LoadInput()));
        }

        private static int FindShortestPath(string[] map, bool sameLevel = false)
        {
            var upperAlphabet = Alphabet.ToUpper();
            var labels = new Dictionary<string, List<Point>>();
            for (var y = 1; y < map.Length - 1; y++)
            for (var x = 1; x < map[y].Length - 1; x++)
            {
                if (!upperAlphabet.Contains(map[y][x])) continue;

                if (upperAlphabet.Contains(map[y - 1][x]) && map[y + 1][x] == '.')
                {
                    var label = $"{map[y - 1][x]}{map[y][x]}";
                    if (!labels.ContainsKey(label)) labels[label] = new List<Point>();
                    labels[label].Add(new Point(x, y + 1));
                }

                if (upperAlphabet.Contains(map[y + 1][x]) && map[y - 1][x] == '.')
                {
                    var label = $"{map[y][x]}{map[y + 1][x]}";
                    if (!labels.ContainsKey(label)) labels[label] = new List<Point>();
                    labels[label].Add(new Point(x, y - 1));
                }

                if (upperAlphabet.Contains(map[y][x - 1]) && map[y][x + 1] == '.')
                {
                    var label = $"{map[y][x - 1]}{map[y][x]}";
                    if (!labels.ContainsKey(label)) labels[label] = new List<Point>();
                    labels[label].Add(new Point(x + 1, y));
                }

                if (upperAlphabet.Contains(map[y][x + 1]) && map[y][x - 1] == '.')
                {
                    var label = $"{map[y][x]}{map[y][x + 1]}";
                    if (!labels.ContainsKey(label)) labels[label] = new List<Point>();
                    labels[label].Add(new Point(x - 1, y));
                }
            }

            var start = labels["AA"].First();
            labels.Remove("AA");
            var end = labels["ZZ"].First();
            labels.Remove("ZZ");

            var center = new Point(map[0].Length / 2, map.Length / 2);
            var portals = new Dictionary<Point, Tuple<Point, int>>();
            foreach (var (_, points) in labels)
            {
                var firstPoint = points.First();
                var firstPointCenterDistance =
                    Math.Pow(firstPoint.X - center.X, 2) + Math.Pow(firstPoint.Y - center.Y, 2);
                var secondPoint = points.Last();
                var secondPointCenterDistance =
                    Math.Pow(secondPoint.X - center.X, 2) + Math.Pow(secondPoint.Y - center.Y, 2);

                var innerPoint = firstPointCenterDistance < secondPointCenterDistance ? firstPoint : secondPoint;
                var outerPoint = firstPointCenterDistance >= secondPointCenterDistance ? firstPoint : secondPoint;

                portals[outerPoint] = new Tuple<Point, int>(innerPoint, -1);
                portals[innerPoint] = new Tuple<Point, int>(outerPoint, 1);
            }

            var q = new Queue<Tuple<Point, int>>();
            q.Enqueue(new Tuple<Point, int>(start, 0));
            var seen = new HashSet<Point>();
            while (true)
            {
                var (currentPosition, currentDistance) = q.Dequeue();

                if (seen.Contains(currentPosition)) continue;
                seen.Add(currentPosition);

                if (currentPosition.Equals(end)) return currentDistance;

                var current2dPosition = new Point(currentPosition.X, currentPosition.Y);
                if (portals.ContainsKey(current2dPosition))
                {
                    var nextPosition = portals[current2dPosition].Item1;
                    if (sameLevel)
                        nextPosition =
                            nextPosition.Add(new Point(0, 0, currentPosition.Z + portals[current2dPosition].Item2));

                    if (nextPosition.Z >= 0 && nextPosition.Z <= portals.Count)
                        q.Enqueue(new Tuple<Point, int>(nextPosition, currentDistance + 1));
                }

                foreach (var d in Point.ManhattanDirections)
                {
                    var nextPosition = currentPosition.Add(d);
                    if (map[nextPosition.Y][nextPosition.X] != '.') continue;

                    q.Enqueue(new Tuple<Point, int>(nextPosition, currentDistance + 1));
                }
            }
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(FindShortestPath(LoadInput("practice.1"), true));
            Console.WriteLine(FindShortestPath(LoadInput("practice.3"), true));
            Console.WriteLine(FindShortestPath(LoadInput(), true));
        }
    }
}