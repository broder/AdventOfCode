using System;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day02 : BaseOpcodeDay
    {
        protected override void RunPartOne()
        {
            new OpcodeVM("1,9,10,3,2,3,11,0,99,30,40,50").Run().PrintOpcodes();
            new OpcodeVM("1,0,0,0,99").Run().PrintOpcodes();
            new OpcodeVM("2,3,0,3,99").Run().PrintOpcodes();
            new OpcodeVM("2,4,4,5,99,0").Run().PrintOpcodes();
            new OpcodeVM("1,1,1,4,99,5,6,0,99").Run().PrintOpcodes();
            var opcodes = ParseOpcodesFromString(LoadInput().First());
            opcodes[1] = 12;
            opcodes[2] = 2;
            Console.WriteLine(new OpcodeVM(opcodes).Run().GetOpcodes().First());
        }

        protected override void RunPartTwo()
        {
            var opcodes = ParseOpcodesFromString(LoadInput().First());
            var (noun, verb) = FindTargetNounAndVerb(opcodes, 19690720);
            Console.WriteLine(100 * noun + verb);
        }

        private static (int noun, int verb) FindTargetNounAndVerb(int[] opcodes, int target)
        {
            for (var noun = 0; noun < opcodes.Length; noun += 1)
            {
                for (var verb = 0; verb < opcodes.Length; verb += 1)
                {
                    opcodes[1] = noun;
                    opcodes[2] = verb;
                    if (new OpcodeVM((int[]) opcodes.Clone()).Run().GetOpcodes().First() == target)
                    {
                        return (noun, verb);
                    }
                }
            }

            throw new ArgumentException();
        }
    }
}