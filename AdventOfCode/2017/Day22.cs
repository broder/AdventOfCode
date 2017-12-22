using System;
using System.Collections.Generic;

namespace AdventOfCode._2017
{
    internal class Day22 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetInfections(LoadInput("practice"), 10000, 10000));
            Console.WriteLine(GetInfections(LoadInput(), 10000, 10000));
        }

        private static int GetInfections(IReadOnlyList<string> input, int bursts, int gridSize, bool advanced = false)
        {
            var grid = new InfectionStates[gridSize, gridSize];
            for (var y = 0; y < input.Count; y++)
            for (var x = 0; x < input[y].Length; x++)
                if (input[y][x] == '#')
                    grid[gridSize / 2 + y, gridSize / 2 + x] = InfectionStates.Infected;

            var xPosition = gridSize / 2 + input[0].Length / 2;
            var yPosition = gridSize / 2 + input.Count / 2;
            var direction = Direction.Up;
            var infections = 0;

            for (var i = 0; i < bursts; i++)
            {
                switch (grid[yPosition, xPosition])
                {
                    case InfectionStates.Clean:
                        if (advanced)
                            grid[yPosition, xPosition] = InfectionStates.Weakened;
                        else
                        {
                            grid[yPosition, xPosition] = InfectionStates.Infected;
                            infections++;
                        }

                        switch (direction)
                        {
                            case Direction.Up:
                                direction = Direction.Left;
                                xPosition--;
                                break;
                            case Direction.Left:
                                direction = Direction.Down;
                                yPosition++;
                                break;
                            case Direction.Down:
                                direction = Direction.Right;
                                xPosition++;
                                break;
                            case Direction.Right:
                                direction = Direction.Up;
                                yPosition--;
                                break;
                        }
                        break;
                    case InfectionStates.Weakened:
                        grid[yPosition, xPosition] = InfectionStates.Infected;
                        infections++;

                        switch (direction)
                        {
                            case Direction.Up:
                                yPosition--;
                                break;
                            case Direction.Left:
                                xPosition--;
                                break;
                            case Direction.Down:
                                yPosition++;
                                break;
                            case Direction.Right:
                                xPosition++;
                                break;
                        }
                        break;
                    case InfectionStates.Infected:
                        if (advanced)
                            grid[yPosition, xPosition] = InfectionStates.Flagged;
                        else
                            grid[yPosition, xPosition] = InfectionStates.Clean;

                        switch (direction)
                        {
                            case Direction.Up:
                                direction = Direction.Right;
                                xPosition++;
                                break;
                            case Direction.Left:
                                direction = Direction.Up;
                                yPosition--;
                                break;
                            case Direction.Down:
                                direction = Direction.Left;
                                xPosition--;
                                break;
                            case Direction.Right:
                                direction = Direction.Down;
                                yPosition++;
                                break;
                        }
                        break;
                    case InfectionStates.Flagged:
                        grid[yPosition, xPosition] = InfectionStates.Clean;

                        switch (direction)
                        {
                            case Direction.Up:
                                direction = Direction.Down;
                                yPosition++;
                                break;
                            case Direction.Left:
                                direction = Direction.Right;
                                xPosition++;
                                break;
                            case Direction.Down:
                                direction = Direction.Up;
                                yPosition--;
                                break;
                            case Direction.Right:
                                direction = Direction.Left;
                                xPosition--;
                                break;
                        }
                        break;
                }
            }
            return infections;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetInfections(LoadInput("practice"), 10000000, 10000, true));
            Console.WriteLine(GetInfections(LoadInput(), 10000000, 10000, true));
        }

        private enum InfectionStates
        {
            Clean,
            Weakened,
            Infected,
            Flagged
        }

        private enum Direction
        {
            Up,
            Left,
            Down,
            Right
        }
    }
}