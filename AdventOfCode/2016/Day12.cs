using System;

namespace AdventOfCode._2016
{
    internal class Day12 : AssemBunnyDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(RunSimulation(new int[4], "practice"));
            Console.WriteLine(RunSimulation(new int[4]));
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(RunSimulation(new[] {0, 0, 1, 0}));
        }
    }
}