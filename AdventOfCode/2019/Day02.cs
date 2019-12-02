using System;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day02 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(string.Join(",", RunOpcodes(ParseOpcodes("1,9,10,3,2,3,11,0,99,30,40,50"))));
            Console.WriteLine(string.Join(",", RunOpcodes(ParseOpcodes("1,0,0,0,99"))));
            Console.WriteLine(string.Join(",", RunOpcodes(ParseOpcodes("2,3,0,3,99"))));
            Console.WriteLine(string.Join(",", RunOpcodes(ParseOpcodes("2,4,4,5,99,0"))));
            Console.WriteLine(string.Join(",", RunOpcodes(ParseOpcodes("1,1,1,4,99,5,6,0,99"))));
            var opcodes = ParseOpcodes(LoadInput().First());
            opcodes[1] = 12;
            opcodes[2] = 2;
            Console.WriteLine(RunOpcodes(opcodes)[0]);
        }

        private static int[] ParseOpcodes(string opcodesString)
        {
            return opcodesString.Split(",").Select(int.Parse).ToArray();
        }

        private static int[] RunOpcodes(int[] opcodes)
        {
            for (var i = 0; i < opcodes.Length; i += 4)
            {
                switch (opcodes[i])
                {
                    case 1:
                        opcodes[opcodes[i + 3]] = opcodes[opcodes[i + 1]] + opcodes[opcodes[i + 2]];
                        break;
                    case 2:
                        opcodes[opcodes[i + 3]] = opcodes[opcodes[i + 1]] * opcodes[opcodes[i + 2]];
                        break;
                    case 99:
                        i = opcodes.Length;
                        break;
                    default:
                        throw new ArgumentException();
                }
            }

            return opcodes;
        }

        protected override void RunPartTwo()
        {
            var opcodes = ParseOpcodes(LoadInput().First());
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
                    if (RunOpcodes((int[]) opcodes.Clone())[0] == target)
                    {
                        return (noun, verb);
                    }
                }
            }

            throw new ArgumentException();
        }
    }
}