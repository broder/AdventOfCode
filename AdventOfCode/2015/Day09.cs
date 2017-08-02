using System;
using System.Linq;
using Combinatorics.Collections;

namespace AdventOfCode._2015
{
    internal class Day09 : BaseDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(GetTripDistance(3, Math.Min, "practice"));
            Console.WriteLine(GetTripDistance(8, Math.Min));
        }

        private int GetTripDistance(int numberOfLocations, Func<int, int, int> method, string fileVariant = null)
        {
            var locations = new string[numberOfLocations];
            var distances = new int[numberOfLocations, numberOfLocations];
            foreach (var line in LoadInput(fileVariant))
            {
                var split = line.Split(new[] {" to ", " = "}, StringSplitOptions.None);
                var location1 = GetLocationIndex(locations, split[0]);
                var location2 = GetLocationIndex(locations, split[1]);
                var distance = int.Parse(split[2]);

                distances[Math.Min(location1, location2), Math.Max(location1, location2)] = distance;
            }

            var minDistance = -1;
            foreach (var permutation in new Permutations<int>(Enumerable.Range(0, numberOfLocations).ToList()))
            {
                var distance = 0;
                for (var i = 1; i < permutation.Count; i++)
                {
                    var location1 = permutation[i - 1];
                    var location2 = permutation[i];
                    distance += distances[Math.Min(location1, location2), Math.Max(location1, location2)];
                }
                minDistance = minDistance == -1 ? distance : method(minDistance, distance);
            }
            return minDistance;
        }

        private int GetLocationIndex(string[] locations, string location)
        {
            var index = Array.IndexOf(locations, location);
            if (index != -1) return index;
            var nullIndex = Array.IndexOf(locations, null);
            locations[nullIndex] = location;
            return nullIndex;
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(GetTripDistance(3, Math.Max, "practice"));
            Console.WriteLine(GetTripDistance(8, Math.Max));
        }
    }
}