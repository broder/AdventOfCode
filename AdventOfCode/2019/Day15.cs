using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day15 : BaseIntcodeDay
    {
        private const int Floor = 0;
        private const int Wall = 1;
        private const int RepairSystem = 2;

        protected override void RunPartOne()
        {
            Console.WriteLine(FindMinimumRepairSystemDistance(LoadInput().First()));
        }

        private static int FindMinimumRepairSystemDistance(string opcodeString)
        {
            var vm = new IntcodeVM(opcodeString, 6000);

            var start = new Point(0, 0);
            var map = new Dictionary<Point, int> {[start] = Floor};
            ComputeMap(vm, new Stack<Point>(new[] {start}), map);
            
            PrintMap(map);
            
            return FindMinimumRepairSystemDistance(start, map);
        }

        private static void ComputeMap(IntcodeVM vm, Stack<Point> path, Dictionary<Point, int> map)
        {
            for (var direction = 1; direction <= 4; direction++)
            {
                var nextPosition = path.Peek().Add(DirectionToPoint(direction));

                if (map.ContainsKey(nextPosition)) continue;

                var outputs = vm.SendInput(direction).Run().GetOutputs();
                var response = outputs.Last();
                outputs.Clear();

                // wall
                if (response == 0)
                {
                    map[nextPosition] = Wall;
                    continue;
                }

                // oxygen systen
                if (response == 2)
                {
                    map[nextPosition] = RepairSystem;
                }
                else
                {
                    map[nextPosition] = Floor;
                }

                // floor
                path.Push(nextPosition);

                ComputeMap(vm, path, map);

                // backtrack
                path.Pop();
                vm.SendInput(ReverseDirection(direction)).Run().GetOutputs().Clear();
            }
        }
        
        private static void PrintMap(Dictionary<Point, int> map)
        {
            var xMin = map.Keys.Min(p => p.X);
            var yMin = map.Keys.Min(p => p.Y);

            var xMax = map.Keys.Max(p => p.X);
            var yMax = map.Keys.Max(p => p.Y);

            for (var y = yMin; y <= yMax; y++)
            {
                for (var x = xMin; x <= xMax; x++)
                {
                    switch (map.GetValueOrDefault(new Point(x, y), -1))
                    {
                        case Floor:
                            Console.Write('.');
                            break;
                        case Wall:
                            Console.Write('#');
                            break;
                        case RepairSystem:
                            Console.Write('R');
                            break;
                        default:
                            Console.Write(' ');
                            break;
                    }
                }

                Console.WriteLine();
            }
        }

        private static int FindMinimumRepairSystemDistance(Point start, Dictionary<Point, int> map)
        {
            var queue = new Queue<Tuple<Point, int>>(new[] {new Tuple<Point, int>(start, 0)});
            var seen = new HashSet<Point>();

            while (true)
            {
                var (position, distance) = queue.Dequeue();

                for (var direction = 1; direction <= 4; direction++)
                {
                    var nextPosition = position.Add(DirectionToPoint(direction));

                    if (seen.Contains(nextPosition)) continue;
                    seen.Add(nextPosition);
                    
                    if (map[nextPosition] == Wall) continue;

                    if (map[nextPosition] == RepairSystem) return distance + 1;
                    
                    queue.Enqueue(new Tuple<Point, int>(nextPosition, distance + 1));
                }
            }
        }

        private static Point DirectionToPoint(int direction)
        {
            switch (direction)
            {
                case 1:
                    return new Point(0, -1);
                case 2:
                    return new Point(0, 1);
                case 3:
                    return new Point(-1, 0);
                case 4:
                    return new Point(1, 0);
                default:
                    throw new ArgumentException();
            }
        }

        private static int ReverseDirection(int direction)
        {
            switch (direction)
            {
                case 1:
                    return 2;
                case 2:
                    return 1;
                case 3:
                    return 4;
                case 4:
                    return 3;
                default:
                    throw new ArgumentException();
            }
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(FindMaximumDistance(LoadInput().First()));
        }

        private static int FindMaximumDistance(string opcodeString)
        {
            var vm = new IntcodeVM(opcodeString, 6000);

            var start = new Point(0, 0);
            var map = new Dictionary<Point, int> {[start] = Floor};
            ComputeMap(vm, new Stack<Point>(new[] {start}), map);
            
            var repairSystem = map.First(kvp => kvp.Value == RepairSystem).Key;
            return FindMaximumDistance(repairSystem, map);
        }
        
        private static int FindMaximumDistance(Point start, Dictionary<Point, int> map)
        {
            var queue = new Queue<Tuple<Point, int>>(new[] {new Tuple<Point, int>(start, 0)});
            var seen = new HashSet<Point>();
            var maxDistance = 0;

            while (queue.Count > 0)
            {
                var (position, distance) = queue.Dequeue();
                maxDistance = Math.Max(maxDistance, distance);

                for (var direction = 1; direction <= 4; direction++)
                {
                    var nextPosition = position.Add(DirectionToPoint(direction));

                    if (seen.Contains(nextPosition)) continue;
                    seen.Add(nextPosition);
                    
                    if (map[nextPosition] == Wall) continue;
                    
                    queue.Enqueue(new Tuple<Point, int>(nextPosition, distance + 1));
                }
            }

            return maxDistance;
        }
    }
}