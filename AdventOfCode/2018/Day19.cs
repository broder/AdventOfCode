using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018
{
    
    internal class Day19 : BaseOpCodeDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetFirstRegisterValue(LoadInput("practice")));
            Console.WriteLine(GetFirstRegisterValue(LoadInput()));
        }

        private static int GetFirstRegisterValue(IReadOnlyList<string> input, int firstRegisterValue = 0)
        {
            var (instructions, instructionRegister) = ParseInstructions(input);

            var registers = new int[6];
            registers[0] = firstRegisterValue;
            while (registers[instructionRegister] < instructions.Length)
            {
                if (instructionRegister == 3 && registers[instructionRegister] == 3)
                {
                    if (registers[4] % registers[5] == 0)
                        registers[0] += registers[5];
                    registers[1] = 0;
                    registers[2] = registers[4];
                    registers[instructionRegister] = 12;
                    continue;
                }
                var instruction = instructions[registers[instructionRegister]];
                registers = RunInstruction(registers, instruction);
                registers[instructionRegister]++;
            }

            return registers[0];
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetFirstRegisterValue(LoadInput(), 1));
        }
    }
}