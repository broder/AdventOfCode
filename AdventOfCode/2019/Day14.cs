using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day14 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetRequiredOreForSingleFuel(LoadInput("practice.1")));
            Console.WriteLine(GetRequiredOreForSingleFuel(LoadInput("practice.2")));
            Console.WriteLine(GetRequiredOreForSingleFuel(LoadInput("practice.3")));
            Console.WriteLine(GetRequiredOreForSingleFuel(LoadInput("practice.4")));
            Console.WriteLine(GetRequiredOreForSingleFuel(LoadInput("practice.5")));
            Console.WriteLine(GetRequiredOreForSingleFuel(LoadInput()));
        }

        private static long GetRequiredOreForSingleFuel(string[] formulaeString) =>
            GetRequiredOre(ParseFormulae(formulaeString), 1);

        private static long GetRequiredOre(Dictionary<string, Reaction> formulae, int requiredFuel)
        {
            var required = new Dictionary<string, long> {["FUEL"] = requiredFuel};
            var surplus = new Dictionary<string, long>();
            while (required.Keys.Any(k => k != "ORE"))
            {
                var keyToProcess = required.Keys.First(k => k != "ORE");

                var outputRequired = required[keyToProcess];

                var reaction = formulae[keyToProcess];
                var reactionOutput = reaction.Output.Value;

                var reactionsRequired = outputRequired / reactionOutput;
                if (outputRequired % reactionOutput != 0) reactionsRequired++;

                foreach (var (inputKey, inputAmount) in reaction.Inputs)
                {
                    var amountRequired = inputAmount * reactionsRequired;

                    if (surplus.ContainsKey(inputKey))
                    {
                        if (amountRequired >= surplus[inputKey])
                        {
                            amountRequired -= surplus[inputKey];
                            surplus.Remove(inputKey);
                        }
                        else
                        {
                            surplus[inputKey] -= amountRequired;
                            amountRequired = 0;
                        }
                    }

                    if (amountRequired == 0) continue;

                    if (!required.ContainsKey(inputKey))
                        required[inputKey] = 0;

                    required[inputKey] += amountRequired;
                }

                required.Remove(keyToProcess);

                if (reactionOutput * reactionsRequired <= outputRequired) continue;

                if (!surplus.ContainsKey(keyToProcess))
                    surplus[keyToProcess] = 0;
                surplus[keyToProcess] += reactionOutput * reactionsRequired - outputRequired;
            }

            return required["ORE"];
        }

        private static Dictionary<string, Reaction> ParseFormulae(string[] formulaeString)
        {
            var formulae = new Dictionary<string, Reaction>();
            foreach (var formula in formulaeString)
            {
                var reaction = new Reaction();
                var split = formula.Split(new[] {" ", ",", "=>"}, StringSplitOptions.RemoveEmptyEntries);
                for (var i = 0; i < split.Length; i += 2)
                {
                    if (i == split.Length - 2)
                        reaction.Output = new KeyValuePair<string, long>(split[i + 1], long.Parse(split[i]));
                    else
                        reaction.Inputs.Add(split[i + 1], long.Parse(split[i]));
                }

                formulae[reaction.Output.Key] = reaction;
            }

            return formulae;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetRequiredOreForOneTrillionOre(LoadInput("practice.3")));
            Console.WriteLine(GetRequiredOreForOneTrillionOre(LoadInput("practice.4")));
            Console.WriteLine(GetRequiredOreForOneTrillionOre(LoadInput("practice.5")));
            Console.WriteLine(GetRequiredOreForOneTrillionOre(LoadInput()));
        }

        private static int GetRequiredOreForOneTrillionOre(string[] formulaeString)
        {
            var formulae = ParseFormulae(formulaeString);
            const long target = 1000000000000;

            var lowFuel = 1;
            var highFuel = 100000000;

            while (lowFuel + 1 < highFuel)
            {
                var midFuel = (lowFuel + highFuel) / 2;
                var midOre = GetRequiredOre(formulae, midFuel);

                if (midOre < target)
                    lowFuel = midFuel;
                else
                    highFuel = midFuel;
            }

            return lowFuel;
        }

        private class Reaction
        {
            public readonly Dictionary<string, long> Inputs = new Dictionary<string, long>();
            public KeyValuePair<string, long> Output;
        }
    }
}