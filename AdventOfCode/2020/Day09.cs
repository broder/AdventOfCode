using System;
using System.Linq;

namespace AdventOfCode._2020
{
    internal class Day09 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(FindInvalidNumber(LoadInput("practice"), 5));
            Console.WriteLine(FindInvalidNumber(LoadInput(), 25));
        }

        private static long FindInvalidNumber(string[] input, int preambleLength) => FindInvalidNumber(input.Select(long.Parse).ToArray(), preambleLength);

        private static long FindInvalidNumber(long[] numbers, int preambleLength)
        {
            for (var i = preambleLength + 1; i < numbers.Length; i++)
                if (!IsValidNumber(numbers, i - preambleLength - 1, i - 1, i))
                    return numbers[i];

            return -1;
        }

        private static bool IsValidNumber(long[] input, int startIndex, int endIndex, int targetIndex)
        {
            for (var i = startIndex; i <= endIndex; i++)
                for (var j = i + 1; j <= endIndex; j++)
                    if (input[i] + input[j] == input[targetIndex])
                        return true;
            return false;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(FindEncryptionWeakness(LoadInput("practice"), 5));
            Console.WriteLine(FindEncryptionWeakness(LoadInput(), 25));
        }

        private static long FindEncryptionWeakness(string[] input, int preambleLength)
        {
            var numbers = input.Select(long.Parse).ToArray();
            var invalidNumber = FindInvalidNumber(numbers, preambleLength);

            for (var i = 0; i < numbers.Length; i++)
            {
                var currentSum = 0L;
                var smallest = long.MaxValue;
                var largest = long.MinValue;
                for (var j = i; j < numbers.Length; j++)
                {
                    currentSum += numbers[j];
                    smallest = Math.Min(numbers[j], smallest);
                    largest = Math.Max(numbers[j], largest);
                    if (currentSum == invalidNumber)
                        return smallest + largest;
                }
            }
            return -1;
        }
    }
}