using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode._2018
{
    internal class Day18 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetResourceValueAfterMinutes(LoadInput("practice"), 10));
            Console.WriteLine(GetResourceValueAfterMinutes(LoadInput(), 10));
        }

        private static int GetResourceValueAfterMinutes(IReadOnlyList<string> input, int minutes)
        {
            var map = new char[input[0].Length, input.Count];
            for (var y = 0; y < input.Count; y++)
            for (var x = 0; x < input[y].Length; x++)
            {
                map[x, y] = input[y][x];
            }

            var seen = new Dictionary<string, int>();
            var history = new Dictionary<int, string>();
            for (var time = 0; time < minutes; time++)
            {
                var mapKey = map.Cast<char>().Aggregate(new StringBuilder(), (sb, c) => sb.Append(c)).ToString();
                if (seen.ContainsKey(mapKey))
                {
                    var loopStart = seen[mapKey];
                    var loopLength = time - loopStart;
                    var finalMap = history[(minutes - loopStart) % loopLength + loopStart];
                    return finalMap.Count(c => c == '|') * finalMap.Count(c => c == '#');
                }

                seen[mapKey] = time;
                history[time] = mapKey;
                
                var nextMap = new char[map.GetLength(0), map.GetLength(1)];
                for (var x = 0; x < map.GetLength(0); x++)
                for (var y = 0; y < map.GetLength(1); y++)
                {
                    var treeCount = 0;
                    var lumberyardCount = 0;
                    for (var xDiff = -1; xDiff <= 1; xDiff++)
                    for (var yDiff = -1; yDiff <= 1; yDiff++)
                    {
                        if (xDiff == 0 && yDiff == 0
                            || x + xDiff < 0 || x + xDiff >= map.GetLength(0)
                            || y + yDiff < 0 || y + yDiff >= map.GetLength(1))
                            continue;

                        if (map[x + xDiff, y + yDiff] == '|')
                            treeCount++;
                        else if (map[x + xDiff, y + yDiff] == '#')
                            lumberyardCount++;
                    }
                    
                    switch (map[x, y])
                    {
                        case '.':
                            nextMap[x, y] = treeCount >= 3 ? '|' : '.';
                            break;
                        case '|':
                            nextMap[x, y] = lumberyardCount >= 3 ? '#' : '|';
                            break;
                        default: // '#'
                            nextMap[x, y] = lumberyardCount < 1 || treeCount < 1 ? '.' : '#';
                            break;
                    }
                }

                map = nextMap;
            }

            return map.Cast<char>().Count(c => c == '|') * map.Cast<char>().Count(c => c == '#');
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetResourceValueAfterMinutes(LoadInput(), 1000000000));
        }
    }
}
