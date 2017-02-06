using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2016
{
    internal class Day01 : BaseDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(GetBlocksAway("R2, L3"));
            Console.WriteLine(GetBlocksAway("R2, R2, R2"));
            Console.WriteLine(GetBlocksAway("L2, L2, L2"));
            Console.WriteLine(GetBlocksAway("R5, L5, R5, R3"));
            Console.WriteLine(GetBlocksAwayFromFile());
        }

        private int GetBlocksAwayFromFile()
        {
            var instructions = LoadInput().First();
            return GetBlocksAway(instructions);
        }

        private int GetBlocksAway(string instructions, bool stopAtIntersection = false)
        {
            var splitInstructions = instructions.Split(new[] {", "}, StringSplitOptions.None);

            var previousLocations = new List<Point>();
            var direction = 0; //N = 0, E = 1, S = 2, W = 3
            var avenuesAway = 0; //N/S avenues
            var streetsAway = 0; //E/W streets

            foreach (var instruction in splitInstructions)
            {
                var turn = instruction.First();
                var steps = int.Parse(new string(instruction.Skip(1).ToArray()));

                if (turn == 'R')
                    direction = Mod(direction + 1, 4);
                else if (turn == 'L')
                    direction = Mod(direction - 1, 4);

                for (var i = 0; i < steps; i++)
                {
                    if (direction == 0)
                        avenuesAway++;
                    else if (direction == 1)
                        streetsAway++;
                    else if (direction == 2)
                        avenuesAway--;
                    else if (direction == 3)
                        streetsAway--;

                    var point = new Point(avenuesAway, streetsAway);
                    if (stopAtIntersection && previousLocations.Contains(point))
                        return Math.Abs(avenuesAway) + Math.Abs(streetsAway);

                    previousLocations.Add(point);
                }
            }

            return Math.Abs(avenuesAway) + Math.Abs(streetsAway);
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(GetBlocksAway("R8, R4, R4, R8", true));
            Console.WriteLine(GetIntersectionBlocksAwayFromFile());
        }

        private int GetIntersectionBlocksAwayFromFile()
        {
            var instructions = LoadInput().First();
            return GetBlocksAway(instructions, true);
        }
    }
}