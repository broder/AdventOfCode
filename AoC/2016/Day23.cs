using System;

namespace AoC._2016
{
    internal class Day23 : AssemBunnyDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(RunSimulation(new int[4], "practice"));
            Console.WriteLine(RunSimulation(new[] {7, 0, 0, 0}));
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(RunSimulation(new[] {12, 0, 0, 0}));
        }
    }
}