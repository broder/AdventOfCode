using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018
{
    internal class Day25 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetConstellationCount(LoadInput("practice.1")));
            Console.WriteLine(GetConstellationCount(LoadInput("practice.2")));
            Console.WriteLine(GetConstellationCount(LoadInput("practice.3")));
            Console.WriteLine(GetConstellationCount(LoadInput("practice.4")));
            Console.WriteLine(GetConstellationCount(LoadInput()));
        }

        private int GetConstellationCount(IEnumerable<string> input)
        {
            var points = new List<Tuple<int, int, int, int>>();
            foreach (var line in input)
            {
                var split = line.Split(',').Select(int.Parse).ToArray();
                points.Add(new Tuple<int, int, int, int>(split[0], split[1], split[2], split[3]));
            }

            var constellations = new List<Tuple<int, int, int, int>>[points.Count];
            for (var i = 0; i < constellations.Length; i++)
                constellations[i] = new List<Tuple<int, int, int, int>>();
            
            var constellationIndex = 0;
            foreach (var point in points)
            {
                var matchedConstellations = new List<int>();
                for (var i = 0; i < constellations.Length; i++)
                {
                    foreach (var otherPoint in constellations[i])
                    {
                        if (Math.Abs(point.Item1 - otherPoint.Item1) + Math.Abs(point.Item2 - otherPoint.Item2) +
                            Math.Abs(point.Item3 - otherPoint.Item3) + Math.Abs(point.Item4 - otherPoint.Item4) >
                            3) continue;

                        matchedConstellations.Add(i);
                        break;
                    }
                }

                if (matchedConstellations.Count == 0)
                {
                    constellations[++constellationIndex].Add(point);
                }
                else
                {
                    var baseConstellation = constellations[matchedConstellations[0]];
                    baseConstellation.Add(point);
                    for (var i = matchedConstellations.Count - 1; i > 0; i--)
                    {
                        baseConstellation.AddRange(constellations[matchedConstellations[i]]);
                        constellations[matchedConstellations[i]].Clear();
                    }
                }
            }

            return constellations.Count(c => c.Count > 0);
        }

        protected override void RunPartTwo()
        {
        }
    }
}