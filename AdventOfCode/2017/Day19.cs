using System;
using System.Text;

namespace AdventOfCode._2017
{
    internal class Day19 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetRoute(LoadInput("practice")));
            Console.WriteLine(GetRoute(LoadInput()));
        }

        private static string GetRoute(string[] input) => FollowPath(input, false);

        private static string FollowPath(string[] input, bool returnSteps)
        {
            var x = 0;
            var y = 0;
            for (var i = 0; i < input[y].Length; i++)
            {
                if (input[y][i] == '|')
                {
                    x = i;
                    break;
                }
            }

            var xDir = 0;
            var yDir = 1;
            var steps = 0;
            var route = new StringBuilder();

            while (true)
            {
                x += xDir;
                y += yDir;
                steps++;
                var nextChar = input[y][x];
                if (Alphabet.ToUpper().IndexOf(nextChar) >= 0)
                {
                    route.Append(nextChar);
                }
                else if (nextChar == '+')
                {
                    if (yDir != 0)
                    {
                        yDir = 0;
                        if (x >= 1 && input[y][x - 1] != ' ')
                            xDir = -1;
                        else if (x < input[y].Length - 1 && input[y][x + 1] != ' ')
                            xDir = 1;
                    }
                    else
                    {
                        xDir = 0;
                        if (y >= 1 && input[y - 1][x] != ' ')
                            yDir = -1;
                        else if (y < input.Length - 1 && input[y + 1][x] != ' ')
                            yDir = 1;
                    }
                }
                else if (nextChar == ' ')
                    return returnSteps ? steps.ToString() : route.ToString();
            }
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetSteps(LoadInput("practice")));
            Console.WriteLine(GetSteps(LoadInput()));
        }

        private static string GetSteps(string[] input) => FollowPath(input, true);
    }
}