using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2020
{
    internal class Day10 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(CountDifferences(LoadInput("practice.1").Select(int.Parse)));
            Console.WriteLine(CountDifferences(LoadInput("practice.2").Select(int.Parse)));
            Console.WriteLine(CountDifferences(LoadInput().Select(int.Parse)));
        }

        private static int CountDifferences(IEnumerable<int> numbersEnumerable)
        {
            var numbers = numbersEnumerable.ToArray();
            Array.Sort(numbers);

            var diffs = new Dictionary<int, int>();
            diffs[numbers.First()] = numbers.First();

            for (var i = 1; i < numbers.Length; i++)
            {
                var diff = numbers[i] - numbers[i - 1];
                if (!diffs.ContainsKey(diff))
                    diffs[diff] = 0;
                diffs[diff]++;
            }

            diffs[3]++;

            return diffs[3] * diffs[1];
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(CountCombinations(LoadInput("practice.1").Select(int.Parse)));
            Console.WriteLine(CountCombinations(LoadInput("practice.2").Select(int.Parse)));
            Console.WriteLine(CountCombinations(LoadInput().Select(int.Parse)));
        }

        private static long CountCombinations(IEnumerable<int> numbersEnumerable)
        {
            var numbersList = numbersEnumerable.ToList();
            numbersList.Sort();
            numbersList.Insert(0, 0);
            numbersList.Append(numbersList.Last() + 3);
            var numbers = numbersList.ToArray();

            var combinations = new long[numbers.Length];
            combinations[numbers.Length - 1] = 1;

            for (var i = numbers.Length - 2; i >= 0; i--)
                for (var j = 1; j <= 3; j++)
                    if (i + j < numbers.Length && numbers[i + j] - numbers[i] <= 3)
                        combinations[i] += combinations[i + j];

            return combinations[0];
        }
    }
}