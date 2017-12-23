using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2017
{
    internal class Day23 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(RunSimulation(LoadInput()));
        }

        private static long RunSimulation(IReadOnlyList<string> steps)
        {
            var registers = new Dictionary<string, long>();
            var mulCount = 0;
            
            for (var i = 0; i < steps.Count; i++)
            {
                var step = steps[i];

                var command = step.Substring(0, 3);
                var parameters = step.Substring(4);

                switch (command)
                {
                    case "set":
                    case "sub":
                    case "mul":
                        var split = parameters.Split(' ').ToArray();
                        var x = split[0];
                        var y = GetValue(registers, split[1]);

                        if (!registers.ContainsKey(x))
                            registers[x] = 0;

                        if (command == "set")
                            registers[x] = y;
                        else if (command == "sub")
                            registers[x] -= y;
                        else if (command == "mul")
                        {
                            registers[x] *= y;
                            mulCount++;
                        }
                        break;
                    case "jnz":
                        var jnzSplit = parameters.Split(' ').ToArray();
                        var jnzX = GetValue(registers, jnzSplit[0]);
                        var jnzY = GetValue(registers, jnzSplit[1]);
                        if (jnzX != 0)
                            i += (int) jnzY - 1;
                        break;
                }
            }
            return mulCount;
        }

        private static long GetValue(IDictionary<string, long> registers, string input)
        {
            if (long.TryParse(input, out var output))
                return output;

            if (!registers.ContainsKey(input))
                registers[input] = 0;

            return registers[input];
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(CountNonPrimeNumbers());
        }

        private static int CountNonPrimeNumbers()
        {
            var h = 0;
            for (var b = 84 * 100 + 100000; b <= 84 * 100 + 117000; b += 17)
                if (!IsPrime(b))
                    h++;
            
            return h;
        }

        private static bool IsPrime(int n)
        {
            for (var i = 2; i * i < n; i++)
                if (n % i == 0)
                    return false;
            return true;
        }
    }
}