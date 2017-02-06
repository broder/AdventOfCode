using System;
using System.Text.RegularExpressions;

namespace AoC._2016
{
    internal class Day22 : BaseDay
    {
        private static readonly Regex NodeRegex = new Regex(
            @"/dev/grid/node-x(\d*?)-y(\d*?)\s*?(\d*?)T\s*?(\d*?)T\s*?(\d*?)T\s*?(\d*?)\%");

        public override void RunPartOne()
        {
            Console.WriteLine(GetViablePairs(35, 29));
        }

        public override void RunPartTwo()
        {
            PrintGrid(3, 3, "practice");
            PrintGrid(35, 29);
        }

        private int GetViablePairs(int w, int h, string fileVariant = null)
        {
            var nodes = new Node[w, h];
            var lines = LoadInput(fileVariant);
            foreach (var line in lines)
            {
                var match = NodeRegex.Match(line);
                if (!match.Success) continue;
                var x = int.Parse(match.Groups[1].Value);
                var y = int.Parse(match.Groups[2].Value);
                var size = int.Parse(match.Groups[3].Value);
                var used = int.Parse(match.Groups[4].Value);
                var avail = int.Parse(match.Groups[5].Value);

                nodes[x, y] = new Node {Avail = avail, Used = used};
            }

            var viableNodes = 0;
            for (var i = 0; i < nodes.GetLength(0); i++)
            {
                for (var j = 0; j < nodes.GetLength(1); j++)
                {
                    for (var k = 0; k < nodes.GetLength(0); k++)
                    {
                        for (var m = 0; m < nodes.GetLength(1); m++)
                        {
                            if (i == k && j == m) continue;
                            var a = nodes[i, j];
                            var b = nodes[k, m];
                            if (a.IsViableWith(b))
                                viableNodes++;
                        }
                    }
                }
            }
            return viableNodes;
        }

        private void PrintGrid(int w, int h, string fileVariant = null)
        {
            var nodes = new Node[w, h];
            var lines = LoadInput(fileVariant);
            foreach (var line in lines)
            {
                var match = NodeRegex.Match(line);
                if (!match.Success) continue;
                var x = int.Parse(match.Groups[1].Value);
                var y = int.Parse(match.Groups[2].Value);
                var used = int.Parse(match.Groups[4].Value);
                var avail = int.Parse(match.Groups[5].Value);

                nodes[x, y] = new Node {Avail = avail, Used = used};
            }

            for (var j = 0; j < nodes.GetLength(1); j++)
            {
                for (var i = 0; i < nodes.GetLength(0); i++)
                {
                    var node = nodes[i, j];
                    if (j == 0)
                    {
                        if (i == 0)
                            Console.Write($"({node.PrintNode()})\t");
                        else if (i == nodes.GetLength(0) - 1)
                            Console.Write($"G\t");
                        else
                            Console.Write($"{node.PrintNode()}\t");
                    }
                    else
                        Console.Write($"{node.PrintNode()}\t");
                }
                Console.Write("\n");
            }
        }

    private class Node
    {
        public int Used;
        public int Avail;

        public bool IsViableWith(Node b)
        {
            return Used > 0 && Used <= b.Avail;
        }

        public string PrintNode()
        {
            if (Used == 0)
                return "_";
            return Used < 92 ? "." : "#";
        }
    }
    }
}