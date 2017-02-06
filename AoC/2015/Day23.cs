using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC._2015
{
    internal class Day23 : BaseDay
    {
        private readonly Regex HalfRegex = new Regex(@"hlf (.+)$");
        private readonly Regex TripleRegex = new Regex(@"tpl (.+)$");
        private readonly Regex IncRegex = new Regex(@"inc (.+)$");
        private readonly Regex JumpRegex = new Regex(@"jmp ([-+\d]+)$");
        private readonly Regex JumpEvenRegex = new Regex(@"jie (.+?), ([-+\d]+)$");
        private readonly Regex JumpOneRegex = new Regex(@"jio (.+?), ([-+\d]+)$");
        private readonly string[] RegisterNames = {"a", "b"};

        public override void RunPartOne()
        {
            Console.WriteLine(GetRegisterValue(0, 0, "practice"));
            Console.WriteLine(GetRegisterValue(1));
        }

        private int GetRegisterValue(int outIndex, int aStart = 0, string fileVariant = null)
        {
            var lines = LoadInput(fileVariant);
            var registers = new int[RegisterNames.Length];
            registers[0] = aStart;
            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines.ElementAt(i);
                var halfMatch = HalfRegex.Match(line);
                var tripleMatch = TripleRegex.Match(line);
                var incMatch = IncRegex.Match(line);
                var jumpMatch = JumpRegex.Match(line);
                var jumpEvenMatch = JumpEvenRegex.Match(line);
                var jumpOneMatch = JumpOneRegex.Match(line);
                if (halfMatch.Success)
                {
                    if (RegisterNames.Contains(halfMatch.Groups[1].Value))
                        registers[halfMatch.Groups[1].Value[0] - 'a'] /= 2;
                }
                else if (tripleMatch.Success)
                {
                    if (RegisterNames.Contains(tripleMatch.Groups[1].Value))
                        registers[tripleMatch.Groups[1].Value[0] - 'a'] *= 3;
                }
                else if (incMatch.Success)
                {
                    if (RegisterNames.Contains(incMatch.Groups[1].Value))
                        registers[incMatch.Groups[1].Value[0] - 'a'] += 1;
                }
                else if (jumpMatch.Success)
                {
                    i += GetValue(registers, jumpMatch.Groups[1].Value) - 1;
                }
                else if (jumpEvenMatch.Success)
                {
                    if (GetValue(registers, jumpEvenMatch.Groups[1].Value) % 2 == 0)
                        i += GetValue(registers, jumpEvenMatch.Groups[2].Value) - 1;
                }
                else if (jumpOneMatch.Success)
                {
                    if (GetValue(registers, jumpOneMatch.Groups[1].Value) == 1)
                        i += GetValue(registers, jumpOneMatch.Groups[2].Value) - 1;
                }
            }
            return registers[outIndex];
        }

        private int GetValue(int[] registers, string input)
        {
            return RegisterNames.Contains(input) ? registers[input[0] - 'a'] : int.Parse(input);
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(GetRegisterValue(1, 1));
        }
    }
}