using System;
using System.Collections.Generic;

namespace AdventOfCode._2018
{
    internal class Day17 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(CalculateWaterStorage(LoadInput("practice")));
            Console.WriteLine(CalculateWaterStorage(LoadInput()));
        }

        private static int CalculateWaterStorage(IEnumerable<string> input)
        {
            const int sourceX = 500;
            const int sourceY = 0;

            var (map, xMin, xMax, yMin, yMax) = GenerateMap(input, sourceX, sourceY);

            FillDown(map, sourceX, sourceY);

            var count = 0;
            for (var y = yMin; y <= yMax; y++)
            for (var x = xMin - 1; x <= xMax + 1; x++)
                if (map[x, y] == '~' || map[x, y] == '|')
                    count++;

            return count;
        }

        private static (char[,] map, int xMin, int xMax, int yMin, int yMax) GenerateMap(IEnumerable<string> input, int sourceX, int sourceY)
        {
            var map = new char[2000, 2000];
            for (var x = 0; x < map.GetLength(0); x++)
            for (var y = 0; y < map.GetLength(1); y++)
                map[x, y] = '.';

            map[sourceX, sourceY] = '+';

            var xMin = int.MaxValue;
            var xMax = int.MinValue;
            var yMin = int.MaxValue;
            var yMax = int.MinValue;
            foreach (var line in input)
            {
                var split = line.Split(new[] {"=", ", ", ".."}, StringSplitOptions.RemoveEmptyEntries);
                var xFixed = split[0] == "x";
                var fixedCoord = int.Parse(split[1]);
                var minCoord = int.Parse(split[3]);
                var maxCoord = int.Parse(split[4]);

                xMin = Math.Min(xMin, xFixed ? fixedCoord : minCoord);
                xMax = Math.Max(xMax, xFixed ? fixedCoord : maxCoord);
                yMin = Math.Min(yMin, xFixed ? minCoord : fixedCoord);
                yMax = Math.Max(yMax, xFixed ? maxCoord : fixedCoord);

                for (var i = minCoord; i <= maxCoord; i++)
                {
                    if (xFixed)
                        map[fixedCoord, i] = '#';
                    else
                        map[i, fixedCoord] = '#';
                }
            }

            return (map, xMin, xMax, yMin, yMax);
        }

        private static void FillDown(char[,] map, int startX, int startY)
        {
            while (true)
            {
                startY++;

                if (map[startX, startY] == '|') return;

                map[startX, startY] = '|';

                if (startY + 1 >= map.GetLength(1)) return;

                if (map[startX, startY + 1] != '#' && map[startX, startY + 1] != '~') continue;

                FillAcross(map, startX, startY);
                return;
            }
        }

        private static void FillAcross(char[,] map, int startX, int startY)
        {
            var leftBoundary = startX - 1;
            while (map[leftBoundary, startY] != '#' &&
                   (map[leftBoundary, startY + 1] == '#' || map[leftBoundary, startY + 1] == '~'))
                leftBoundary--;

            var leftWall = map[leftBoundary, startY] == '#';

            var rightBoundary = startX + 1;
            while (map[rightBoundary, startY] != '#' &&
                   (map[rightBoundary, startY + 1] == '#' || map[rightBoundary, startY + 1] == '~'))
                rightBoundary++;

            var rightWall = map[rightBoundary, startY] == '#';

            if (leftWall && rightWall)
            {
                for (var x = leftBoundary + 1; x < rightBoundary; x++)
                    map[x, startY] = '~';
                FillAcross(map, startX, startY - 1);
            }
            else if (leftWall)
            {
                for (var x = leftBoundary + 1; x <= rightBoundary; x++)
                    map[x, startY] = '|';
                FillDown(map, rightBoundary, startY);
            }
            else if (rightWall)
            {
                for (var x = leftBoundary; x < rightBoundary; x++)
                    map[x, startY] = '|';
                FillDown(map, leftBoundary, startY);
            }
            else
            {
                for (var x = leftBoundary; x <= rightBoundary; x++)
                    map[x, startY] = '|';
                FillDown(map, leftBoundary, startY);
                FillDown(map, rightBoundary, startY);
            }
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(CalculateRetainedStorage(LoadInput("practice")));
            Console.WriteLine(CalculateRetainedStorage(LoadInput()));
        }

        private static int CalculateRetainedStorage(IEnumerable<string> input)
        {
            const int sourceX = 500;
            const int sourceY = 0;

            var (map, xMin, xMax, yMin, yMax) = GenerateMap(input, sourceX, sourceY);

            FillDown(map, sourceX, sourceY);

            var count = 0;
            for (var y = yMin; y <= yMax; y++)
            for (var x = xMin; x <= xMax; x++)
                if (map[x, y] == '~')
                    count++;

            return count;
        }
    }
}