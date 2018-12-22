using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018
{
    internal class Day12 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetFinalPlanSum(LoadInput("practice"), 20));
            Console.WriteLine(GetFinalPlanSum(LoadInput(), 20));
        }

        private static long GetFinalPlanSum(IReadOnlyList<string> instructions, long generations)
        {
            var initialState = instructions[0].Replace("initial state: ", "").Select(c => c == '#').ToArray();

            var transformations = new Dictionary<int, bool>();
            foreach (var instruction in instructions.Skip(2))
            {
                var split = instruction.Split(" => ");
                var previous = GetKey(split[0].Select(c => c == '#').ToArray());
                var next = split[1] == "#";
                transformations[previous] = next;
            }

            var width = initialState.Length;
            var state = new bool[width * 4];
            var previousSum = 0;

            for (var i = 0; i < width; i++)
                state[width + i] = initialState[i];

            long time;
            for (time = 0; time < generations; time++)
            {
                var nextState = new bool[state.Length];
                for (var i = 2; i < state.Length - 2; i++)
                {
                    var neighbours = GetKey(new[] {state[i - 2], state[i - 1], state[i], state[i + 1], state[i + 2]});
                    if (transformations.ContainsKey(neighbours))
                        nextState[i] = transformations[neighbours];
                    else
                        nextState[i] = false;
                }

                state = nextState;

                if (time == 100) break;
                
                previousSum = GetSum(width, state);
            }

            var sum = GetSum(width, state);
            if (time < generations)
                return sum + (sum - previousSum) * (generations - time - 1);

            return sum;
        }

        private static int GetSum(int width, IEnumerable<bool> state) => state.Select((t, i) => t ? i - width : 0).Sum();

        private static int GetKey(IReadOnlyList<bool> arr)
        {
            var output = 0;
            for (var i = 0; i < arr.Count; i++)
                if (arr[i])
                    output |= 1 << i;
            return output;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetFinalPlanSum(LoadInput(), 50000000000));
        }
    }
}