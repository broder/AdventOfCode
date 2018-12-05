using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018
{
    internal class Day05 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetRemainingPolymerLength("dabAcCaCBAcCcaDA"));
            Console.WriteLine(GetRemainingPolymerLength(LoadInput().First()));
        }

        private static int GetRemainingPolymerLength(string polymer, char rejectionChar = char.MinValue)
        {
            var stack = new Stack<char>();
            foreach (var c in polymer)
            {
                if (char.ToUpper(c) == char.ToUpper(rejectionChar))
                    continue;
                if (stack.Count > 0 && stack.Peek() == ToggleCase(c))
                    stack.Pop();
                else
                    stack.Push(c);
            }

            return stack.Count;
        }

        private static char ToggleCase(char c) => char.IsLower(c) ? char.ToUpper(c) : char.ToLower(c);

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetOptimalRemainingPolymerLength("dabAcCaCBAcCcaDA"));
            Console.WriteLine(GetOptimalRemainingPolymerLength(LoadInput().First()));
        }

        private static int GetOptimalRemainingPolymerLength(string polymer) =>
            Alphabet.Min(c => GetRemainingPolymerLength(polymer, c));
    }
}