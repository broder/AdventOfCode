using System;
using System.Linq;

namespace AdventOfCode._2015
{
    internal class Day02 : BaseDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(GetArea("2x3x4"));
            Console.WriteLine(GetArea("1x1x10"));
            Console.WriteLine(GetTotalAreaForFile());
        }

        private int GetTotalAreaForFile()
        {
            return LoadInput().Sum(GetArea);
        }

        private int GetArea(string input)
        {
            var dimensions = input.Split('x').Select(int.Parse).ToArray();
            var side1 = dimensions[0] * dimensions[1];
            var side2 = dimensions[0] * dimensions[2];
            var side3 = dimensions[1] * dimensions[2];

            return 2 * side1 + 2 * side2 + 2 * side3 + Math.Min(Math.Min(side1, side2), side3);
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(GetRibbonLength("2x3x4"));
            Console.WriteLine(GetRibbonLength("1x1x10"));
            Console.WriteLine(GetTotalRibbonLengthForFile());
        }

        private int GetTotalRibbonLengthForFile()
        {
            return LoadInput().Sum(GetRibbonLength);
        }

        private int GetRibbonLength(string input)
        {
            return GetSmallestPerimeter(input) + GetVolume(input);
        }

        private int GetSmallestPerimeter(string input)
        {
            var dimensions = input.Split('x').Select(int.Parse).OrderBy(x => x).Take(2).ToArray();
            return (dimensions[0] + dimensions[1]) * 2;
        }

        private int GetVolume(string input)
        {
            var dimensions = input.Split('x').Select(int.Parse).ToArray();
            return dimensions[0] * dimensions[1] * dimensions[2];
        }
    }
}