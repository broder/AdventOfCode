using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2017
{
    internal class Day02 : BaseDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(GetMinMaxChecksum(LoadInput("1.practice")));
            Console.WriteLine(GetMinMaxChecksum(LoadInput()));
        }

        private static int GetMinMaxChecksum(IEnumerable<string> input)
        {
            return input
                .Sum(row =>
                {
                    var minMax = row
                        .Split('	')
                        .Select(int.Parse)
                        .Aggregate(new Tuple<int, int>(int.MaxValue, int.MinValue),
                            (currentMinMax, i) => new Tuple<int, int>(
                                Math.Min(i, currentMinMax.Item1),
                                Math.Max(i, currentMinMax.Item2)
                            ));
                    return Math.Abs(minMax.Item1 - minMax.Item2);
                });
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(GetDivisibleChecksum(LoadInput("2.practice")));
            Console.WriteLine(GetDivisibleChecksum(LoadInput()));
        }

        private static int GetDivisibleChecksum(IEnumerable<string> input)
        {
            return input
                .Sum(row =>
                {
                    var numbers = row.Split('	').Select(int.Parse).ToList();
                    for (var i = 0; i < numbers.Count; i++)
                    {
                        var a = numbers[i];
                        for (var j = i + 1; j < numbers.Count; j++)
                        {
                            var b = numbers[j];
                            if (a % b == 0)
                            {
                                return a / b;
                            }
                            if (b % a == 0)
                            {
                                return b / a;
                            }
                        }
                    }
                    return 0;
                });
        }
    }
}