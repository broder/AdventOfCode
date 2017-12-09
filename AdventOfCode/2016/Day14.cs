using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2016
{
    internal class Day14 : BaseDay
    {
        private readonly Regex ThreeCharRegex = new Regex(@"([abcdef\d])\1\1");
        private Regex FiveCharRegex(char ch) => new Regex(@"([" + ch + @"])\1\1\1\1");

        private Dictionary<string, string> CachedMD5;

        protected override void RunPartOne()
        {
            Console.WriteLine(GetKeyIndex("abc"));
            Console.WriteLine(GetKeyIndex("zpqevtbw"));
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetKeyIndex("abc", 2016));
            Console.WriteLine(GetKeyIndex("zpqevtbw", 2016));
        }

        private int GetKeyIndex(string salt, int additionalHashings = 0)
        {
            CachedMD5 = new Dictionary<string, string>();
            var keys = 0;
            var index = 0;
            while (keys < 64)
            {
                if (IsValidKey(salt, index, additionalHashings))
                    keys++;

                index++;
            }
            return index - 1;
        }

        private bool IsValidKey(string salt, int index, int additionalHashings)
        {
            var key = GenerateKey(salt + index, additionalHashings);
            var threeMatch = ThreeCharRegex.Match(key);
            if (!threeMatch.Success)
                return false;

            var matchedFiveCharRegex = FiveCharRegex(threeMatch.Groups[1].Value[0]);

            return Enumerable.Range(index + 1, 1000)
                .Any(i => matchedFiveCharRegex.IsMatch(GenerateKey(salt + i, additionalHashings)));
        }

        private string GenerateKey(string input, int additionalHashings)
        {
            if (CachedMD5.ContainsKey(input))
                return CachedMD5[input];

            var hash = CalculateMd5Hash(input);
            for (var i = 0; i < additionalHashings; i++)
                hash = CalculateMd5Hash(hash);

            CachedMD5[input] = hash;
            return hash;
        }
    }
}