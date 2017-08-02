using System;
using System.Linq;
using System.Text;

namespace AdventOfCode._2016
{
    internal class Day08 : BaseDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(CountLitScreenPixels(7, 3, "practice"));
            Console.WriteLine(CountLitScreenPixels(50, 6));
        }

        private int CountLitScreenPixels(int rows, int cols, string fileVariant = null)
        {
            return CountPixels(GetScreen(rows, cols, fileVariant));
        }

        private bool[,] GetScreen(int rows, int cols, string fileVariant = null)
        {
            var screen = new bool[rows, cols];
            foreach (var line in LoadInput(fileVariant))
            {
                if (line.StartsWith("rect"))
                {
                    var split = line.Split(new[] {" ", "x"}, StringSplitOptions.None);
                    var a = int.Parse(split[1]);
                    var b = int.Parse(split[2]);
                    DrawRect(screen, a, b);
                }
                else if (line.StartsWith("rotate row"))
                {
                    var split = line.Split(new[] {"row y=", " by "}, StringSplitOptions.None);
                    var a = int.Parse(split[1]);
                    var b = int.Parse(split[2]);
                    RotateRow(screen, a, b);
                }
                else if (line.StartsWith("rotate column"))
                {
                    var split = line.Split(new[] {"column x=", " by "}, StringSplitOptions.None);
                    var a = int.Parse(split[1]);
                    var b = int.Parse(split[2]);
                    RotateCol(screen, a, b);
                }
            }
            return screen;
        }

        private static int CountPixels(bool[,] screen)
        {
            return screen.Cast<bool>().Count(p => p);
        }

        private static void RotateCol(bool[,] screen, int row, int shift)
        {
            var height = screen.GetLength(1);
            for (var s = 0; s < shift; s++)
            {
                var last = screen[row, height - 1];
                for (var i = height - 1; i > 0; i--)
                    screen[row, i] = screen[row, i - 1];

                screen[row, 0] = last;
            }
        }

        private static void RotateRow(bool[,] screen, int col, int shift)
        {
            var width = screen.GetLength(0);
            for (var s = 0; s < shift; s++)
            {
                var last = screen[width - 1, col];
                for (var i = width - 1; i > 0; i--)
                    screen[i, col] = screen[i - 1, col];

                screen[0, col] = last;
            }
        }

        private static void DrawRect(bool[,] screen, int rows, int cols)
        {
            for (var r = 0; r < rows; r++)
                for (var c = 0; c < cols; c++)
                    screen[r, c] = true;
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(PrintScreenPixels(50, 6));
        }

        private string PrintScreenPixels(int rows, int cols, string fileVariant = null)
        {
            return Print(GetScreen(rows, cols, fileVariant));
        }

        private static string Print(bool[,] screen)
        {
            var sb = new StringBuilder();
            for (var c = 0; c < screen.GetLength(1); c++)
            {
                for (var r = 0; r < screen.GetLength(0); r++)
                    sb.Append(screen[r, c] ? '#' : '.');

                sb.Append('\n');
            }
            return sb.ToString();
        }
    }
}