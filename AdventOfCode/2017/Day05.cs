using System;
using System.Linq;

namespace AdventOfCode._2017
{
    internal class Day05 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetSteps(new[] {0, 3, 0, 1, -3}));
            Console.WriteLine(GetSteps(LoadInput().Select(int.Parse).ToArray()));
        }

        private static int GetSteps(int[] instructions, bool extraStrangeInstructions = false)
        {
            var steps = 0;
            var currentPosition = 0;
            
            while (currentPosition >= 0 && currentPosition < instructions.Length)
            {
                var nextPosision = currentPosition + instructions[currentPosition];
                
                if (extraStrangeInstructions && instructions[currentPosition] >= 3)
                    instructions[currentPosition]--;
                else
                    instructions[currentPosition]++;
                
                currentPosition = nextPosision;
                steps++;
            }
            
            return steps;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetSteps(new[] {0, 3, 0, 1, -3}, true));
            Console.WriteLine(GetSteps(LoadInput().Select(int.Parse).ToArray(), true));
        }
    }
}