using System;
using System.Linq;
using System.Text;

namespace AdventOfCode._2015
{
    internal class Day11 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(IsValidPassword("hijklmmn"));
            Console.WriteLine(IsValidPassword("abbceffg"));
            Console.WriteLine(IsValidPassword("abbcegjk"));
            Console.WriteLine(GetNextValidPassword("abcdefgh"));
            Console.WriteLine(GetNextValidPassword("ghijklmn"));
            Console.WriteLine(GetNextValidPassword("hepxcrrq"));
        }

        private readonly char[] InvalidCharacters = {'i', 'o', 'l'};

        private string GetNextValidPassword(string seed)
        {
            var password = seed;
            var invalidCharacterIndexes = InvalidCharacters.Select(c => c - 97).ToArray();
            while (true)
            {
                var sb = new StringBuilder(password);

                var carry = 1;
                for (var i = sb.Length - 1; i >= 0; i--)
                {
                    var c = sb[i] - 97 + carry;

                    if (carry > 0 && invalidCharacterIndexes.Contains(c))
                        c++;

                    sb[i] = (char) (c % 26 + 97);
                    carry = c / 26;
                }

                password = sb.ToString();
                if (IsValidPassword(password))
                    return password;
            }
        }

        private bool IsValidPassword(string password)
        {
            var straights = Enumerable.Range(1, 24).Select(i => $"{Alphabet[i - 1]}{Alphabet[i]}{Alphabet[i + 1]}");
            var doubles = Alphabet.Select(c => $"{c}{c}");
            return straights.Any(password.Contains) &&
                   !InvalidCharacters.Any(password.Contains) &&
                   doubles.Count(password.Contains) >= 2;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetNextValidPassword("hepxxyzz"));
        }
    }
}