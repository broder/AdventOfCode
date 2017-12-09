using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2015
{
    internal class Day07 : BaseDay
    {
        private readonly Regex AndRegex = new Regex(@"(.+) AND (.+) -> (.+)");
        private readonly Regex OrRegex = new Regex(@"(.+) OR (.+) -> (.+)");
        private readonly Regex LShiftRegex = new Regex(@"(.+) LSHIFT (\d+) -> (.+)");
        private readonly Regex RShiftRegex = new Regex(@"(.+) RSHIFT (\d+) -> (.+)");
        private readonly Regex NotRegex = new Regex(@"NOT (.+) -> (.+)");
        private readonly Regex SetRegex = new Regex(@"(.+) -> (.+)");

        protected override void RunPartOne()
        {
            Console.WriteLine(GetWireOutputFromFile("d", "practice"));
            Console.WriteLine(GetWireOutputFromFile("e", "practice"));
            Console.WriteLine(GetWireOutputFromFile("f", "practice"));
            Console.WriteLine(GetWireOutputFromFile("g", "practice"));
            Console.WriteLine(GetWireOutputFromFile("h", "practice"));
            Console.WriteLine(GetWireOutputFromFile("i", "practice"));
            Console.WriteLine(GetWireOutputFromFile("x", "practice"));
            Console.WriteLine(GetWireOutputFromFile("y", "practice"));
            Console.WriteLine(GetWireOutputFromFile("a"));
        }

        private int GetWireOutputFromFile(string outputWire, string fileVariant = null)
        {
            return GetWireOutput(LoadInput(fileVariant), outputWire);
        }

        private int GetWireOutput(IEnumerable<string> input, string outputWire)
        {
            var wires = new Dictionary<string, Func<ushort>>();
            var values = new Dictionary<string, ushort>();
            foreach (var line in input)
            {
                if (AndRegex.IsMatch(line))
                {
                    var match = AndRegex.Match(line);
                    var value1 = match.Groups[1].Value;
                    var value2 = match.Groups[2].Value;
                    var wire = match.Groups[3].Value;
                    wires[wire] = () => (ushort) (GetValue(wires, values, value1) & GetValue(wires, values, value2));
                }
                else if (OrRegex.IsMatch(line))
                {
                    var match = OrRegex.Match(line);
                    var value1 = match.Groups[1].Value;
                    var value2 = match.Groups[2].Value;
                    var wire = match.Groups[3].Value;
                    wires[wire] = () => (ushort) (GetValue(wires, values, value1) | GetValue(wires, values, value2));
                }
                else if (LShiftRegex.IsMatch(line))
                {
                    var match = LShiftRegex.Match(line);
                    var value = match.Groups[1].Value;
                    var shift = int.Parse(match.Groups[2].Value);
                    var wire = match.Groups[3].Value;
                    wires[wire] = () => (ushort) (GetValue(wires, values, value) << shift);
                }
                else if (RShiftRegex.IsMatch(line))
                {
                    var match = RShiftRegex.Match(line);
                    var value = match.Groups[1].Value;
                    var shift = int.Parse(match.Groups[2].Value);
                    var wire = match.Groups[3].Value;
                    wires[wire] = () => (ushort) (GetValue(wires, values, value) >> shift);
                }
                else if (NotRegex.IsMatch(line))
                {
                    var match = NotRegex.Match(line);
                    var value = match.Groups[1].Value;
                    var wire = match.Groups[2].Value;
                    wires[wire] = () => (ushort) ~GetValue(wires, values, value);
                }
                else if (SetRegex.IsMatch(line))
                {
                    var match = SetRegex.Match(line);
                    var value = match.Groups[1].Value;
                    var wire = match.Groups[2].Value;
                    wires[wire] = () => GetValue(wires, values, value);
                }
            }
            return wires[outputWire].Invoke();
        }

        private static ushort GetValue(Dictionary<string, Func<ushort>> wires, Dictionary<string, ushort> values,
            string input)
        {
            ushort s;
            if (ushort.TryParse(input, out s))
                return s;

            if (!values.ContainsKey(input))
                values[input] = wires[input].Invoke();
            return values[input];
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetWireOutputFromFileWithExtraInput("a", 3176));
        }

        private int GetWireOutputFromFileWithExtraInput(string outputWire, int bOverrideValue)
        {
            return GetWireOutput(LoadInput().Concat(new[] {$"{bOverrideValue} -> b"}), outputWire);
        }
    }
}