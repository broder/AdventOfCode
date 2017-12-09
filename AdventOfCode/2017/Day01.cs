using System;
using System.Linq;

namespace AdventOfCode._2017
{
    internal class Day01 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(SumNextMatched("1122"));
            Console.WriteLine(SumNextMatched("1111"));
            Console.WriteLine(SumNextMatched("1234"));
            Console.WriteLine(SumNextMatched("91212129"));
            Console.WriteLine(SumNextMatched(LoadInput().First()));
        }
        
        private static int SumNextMatched(string input) => SumMatched(input, 1);

        private static int SumMatched(string input, int matchDistance)
        {
            return input
                .Where((c, i) => c == input[Mod(i + matchDistance, input.Length)])
                .Sum(c => (int) char.GetNumericValue(c));
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(SumMidMatched("1212"));
            Console.WriteLine(SumMidMatched("1221"));
            Console.WriteLine(SumMidMatched("123425"));
            Console.WriteLine(SumMidMatched("123123"));
            Console.WriteLine(SumMidMatched("12131415"));
            Console.WriteLine(SumMidMatched(LoadInput().First()));
        }
        
        private static int SumMidMatched(string input) => SumMatched(input, input.Length / 2);
    }
}