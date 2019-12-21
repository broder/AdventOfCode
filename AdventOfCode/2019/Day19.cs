using System;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day19 : BaseIntcodeDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(CountTractorBeamPoints(LoadInput().First()));
        }

        private static int CountTractorBeamPoints(string opcodeString)
        {
            var count = 0;
            for (var y = 0; y < 50; y++)
            for (var x = 0; x < 50; x++)
                if (new IntcodeVM(opcodeString).SendInput(x, y).Run().GetOutputs().First() == 1)
                    count++;

            return count;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(FindSantaShipLocationChecksum(LoadInput().First()));
        }

        private static long FindSantaShipLocationChecksum(string opcodeString)
        {
            var y = 50L;
            var x = 0L;

            while (true)
            {
                while (new IntcodeVM(opcodeString).SendInput(x, y).Run().GetOutputs().First() == 0) x++;

                if (new IntcodeVM(opcodeString).SendInput(x + 99, y - 99).Run().GetOutputs().First() == 1)
                    return x * 10000 + y - 99;

                y++;
            }
        }
    }
}