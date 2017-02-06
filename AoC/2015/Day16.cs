using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2015
{
    internal class Day16 : BaseDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(GetSue(MatchesTickerExactly));
        }

        private int GetSue(Func<Dictionary<string, int>, Dictionary<string, int>, bool> matchMethod)
        {
            var sues = new List<Dictionary<string, int>>();
            foreach (var line in LoadInput())
            {
                var lineSplit = line.Split(new[] {": ", ", "}, StringSplitOptions.None);
                var sue = new Dictionary<string, int>();
                for (var i = 1; i < lineSplit.Length - 1; i += 2)
                {
                    sue.Add(lineSplit[i], int.Parse(lineSplit[i + 1]));
                }
                sues.Add(sue);
            }

            var ticker = LoadInput("ticker")
                .Select(line => line.Split(new[] {": "}, StringSplitOptions.None))
                .ToDictionary(lineSplit => lineSplit[0], lineSplit => int.Parse(lineSplit[1]));

            return sues
                .Select((v, i) => new {v, i = i + 1})
                .FirstOrDefault(kvp => matchMethod(ticker, kvp.v))?.i ?? 0;
        }

        private bool MatchesTickerExactly(Dictionary<string, int> ticker, Dictionary<string, int> sue)
        {
            return ticker
                .Where(compound => sue.ContainsKey(compound.Key))
                .All(compound => sue[compound.Key] == compound.Value);
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(GetSue(MatchesTickerRange));
        }

        private readonly List<string> GreaterThanKeys = new List<string>{"cats", "trees"};
        private readonly List<string> FewerThanKeys = new List<string>{"pomeranians", "goldfish"};

        private bool MatchesTickerRange(Dictionary<string, int> ticker, Dictionary<string, int> sue)
        {
            return ticker
                .Where(compound => sue.ContainsKey(compound.Key))
                .All(compound => GreaterThanKeys.Contains(compound.Key) && sue[compound.Key] > compound.Value ||
                                 FewerThanKeys.Contains(compound.Key) && sue[compound.Key] < compound.Value ||
                                 !GreaterThanKeys.Contains(compound.Key) &&
                                     !FewerThanKeys.Contains(compound.Key) &&
                                     sue[compound.Key] == compound.Value);
        }
    }
}