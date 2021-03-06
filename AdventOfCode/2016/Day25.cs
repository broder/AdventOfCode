﻿using System;

namespace AdventOfCode._2016
{
    internal class Day25 : AssemBunnyDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetAlternatingRegisterValue());
        }

        protected override void RunPartTwo()
        {
        }

        private int GetAlternatingRegisterValue()
        {
            var aStart = 0;
            while (true)
            {
                try
                {
                    if (RunSimulation(new[] {aStart, 0, 0, 0}) == OUTPUT_ALTERNATING)
                        return aStart;
                }
                catch (InvalidOperationException)
                {
                    aStart++;
                }
            }
        }
    }
}