using System;
using System.Linq;

namespace AdventOfCode._2020
{
    internal class Day13 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(FindBus(LoadInput("practice")));
            Console.WriteLine(FindBus(LoadInput()));
        }

        private static int FindBus(string[] input)
        {
            var initialTime = int.Parse(input[0]);
            var buses = input[1].Split(',').Where(s => s != "x").Select(int.Parse).ToArray();
            for (var wait = 0; ; wait++)
                foreach (var bus in buses)
                    if ((initialTime + wait) % bus == 0)
                        return wait * bus;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(FindEarliestBusSequence(LoadInput("practice")[1]));
            Console.WriteLine(FindEarliestBusSequence("17,x,13,19"));
            Console.WriteLine(FindEarliestBusSequence("67,7,59,61"));
            Console.WriteLine(FindEarliestBusSequence("67,x,7,59,61"));
            Console.WriteLine(FindEarliestBusSequence("67,7,x,59,61"));
            Console.WriteLine(FindEarliestBusSequence("1789,37,47,1889"));
            Console.WriteLine(FindEarliestBusSequence(LoadInput()[1]));
        }

        private static long FindEarliestBusSequence(string busesString)
        {
            var buses = busesString.Split(',').Select((s, i) => new { s, i }).Where(t => t.s != "x").Select(t => new { Id = int.Parse(t.s), Offset = t.i }).ToArray();

            var time = 0L;
            var lcm = 1L;
            foreach (var bus in buses)
            {
                while ((time + bus.Offset) % bus.Id != 0)
                    time += lcm;
                lcm *= bus.Id;
            }

            return time;
        }
    }
}