using System;
using System.Collections.Generic;

namespace AdventOfCode._2017
{
    internal class Day03 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetCenterDistanceOnSpiral(1));
            Console.WriteLine(GetCenterDistanceOnSpiral(12));
            Console.WriteLine(GetCenterDistanceOnSpiral(23));
            Console.WriteLine(GetCenterDistanceOnSpiral(1024));
            Console.WriteLine(GetCenterDistanceOnSpiral(277678));
        }

        private static int GetCenterDistanceOnSpiral(int value)
        {
            if (value == 1) return 0;
            
            var side = (int) Math.Ceiling(Math.Sqrt(value));
            if (side % 2 == 0) side++;
            
            var perpendicularDist = side / 2;
            var tangentialDist = Math.Abs((value - (side - 2) * (side - 2)) % (side - 1) - side / 2);
            
            return perpendicularDist + tangentialDist;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetSumSpiralNumberAfterValue(277678));
        }

        private static int GetSumSpiralNumberAfterValue(int value)
        {
            var previousPoints = new Dictionary<Point, int>();

            foreach (var point in GetSpiralPoints())
            {
                if (point.X == 0 && point.Y == 0)
                {
                    previousPoints[point] = 1;
                    continue;
                }

                var currentValue = 0;
                for (var i = -1; i <= 1; i++)
                {
                    for (var j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0) continue;
                        var neighbourPoint = point.Add(new Point(i, j));
                        if (previousPoints.ContainsKey(neighbourPoint))
                            currentValue += previousPoints[neighbourPoint];
                    }
                }

                if (currentValue > value)
                    return currentValue;

                previousPoints[point] = currentValue;
            }

            return 0;
        }

        private static IEnumerable<Point> GetSpiralPoints()
        {
            var y = 0;
            var x = 0;
            yield return new Point(x, y);
            while (true)
            {
                x++;
                yield return new Point(x, y);

                while (Math.Abs(x) != Math.Abs(y))
                {
                    y--;
                    yield return new Point(x, y);
                }

                do
                {
                    x--;
                    yield return new Point(x, y);
                } while (Math.Abs(x) != Math.Abs(y));

                do
                {
                    y++;
                    yield return new Point(x, y);
                } while (Math.Abs(x) != Math.Abs(y));

                do
                {
                    x++;
                    yield return new Point(x, y);
                } while (Math.Abs(x) != Math.Abs(y));
            }
        }
    }
}