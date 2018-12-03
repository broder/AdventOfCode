using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018
{
    internal class Day01 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(SumFrequencies("+1, -2, +3, +1".Split(", ")));
            Console.WriteLine(SumFrequencies("+1, +1, +1".Split(", ")));
            Console.WriteLine(SumFrequencies("+1, +1, -2".Split(", ")));
            Console.WriteLine(SumFrequencies("-1, -2, -3".Split(", ")));
            Console.WriteLine(SumFrequencies(LoadInput()));
        }

        private static int SumFrequencies(IEnumerable<string> frequencyStrings) => frequencyStrings.Select(int.Parse).Sum();

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetRepeatFrequency("+1, -2, +3, +1".Split(", ")));
            Console.WriteLine(GetRepeatFrequency("+1, -1".Split(", ")));
            Console.WriteLine(GetRepeatFrequency("+3, +3, +4, -2, -4".Split(", ")));
            Console.WriteLine(GetRepeatFrequency("-6, +3, +8, +5, -6".Split(", ")));
            Console.WriteLine(GetRepeatFrequency("+7, +7, -2, -7, -4".Split(", ")));
            Console.WriteLine(GetRepeatFrequency(LoadInput()));
        }

        private static int GetRepeatFrequency(IEnumerable<string> frequencyStrings)
        {
            var seen = new HashSet<int>();
            var frequencies = frequencyStrings.Select(int.Parse).ToArray();
            var current = 0;
            var i = 0;
            while (true)
            {
                current += frequencies[i];
                if (!seen.Add(current))
                    return current;
                i++;
                i %= frequencies.Length;
            }
        }
    }
}