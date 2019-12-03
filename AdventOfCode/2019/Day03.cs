using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day03 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetClosestSpatialIntersection("R8,U5,L5,D3",
                "U7,R6,D4,L4"));
            Console.WriteLine(GetClosestSpatialIntersection("R75,D30,R83,U83,L12,D49,R71,U7,L72",
                "U62,R66,U55,R34,D71,R55,D58,R83"));
            Console.WriteLine(GetClosestSpatialIntersection("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51",
                "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7"));
            var input = LoadInput();
            Console.WriteLine(GetClosestSpatialIntersection(input[0], input[1]));
        }

        private static int GetClosestSpatialIntersection(string wireOne, string wireTwo) => GetWirePath(wireOne).Keys
            .Intersect(GetWirePath(wireTwo).Keys).Min(p => Math.Abs(p.X) + Math.Abs(p.Y));

        private static Dictionary<Point, int> GetWirePath(string wire)
        {
            var dict = new Dictionary<Point, int>();
            var time = 0;
            var currentPosition = new Point(0, 0);
            foreach (var instruction in wire.Split(","))
            {
                var direction = instruction[0];
                Point diff;
                switch (direction)
                {
                    case 'R':
                        diff = new Point(1, 0);
                        break;
                    case 'L':
                        diff = new Point(-1, 0);
                        break;
                    case 'U':
                        diff = new Point(0, 1);
                        break;
                    case 'D':
                        diff = new Point(0, -1);
                        break;
                    default:
                        throw new ArgumentException();
                }

                var distance = int.Parse(instruction.Substring(1));
                for (var d = 0; d < distance; d++)
                {
                    currentPosition = currentPosition.Add(diff);
                    time++;
                    if (dict.ContainsKey(currentPosition)) continue;
                    dict.Add(currentPosition, time);
                }
            }

            return dict;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetClosestTemporalIntersection("R8,U5,L5,D3",
                "U7,R6,D4,L4"));
            Console.WriteLine(GetClosestTemporalIntersection("R75,D30,R83,U83,L12,D49,R71,U7,L72",
                "U62,R66,U55,R34,D71,R55,D58,R83"));
            Console.WriteLine(GetClosestTemporalIntersection("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51",
                "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7"));
            var input = LoadInput();
            Console.WriteLine(GetClosestTemporalIntersection(input[0], input[1]));
        }

        private static int GetClosestTemporalIntersection(string wireOne, string wireTwo)
        {
            var wireOnePath = GetWirePath(wireOne);
            var wireTwoPath = GetWirePath(wireTwo);
            return wireOnePath.Keys
                .Intersect(wireTwoPath.Keys).Min(p => wireOnePath[p] + wireTwoPath[p]);
        }
    }
}