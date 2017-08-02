using System;
using System.Linq;

namespace AdventOfCode._2016
{
    internal class Day20 : BaseDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(GetFirstAllowedIp("practice"));
            Console.WriteLine(GetFirstAllowedIp());
        }

        private long GetFirstAllowedIp(string fileVariant = null)
        {
            var ranges = LoadInput(fileVariant)
                .Select(s => s.Split('-'))
                .Select(s => new Tuple<long, long>(long.Parse(s[0]), long.Parse(s[1])))
                .OrderBy(t => t.Item1);

            var minAllowed = -1L;
            foreach (var range in ranges)
            {
                if (minAllowed >= range.Item1)
                    minAllowed = range.Item2 + 1;
                else
                    return minAllowed;
            }
            return minAllowed;
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(CountAllowedIps("practice"));
            Console.WriteLine(CountAllowedIps());
        }

        private long CountAllowedIps(string fileVariant = null)
        {
            var ranges = LoadInput(fileVariant)
                .Select(s => s.Split('-'))
                .Select(s => new Tuple<long, long>(long.Parse(s[0]), long.Parse(s[1])))
                .OrderBy(t => t.Item1)
                .ToArray();

            var index = 0;
            var current = 0L;
            var allowed = 0L;
            while (current <= 4294967295L)
            {
                if (index < ranges.Length && current >= ranges[index].Item1)
                {
                    if (current <= ranges[index].Item2)
                    {
                        current = ranges[index].Item2 + 1;
                        continue;
                    }
                    index++;
                }
                else
                {
                    allowed++;
                    current++;
                }
            }
            return allowed;
        }
    }
}