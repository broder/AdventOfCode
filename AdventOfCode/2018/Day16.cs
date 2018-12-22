using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018
{
    internal class Day16 : BaseOpCodeDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetAmbiguousSamples(LoadInput("practice")));
            Console.WriteLine(GetAmbiguousSamples(LoadInput()));
        }

        private static int GetAmbiguousSamples(IReadOnlyList<string> input)
        {
            var (samples, _) = ParseSamples(input);

            var ambiguousSampleCount = 0;
            foreach (var sample in samples)
            {
                var correctInstructionCount = 0;
                foreach (var opCode in Enum.GetValues(typeof(OpCode)).Cast<OpCode>())
                {
                    var instruction = new Instruction {OpCode = opCode, A = sample.A, B = sample.B, C = sample.C};
                    var instructionRegisters = RunInstruction(sample.RegistersBefore, instruction);
                    if (instructionRegisters.SequenceEqual(sample.RegistersAfter))
                        correctInstructionCount++;
                }

                if (correctInstructionCount >= 3)
                    ambiguousSampleCount++;
            }

            return ambiguousSampleCount;
        }

        private static (IEnumerable<Sample> samples, int nextIndex) ParseSamples(IReadOnlyList<string> input)
        {
            var samples = new List<Sample>();
            var i = 0;
            while (i < input.Count && input[i].StartsWith("Before"))
            {
                var sample = new Sample();
                var beforeSplit = input[i].Split(new[] {"Before: [", ", ", "]"}, StringSplitOptions.RemoveEmptyEntries);
                sample.RegistersBefore = beforeSplit.Select(int.Parse).ToArray();

                var sampleSplit = input[i + 1].Split(' ').Select(int.Parse).ToArray();
                sample.UnmappedOpCode = sampleSplit[0];
                sample.A = sampleSplit[1];
                sample.B = sampleSplit[2];
                sample.C = sampleSplit[3];

                var afterSplit = input[i + 2]
                    .Split(new[] {"After:  [", ", ", "]"}, StringSplitOptions.RemoveEmptyEntries);
                sample.RegistersAfter = afterSplit.Select(int.Parse).ToArray();

                samples.Add(sample);
                i += 4;
            }

            return (samples, i);
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(RunSimulation(LoadInput()));
        }

        private static int RunSimulation(IReadOnlyList<string> input)
        {
            var (samples, i) = ParseSamples(input);

            var potentialOpCodeMapping = new Dictionary<int, List<OpCode>>();
            foreach (var sample in samples)
            {
                if (!potentialOpCodeMapping.ContainsKey(sample.UnmappedOpCode))
                    potentialOpCodeMapping[sample.UnmappedOpCode] =
                        Enum.GetValues(typeof(OpCode)).Cast<OpCode>().ToList();

                var validOpCodes = new List<OpCode>();
                foreach (var opCode in Enum.GetValues(typeof(OpCode)).Cast<OpCode>())
                {
                    var instruction = new Instruction {OpCode = opCode, A = sample.A, B = sample.B, C = sample.C};
                    var instructionRegisters = RunInstruction(sample.RegistersBefore, instruction);
                    if (instructionRegisters.SequenceEqual(sample.RegistersAfter))
                        validOpCodes.Add(opCode);
                }

                potentialOpCodeMapping[sample.UnmappedOpCode] =
                    potentialOpCodeMapping[sample.UnmappedOpCode].Intersect(validOpCodes).ToList();
            }

            var opCodeMapping = new Dictionary<int, OpCode>();
            while (potentialOpCodeMapping.Any(kvp => kvp.Value.Count == 1))
            {
                var actualMapping = potentialOpCodeMapping.First(kvp => kvp.Value.Count == 1);
                opCodeMapping[actualMapping.Key] = actualMapping.Value.First();
                potentialOpCodeMapping.Remove(actualMapping.Key);

                foreach (var (_, value) in potentialOpCodeMapping)
                {
                    value.Remove(actualMapping.Value.First());
                }
            }

            var registers = new int[4];

            i += 2;
            while (i < input.Count)
            {
                var split = input[i].Split(' ').Select(int.Parse).ToArray();
                var instruction = new Instruction
                    {OpCode = opCodeMapping[split[0]], A = split[1], B = split[2], C = split[3]};
                registers = RunInstruction(registers, instruction);
                i++;
            }

            return registers[0];
        }

        private class Sample
        {
            public int[] RegistersBefore;
            public int UnmappedOpCode;
            public int A;
            public int B;
            public int C;
            public int[] RegistersAfter;
        }
    }
}