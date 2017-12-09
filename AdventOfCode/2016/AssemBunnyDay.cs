using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2016
{
    internal class AssemBunnyDay : BaseDay
    {
        private readonly Regex CopyRegex = new Regex(@"cpy ([-\da-z]+) (.+?)$");
        private readonly Regex IncRegex = new Regex(@"inc (.+)$");
        private readonly Regex DecRegex = new Regex(@"dec (.+)$");
        private readonly Regex JumpRegex = new Regex(@"jnz (.+?) (.+?)$");
        private readonly Regex ToggleRegex = new Regex(@"tgl ([-\da-z]+)$");
        private readonly Regex OutRegex = new Regex(@"out ([-\da-z]+)$");
        private readonly string[] RegisterNames = {"a", "b", "c", "d"};

        protected const int OUTPUT_ALTERNATING = int.MinValue;

        protected int RunSimulation(int[] initialValues, string fileVariant = null)
        {
            var lines = LoadInput(fileVariant);
            var outputLastValue = -1;
            var outputCount = 0;
            var registers = initialValues;
            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var copyMatch = CopyRegex.Match(line);
                var incMatch = IncRegex.Match(line);
                var decMatch = DecRegex.Match(line);
                var jumpMatch = JumpRegex.Match(line);
                var toggleMatch = ToggleRegex.Match(line);
                var outMatch = OutRegex.Match(line);
                if (copyMatch.Success)
                {
                    var copyVal = GetValue(registers, copyMatch.Groups[1].Value);
                    if (RegisterNames.Contains(copyMatch.Groups[2].Value))
                        registers[copyMatch.Groups[2].Value[0] - 'a'] = copyVal;
                }
                else if (incMatch.Success)
                {
                    if (i - 1 >= 0 && i + 4 < lines.Length && lines[i - 1].StartsWith("cpy") &&
                        lines[i + 1].StartsWith("dec") && lines[i + 2].StartsWith("jnz") &&
                        lines[i + 3].StartsWith("dec") && lines[i + 4].StartsWith("jnz"))
                    {
                        var cpy = lines[i - 1].Split(' ').Skip(1).ToArray();
                        var inc = lines[i].Split(' ').Last();
                        var dec = lines[i + 3].Split(' ').Last();

                        if (lines[i + 1].EndsWith(cpy[1]) && lines[i + 2].EndsWith($"{cpy[1]} -2") &&
                            lines[i + 4].EndsWith($"{dec} -5"))
                        {
                            registers[inc[0] - 'a'] += GetValue(registers, cpy[0]) * GetValue(registers, dec);
                            registers[cpy[1][0] - 'a'] = 0;
                            registers[dec[0] - 'a'] = 0;
                            i += 4;
                            continue;
                        }
                    }
                    if (RegisterNames.Contains(incMatch.Groups[1].Value))
                        registers[incMatch.Groups[1].Value[0] - 'a'] += 1;
                }
                else if (decMatch.Success)
                {
                    if (RegisterNames.Contains(decMatch.Groups[1].Value))
                        registers[decMatch.Groups[1].Value[0] - 'a'] -= 1;
                }
                else if (jumpMatch.Success)
                {
                    if (GetValue(registers, jumpMatch.Groups[1].Value) != 0)
                        i += GetValue(registers, jumpMatch.Groups[2].Value) - 1;
                }
                else if (toggleMatch.Success)
                {
                    var toggleIndex = i + GetValue(registers, toggleMatch.Groups[1].Value);

                    if (toggleIndex < 0 || toggleIndex >= lines.Length) continue;

                    var toggleLine = lines[toggleIndex];
                    var copyToggleMatch = CopyRegex.Match(toggleLine);
                    var incToggleMatch = IncRegex.Match(toggleLine);
                    var decToggleMatch = DecRegex.Match(toggleLine);
                    var jumpToggleMatch = JumpRegex.Match(toggleLine);
                    var toggleToggleMatch = ToggleRegex.Match(toggleLine);
                    var outToggleMatch = OutRegex.Match(toggleLine);
                    if (copyToggleMatch.Success)
                    {
                        toggleLine = $"jnz {copyToggleMatch.Groups[1].Value} {copyToggleMatch.Groups[2].Value}";
                    }
                    else if (incToggleMatch.Success)
                    {
                        toggleLine = $"dec {incToggleMatch.Groups[1].Value}";
                    }
                    else if (decToggleMatch.Success)
                    {
                        toggleLine = $"inc {decToggleMatch.Groups[1].Value}";
                    }
                    else if (jumpToggleMatch.Success)
                    {
                        toggleLine = $"cpy {jumpToggleMatch.Groups[1].Value} {jumpToggleMatch.Groups[2].Value}";
                    }
                    else if (toggleToggleMatch.Success)
                    {
                        toggleLine = $"inc {toggleToggleMatch.Groups[1].Value}";
                    }
                    else if (outToggleMatch.Success)
                    {
                        toggleLine = $"inc {outToggleMatch.Groups[1].Value}";
                    }
                    lines[toggleIndex] = toggleLine;
                }
                else if (outMatch.Success)
                {
                    var val = GetValue(registers, outMatch.Groups[1].Value);
                    if (outputCount == 0 && val == 0 || outputLastValue == 0 && val == 1 ||
                        outputLastValue == 1 && val == 0)
                    {
                        outputLastValue = val;
                        outputCount++;
                        if (outputCount == 100)
                        {
                            return OUTPUT_ALTERNATING;
                        }
                    }
                    else
                        throw new InvalidOperationException("Invalid output!");
                }
            }
            return registers[0];
        }

        private int GetValue(int[] registers, string input)
        {
            return RegisterNames.Contains(input) ? registers[input[0] - 'a'] : int.Parse(input);
        }

        protected override void RunPartOne()
        {
            throw new NotImplementedException();
        }

        protected override void RunPartTwo()
        {
            throw new NotImplementedException();
        }
    }
}