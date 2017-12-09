using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2017
{
    internal class Day09 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetScore("{}"));
            Console.WriteLine(GetScore("{{{}}}"));
            Console.WriteLine(GetScore("{{},{}}"));
            Console.WriteLine(GetScore("{{{},{},{{}}}}"));
            Console.WriteLine(GetScore("{<a>,<a>,<a>,<a>}"));
            Console.WriteLine(GetScore("{{<ab>},{<ab>},{<ab>},{<ab>}}"));
            Console.WriteLine(GetScore("{{<!!>},{<!!>},{<!!>},{<!!>}}"));
            Console.WriteLine(GetScore("{{<a!>},{<a!>},{<a!>},{<ab>}}"));
            Console.WriteLine(GetScore(LoadInput().First()));
        }

        private static int GetScore(string input) => ParseString(input).Item1;

        private static Tuple<int, int> ParseString(string input)
        {
            var score = 0;
            var openingBrackets = 0;
            var garbage = false;
            var garbageCharacters = 0;
            for (var i = 0; i < input.Length; i++)
            {
                var c = input[i];
                if (c == '!')
                    i++;
                else if (garbage)
                    if (c == '>')
                        garbage = false;
                    else
                        garbageCharacters++;
                else if (c == '{')
                    openingBrackets++;
                else if (c == '}')
                    score += openingBrackets--;
                else if (c == '<')
                    garbage = true;
            }
            return new Tuple<int, int>(score, garbageCharacters);
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(CountGarbage("<>"));
            Console.WriteLine(CountGarbage("<random characters>"));
            Console.WriteLine(CountGarbage("<<<<>"));
            Console.WriteLine(CountGarbage("<{!>}>"));
            Console.WriteLine(CountGarbage("<!!>"));
            Console.WriteLine(CountGarbage("<!!!>>"));
            Console.WriteLine(CountGarbage("<{o\"i!a,<{i<a>"));
            Console.WriteLine(CountGarbage(LoadInput().First()));
        }

        private static int CountGarbage(string input) => ParseString(input).Item2;
    }
}