using System;

namespace AdventOfCode._2016
{
    internal class Day23 : AssemBunnyDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(RunSimulation(new int[4], "practice"));
            Console.WriteLine(RunSimulation(new[] {7, 0, 0, 0}));
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(RunSimulation(new[] {12, 0, 0, 0}));
        }
    }
}