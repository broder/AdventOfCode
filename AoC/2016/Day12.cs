using System;

namespace AoC._2016
{
    internal class Day12 : AssemBunnyDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(RunSimulation(new int[4], "practice"));
            Console.WriteLine(RunSimulation(new int[4]));
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(RunSimulation(new[] {0, 0, 1, 0}));
        }
    }
}