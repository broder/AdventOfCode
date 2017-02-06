using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC._2016
{
    internal class Day10 : BaseDay
    {
        private readonly Regex ValueRegex = new Regex(@"value (.*?) goes to bot (.*?)$");

        private readonly Regex PassRegex = new Regex(
            @"bot (.*?) gives low to (bot|output) (.*?) and high to (bot|output) (.*?)$");

        private Dictionary<int, Bot> Bots;
        private Dictionary<int, int> Outputs;

        public override void RunPartOne()
        {
            SetupBots("practice");
            RunSimulation(2, 5);
            SetupBots();
            RunSimulation(17, 61);
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(Outputs[0] * Outputs[1] * Outputs[2]);
        }

        private void SetupBots(string fileVariant = null)
        {
            Bots = new Dictionary<int, Bot>();
            Outputs = new Dictionary<int, int>();
            foreach (var line in LoadInput(fileVariant))
            {
                if (ValueRegex.IsMatch(line))
                {
                    var match = ValueRegex.Match(line);
                    var botNumber = int.Parse(match.Groups[2].Value);

                    if (!Bots.ContainsKey(botNumber))
                        Bots[botNumber] = new Bot {Number = botNumber};

                    Bots[botNumber].HeldValues.Add(int.Parse(match.Groups[1].Value));
                }
                else if (PassRegex.IsMatch(line))
                {
                    var match = PassRegex.Match(line);
                    var botNumber = int.Parse(match.Groups[1].Value);

                    if (!Bots.ContainsKey(botNumber))
                        Bots[botNumber] = new Bot {Number = botNumber};

                    Bots[botNumber].LowPassOutput = match.Groups[2].Value == "output";
                    Bots[botNumber].LowPassIndex = int.Parse(match.Groups[3].Value);
                    Bots[botNumber].HighPassOutput = match.Groups[4].Value == "output";
                    Bots[botNumber].HighPassIndex = int.Parse(match.Groups[5].Value);
                }
            }
        }

        private void RunSimulation(int compare1, int compare2)
        {
            var passingBot = Bots.FirstOrDefault(x => x.Value.CanPass()).Value;
            while (passingBot != null)
            {
                var value1 = passingBot.HeldValues[0];
                var value2 = passingBot.HeldValues[1];

                if (value1 == compare1 && value2 == compare2 || value1 == compare2 && value2 == compare1)
                    Console.WriteLine(passingBot.Number);

                var highValue = Math.Max(value1, value2);
                var lowValue = Math.Min(value1, value2);

                if (!passingBot.HighPassOutput)
                    Bots[passingBot.HighPassIndex].HeldValues.Add(highValue);
                else
                    Outputs[passingBot.HighPassIndex] = highValue;

                if (!passingBot.LowPassOutput)
                    Bots[passingBot.LowPassIndex].HeldValues.Add(lowValue);
                else
                    Outputs[passingBot.LowPassIndex] = lowValue;

                passingBot.HeldValues.RemoveRange(0, 2);
                passingBot = Bots.FirstOrDefault(x => x.Value.CanPass()).Value;
            }
        }

        private class Bot
        {
            public int Number;
            public bool HighPassOutput;
            public int HighPassIndex;
            public bool LowPassOutput;
            public int LowPassIndex;
            public readonly List<int> HeldValues = new List<int>();

            public bool CanPass()
            {
                return HeldValues.Count >= 2;
            }
        }
    }
}