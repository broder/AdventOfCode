using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day13 : BaseOpcodeDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(new OpcodeVM(LoadInput().First(), 4000).Run().GetOutputs()
                .Where((t, i) => t == 2 && (i + 1) % 3 == 0).Count());
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(PlayGame(LoadInput().First()));
        }

        private static long PlayGame(string opcodeString, bool autoplay = true)
        {
            var opcodes = ParseOpcodesFromString(opcodeString);
            opcodes[0] = 2;

            var vm = new OpcodeVM(opcodes, 4000);
            var screen = new Dictionary<Point, long>();

            while (!vm.IsFinished())
            {
                var outputs = vm.Run().GetOutputs();
                for (var i = 0; i < outputs.Count; i += 3)
                    screen[new Point((int) outputs[i], (int) outputs[i + 1])] = outputs[i + 2];
                outputs.Clear();

                if (autoplay)
                {
                    var paddle = screen.First(kvp => kvp.Value == 3);
                    var ball = screen.First(kvp => kvp.Value == 4);

                    if (paddle.Key.X < ball.Key.X)
                        vm.SendInput(1);
                    else if (paddle.Key.X > ball.Key.X)
                        vm.SendInput(-1);
                    else
                        vm.SendInput(0);
                }
                else
                {
                    PrintScreen(screen);
                    var key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.A:
                            vm.SendInput(-1);
                            break;

                        case ConsoleKey.S:
                            vm.SendInput(0);
                            break;

                        case ConsoleKey.D:
                            vm.SendInput(1);
                            break;
                    }
                }
            }

            var score = screen[new Point(-1, 0)];
            return score;
        }

        private static void PrintScreen(Dictionary<Point, long> screen)
        {
            Console.Clear();
            var maxX = screen.Keys.Max(p => p.X);
            var maxY = screen.Keys.Max(p => p.Y);

            for (var y = 0; y <= maxY; y++)
            {
                for (var x = 0; x <= maxX; x++)
                {
                    switch (screen[new Point(x, y)])
                    {
                        case 1:
                            Console.Write('W');
                            break;
                        case 2:
                            Console.Write('B');
                            break;
                        case 3:
                            Console.Write('P');
                            break;
                        case 4:
                            Console.Write('O');
                            break;
                        default:
                            Console.Write('.');
                            break;
                    }
                }

                Console.WriteLine();
            }

            var score = screen[new Point(-1, 0)];
            Console.WriteLine($"Score: {score}");
        }
    }
}