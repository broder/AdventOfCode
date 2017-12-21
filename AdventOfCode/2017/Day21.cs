using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode._2017
{
    internal class Day21 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetOnPixels(LoadInput("practice"), ".#./..#/###", 2));
            Console.WriteLine(GetOnPixels(LoadInput(), ".#./..#/###", 5));
        }

        private static int GetOnPixels(IEnumerable<string> input, string startingGrid, int iterations)
        {
            var rules = new Dictionary<string, string>();
            foreach (var line in input)
            {
                var split = line.Split(new[] {" => "}, StringSplitOptions.RemoveEmptyEntries);
                var grid = split[0];
                var reverse = string.Join("/", grid.Split('/').Reverse());
                var output = split[1];

                for (var i = 0; i < 4; i++)
                {
                    grid = Rotate(grid);
                    rules[grid] = output;
                    reverse = Rotate(reverse);
                    rules[reverse] = output;
                }
            }

            var currentGrid = startingGrid;

            for (var i = 0; i < iterations; i++)
            {
                var gridLength = currentGrid.IndexOf('/');
                if (gridLength % 2 == 0)
                    currentGrid = GetNextGrid(rules, currentGrid, 2);
                else if (gridLength % 3 == 0)
                    currentGrid = GetNextGrid(rules, currentGrid, 3);
            }

            return currentGrid.Count(c => c == '#');
        }

        private static string Rotate(string input)
        {
            var grid = input.Split('/').ToArray();
            var sb = new StringBuilder();
            for (var y = 0; y < grid.Length; y++)
            {
                for (var x = 0; x < grid.Length; x++)
                    sb.Append(grid[x][grid.Length - 1 - y]);

                if (y != grid.Length - 1)
                    sb.Append('/');
            }
            return sb.ToString();
        }

        private static string GetNextGrid(IReadOnlyDictionary<string, string> rules, string currentGrid, int size)
        {
            var currentGridSplit = currentGrid.Split('/').ToArray();
            var subgridNumber = currentGridSplit.Length / size;
            var nextGrid = new string[subgridNumber * (size + 1)];
                    
            for (var offsetY = 0; offsetY < subgridNumber; offsetY++)
            {
                for (var offsetX = 0; offsetX < subgridNumber; offsetX++)
                {
                    var currentSubgrid = new string[size];
                    for (var y = 0; y < size; y++)
                        currentSubgrid[y] = currentGridSplit[offsetY * size + y].Substring(offsetX * size, size);
                            
                    var nextSubgrid = rules[string.Join("/", currentSubgrid)].Split('/').ToArray();
                            
                    for (var y = 0; y < size + 1; y++)
                        nextGrid[offsetY * (size + 1) + y] += nextSubgrid[y];
                }
            }
            return string.Join("/", nextGrid);
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetOnPixels(LoadInput(), ".#./..#/###", 18));
        }
    }
}