using System;
using System.Collections.Generic;

namespace AdventOfCode._2020
{
    internal class Day08 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(RunProgram(LoadInput("practice")).Item2);
            Console.WriteLine(RunProgram(LoadInput()).Item2);
        }

        private static Tuple<bool, int> RunProgram(string[] instructions)
        {
            var infinite = false;
            var acc = 0;
            var currentInstruction = 0;
            var executedInstructions = new HashSet<int>();
            while (currentInstruction < instructions.Length)
            {
                if (executedInstructions.Contains(currentInstruction))
                {
                    infinite = true;
                    break;
                }

                executedInstructions.Add(currentInstruction);
                var s = instructions[currentInstruction].Split(' ');

                switch (s[0])
                {
                    case "nop":
                        currentInstruction += 1;
                        continue;
                    case "acc":
                        acc += int.Parse(s[1]);
                        currentInstruction += 1;
                        continue;
                    case "jmp":
                        currentInstruction += int.Parse(s[1]);
                        continue;
                    default:
                        throw new Exception(s[0]);
                }

            }

            return new Tuple<bool, int>(infinite, acc);
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(FindNonInfiniteProgram(LoadInput("practice")));
            Console.WriteLine(FindNonInfiniteProgram(LoadInput()));
        }

        private static int FindNonInfiniteProgram(string[] instructions)
        {
            for (var i = 0; i < instructions.Length; i++)
            {
                if (instructions[i].StartsWith("jmp"))
                {
                    instructions[i] = instructions[i].Replace("jmp", "nop");

                    var output = RunProgram(instructions);
                    if (!output.Item1)
                        return output.Item2;

                    instructions[i] = instructions[i].Replace("nop", "jmp");
                }
                else if (instructions[i].StartsWith("nop"))
                {
                    instructions[i] = instructions[i].Replace("nop", "jmp");

                    var output = RunProgram(instructions);
                    if (!output.Item1)
                        return output.Item2;

                    instructions[i] = instructions[i].Replace("jmp", "nop");
                }
            }
            return -1;
        }
    }
}