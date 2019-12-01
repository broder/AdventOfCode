using System;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day01 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetRequiredFuel(12));
            Console.WriteLine(GetRequiredFuel(14));
            Console.WriteLine(GetRequiredFuel(1969));
            Console.WriteLine(GetRequiredFuel(100756));
            Console.WriteLine(LoadInput().Select(int.Parse).Select(GetRequiredFuel).Sum());
        }

        private static int GetRequiredFuel(int moduleMass) => moduleMass / 3 - 2;

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetRecursivelyRequiredFuel(12));
            Console.WriteLine(GetRecursivelyRequiredFuel(14));
            Console.WriteLine(GetRecursivelyRequiredFuel(1969));
            Console.WriteLine(GetRecursivelyRequiredFuel(100756));
            Console.WriteLine(LoadInput().Select(int.Parse).Select(GetRecursivelyRequiredFuel).Sum());
        }

        private static int GetRecursivelyRequiredFuel(int moduleMass)
        {
            var fuel = GetRequiredFuel(moduleMass);
            if (fuel <= 0)
                return 0;

            return fuel + GetRecursivelyRequiredFuel(fuel);
        }
    }
}