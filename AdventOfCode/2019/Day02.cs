using System;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day02 : BaseOpcodeDay
    {
        protected override void RunPartOne()
        {
            var opcodes = ParseOpcodesFromString(LoadInput().First());
            opcodes[1] = 12;
            opcodes[2] = 2;
            Console.WriteLine(new OpcodeVM(opcodes).Run().GetMemory().First());
        }

        protected override void RunPartTwo()
        {
            var opcodes = ParseOpcodesFromString(LoadInput().First());
            var (noun, verb) = FindTargetNounAndVerb(opcodes, 19690720);
            Console.WriteLine(100 * noun + verb);
        }

        private static (int noun, int verb) FindTargetNounAndVerb(long[] opcodes, int target)
        {
            for (var noun = 0; noun < opcodes.Length; noun += 1)
            {
                for (var verb = 0; verb < opcodes.Length; verb += 1)
                {
                    opcodes[1] = noun;
                    opcodes[2] = verb;
                    if (new OpcodeVM(opcodes).Run().GetMemory().First() == target)
                    {
                        return (noun, verb);
                    }
                }
            }

            throw new ArgumentException();
        }
    }
}