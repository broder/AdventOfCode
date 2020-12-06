using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2020
{
    internal class Day06 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetQuestionGroups(LoadInput("practice")).Sum(g => g.Item2.Count()));
            Console.WriteLine(GetQuestionGroups(LoadInput()).Sum(g => g.Item2.Count()));
        }

        private static IEnumerable<Tuple<int, Dictionary<char, int>>> GetQuestionGroups(string[] questions)
        {
            var people = 0;
            var currentQuestions = new Dictionary<char, int>();
            foreach (var line in questions) {
                if (line.Length == 0) {
                    yield return new Tuple<int, Dictionary<char, int>>(people, currentQuestions);
                    people = 0;
                    currentQuestions = new Dictionary<char, int>();
                    continue;
                }

                people++;
                foreach (var c in line) {
                    if (!currentQuestions.ContainsKey(c))
                        currentQuestions[c] = 0;
                    currentQuestions[c]++;
                }
            }

            if (people > 0)
                yield return new Tuple<int, Dictionary<char, int>>(people, currentQuestions);
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetQuestionGroups(LoadInput("practice")).Sum(g => g.Item2.Count(c => c.Value == g.Item1)));
            Console.WriteLine(GetQuestionGroups(LoadInput()).Sum(g => g.Item2.Count(c => c.Value == g.Item1)));
        }
    }
}