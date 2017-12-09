using System;
using System.Collections.Generic;
using System.Linq;
using Combinatorics.Collections;

namespace AdventOfCode._2015
{
    internal class Day24 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetFirstQuantumEntaglement(3, "practice"));
            Console.WriteLine(GetFirstQuantumEntaglement(3));
        }

        public long GetFirstQuantumEntaglement(int groups, string fileVariant = null)
        {
            var presents = LoadInput(fileVariant).Select(int.Parse).ToList();
            var groupWeight = presents.Sum() / groups;
            var combinations = GetLowestCombinationsWithSum(presents, groupWeight);
            var qe = long.MaxValue;
            foreach (var comb in combinations)
                if (comb.Sum() == groupWeight)
                    qe = Math.Min(qe, GetQuantumEntanglement(comb));
            return qe;
        }

        public long GetQuantumEntanglement(IEnumerable<int> weights)
        {
            return weights.Aggregate(1L, (qe, weight) => qe * weight);
        }

        public static Combinations<int> GetLowestCombinationsWithSum(IList<int> list, int sum)
        {
            return list.Select((t, x) => new Combinations<int>(list, x))
                .FirstOrDefault(combinations => combinations.Any(p => p.Sum() == sum));
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetFirstQuantumEntaglement(4, "practice"));
            Console.WriteLine(GetFirstQuantumEntaglement(4));
        }
    }
}