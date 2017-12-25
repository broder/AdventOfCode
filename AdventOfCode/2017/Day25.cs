using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2017
{
    internal class Day25 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetChecksum(LoadInput("practice")));
            Console.WriteLine(GetChecksum(LoadInput()));
        }

        private static int GetChecksum(IReadOnlyList<string> input)
        {
            var steps = 0;
            string currentState = null;
            var states = new Dictionary<string, StateInfo>();
            for (var i = 0; i < input.Count; i++)
            {
                if (input[i].StartsWith("Begin in state"))
                    currentState =
                        input[i].Split(new[] {"Begin in state ", "."}, StringSplitOptions.RemoveEmptyEntries)[0];

                if (input[i].StartsWith("Perform a diagnostic checksum after"))
                    steps = int.Parse(input[i].Split(new[] {"Perform a diagnostic checksum after ", " steps."},
                        StringSplitOptions.RemoveEmptyEntries)[0]);

                if (!input[i].StartsWith("In state")) continue;

                var state = input[i].Split(new[] {"In state ", ":"}, StringSplitOptions.RemoveEmptyEntries)[0];
                i++;
                for (var j = 0; j < 2; j++)
                {
                    var ifValue = input[i].Contains("1");
                    i++;
                    var writeValue = input[i].Contains("1");
                    i++;
                    var moveValue = input[i].Contains("right") ? 1 : -1;
                    i++;
                    var nextStateValue = input[i].Split(new[] {"    - Continue with state ", "."},
                        StringSplitOptions.RemoveEmptyEntries)[0];
                    i++;

                    states[state + ifValue] = new StateInfo
                    {
                        Write = writeValue,
                        Move = moveValue,
                        NextState = nextStateValue
                    };
                }
            }

            var tape = new bool[steps];
            var currentPosition = steps / 2;

            for (var i = 0; i < steps; i++)
            {
                var info = states[currentState + tape[currentPosition]];
                tape[currentPosition] = info.Write;
                currentPosition += info.Move;
                currentState = info.NextState;
            }

            return tape.Count(v => v);
        }

        protected override void RunPartTwo()
        {
        }

        private struct StateInfo
        {
            public bool Write;
            public int Move;
            public string NextState;
        }
    }
}