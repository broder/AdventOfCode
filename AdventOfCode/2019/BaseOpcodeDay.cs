using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2019
{
    internal abstract class BaseOpcodeDay : BaseDay
    {
        protected static int[] ParseOpcodesFromString(string opcodesString) =>
            opcodesString.Split(",").Select(int.Parse).ToArray();

        protected class OpcodeVM
        {
            private readonly int[] Opcodes;
            private int CurrentIndex;
            private bool Finished;
            private readonly Queue<int> Inputs = new Queue<int>();
            private readonly List<int> Outputs = new List<int>();

            public OpcodeVM(string opcodeString) :
                this(ParseOpcodesFromString(opcodeString))
            {
            }

            public OpcodeVM(int[] opcodes)
            {
                Opcodes = opcodes;
            }

            public int[] GetOpcodes() => Opcodes;

            public void PrintOpcodes() => Console.WriteLine(string.Join(",", GetOpcodes()));

            public OpcodeVM SendInput(int input)
            {
                Inputs.Enqueue(input);
                return this;
            }

            public List<int> GetOutputs() => Outputs;

            public bool IsFinished() => Finished;

            public OpcodeVM Run()
            {
                while (!Finished)
                {
                    var instruction = Opcodes[CurrentIndex];
                    var opcode = instruction % 100;
                    var firstParameterMode = (instruction % 1000) / 100;
                    var firstParameterIndex =
                        firstParameterMode == 0 && CurrentIndex + 1 < Opcodes.Length
                            ? Opcodes[CurrentIndex + 1]
                            : CurrentIndex + 1;
                    var secondParameterMode = (instruction % 10000) / 1000;
                    var secondParameterIndex =
                        secondParameterMode == 0 && CurrentIndex + 2 < Opcodes.Length
                            ? Opcodes[CurrentIndex + 2]
                            : CurrentIndex + 2;
                    var thirdParameterMode = instruction / 10000;
                    var thirdParameterIndex =
                        thirdParameterMode == 0 && CurrentIndex + 3 < Opcodes.Length
                            ? Opcodes[CurrentIndex + 3]
                            : CurrentIndex + 3;
                    switch (opcode)
                    {
                        case 1:
                            Opcodes[thirdParameterIndex] = Opcodes[firstParameterIndex] + Opcodes[secondParameterIndex];
                            CurrentIndex += 4;
                            break;
                        case 2:
                            Opcodes[thirdParameterIndex] = Opcodes[firstParameterIndex] * Opcodes[secondParameterIndex];
                            CurrentIndex += 4;
                            break;
                        case 3:
                            if (Inputs.Count == 0) return this;
                            Opcodes[firstParameterIndex] = Inputs.Dequeue();
                            CurrentIndex += 2;
                            break;
                        case 4:
                            Outputs.Add(Opcodes[firstParameterIndex]);
                            CurrentIndex += 2;
                            break;
                        case 5:
                            if (Opcodes[firstParameterIndex] != 0)
                                CurrentIndex = Opcodes[secondParameterIndex];
                            else
                                CurrentIndex += 3;
                            break;
                        case 6:
                            if (Opcodes[firstParameterIndex] == 0)
                                CurrentIndex = Opcodes[secondParameterIndex];
                            else
                                CurrentIndex += 3;
                            break;
                        case 7:
                            Opcodes[thirdParameterIndex] =
                                Opcodes[firstParameterIndex] < Opcodes[secondParameterIndex] ? 1 : 0;
                            CurrentIndex += 4;
                            break;
                        case 8:
                            Opcodes[thirdParameterIndex] =
                                Opcodes[firstParameterIndex] == Opcodes[secondParameterIndex] ? 1 : 0;
                            CurrentIndex += 4;
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
        }
    }
}