using System;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day16 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetOutputAfterPhases("12345678", 4));
            Console.WriteLine(GetOutputAfterPhases("80871224585914546619083218645595", 100).Substring(0, 8));
            Console.WriteLine(GetOutputAfterPhases("19617804207202209144916044189917", 100).Substring(0, 8));
            Console.WriteLine(GetOutputAfterPhases("69317163492948606335995924319873", 100).Substring(0, 8));
            Console.WriteLine(GetOutputAfterPhases(LoadInput().First(), 100).Substring(0, 8));
        }

        private static string GetOutputAfterPhases(string inputSignal, int phases)
        {
            var currentState = inputSignal.Select(c => c - '0').ToArray();
            for (var i = 0; i < phases; i++)
                currentState = TransformState(currentState);

            return string.Join("", currentState);
        }

        private static int[] TransformState(int[] input)
        {
            var output = new int[input.Length];
            for (var i = 0; i < input.Length; i++)
                output[i] = Math.Abs(input.Select((inp, j) => inp * GetTransformMultiplier(j, i + 1)).Sum()) % 10;

            return output;
        }

        private static int GetTransformMultiplier(int i, int length)
        {
            i = (i + 1) % (4 * length);
            if (i < length) return 0;
            if (i < 2 * length) return 1;
            if (i < 3 * length) return 0;
            return -1;
        }

        protected override void RunPartTwo()
        {
            var inputSignal = LoadInput().First();
            var offset = int.Parse(inputSignal.Substring(0, 7));
            Console.WriteLine(GetOutputAfterPhases(LoadInput().First(), 100, 10000, offset).Substring(0, 8));
        }
        
        private static string GetOutputAfterPhases(string inputSignal, int phases, int repeat, int offset)
        {
            var initialState = inputSignal.Select(c => c - '0').ToArray();
            var currentState = new int[initialState.Length * repeat - offset];
            for (var i = 0; i < currentState.Length; i++)
                currentState[i] = initialState[(i + offset) % initialState.Length];

            if (offset < initialState.Length * repeat / 2)
                throw new ArgumentException();
            
            for (var i = 0; i < phases; i++)
                currentState = TransformStateForLargeOffset(currentState);

            return string.Join("", currentState);
        }

        private static int[] TransformStateForLargeOffset(int[] input)
        {
            var output = new int[input.Length];
            var sum = input.Sum();
            for (var i = 0; i < input.Length; i++)
            {
                output[i] = Math.Abs(sum) % 10;
                sum -= input[i];
            }

            return output;
        }
    }
}