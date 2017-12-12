using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2017
{
    internal class Day12 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetGroupSize(LoadInput("practice")));
            Console.WriteLine(GetGroupSize(LoadInput()));
        }

        private static int GetGroupSize(string[] input)
        {
            var connections = ParseInput(input);

            var seen = RunDepthFirstSearch(0, connections, new bool[input.Length]);

            return seen.Count(s => s);
        }

        private static Dictionary<int, List<int>> ParseInput(string[] input)
        {
            var connections = new Dictionary<int, List<int>>();

            foreach (var line in input)
            {
                var split = line.Split(new [] {" <-> "}, StringSplitOptions.None);
                var start = int.Parse(split[0]);
                var ends = split[1].Split(new[] {", "}, StringSplitOptions.None).Select(int.Parse);

                foreach (var end in ends)
                {
                    if (!connections.ContainsKey(start))
                        connections[start] = new List<int>();
                    connections[start].Add(end);
                    
                    if (!connections.ContainsKey(end))
                        connections[end] = new List<int>();
                    connections[end].Add(start);
                }
            }

            return connections;
        }

        private static bool[] RunDepthFirstSearch(int initialValue, Dictionary<int, List<int>> connections, bool[] seen)
        {
            var dfs = new Queue<int>();
            dfs.Enqueue(initialValue);
            seen[initialValue] = true;
            while (dfs.Count > 0)
            {
                var currentConnection = dfs.Dequeue();
                seen[currentConnection] = true;
                foreach (var connection in connections[currentConnection])
                {
                    if (seen[connection]) continue;
                    
                    dfs.Enqueue(connection);
                    seen[connection] = true;
                }
            }
            
            return seen;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetGroupCount(LoadInput("practice")));
            Console.WriteLine(GetGroupCount(LoadInput()));
        }
        
        private static int GetGroupCount(string[] input)
        {
            var connections = ParseInput(input);            

            var groups = 0;
            var seen = new bool[input.Length];
            while (true)
            {
                var firstConnection = seen
                    .Select((b, i) => new {Index = i, Value = b})
                    .FirstOrDefault(i => !i.Value)?
                    .Index;
                if (!firstConnection.HasValue) return groups;
                groups++;
                seen = RunDepthFirstSearch(firstConnection.Value, connections, seen);
            }
        }
    }
}