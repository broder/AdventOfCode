using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2015
{
    internal class Day03 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetHousesVisited(">", 1));
            Console.WriteLine(GetHousesVisited("^>v<", 1));
            Console.WriteLine(GetHousesVisited("^v^v^v^v^v", 1));
            Console.WriteLine(GetHousesVisitedForFile(1));
        }

        private int GetHousesVisitedForFile(int santas)
        {
            return GetHousesVisited(LoadInput().First(), santas);
        }

        private int GetHousesVisited(string instructions, int santas)
        {
            var housesVisited = 1;
            var xs = new int[santas];
            var ys = new int[santas];
            var previousVisits = new List<Point>
            {
                new Point(xs[0], ys[0])
            };
            for (var i = 0; i < instructions.Length; i++)
            {
                var santaIndex = i % santas;

                if (instructions[i] == '^')
                    ys[santaIndex]++;
                else if (instructions[i] == 'v')
                    ys[santaIndex]--;
                else if (instructions[i] == '>')
                    xs[santaIndex]++;
                else if (instructions[i] == '<')
                    xs[santaIndex]--;

                var currentPoint = new Point(xs[santaIndex], ys[santaIndex]);
                if (previousVisits.Contains(currentPoint)) continue;

                housesVisited++;
                previousVisits.Add(currentPoint);
            }
            return housesVisited;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetHousesVisited(">", 2));
            Console.WriteLine(GetHousesVisited("^>v<", 2));
            Console.WriteLine(GetHousesVisited("^v^v^v^v^v", 2));
            Console.WriteLine(GetHousesVisitedForFile(2));
        }
    }
}