using System;
using System.Linq;

namespace AdventOfCode._2015
{
    internal class Day18 : BaseDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(GetNumberOfLightsOnAfterSteps(4, false, "practice"));
            Console.WriteLine(GetNumberOfLightsOnAfterSteps(100));
        }

        private int GetNumberOfLightsOnAfterSteps(int steps, bool cornersPermanentlyOn = false,
            string fileVariant = null)
        {
            var input = LoadInput(fileVariant);
            var dim = input[0].Length;

            var lights = new bool[dim, dim];
            for (var y = 0; y < dim; y++)
            for (var x = 0; x < dim; x++)
                lights[x, y] = cornersPermanentlyOn &&
                               (x == 0 && y == 0 || x == 0 && y == dim - 1 || x == dim - 1 && y == 0 ||
                                x == dim - 1 && y == dim - 1) || input[y][x] == '#';

            for (var i = 0; i < steps; i++)
            {
                var temp = new bool[dim, dim];
                for (var y = 0; y < dim; y++)
                {
                    for (var x = 0; x < dim; x++)
                    {
                        if (cornersPermanentlyOn && (x == 0 && y == 0 || x == 0 && y == dim - 1 ||
                                                     x == dim - 1 && y == 0 ||
                                                     x == dim - 1 && y == dim - 1))
                            temp[x, y] = true;
                        else
                        {
                            var count = 0;
                            for (var yDiff = -1; yDiff <= 1; yDiff++)
                            for (var xDiff = -1; xDiff <= 1; xDiff++)
                                count += !(xDiff == 0 && yDiff == 0) && x + xDiff >= 0 && x + xDiff < dim &&
                                         y + yDiff >= 0 && y + yDiff < dim &&
                                         lights[x + xDiff, y + yDiff]? 1: 0;
                            temp[x, y] = lights[x, y] && count == 2 || count == 3;
                        }
                    }
                }
                lights = temp;
            }
            return lights.Cast<bool>().Count(b => b);
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(GetNumberOfLightsOnAfterSteps(5, true, "practice"));
            Console.WriteLine(GetNumberOfLightsOnAfterSteps(100, true));
        }
    }
}