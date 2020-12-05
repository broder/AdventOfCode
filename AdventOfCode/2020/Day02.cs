using System;
using System.Linq;

namespace AdventOfCode._2020
{
    internal class Day02 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(LoadInput("practice").Count(IsPasswordValid));
            Console.WriteLine(LoadInput().Count(IsPasswordValid));
        }

        private static bool IsPasswordValid(string s)
        {
            var parts = s.Split(new [] {"-", " ", ": "}, StringSplitOptions.RemoveEmptyEntries);
            var minCount = int.Parse(parts[0]);
            var maxCount = int.Parse(parts[1]);
            var requiredChar = parts[2][0];
            var pw = parts[3];
            var charOccurrences = pw.Count(c => c == requiredChar);
            return minCount <= charOccurrences && charOccurrences <= maxCount;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(LoadInput("practice").Count(IsNewPasswordValid));
            Console.WriteLine(LoadInput().Count(IsNewPasswordValid));
        }

        private static bool IsNewPasswordValid(string s)
        {
            var parts = s.Split(new [] {"-", " ", ": "}, StringSplitOptions.RemoveEmptyEntries);
            var firstIndex = int.Parse(parts[0]) - 1;
            var secondIndex = int.Parse(parts[1]) - 1;
            var requiredChar = parts[2][0];
            var pw = parts[3];
            return pw[firstIndex] == requiredChar ^ pw[secondIndex] == requiredChar;
        }
    }
}