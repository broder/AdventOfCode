using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2017
{
    internal class Day08 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetMaxValueFromSimulation(LoadInput("practice")));
            Console.WriteLine(GetMaxValueFromSimulation(LoadInput()));
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetMaxValueFromSimulation(LoadInput("practice"), true));
            Console.WriteLine(GetMaxValueFromSimulation(LoadInput(), true));
        }


        protected int GetMaxValueFromSimulation(string[] lines, bool maxEver = false)
        {
            var max = 0;
            var registers = new Dictionary<string, int>();
            var regex = new Regex(@"^([a-z]+) (inc|dec) ([-\d]+) if ([a-z]+) (.+) ([-\d]+)$");
            foreach (var line in lines)
            {
                var match = regex.Match(line);
                
                var register = match.Groups[1].Value;
                var incrememnt = match.Groups[2].Value == "inc";
                var value = int.Parse(match.Groups[3].Value);
                var conditionRegister = match.Groups[4].Value;
                var condition = match.Groups[5].Value;
                var conditionValue = int.Parse(match.Groups[6].Value);

                if (!registers.ContainsKey(register))
                    registers[register] = 0;

                if (!registers.ContainsKey(conditionRegister))
                    registers[conditionRegister] = 0;

                if (CheckCondition(condition, registers[conditionRegister], conditionValue))
                    if (incrememnt)
                        registers[register] += value;
                    else
                        registers[register] -= value;

                if (maxEver)
                    max = Math.Max(max, registers.Max(pair => pair.Value));
            }
            return maxEver ? max : registers.Max(pair => pair.Value);
        }

        private static bool CheckCondition(string condition, int a, int b)
        {
            return condition == "<" && a < b
                   || condition == "<=" && a <= b
                   || condition == ">" && a > b
                   || condition == ">=" && a >= b
                   || condition == "==" && a == b
                   || condition == "!=" && a != b;
        }
    }
}