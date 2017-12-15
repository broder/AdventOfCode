using System;
using System.Collections.Generic;

namespace AdventOfCode._2017
{
    internal class Day15 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetMatchingPairs(new GeneratorInfo
                {
                    Seed = 65,
                    Factor = 16807
                },
                new GeneratorInfo
                {
                    Seed = 8921,
                    Factor = 48271
                }));
            Console.WriteLine(GetMatchingPairs(new GeneratorInfo
                {
                    Seed = 516,
                    Factor = 16807
                },
                new GeneratorInfo
                {
                    Seed = 190,
                    Factor = 48271
                }));
        }

        private static int GetMatchingPairs(GeneratorInfo aInfo, GeneratorInfo bInfo, int totalPairs = 40000000)
        {
            var matches = 0;
            using (var aValues = GetGeneratorValues(aInfo.Seed, aInfo.Factor, aInfo.Divisor).GetEnumerator())
            using (var bValues = GetGeneratorValues(bInfo.Seed, bInfo.Factor, bInfo.Divisor).GetEnumerator())
            {
                aValues.MoveNext();
                bValues.MoveNext();
                for (var i = 0; i < totalPairs; i++)
                {
                    if ((aValues.Current & 65535) == (bValues.Current & 65535))
                        matches++;
                    
                    aValues.MoveNext();
                    bValues.MoveNext();
                }
            }
            return matches;
        }

        private static IEnumerable<int> GetGeneratorValues(int seed, int factor, int divisible)
        {
            while (true)
            {
                seed = (int) ((long) seed * factor % 2147483647);
                if (seed % divisible != 0) continue;
                yield return seed;
            }
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetMatchingPairs(new GeneratorInfo
                {
                    Seed = 65,
                    Factor = 16807,
                    Divisor = 4
                },
                new GeneratorInfo
                {
                    Seed = 8921,
                    Factor = 48271,
                    Divisor = 8
                }, 5000000));
            Console.WriteLine(GetMatchingPairs(new GeneratorInfo
                {
                    Seed = 516,
                    Factor = 16807,
                    Divisor = 4
                },
                new GeneratorInfo
                {
                    Seed = 190,
                    Factor = 48271,
                    Divisor = 8
                }, 5000000));
        }

        private class GeneratorInfo
        {
            public int Seed;
            public int Factor;
            public int Divisor = 1;
        }
    }
}