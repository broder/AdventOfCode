using System;
using System.Linq;
using System.Text;

namespace AdventOfCode._2016
{
    internal class Day05 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetPassword("abc"));
            Console.WriteLine(GetPassword("ffykfhsq"));
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetPasswordWIthComputedIndex("abc"));
            Console.WriteLine(GetPasswordWIthComputedIndex("ffykfhsq"));
        }

        private string GetPassword(string doorId)
        {
            var password = new StringBuilder();
            for (var i = 0; password.Length < 8; i++)
            {
                var hash = CalculateMd5Hash(doorId + i);
                if (hash.Take(5).All(c => c == '0'))
                    password.Append(hash[5]);
            }
            return password.ToString();
        }

        private string GetPasswordWIthComputedIndex(string doorId)
        {
            var charsComputed = 0;
            var password = new StringBuilder("--------");
            for (var i = 0; charsComputed < 8; i++)
            {
                var hash = CalculateMd5Hash(doorId + i);
                var charPosition = hash[5] - 48;
                if (charPosition < 0 ||
                    charPosition >= 8 ||
                    password[charPosition] != '-' ||
                    hash.Take(5).Any(c => c != '0')) continue;

                password[charPosition] = hash[6];
                charsComputed++;
            }
            return password.ToString();
        }
    }
}