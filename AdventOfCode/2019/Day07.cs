using System;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day07 : BaseOpcodeDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(FindMaxAmpOutput("3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0"));
            Console.WriteLine(FindMaxAmpOutput(
                "3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0"));
            Console.WriteLine(FindMaxAmpOutput(
                "3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0"));
            Console.WriteLine(FindMaxAmpOutput(LoadInput().First()));
        }

        private static int FindMaxAmpOutput(string opcodeString)
        {
            var opcodes = ParseOpcodesFromString(opcodeString);

            var maxOutput = 0;
            for (var amp1Seed = 0; amp1Seed <= 4; amp1Seed++)
            for (var amp2Seed = 0; amp2Seed <= 4; amp2Seed++)
            for (var amp3Seed = 0; amp3Seed <= 4; amp3Seed++)
            for (var amp4Seed = 0; amp4Seed <= 4; amp4Seed++)
            for (var amp5Seed = 0; amp5Seed <= 4; amp5Seed++)
            {
                if (new[] {amp1Seed, amp2Seed, amp3Seed, amp4Seed, amp5Seed}.ToHashSet().Count != 5) continue;

                var amp1Output = new OpcodeVM((int[]) opcodes.Clone()).SendInput(amp1Seed).SendInput(0).Run()
                    .GetOutputs().First();
                var amp2Output = new OpcodeVM((int[]) opcodes.Clone()).SendInput(amp2Seed).SendInput(amp1Output).Run()
                    .GetOutputs().First();
                var amp3Output = new OpcodeVM((int[]) opcodes.Clone()).SendInput(amp3Seed).SendInput(amp2Output).Run()
                    .GetOutputs().First();
                var amp4Output = new OpcodeVM((int[]) opcodes.Clone()).SendInput(amp4Seed).SendInput(amp3Output).Run()
                    .GetOutputs().First();
                var amp5Output = new OpcodeVM((int[]) opcodes.Clone()).SendInput(amp5Seed).SendInput(amp4Output).Run()
                    .GetOutputs().First();

                maxOutput = Math.Max(maxOutput, amp5Output);
            }

            return maxOutput;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(
                FindMaxAmpOutputWithFeedback(
                    "3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5"));
            Console.WriteLine(FindMaxAmpOutputWithFeedback(
                "3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,-5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10"));
            Console.WriteLine(FindMaxAmpOutputWithFeedback(LoadInput().First()));
        }

        private static int FindMaxAmpOutputWithFeedback(string opcodeString)
        {
            var opcodes = ParseOpcodesFromString(opcodeString);

            var maxOutput = 0;
            for (var amp1Seed = 5; amp1Seed <= 9; amp1Seed++)
            for (var amp2Seed = 5; amp2Seed <= 9; amp2Seed++)
            for (var amp3Seed = 5; amp3Seed <= 9; amp3Seed++)
            for (var amp4Seed = 5; amp4Seed <= 9; amp4Seed++)
            for (var amp5Seed = 5; amp5Seed <= 9; amp5Seed++)
            {
                if (new[] {amp1Seed, amp2Seed, amp3Seed, amp4Seed, amp5Seed}.ToHashSet().Count != 5) continue;

                var amp1VM = new OpcodeVM((int[]) opcodes.Clone()).SendInput(amp1Seed);
                var amp2VM = new OpcodeVM((int[]) opcodes.Clone()).SendInput(amp2Seed);
                var amp3VM = new OpcodeVM((int[]) opcodes.Clone()).SendInput(amp3Seed);
                var amp4VM = new OpcodeVM((int[]) opcodes.Clone()).SendInput(amp4Seed);
                var amp5VM = new OpcodeVM((int[]) opcodes.Clone()).SendInput(amp5Seed);

                var amp5Output = 0;

                while (!amp5VM.IsFinished())
                {
                    var amp1Output = amp1VM.SendInput(amp5Output).Run().GetOutputs().Last();
                    var amp2Output = amp2VM.SendInput(amp1Output).Run().GetOutputs().Last();
                    var amp3Output = amp3VM.SendInput(amp2Output).Run().GetOutputs().Last();
                    var amp4Output = amp4VM.SendInput(amp3Output).Run().GetOutputs().Last();
                    amp5Output = amp5VM.SendInput(amp4Output).Run().GetOutputs().Last();
                }

                maxOutput = Math.Max(maxOutput, amp5Output);
            }

            return maxOutput;
        }
    }
}