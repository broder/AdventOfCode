using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2019
{
    internal abstract class BaseOpcodeDay : BaseDay
    {
        protected static long[] ParseOpcodesFromString(string opcodesString) =>
            opcodesString.Split(",").Select(long.Parse).ToArray();

        protected class OpcodeVM
        {
            private readonly long[] Memory;
            private long CurrentIndex;
            private long RelativeBase;
            private bool Finished;
            private readonly Queue<long> Inputs = new Queue<long>();
            private readonly List<long> Outputs = new List<long>();

            public OpcodeVM(string opcodeString, int memorySize = 2000) :
                this(ParseOpcodesFromString(opcodeString), memorySize)
            {
            }

            public OpcodeVM(long[] opcodes, int memorySize = 2000)
            {
                Memory = new long[memorySize];
                for (var i = 0; i < opcodes.Length; i++)
                    Memory[i] = opcodes[i];
            }

            public long[] GetMemory() => Memory;

            public OpcodeVM SendInput(long input)
            {
                Inputs.Enqueue(input);
                return this;
            }

            public List<long> GetOutputs() => Outputs;

            public void PrintOutputs() => Console.WriteLine(string.Join(",", GetOutputs()));

            public bool IsFinished() => Finished;

            public OpcodeVM Run()
            {
                while (!Finished)
                {
                    var instruction = Memory[CurrentIndex];
                    var opcode = instruction % 100;
                    var firstParameterIndex = GetParameterIndex(instruction / 100 % 10, CurrentIndex + 1);
                    var secondParameterIndex = GetParameterIndex(instruction / 1000 % 10, CurrentIndex + 2);
                    var thirdParameterIndex = GetParameterIndex(instruction / 10000, CurrentIndex + 3);
                    switch (opcode)
                    {
                        case 1:
                            Memory[thirdParameterIndex] = Memory[firstParameterIndex] + Memory[secondParameterIndex];
                            CurrentIndex += 4;
                            break;
                        case 2:
                            Memory[thirdParameterIndex] = Memory[firstParameterIndex] * Memory[secondParameterIndex];
                            CurrentIndex += 4;
                            break;
                        case 3:
                            if (Inputs.Count == 0) return this;
                            Memory[firstParameterIndex] = Inputs.Dequeue();
                            CurrentIndex += 2;
                            break;
                        case 4:
                            Outputs.Add(Memory[firstParameterIndex]);
                            CurrentIndex += 2;
                            break;
                        case 5:
                            if (Memory[firstParameterIndex] != 0)
                                CurrentIndex = Memory[secondParameterIndex];
                            else
                                CurrentIndex += 3;
                            break;
                        case 6:
                            if (Memory[firstParameterIndex] == 0)
                                CurrentIndex = Memory[secondParameterIndex];
                            else
                                CurrentIndex += 3;
                            break;
                        case 7:
                            Memory[thirdParameterIndex] =
                                Memory[firstParameterIndex] < Memory[secondParameterIndex] ? 1 : 0;
                            CurrentIndex += 4;
                            break;
                        case 8:
                            Memory[thirdParameterIndex] =
                                Memory[firstParameterIndex] == Memory[secondParameterIndex] ? 1 : 0;
                            CurrentIndex += 4;
                            break;
                        case 9:
                            RelativeBase += Memory[firstParameterIndex];
                            CurrentIndex += 2;
                            break;
                        case 99:
                            Finished = true;
                            return this;
                        default:
                            throw new ArgumentException();
                    }
                }

                return this;
            }

            private long GetParameterIndex(long mode, long rawIndex)
            {
                switch (mode)
                {
                    case 0:
                        return Memory[rawIndex];
                    case 1:
                        return rawIndex;
                    case 2:
                        return Memory[rawIndex] + RelativeBase;
                    default:
                        throw new ArgumentException();
                }
            }
        }
    }
}