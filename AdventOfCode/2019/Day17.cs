using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode._2019
{
    internal class Day17 : BaseOpcodeDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(
                GetScaffoldAlignmentParameterSum(StringArrayToMap(LoadInput("practice.1"))));

            Console.WriteLine(GetScaffoldAlignmentParameterSum(OpcodeToMap(LoadInput().First())));
        }

        private static List<List<char>> StringArrayToMap(string[] arr) =>
            arr.Select(s => s.ToCharArray().ToList())
                .ToList();

        private static List<List<char>> OpcodeToMap(string opcodeString) =>
            ListToMap(new OpcodeVM(opcodeString, 4000).Run().GetOutputs());

        private static List<List<char>> ListToMap(List<long> list) => list.Aggregate(
            new List<List<char>> {new List<char>()}, (l, i) =>
            {
                if (i == '\n')
                    l.Add(new List<char>());
                else
                    l.Last().Add((char) i);
                return l;
            });

        private static int GetScaffoldAlignmentParameterSum(List<List<char>> map)
        {
            var sum = 0;
            for (var y = 1; y < map.Count - 1; y++)
            {
                for (var x = 1; x < map[y].Count - 1; x++)
                {
                    if (map[y][x] == '#'
                        && map[y][x - 1] == '#'
                        && map[y][x + 1] == '#'
                        && map[y - 1][x] == '#'
                        && map[y + 1][x] == '#')
                        sum += x * y;
                }
            }

            return sum;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(PathToRoutine(GetPath(StringArrayToMap(LoadInput("practice.2")))));

            var opcodes = ParseOpcodesFromString(LoadInput().First());
            opcodes[0] = 2;
            Console.WriteLine(GetCollectedSpaceDust(opcodes));
        }

        private static long GetCollectedSpaceDust(long[] opcodes)
        {
            var vm = new OpcodeVM(opcodes, 4000).Run();
            var outputs = vm.GetOutputs();
            var map = ListToMap(outputs);
            outputs.Clear();
            var path = GetPath(map);
            var routine = PathToRoutine(path);
            foreach (var c in routine)
                vm.SendInput(c);


            vm.SendInput('n').SendInput('\n').Run();
            return vm.GetOutputs().Last();
        }

        private static string GetPath(List<List<char>> map)
        {
            var (robotLocation, robotDirection) = FindRobot(map);
            var currentForward = 0;

            var path = new StringBuilder();

            while (true)
            {
                var nextLocation = robotLocation.Add(robotDirection);
                if (nextLocation.Y >= 0
                    && nextLocation.Y < map.Count
                    && nextLocation.X >= 0
                    && nextLocation.X < map[nextLocation.Y].Count
                    && map[nextLocation.Y][nextLocation.X] == '#')
                {
                    currentForward++;
                    robotLocation = nextLocation;
                    continue;
                }

                var rightDirection = RotateRight(robotDirection);
                var rightLocation = robotLocation.Add(rightDirection);
                if (rightLocation.Y >= 0
                    && rightLocation.Y < map.Count
                    && rightLocation.X >= 0
                    && rightLocation.X < map[rightLocation.Y].Count
                    && map[rightLocation.Y][rightLocation.X] == '#')
                {
                    if (currentForward > 0)
                    {
                        path.Append(currentForward).Append(',');
                        currentForward = 0;
                    }

                    robotDirection = rightDirection;
                    path.Append('R').Append(',');
                    continue;
                }

                var leftDirection = RotateLeft(robotDirection);
                var leftLocation = robotLocation.Add(leftDirection);
                if (leftLocation.Y >= 0
                    && leftLocation.Y < map.Count
                    && leftLocation.X >= 0
                    && leftLocation.X < map[leftLocation.Y].Count
                    && map[leftLocation.Y][leftLocation.X] == '#')
                {
                    if (currentForward > 0)
                    {
                        path.Append(currentForward).Append(',');
                        currentForward = 0;
                    }

                    robotDirection = leftDirection;
                    path.Append('L').Append(',');
                    continue;
                }

                if (currentForward > 0) path.Append(currentForward).Append(',');
                break;
            }

            return path.ToString();
        }

        private static Point RotateLeft(Point p) => new Point(p.Y, -p.X);
        private static Point RotateRight(Point p) => new Point(-p.Y, p.X);

        private static (Point location, Point direction) FindRobot(List<List<char>> map)
        {
            for (var y = 0; y < map.Count; y++)
            for (var x = 0; x < map[y].Count; x++)
            {
                switch (map[y][x])
                {
                    case '^':
                        return (new Point(x, y), new Point(0, -1));
                    case 'v':
                        return (new Point(x, y), new Point(0, 1));
                    case '<':
                        return (new Point(x, y), new Point(-1, 0));
                    case '>':
                        return (new Point(x, y), new Point(1, 0));
                }
            }

            throw new ArgumentException();
        }

        private static string PathToRoutine(string path)
        {
            var (a, b, c) = FactoriseString(path);

            var mainRoutine = path.Replace(a, "A,").Replace(b, "B,").Replace(c, "C,");

            return
                $"{mainRoutine.Substring(0, mainRoutine.Length - 1)}\n{a.Substring(0, a.Length - 1)}\n{b.Substring(0, b.Length - 1)}\n{c.Substring(0, c.Length - 1)}\n";
        }

        private static (string a, string b, string c) FactoriseString(string path)
        {
            for (var a = 8; a <= 20; a++)
            {
                var aString = path.Substring(0, a);
                if (aString.Last() != ',') continue;
                if (aString.Count(i => i == ',') % 2 != 0) continue;

                for (var b = 8; b <= 20; b++)
                {
                    var bString = path.Substring(path.Length - b, b);
                    if (bString.First() == ',') continue;
                    if (bString.Count(i => i == ',') % 2 != 0) continue;

                    var pathWithAAndB = path.Replace(aString, "A,").Replace(bString, "B,");
                    var startIndex = FindFirstNonCharacterIndex(pathWithAAndB, "AB,");
                    for (var c = 8; c <= 20; c++)
                    {
                        var cString = pathWithAAndB.Substring(startIndex, c);
                        if (cString.Last() != ',') continue;
                        if (cString.Count(i => i == ',') % 2 != 0) continue;

                        var pathWithABAndC = pathWithAAndB.Replace(cString, "C,");
                        if (FindFirstNonCharacterIndex(pathWithABAndC, "ABC,") == -1)
                            return (aString, bString, cString);
                    }
                }
            }

            throw new ArgumentException();
        }

        private static int FindFirstNonCharacterIndex(string path, string chars)
        {
            for (var i = 0; i < path.Length; i++)
                if (!chars.Contains(path[i]))
                    return i;
            return -1;
        }
    }
}