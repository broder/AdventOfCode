using System;
using System.Linq;

namespace AdventOfCode._2017
{
    internal class Day11 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetDistance("ne,ne,ne"));
            Console.WriteLine(GetDistance("ne,ne,sw,sw"));
            Console.WriteLine(GetDistance("ne,ne,s,s"));
            Console.WriteLine(GetDistance("se,sw,se,sw,sw"));
            Console.WriteLine(GetDistance(LoadInput().First()));
        }

        private static int GetDistance(string input, bool returnMaximum = false)
        {
            var x = 0;
            var y = 0;
            var maximumDistance = 0;

            foreach (var step in input.Split(','))
            {
                switch (step)
                {
                    case "n":
                        y++;
                        break;
                    case "s":
                        y--;
                        break;
                    case "ne":
                        x++;
                        break;
                    case "sw":
                        x--;
                        break;
                    case "se":
                        x++;
                        y--;
                        break;
                    case "nw":
                        x--;
                        y++;
                        break;
                }
                maximumDistance = Math.Max(maximumDistance, (Math.Abs(x) + Math.Abs(y) + Math.Abs(x + y)) / 2);
            }
            
            if (returnMaximum)
                return maximumDistance;
            return (Math.Abs(x) + Math.Abs(y) + Math.Abs(x + y)) / 2;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetDistance("ne,ne,ne", true));
            Console.WriteLine(GetDistance("ne,ne,sw,sw", true));
            Console.WriteLine(GetDistance("ne,ne,s,s", true));
            Console.WriteLine(GetDistance("se,sw,se,sw,sw", true));
            Console.WriteLine(GetDistance(LoadInput().First(), true));
        }
    }
}