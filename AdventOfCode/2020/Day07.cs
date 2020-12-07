using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2020
{
    internal class Day07 : BaseDay
    {
        private const string TargetBag = "shiny gold";

        protected override void RunPartOne()
        {
            Console.WriteLine(FindGoldContainersBags(LoadInput("practice.1"), TargetBag));
            Console.WriteLine(FindGoldContainersBags(LoadInput(), TargetBag));
        }

        private static int FindGoldContainersBags(string[] input, string targetBag)
        {
            var graph = GenerateGraph(input);
            return graph.Count(n => CanContainBag(graph, n.Key, targetBag));
        }

        private static Dictionary<string, Dictionary<string, int>> GenerateGraph(string[] input)
        {
            var graph = new Dictionary<string, Dictionary<string, int>>();
            foreach (var line in input)
            {
                var s = line.Split(new[] { " bags contain ", " bag, ", " bags, ", " bag.", " bags." }, StringSplitOptions.RemoveEmptyEntries);

                graph[s[0]] = new Dictionary<string, int>();
                if (s[1] == "no other")
                    continue;

                foreach (var bag in s.Skip(1))
                {
                    var t = bag.Split(' ');
                    graph[s[0]][string.Join(' ', t.Skip(1))] = int.Parse(t[0]);
                }
            }
            return graph;
        }

        private static bool CanContainBag(Dictionary<string, Dictionary<string, int>> graph, string currentBag, string targetBag)
        {
            if (currentBag == targetBag)
                return false;

            foreach (var nextBag in graph[currentBag])
                if (nextBag.Key == targetBag || CanContainBag(graph, nextBag.Key, targetBag))
                    return true;

            return false;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(CountContainedBags(LoadInput("practice.2"), TargetBag));
            Console.WriteLine(CountContainedBags(LoadInput(), TargetBag));
        }

        private static int CountContainedBags(string[] input, string bag) => CountContainedBags(GenerateGraph(input), bag) - 1;

        private static int CountContainedBags(Dictionary<string, Dictionary<string, int>> graph, string bag) => graph[bag].Sum(n => n.Value * CountContainedBags(graph, n.Key)) + 1;
    }
}