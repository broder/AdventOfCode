using System;
using System.Linq;

namespace AoC._2015
{
    internal class Day17 : BaseDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(GetContainerCombinationsFromFile(25, "practice"));
            Console.WriteLine(GetContainerCombinationsFromFile(150));
        }

        private int GetContainerCombinationsFromFile(int totalLiters, string fileVariant = null)
        {
            var containers = LoadInput(fileVariant).Select(int.Parse).ToList();
            return Enumerable
                .Range(1, (1 << containers.Count) - 1)
                .Select(i1 => containers.Where((item, i2) => ((1 << i2) & i1) != 0))
                .Count(c => c.Sum() == totalLiters);
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(GetMinimumContainerCombinationsFromFile(25, "practice"));
            Console.WriteLine(GetMinimumContainerCombinationsFromFile(150));
        }

        private int GetMinimumContainerCombinationsFromFile(int totalLiters, string fileVariant = null)
        {
            var containers = LoadInput(fileVariant).Select(int.Parse).ToList();
            var combinations = Enumerable
                .Range(1, (1 << containers.Count) - 1)
                .Select(i1 => containers.Where((item, i2) => ((1 << i2) & i1) != 0))
                .Where(c => c.Sum() == totalLiters).ToArray();
            return combinations.Count(c1 => c1.Count() == combinations.Min(c2 => c2.Count()));
        }
    }
}