using System;
using System.Text.RegularExpressions;

namespace AdventOfCode._2015
{
    internal class Day06 : BaseDay
    {
        private readonly Regex TurnOnRegex = new Regex(@"turn on (\d*),(\d*) through (\d*),(\d*)");
        private readonly Regex ToggleRegex = new Regex(@"toggle (\d*),(\d*) through (\d*),(\d*)");
        private readonly Regex TurnOffRegex = new Regex(@"turn off (\d*),(\d*) through (\d*),(\d*)");

        public override void RunPartOne()
        {
            Console.WriteLine(CountLights(new[] {"turn on 0,0 through 999,999"}));
            Console.WriteLine(CountLights(new[] {"toggle 0,0 through 999,0"}));
            Console.WriteLine(CountLights(new[] {"turn off 499,499 through 500,500"}));
            Console.WriteLine(CountLightsFromFile());
        }

        private int CountLightsFromFile()
        {
            return CountLights(LoadInput());
        }

        private int CountLights(string[] input)
        {
            var lights = new bool[1000, 1000];
            var count = 0;
            foreach (var instruction in input)
            {
                var points = GetPoints(instruction);

                for (var x = points[0].X; x <= points[1].X; x++)
                {
                    for (var y = points[0].Y; y <= points[1].Y; y++)
                    {
                        var original = lights[x, y];
                        if (TurnOnRegex.IsMatch(instruction))
                        {
                            lights[x, y] = true;
                        }
                        else if (ToggleRegex.IsMatch(instruction))
                        {
                            lights[x, y] = !lights[x, y];
                        }
                        else
                        {
                            lights[x, y] = false;
                        }

                        if (original && !lights[x, y])
                            count--;
                        else if (!original && lights[x, y])
                            count++;
                    }
                }
            }
            return count;
        }

        private Point[] GetPoints(string instruction)
        {
            var points = new Point[2];
            Match match;
            if (TurnOnRegex.IsMatch(instruction))
            {
                match = TurnOnRegex.Match(instruction);
            }
            else if (ToggleRegex.IsMatch(instruction))
            {
                match = ToggleRegex.Match(instruction);
            }
            else
            {
                match = TurnOffRegex.Match(instruction);
            }

            points[0] = new Point(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
            points[1] = new Point(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
            return points;
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(GetBrightness(new[] {"turn on 0,0 through 0,0"}));
            Console.WriteLine(GetBrightness(new[] {"toggle 0,0 through 999,999"}));
            Console.WriteLine(GetBrightnessFromFile());
        }

        private int GetBrightnessFromFile()
        {
            return GetBrightness(LoadInput());
        }

        private int GetBrightness(string[] input)
        {
            var lights = new int[1000, 1000];
            var totalBrightness = 0;
            foreach (var instruction in input)
            {
                var points = GetPoints(instruction);

                for (var x = points[0].X; x <= points[1].X; x++)
                {
                    for (var y = points[0].Y; y <= points[1].Y; y++)
                    {
                        var originalValue = lights[x, y];
                        if (TurnOnRegex.IsMatch(instruction))
                        {
                            lights[x, y] = lights[x, y] + 1;
                        }
                        else if (ToggleRegex.IsMatch(instruction))
                        {
                            lights[x, y] = lights[x, y] + 2;
                        }
                        else
                        {
                            lights[x, y] = Math.Max(lights[x, y] - 1, 0);
                        }
                        totalBrightness += lights[x, y] - originalValue;
                    }
                }
            }
            return totalBrightness;
        }
    }
}