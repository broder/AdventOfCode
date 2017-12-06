using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2017
{
    internal class Day06 : BaseDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(CalculateReallocation(new[] {0, 2, 7, 0}));
            Console.WriteLine(CalculateReallocation(LoadInput().First().Split('	').Select(int.Parse).ToArray()));
        }

        private static int CalculateReallocation(int[] blocks, bool returnLoopSize = false)
        {
            var seenStates = new Dictionary<string, int>();
            var currentState = string.Join(":", blocks.Select(i => i.ToString()));

            while (!seenStates.ContainsKey(currentState))
            {
                seenStates.Add(currentState, seenStates.Count);

                var maxValue = -1;
                var maxIndex = -1;
                for (var i = 0; i < blocks.Length; i++)
                {
                    if (blocks[i] <= maxValue) continue;
                    maxIndex = i;
                    maxValue = blocks[i];
                }
                blocks[maxIndex] = 0;

                var currentIndex = Mod(maxIndex + 1, blocks.Length);
                while (maxValue > 0)
                {
                    blocks[currentIndex]++;
                    maxValue--;
                    currentIndex = Mod(currentIndex + 1, blocks.Length);
                }

                currentState = string.Join(":", blocks.Select(i => i.ToString()));
            }

            if (returnLoopSize)
                return seenStates.Count - seenStates[currentState];
            return seenStates.Count;
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(CalculateReallocation(new[] {2, 4, 1, 2}, true));
            Console.WriteLine(CalculateReallocation(LoadInput().First().Split('	').Select(int.Parse).ToArray(), true));
        }
    }
}