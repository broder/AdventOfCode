using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2015
{
    internal class Day05 : BaseDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(IsNice("ugknbfddgicrmopn"));
            Console.WriteLine(IsNice("aaa"));
            Console.WriteLine(IsNice("jchzalrnumimnmhp"));
            Console.WriteLine(IsNice("haegwjzuvuyypxyu"));
            Console.WriteLine(IsNice("dvszwmarrgswjxmb"));
            Console.WriteLine(CountNiceStringsFromFile());
        }

        private int CountNiceStringsFromFile()
        {
            return LoadInput().Count(IsNice);
        }

        private bool IsNice(string input)
        {
            var ruleOne = new Regex(@"(.*[aeiou]){3}");
            var ruleTwo = new Regex(@"(.)\1");
            var ruleThree = new Regex(@"ab|cd|pq|xy");
            return ruleOne.IsMatch(input) && ruleTwo.IsMatch(input) && !ruleThree.IsMatch(input);
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(IsNiceRevised("qjhvhtzxzqqjkmpb"));
            Console.WriteLine(IsNiceRevised("xxyxx"));
            Console.WriteLine(IsNiceRevised("uurcxstgmygtbstg"));
            Console.WriteLine(IsNiceRevised("ieodomkazucvgmuy"));
            Console.WriteLine(CountRevisedNiceStringsFromFile());
        }

        private int CountRevisedNiceStringsFromFile()
        {
            return LoadInput().Count(IsNiceRevised);
        }

        private bool IsNiceRevised(string input)
        {
            var ruleOne = new Regex(@"(..).*\1");
            var ruleTwo = new Regex(@"(.).\1");
            return ruleOne.IsMatch(input) && ruleTwo.IsMatch(input);
        }
    }
}