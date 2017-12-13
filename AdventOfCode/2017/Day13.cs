using System;
using System.Linq;

namespace AdventOfCode._2017
{
    internal class Day13 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetSeverity(LoadInput("practice")));
            Console.WriteLine(GetSeverity(LoadInput()));
        }

        private static int GetSeverity(string[] input)
        {
            var severity = 0;
            foreach (var line in input)
            {
                var split = line.Split(new[] {": "}, StringSplitOptions.None);
                var layer = int.Parse(split[0]);
                var depth = int.Parse(split[1]);
                if (layer % (depth * 2 - 2) == 0)
                    severity += layer * depth;
            }
            return severity;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetDelay(LoadInput("practice")));
            Console.WriteLine(GetDelay(LoadInput()));
        }
        
        private static int GetDelay(string[] input)
        {
            var length = int.Parse(input.Last().Split(':')[0]) + 1;
            var gates = new int[length];

            foreach (var line in input)
            {
                var split = line.Split(new[] {": "}, StringSplitOptions.None);
                var layer = int.Parse(split[0]);
                var depth = int.Parse(split[1]);
                gates[layer] = depth;
            }

            var offset = 1;
            while (true)
            {
                var caught = false;
                for (var time = 0; time < gates.Length; time++)
                {
                    var position = time;
                    
                    var depth = gates[position];
                    if (depth <= 1) continue;
                    
                    var gatePosition = (time + offset) % (depth * 2 - 2);
                    if (gatePosition != 0) continue;
                    
                    caught = true;
                    break;
                }
                if (!caught)
                    return offset;
                offset++;
            }
        }
    }
}