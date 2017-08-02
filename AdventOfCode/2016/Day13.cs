using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2016
{
    internal class Day13 : BaseDay
    {
        private readonly Point[] Moves = {new Point(1, 0), new Point(-1, 0), new Point(0, 1), new Point(0, -1)};

        public override void RunPartOne()
        {
            Console.WriteLine(GetMinimumSteps(10, new Point(7, 4)));
            Console.WriteLine(GetMinimumSteps(1350, new Point(31, 39)));
        }

        private int GetMinimumSteps(int seed, Point target)
        {
            return RunSearch(seed, target, int.MaxValue);
        }

        private int RunSearch(int seed, Point target, int maxSteps)
        {
            var map = GenerateMap(seed);

            var queue = new Queue<State>();
            queue.Enqueue(new State(new Point(1, 1)));

            var seen = new HashSet<Point>();

            while (queue.Count > 0)
            {
                var state = queue.Dequeue();
                if (target.Equals(state.Location))
                    return state.Steps;

                if (state.Steps > maxSteps)
                    break;

                seen.Add(state.Location);

                var nextSteps = state.Steps + 1;

                foreach (var move in Moves)
                {
                    var nextLocation = move.Add(state.Location);

                    if (nextLocation.X < 0 || nextLocation.X >= map.GetLength(0) || nextLocation.Y < 0 ||
                        nextLocation.Y >= map.GetLength(1)) continue;

                    if (!map[nextLocation.X, nextLocation.Y] && !seen.Contains(nextLocation))
                        queue.Enqueue(new State(nextLocation, nextSteps));
                }
            }
            return seen.Count;
        }

        private bool[,] GenerateMap(int favouriteNumber)
        {
            var map = new bool[50, 50];
            for (var x = 0; x < map.GetLength(0); x++)
            {
                for (var y = 0; y < map.GetLength(1); y++)
                {
                    var number = x * x + 3 * x + 2 * x * y + y + y * y;
                    number += favouriteNumber;
                    var binary = Convert.ToString(number, 2);
                    map[x, y] = binary.Count(c => c == '1') % 2 == 1;
                }
            }
            return map;
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(GetMaxVisited(1350, 50));
        }

        private int GetMaxVisited(int seed, int maxSteps)
        {
            return RunSearch(seed, new Point(-1, -1), maxSteps);
        }

        private struct State
        {
            public readonly Point Location;
            public readonly int Steps;

            public State(Point location, int steps = 0)
            {
                Location = location;
                Steps = steps;
            }
        }
    }
}