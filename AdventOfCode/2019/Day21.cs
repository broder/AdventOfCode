using System;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day21 : BaseIntcodeDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetHullDamage(LoadInput().First()));
        }

        private static long GetHullDamage(string opcodeString) =>
            new IntcodeVM(opcodeString, 4000)
                .SendInput(string.Join("\n",
                    "OR A J",
                    "AND B J",
                    "AND C J",
                    "NOT J J",
                    "AND D J",
                    "WALK",
                    "")).Run().GetOutputs().Last();

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetRunningHullDamage(LoadInput().First()));
        }

        private static long GetRunningHullDamage(string opcodeString) =>
            new IntcodeVM(opcodeString, 4000)
                .SendInput(string.Join("\n",
                    "OR A J",
                    "AND B J",
                    "AND C J",
                    "NOT J J",
                    "AND D J",
                    "OR E T",
                    "OR H T",
                    "AND T J",
                    "RUN",
                    "")).Run().GetOutputs().Last();
    }
}