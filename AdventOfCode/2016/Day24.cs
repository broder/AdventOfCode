using System;
using System.Collections.Generic;
using System.Linq;
using Combinatorics.Collections;

namespace AdventOfCode._2016
{
    internal class Day24 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetShortestRoute(5, false, "practice"));
            Console.WriteLine(GetShortestRoute(8));
        }

        private int GetShortestRoute(int numberOfLocations, bool backToZero = false, string fileVariant = null)
        {
            var map = LoadInput(fileVariant);
            var requiredPoints = new Point[numberOfLocations];
            for (var y = 0; y < map.Length; y++)
            {
                var row = map[y];
                for (var x = 0; x < row.Length; x++)
                {
                    var point = row[x];
                    if (point != '#' && point != '.')
                        requiredPoints[int.Parse(point.ToString())] = new Point(x, y);
                }
            }

            var distances = new int[numberOfLocations, numberOfLocations];
            for (var from = 0; from < numberOfLocations; from++)
            {
                var d = FindDistance(map, requiredPoints[from], numberOfLocations);
                for (var to = from + 1; to < numberOfLocations; to++)
                    distances[from, to] = d[to];
            }

            var minSteps = int.MaxValue;
            foreach (var perm in new Permutations<int>(Enumerable.Range(1, numberOfLocations - 1).ToList()))
            {
                var newSteps = 0;
                var num = 0;
                for (var i = 0; i < perm.Count; i++)
                {
                    num = perm[i];
                    var prevNum = i == 0 ? 0 : perm[i - 1];

                    newSteps += distances[Math.Min(prevNum, num), Math.Max(prevNum, num)];
                }
                if (backToZero)
                    newSteps += distances[0, num];
                minSteps = Math.Min(minSteps, newSteps);
            }

            return minSteps;
        }

        private int[] FindDistance(string[] map, Point from, int numberOfLocations)
        {
            var distances = new int[numberOfLocations];
            var moves = new[] {new Point(1, 0), new Point(-1, 0), new Point(0, 1), new Point(0, -1)};

            var queue = new Queue<Tuple<int, Point>>();
            queue.Enqueue(new Tuple<int, Point>(0, from));

            var seen = new HashSet<Point>();
            while (queue.Count > 0)
            {
                var state = queue.Dequeue();
                seen.Add(state.Item2);
                foreach (var move in moves)
                {
                    var next = state.Item2.Add(move);
                    if (next.X < 0 || next.X >= map[0].Length ||
                        next.Y < 0 || next.Y >= map.Length ||
                        seen.Contains(next)) continue;

                    if (map[next.Y][next.X] == '.')
                        queue.Enqueue(new Tuple<int, Point>(state.Item1 + 1, next));
                    else if (map[next.Y][next.X] != '#')
                    {
                        if (AddDistance(distances, int.Parse(map[next.Y][next.X].ToString()), state.Item1 + 1))
                            return distances;
                        queue.Enqueue(new Tuple<int, Point>(state.Item1 + 1, next));
                    }
                }
            }
            return distances;
        }

        private bool AddDistance(int[] distances, int index, int distance)
        {
            if (distances[index] == 0)
                distances[index] = distance;

            return !distances.Any(d => d == 0);
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetShortestRoute(8, true));
        }
    }
}