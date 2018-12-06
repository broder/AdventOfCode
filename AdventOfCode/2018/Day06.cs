using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018
{
    internal class Day06 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetLargestArea(LoadInput("practice")));
            Console.WriteLine(GetLargestArea(LoadInput()));
        }

        private static int GetLargestArea(IEnumerable<string> rawCoordinates)
        {
            var coordinates = rawCoordinates.Select(c => c.Split(", ").Select(int.Parse).ToArray()).ToArray();

            var closestPoints = new int[400, 400];
            for (var x = 0; x < closestPoints.GetLength(0); x++)
            {
                for (var y = 0; y < closestPoints.GetLength(1); y++)
                {
                    closestPoints[x, y] = int.MinValue;
                    var closestDistance = int.MaxValue;
                    for (var c = 0; c < coordinates.Length; c++)
                    {
                        var dist = Math.Abs(coordinates[c][0] - x) + Math.Abs(coordinates[c][1] - y);
                        if (dist < closestDistance)
                        {
                            closestDistance = dist;
                            closestPoints[x, y] = c;
                        }
                        else if (dist == closestDistance)
                        {
                            closestPoints[x, y] = int.MinValue;
                        }
                    }
                }
            }

            var edgeCoords = new HashSet<int>();
            var areas = new Dictionary<int, int>();
            for (var x = 0; x < closestPoints.GetLength(0); x++)
            {
                for (var y = 0; y < closestPoints.GetLength(0); y++)
                {
                    if (!areas.ContainsKey(closestPoints[x, y]))
                        areas[closestPoints[x, y]] = 0;

                    areas[closestPoints[x, y]]++;

                    if (x == 0 || x == closestPoints.GetLength(0) - 1 || y == 0 || y == closestPoints.GetLength(1) - 1)
                        edgeCoords.Add(closestPoints[x, y]);
                }
            }

            return areas.Where(p => p.Key != int.MinValue && !edgeCoords.Contains(p.Key))
                .Aggregate((l, r) => l.Value > r.Value ? l : r).Value;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetSafestArea(LoadInput("practice"), 32));
            Console.WriteLine(GetSafestArea(LoadInput(), 10000));
        }

        private static int GetSafestArea(IEnumerable<string> rawCoordinates, int maxDistance)
        {
            var coordinates = rawCoordinates.Select(c => c.Split(", ").Select(int.Parse).ToArray()).ToArray();

            var countWithinMaxDistance = 0;
            for (var x = 0; x < 400; x++)
            {
                for (var y = 0; y < 400; y++)
                {
                    var totalDistance = coordinates.Sum(c => Math.Abs(c[0] - x) + Math.Abs(c[1] - y));

                    if (totalDistance < maxDistance)
                        countWithinMaxDistance++;
                }
            }

            return countWithinMaxDistance;
        }
    }
}