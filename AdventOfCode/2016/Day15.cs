using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2016
{
    internal class Day15 : BaseDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(GetFallTime("practice"));
            Console.WriteLine(GetFallTime("1"));
        }

        private int GetFallTime(string fileVariant)
        {
            var discs = LoadInput(fileVariant)
                .Select(line => line.Split(new[] {" has ", " positions; at time=0, it is at position "}, StringSplitOptions.None))
                .Select(lineSplit => new Disc
                {
                    MaxPosition = int.Parse(lineSplit[1]),
                    InitialPosition = int.Parse(lineSplit[2].Trim('.'))
                })
                .ToList();

            for (var time = 0;; time++)
                if (CanFallAtTime(discs, time))
                    return time;
        }

        private static bool CanFallAtTime(IEnumerable<Disc> discs, int time)
        {
            foreach (var disc in discs)
            {
                time++;
                if (!disc.OpenAtTime(time))
                    return false;
            }
            return true;
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(GetFallTime("2"));
        }

        private class Disc
        {
            public int InitialPosition;
            public int MaxPosition;

            public bool OpenAtTime(int time)
            {
                return (time + InitialPosition) % MaxPosition == 0;
            }
        }
    }
}