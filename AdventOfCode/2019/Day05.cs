using System;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day05 : BaseDay
    {
        protected override void RunPartOne()
        {
            RunOpcodes(ParseOpcodes(LoadInput().First()), 1);
        }

        private static int[] ParseOpcodes(string opcodesString)
        {
            return opcodesString.Split(",").Select(int.Parse).ToArray();
        }

        private static void RunOpcodes(int[] opcodes, int input)
        {
            var i = 0;
            while (i < opcodes.Length)
            {
                var instruction = opcodes[i];
                var opcode = instruction % 100;
                var firstParameterMode = (instruction % 1000) / 100;
                var firstParameterIndex = firstParameterMode == 0 && i + 1 < opcodes.Length ? opcodes[i + 1] : i + 1;
                var secondParameterMode = (instruction % 10000) / 1000;
                var secondParameterIndex = secondParameterMode == 0 && i + 2 < opcodes.Length ? opcodes[i + 2] : i + 2;
                var thirdParameterMode = instruction / 10000;
                var thirdParameterIndex = thirdParameterMode == 0 && i + 3 < opcodes.Length ? opcodes[i + 3] : i + 3;
                switch (opcode)
                {
                    case 1:
                        opcodes[thirdParameterIndex] = opcodes[firstParameterIndex] + opcodes[secondParameterIndex];
                        i += 4;
                        break;
                    case 2:
                        opcodes[thirdParameterIndex] = opcodes[firstParameterIndex] * opcodes[secondParameterIndex];
                        i += 4;
                        break;
                    case 3:
                        opcodes[firstParameterIndex] = input;
                        i += 2;
                        break;
                    case 4:
                        Console.WriteLine(opcodes[firstParameterIndex]);
                        i += 2;
                        break;
                    case 5:
                        if (opcodes[firstParameterIndex] != 0)
                            i = opcodes[secondParameterIndex];
                        else
                            i += 3;
                        break;
                    case 6:
                        if (opcodes[firstParameterIndex] == 0)
                            i = opcodes[secondParameterIndex];
                        else
                            i += 3;
                        break;
                    case 7:
                        opcodes[thirdParameterIndex] =
                            opcodes[firstParameterIndex] < opcodes[secondParameterIndex] ? 1 : 0;
                        i += 4;
                        break;
                    case 8:
                        opcodes[thirdParameterIndex] =
                            opcodes[firstParameterIndex] == opcodes[secondParameterIndex] ? 1 : 0;
                        i += 4;
                        break;
                    case 99:
                        i = opcodes.Length;
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
        }

        protected override void RunPartTwo()
        {
            RunOpcodes(ParseOpcodes("3,9,8,9,10,9,4,9,99,-1,8"), 7);
            RunOpcodes(ParseOpcodes("3,9,8,9,10,9,4,9,99,-1,8"), 8);

            RunOpcodes(ParseOpcodes("3,9,7,9,10,9,4,9,99,-1,8"), 8);
            RunOpcodes(ParseOpcodes("3,9,7,9,10,9,4,9,99,-1,8"), 7);

            RunOpcodes(ParseOpcodes("3,3,1108,-1,8,3,4,3,99"), 7);
            RunOpcodes(ParseOpcodes("3,3,1108,-1,8,3,4,3,99"), 8);

            RunOpcodes(ParseOpcodes("3,3,1107,-1,8,3,4,3,99"), 8);
            RunOpcodes(ParseOpcodes("3,3,1107,-1,8,3,4,3,99"), 7);

            RunOpcodes(ParseOpcodes("3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9"), 0);
            RunOpcodes(ParseOpcodes("3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9"), 1);

            RunOpcodes(ParseOpcodes("3,3,1105,-1,9,1101,0,0,12,4,12,99,1"), 0);
            RunOpcodes(ParseOpcodes("3,3,1105,-1,9,1101,0,0,12,4,12,99,1"), 1);

            RunOpcodes(
                ParseOpcodes(
                    "3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99"),
                7);
            RunOpcodes(
                ParseOpcodes(
                    "3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99"),
                8);
            RunOpcodes(
                ParseOpcodes(
                    "3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99"),
                9);

            RunOpcodes(ParseOpcodes(LoadInput().First()), 5);
        }
    }
}