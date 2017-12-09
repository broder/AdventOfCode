using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2017
{
    internal class Day07 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetBottomProgram(LoadInput("practice")));
            Console.WriteLine(GetBottomProgram(LoadInput()));
        }

        public static string GetBottomProgram(string[] programs) => ParsePrograms(programs)
            .First(p => p.Value.Parents.Count == 0 && p.Value.Children.Count > 0).Value.Name;

        private static Dictionary<string, Program> ParsePrograms(string[] programs)
        {
            var parsedPrograms = new Dictionary<string, Program>();
            foreach (var program in programs)
            {
                var parent = program.Split(' ')[0];
                if (!parsedPrograms.ContainsKey(parent))
                    parsedPrograms[parent] = new Program {Name = parent};

                var weight = int.Parse(program.Split(new[] {"(", ")"}, StringSplitOptions.None)[1]);
                parsedPrograms[parent].Weight = weight;

                var splitProgram = program
                    .Split(new[] {" -> "}, StringSplitOptions.None);
                if (splitProgram.Length <= 1) continue;

                var children = splitProgram[1]
                    .Split(new[] {", "}, StringSplitOptions.None);
                foreach (var child in children)
                {
                    if (!parsedPrograms.ContainsKey(child))
                        parsedPrograms[child] = new Program {Name = child};

                    parsedPrograms[child].Parents.Add(parent);
                    parsedPrograms[parent].Children.Add(child);
                }
            }
            return parsedPrograms;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetModeChildWeight(LoadInput("practice")));
            Console.WriteLine(GetModeChildWeight(LoadInput()));
        }

        public static int GetModeChildWeight(string[] programs)
        {
            var parsedPrograms = ParsePrograms(programs);
            var root = parsedPrograms.First(p => p.Value.Parents.Count == 0 && p.Value.Children.Count > 0).Value;
            var offset = 0;
            while (true)
            {
                var bottomChildren = root.Children;
                var bottomChildrenWeights = new Dictionary<string, int>();
                foreach (var bottomChild in bottomChildren)
                {
                    var children = new Queue<string>();
                    children.Enqueue(bottomChild);

                    var weight = 0;

                    while (children.Count > 0)
                    {
                        var currentChild = parsedPrograms[children.Dequeue()];
                        weight += currentChild.Weight;

                        foreach (var child in currentChild.Children)
                            children.Enqueue(child);
                    }

                    bottomChildrenWeights[bottomChild] = weight;
                }

                var groups = bottomChildrenWeights
                    .GroupBy(w => w.Value)
                    .ToList();

                var wrongGroup = groups
                    .FirstOrDefault(g => g.Count() == 1);

                if (wrongGroup == null)
                    return root.Weight - offset;

                root = parsedPrograms[wrongGroup.First().Key];
                offset = wrongGroup.First().Value
                         - groups
                             .First(g => g.Count() != 1)
                             .First().Value;
            }
        }

        private class Program
        {
            public string Name;
            public int Weight = -1;
            public readonly List<string> Parents = new List<string>();
            public readonly List<string> Children = new List<string>();
        }
    }
}