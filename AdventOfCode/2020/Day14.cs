using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2020
{
    internal class Day14 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(SumInitialRegisters(LoadInput("practice.1")));
            Console.WriteLine(SumInitialRegisters(LoadInput()));
        }

        private static long SumInitialRegisters(string[] input)
        {
            var mask = new Dictionary<int, bool>();
            var registers = new Dictionary<ulong, ulong>();
            foreach (var line in input)
            {
                if (line.StartsWith("mask = "))
                {
                    mask = new Dictionary<int, bool>();
                    for (var i = 0; i < 36; i++)
                    {
                        var m = line[line.Length - 1 - i];
                        if (m != 'X')
                            mask[i] = m == '1';
                    }
                }
                else
                {
                    var s = line.Split(new[] { "mem[", "] = " }, StringSplitOptions.RemoveEmptyEntries);
                    var i = ulong.Parse(s[0]);
                    var v = ulong.Parse(s[1]);

                    foreach (var m in mask)
                        if (m.Value)
                            v |= 1ul << m.Key;
                        else
                            v &= ~(1ul << m.Key);

                    registers[i] = v;
                }
            }

            return registers.Sum(kvp => (long)kvp.Value);
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(SumInitialRegistersWithFloaters(LoadInput("practice.2")));
            Console.WriteLine(SumInitialRegistersWithFloaters(LoadInput()));
        }

        private static long SumInitialRegistersWithFloaters(string[] input)
        {
            var mask = new List<int>();
            var floaters = new List<int>();
            var registers = new Dictionary<ulong, ulong>();
            foreach (var line in input)
            {
                if (line.StartsWith("mask = "))
                {
                    mask = new List<int>();
                    floaters = new List<int>();
                    for (var i = 0; i < 36; i++)
                    {
                        var m = line[line.Length - 1 - i];
                        if (m == 'X')
                            floaters.Add(i);
                        else if (m == '1')
                            mask.Add(i);
                    }
                }
                else
                {
                    var s = line.Split(new[] { "mem[", "] = " }, StringSplitOptions.RemoveEmptyEntries);
                    var i = ulong.Parse(s[0]);
                    var v = ulong.Parse(s[1]);

                    foreach (var m in mask)
                        i |= 1ul << m;

                    foreach (var fi in ComputeFloaters(i, floaters))
                        registers[fi] = v;
                }
            }

            return registers.Sum(kvp => (long)kvp.Value);
        }

        private static IEnumerable<ulong> ComputeFloaters(ulong value, List<int> floaterIndices)
        {
            if (floaterIndices.Count == 0)
                yield return value;
            else
            {
                var i = floaterIndices.First();
                var nextFloaters = floaterIndices.Skip(1).ToList();
                foreach (var f in ComputeFloaters(value | (1ul << i), nextFloaters))
                    yield return f;
                foreach (var f in ComputeFloaters(value & ~(1ul << i), nextFloaters))
                    yield return f;
            }
        }
    }
}