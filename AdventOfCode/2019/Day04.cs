using System;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day04 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(IsValidPassword(111111));
            Console.WriteLine(IsValidPassword(223450));
            Console.WriteLine(IsValidPassword(123789));
            Console.WriteLine(Enumerable.Range(347312, 805915 - 347312 + 1).Count(IsValidPassword));
        }

        private static bool IsValidPassword(int password)
        {
            var hasAdjacentEqualDigits = false;
            var divisor = (int) Math.Pow(10, Math.Floor(Math.Log10(password)));
            int? lastDigit = null;

            while (divisor >= 1)
            {
                var nextDigit = password / divisor;
                if (lastDigit.HasValue && lastDigit.Value > nextDigit) return false;
                if (lastDigit.HasValue && lastDigit.Value == nextDigit) hasAdjacentEqualDigits = true;
                lastDigit = nextDigit;
                password %= divisor;
                divisor /= 10;
            }

            return hasAdjacentEqualDigits;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(IsValidRestrictedRepeatPassword(112233));
            Console.WriteLine(IsValidRestrictedRepeatPassword(123444));
            Console.WriteLine(IsValidRestrictedRepeatPassword(111122));
            Console.WriteLine(Enumerable.Range(347312, 805915 - 347312 + 1).Count(IsValidRestrictedRepeatPassword));
        }

        private static bool IsValidRestrictedRepeatPassword(int password)
        {
            var hasTwoAdjacentEqualDigits = false;
            var currentAdjacentEqualDigits = 1;
            var divisor = (int) Math.Pow(10, Math.Floor(Math.Log10(password)));
            int? lastDigit = null;

            while (divisor >= 1)
            {
                var nextDigit = password / divisor;
                if (lastDigit.HasValue && lastDigit.Value > nextDigit) return false;
                if (lastDigit.HasValue && lastDigit.Value == nextDigit)
                    currentAdjacentEqualDigits += 1;
                else
                {
                    if (currentAdjacentEqualDigits == 2)
                        hasTwoAdjacentEqualDigits = true;

                    currentAdjacentEqualDigits = 1;
                }

                lastDigit = nextDigit;
                password %= divisor;
                divisor /= 10;
            }

            return hasTwoAdjacentEqualDigits || currentAdjacentEqualDigits == 2;
        }
    }
}