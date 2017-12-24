using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2017
{
    internal class Day24 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetStrongestBridge(LoadInput("practice")));
            Console.WriteLine(GetStrongestBridge(LoadInput()));
        }

        private static int GetStrongestBridge(IEnumerable<string> input) => GetValidBridges(input).Max(b => b.Weight);

        private static IEnumerable<BridgeInfo> GetValidBridges(IEnumerable<string> input)
        {
            var components = new Dictionary<int, List<string>>();
            var weights = new Dictionary<string, int>();
            foreach (var component in input)
            {
                var ends = component.Split('/').Select(int.Parse).ToArray();

                if (!components.ContainsKey(ends[0]))
                    components[ends[0]] = new List<string>();
                components[ends[0]].Add(component);

                if (!components.ContainsKey(ends[1]))
                    components[ends[1]] = new List<string>();
                components[ends[1]].Add(component);

                weights[component] = ends[0] + ends[1];
            }

            var used = new HashSet<string>();
            var completedBridges = new List<BridgeInfo>();

            GetWeightForSubbridge(components, weights, used, completedBridges);

            return completedBridges;
        }

        private static void GetWeightForSubbridge(IReadOnlyDictionary<int, List<string>> components,
            IReadOnlyDictionary<string, int> weights, ISet<string> used, ICollection<BridgeInfo> completedBridges,
            int currentEnd = 0)
        {
            var anyAdded = false;
            foreach (var nextComponent in components[currentEnd])
            {
                if (used.Contains(nextComponent)) continue;

                var ends = nextComponent.Split('/').Select(int.Parse).ToArray();

                if (ends[0] != currentEnd && ends[1] != currentEnd) continue;

                anyAdded = true;
                used.Add(nextComponent);
                GetWeightForSubbridge(components, weights, used, completedBridges,
                    ends[0] == currentEnd ? ends[1] : ends[0]);
                used.Remove(nextComponent);
            }

            if (!anyAdded)
                completedBridges.Add(new BridgeInfo {Length = used.Count, Weight = used.Sum(c => weights[c])});
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetLongestBridge(LoadInput("practice")));
            Console.WriteLine(GetLongestBridge(LoadInput()));
        }

        private static int GetLongestBridge(IEnumerable<string> input) =>
            GetValidBridges(input).Max(b => b.Length * 10000 + b.Weight) % 10000;

        private struct BridgeInfo
        {
            public int Length;
            public int Weight;
        }
    }
}