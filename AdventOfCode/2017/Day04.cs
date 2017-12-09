using System;
using System.Collections.Generic;
using System.Linq;
using Combinatorics.Collections;

namespace AdventOfCode._2017
{
    internal class Day04 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(IsValidPassword("aa bb cc dd ee"));
            Console.WriteLine(IsValidPassword("aa bb cc dd aa"));
            Console.WriteLine(IsValidPassword("aa bb cc dd aaa"));
            Console.WriteLine(LoadInput().Count(s => IsValidPassword(s)));
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(IsValidPassword("abcde fghij", true));
            Console.WriteLine(IsValidPassword("abcde xyz ecdab", true));
            Console.WriteLine(IsValidPassword("a ab abc abd abf abj", true));
            Console.WriteLine(IsValidPassword("iiii oiii ooii oooi oooo", true));
            Console.WriteLine(IsValidPassword("oiii ioii iioi iiio", true));
            Console.WriteLine(LoadInput().Count(s => IsValidPassword(s, true)));
        }

        private static bool IsValidPassword(string password, bool includeAnagrams = false)
        {
            var hashTable = new HashSet<string>();

            foreach (var word in password.Split(' '))
            {
                string wordToCheck;
                if (includeAnagrams)
                {
                    var chars = word.ToCharArray();
                    Array.Sort(chars);
                    wordToCheck = new string(chars);
                }
                else
                    wordToCheck = word;

                if (!hashTable.Add(wordToCheck)) return false;
            }
            return true;
        }
    }
}