using System;
using System.Linq;

namespace AdventOfCode._2015
{
    internal class Day14 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetMaximumDistance(1000, "practice"));
            Console.WriteLine(GetMaximumDistance(2503));
        }

        private int GetMaximumDistance(int raceSeconds, string fileVariant = null)
        {
            var reindeer = GetReindeer(fileVariant);

            var maxDistance = 0;
            for (var i = 0; i < reindeer.GetLength(0); i++)
            {
                var distance = 0;
                var resting = false;
                var timeRemaining = raceSeconds;
                while (timeRemaining > 0)
                {
                    if (!resting)
                        distance += reindeer[i, 0] * Math.Min(timeRemaining, reindeer[i, 1]);

                    timeRemaining -= resting ? reindeer[i, 2] : Math.Min(timeRemaining, reindeer[i, 1]);
                    resting = !resting;
                }

                maxDistance = Math.Max(distance, maxDistance);
            }
            return maxDistance;
        }

        private int[,] GetReindeer(string fileVariant)
        {
            var input = LoadInput(fileVariant);
            var reindeer = new int[input.Length, 3];
            for (var i = 0; i < input.Length; i++)
            {
                var line = input[i];
                var split = line.Split(
                    new[] {" can fly ", " km/s for ", " seconds, but then must rest for ", " seconds."},
                    StringSplitOptions.None);
                reindeer[i, 0] = int.Parse(split[1]);
                reindeer[i, 1] = int.Parse(split[2]);
                reindeer[i, 2] = int.Parse(split[3]);
            }
            return reindeer;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetMaximumPoints(1000, "practice"));
            Console.WriteLine(GetMaximumPoints(2503));
        }

        private int GetMaximumPoints(int raceSeconds, string fileVariant = null)
        {
            var reindeer = GetReindeer(fileVariant);

            var numberOfReindeer = reindeer.GetLength(0);
            var distances = new int[numberOfReindeer];
            var points = new int[numberOfReindeer];
            for (var t = 0; t < raceSeconds; t++)
            {
                for (var i = 0; i < numberOfReindeer; i++)
                {
                    var resting = Mod(t, reindeer[i, 1] + reindeer[i, 2]) - reindeer[i, 1] >= 0;
                    if (!resting)
                        distances[i] += reindeer[i, 0];
                }

                var leader = Array.IndexOf(distances, distances.Max());
                points[leader]++;
            }
            return points.Max();
        }
    }
}