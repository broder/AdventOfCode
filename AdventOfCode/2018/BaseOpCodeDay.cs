using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018
{
    internal abstract class BaseOpCodeDay : BaseDay
    {
        protected static (Instruction[] instructions, int intructionRegister) ParseInstructions(IReadOnlyList<string> input)
        {
            var instructionRegister = int.Parse(input.First().Remove(0, 4));
            var instructions = input.Skip(1).Select(instruction =>
            {
                var split = instruction.Split(' ');
                return new Instruction
                {
                    OpCode = Enum.Parse<OpCode>(split[0]), A = int.Parse(split[1]), B = int.Parse(split[2]),
                    C = int.Parse(split[3])
                };
            }).ToArray();
            return (instructions, instructionRegister);
        }

        protected static int[] RunInstruction(int[] registers, Instruction instruction)
        {
            var nextRegisters = (int[]) registers.Clone();
            switch (instruction.OpCode)
            {
                case OpCode.addr:
                    nextRegisters[instruction.C] = registers[instruction.A] + registers[instruction.B];
                    break;
                case OpCode.addi:
                    nextRegisters[instruction.C] = registers[instruction.A] + instruction.B;
                    break;
                case OpCode.mulr:
                    nextRegisters[instruction.C] = registers[instruction.A] * registers[instruction.B];
                    break;
                case OpCode.muli:
                    nextRegisters[instruction.C] = registers[instruction.A] * instruction.B;
                    break;
                case OpCode.banr:
                    nextRegisters[instruction.C] = registers[instruction.A] & registers[instruction.B];
                    break;
                case OpCode.bani:
                    nextRegisters[instruction.C] = registers[instruction.A] & instruction.B;
                    break;
                case OpCode.borr:
                    nextRegisters[instruction.C] = registers[instruction.A] | registers[instruction.B];
                    break;
                case OpCode.bori:
                    nextRegisters[instruction.C] = registers[instruction.A] | instruction.B;
                    break;
                case OpCode.setr:
                    nextRegisters[instruction.C] = registers[instruction.A];
                    break;
                case OpCode.seti:
                    nextRegisters[instruction.C] = instruction.A;
                    break;
                case OpCode.gtir:
                    nextRegisters[instruction.C] = instruction.A > registers[instruction.B] ? 1 : 0;
                    break;
                case OpCode.gtri:
                    nextRegisters[instruction.C] = registers[instruction.A] > instruction.B ? 1 : 0;
                    break;
                case OpCode.gtrr:
                    nextRegisters[instruction.C] = registers[instruction.A] > registers[instruction.B] ? 1 : 0;
                    break;
                case OpCode.eqir:
                    nextRegisters[instruction.C] = instruction.A == registers[instruction.B] ? 1 : 0;
                    break;
                case OpCode.eqri:
                    nextRegisters[instruction.C] = registers[instruction.A] == instruction.B ? 1 : 0;
                    break;
                default: // OpCode.eqrr
                    nextRegisters[instruction.C] = registers[instruction.A] == registers[instruction.B] ? 1 : 0;
                    break;
            }

            return nextRegisters;
        }

        protected class Instruction
        {
            public OpCode OpCode;
            public int A;
            public int B;
            public int C;
        }

        protected enum OpCode
        {
            addr,
            addi,
            mulr,
            muli,
            banr,
            bani,
            borr,
            bori,
            setr,
            seti,
            gtir,
            gtri,
            gtrr,
            eqir,
            eqri,
            eqrr
        }
    }
}