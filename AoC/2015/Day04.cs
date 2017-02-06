using System;
using System.Linq;

namespace AoC._2015
{
    internal class Day04 : BaseDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(GetLowestZeroHash("abcdef", 5));
            Console.WriteLine(GetLowestZeroHash("pqrstuv", 5));
            Console.WriteLine(GetLowestZeroHash("bgvyzdsv", 5));
        }

        private int GetLowestZeroHash(string input, int zeroes)
        {
            for (var i = 0;; i++)
                if (CalculateMd5Hash(input + i).Take(zeroes).All(c => c == '0'))
                    return i;
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(GetLowestZeroHash("bgvyzdsv", 6));
        }
    }
}