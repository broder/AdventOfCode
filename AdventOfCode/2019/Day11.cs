using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day11 : BaseIntcodeDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetVisitedPanelCount(LoadInput().First()));
        }
        
        private static int GetVisitedPanelCount(string opcodeString) => RunRobot(opcodeString).Count;

        private static Dictionary<Point, bool> RunRobot(string opcodeString, bool initialPanelIsWhite = false)
        {
            var opcodeVM = new IntcodeVM(opcodeString);

            var directions = new[] {new Point(0, -1), new Point(1, 0), new Point(0, 1), new Point(-1, 0)};
            
            var robotPosition = new Point(0, 0);
            var robotDirectionIndex = 0;

            var whitePanels = new Dictionary<Point, bool> {[robotPosition] = initialPanelIsWhite};

            while (!opcodeVM.IsFinished())
            {
                var currentPanelIsWhite = whitePanels.GetValueOrDefault(robotPosition, false);
                var output = opcodeVM.SendInput(currentPanelIsWhite ? 1 : 0).Run().GetOutputs();

                var colourToPaint = output.ElementAt(output.Count - 2);
                whitePanels[robotPosition] = colourToPaint != 0;

                var directionToTurn = output.ElementAt(output.Count - 1);
                robotDirectionIndex = Mod(robotDirectionIndex + (directionToTurn == 0 ? -1 : 1), 4);

                robotPosition = robotPosition.Add(directions[robotDirectionIndex]);
            }

            return whitePanels;
        }
        
        protected override void RunPartTwo()
        {
            PrintRobotPath(LoadInput().First());
        }

        private static void PrintRobotPath(string opcodeString)
        {
            var whitePanels = RunRobot(opcodeString, true);

            var minX = whitePanels.Keys.Min(p => p.X);
            var minY = whitePanels.Keys.Min(p => p.Y);
            var maxX = whitePanels.Keys.Max(p => p.X);
            var maxY = whitePanels.Keys.Max(p => p.Y);

            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    Console.Write(whitePanels.GetValueOrDefault(new Point(x, y), false) ? '#' : '.');
                }
                Console.WriteLine();                
            }
        }
    }
}