using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2020
{
    internal class Day19 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(CountMatchingMessages(LoadInput("practice1")));
            Console.WriteLine(CountMatchingMessages(LoadInput()));
        }

        private static int CountMatchingMessages(string[] input)
        {
            var i = 0;
            var rules = new Dictionary<string, string>();
            while (input[i] != "")
            {
                var s = input[i].Split(new[] { ": ", " | ", "\"" }, StringSplitOptions.RemoveEmptyEntries);
                rules[s[0]] = $"( {string.Join(" | ", s.Skip(1))} )";
                i++;
            }

            var regexBuilder = rules["0"].Split(" ").ToList();
            while (regexBuilder.Any(s => s.Any(c => char.IsDigit(c))) && regexBuilder.Count < 100000)
                regexBuilder = regexBuilder.Select(s => rules.ContainsKey(s) ? rules[s] : s).SelectMany(s => s.Split(" ")).ToList();

            regexBuilder.Remove("8");
            regexBuilder.Remove("11");

            var regex = $"^{string.Join("", regexBuilder)}$";

            return input.Skip(i + 1).Count(s => Regex.IsMatch(s, regex));
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(CountMatchingMessages(LoadInput("practice2")));
            Console.WriteLine(CountMatchingMessagesAfterUpdate(LoadInput("practice2")));
            Console.WriteLine(CountMatchingMessagesAfterUpdate(LoadInput()));
        }

        private static int CountMatchingMessagesAfterUpdate(string[] input)
        {
            for (var i = 0; i < input.Length; i++)
                if (input[i].StartsWith("8:"))
                    input[i] = "8: 42 | 42 8";
                else if (input[i].StartsWith("11:"))
                    input[i] = "11: 42 31 | 42 11 31";
            return CountMatchingMessages(input);
        }
    }
}