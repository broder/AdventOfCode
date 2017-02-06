using System;
using System.Linq;

namespace AoC._2016
{
    internal class Day03 : BaseDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(GetHorizontalTriangesFromFile());
        }

        private int GetHorizontalTriangesFromFile()
        {
            var lines = LoadInput();
            return lines.Count(line => IsTriangle(line.Substring(2, 3), line.Substring(7, 3), line.Substring(12, 3)));
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(GetVerticalTrianglesFromFile());
        }

        private int GetVerticalTrianglesFromFile()
        {
            var lines = LoadInput();
            var numberOfTriangles = 0;

            for (var i = 0; i < lines.Length / 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    if (IsTriangle(lines[3*i].Substring(j*5+2, 3),
                        lines[3*i+1].Substring(j*5+2, 3),
                        lines[3*i+2].Substring(j*5+2, 3)))
                        numberOfTriangles++;
                }
            }

            return numberOfTriangles;
        }

        private static bool IsTriangle(string inputOne, string inputTwo, string inputThree)
        {
            var one = int.Parse(inputOne.Trim());
            var two = int.Parse(inputTwo.Trim());
            var three = int.Parse(inputThree.Trim());
            return (one + two > three) && (one + three > two) && (two + three > one);
        }
    }
}