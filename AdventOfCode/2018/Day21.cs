using System;
using System.Collections.Generic;

namespace AdventOfCode._2018
{
    internal class Day21 : BaseOpCodeDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetHaltingFirstRegisterValueForFewestInstructions(LoadInput()));
        }
        
        private static int GetHaltingFirstRegisterValueForFewestInstructions(IReadOnlyList<string> input)
        {
            var (instructions, instructionRegister) = ParseInstructions(input);
            var registers = new int[6];
            while (registers[instructionRegister] < instructions.Length)
            {
                var instruction = instructions[registers[instructionRegister]];
                registers = RunInstruction(registers, instruction);
                registers[instructionRegister]++;
                
                // L28 checks if r[0] == r[1]. If it is, the program halts. So we should return the value of r[1]
                if (registers[instructionRegister] == 28)
                    return registers[1];
            }

            return -1;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetHaltingFirstRegisterValueForMostInstructions(LoadInput()));
        }
        
        private static int GetHaltingFirstRegisterValueForMostInstructions(IReadOnlyList<string> input)
        {
            var (instructions, instructionRegister) = ParseInstructions(input);
            var seen = new HashSet<int>();
            var lastValue = 0;
            var registers = new int[6];
            while (registers[instructionRegister] < instructions.Length)
            {
                var instruction = instructions[registers[instructionRegister]];
                registers = RunInstruction(registers, instruction);
                registers[instructionRegister]++;
                
                // L28 checks if r[0] == r[1]. If it is, the program halts.
                if (registers[instructionRegister] != 28)
                    continue;
                
                if (!seen.Add(registers[1]))
                    return lastValue;
                
                lastValue = registers[1];
            }

            return -1;
        }
    }
}