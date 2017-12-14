using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode._2017
{
    internal class Day10 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetFirstTwoMultiplied(5, new[] {3, 4, 1, 5}));
            Console.WriteLine(GetFirstTwoMultiplied(256, LoadInput().First().Split(',').Select(int.Parse)));
        }

        private static int GetFirstTwoMultiplied(int size, IEnumerable<int> lengths)
        {
            var list = RunSimulation(size, lengths);
            return list.First.Value * GetNextNode(list.First).Value;
        }

        private static LinkedList<int> RunSimulation(int size, IEnumerable<int> lengths, int rounds = 1)
        {
            var list = new LinkedList<int>(Enumerable.Range(0, size));
            var lengthsArray = lengths.ToArray();

            var current = list.First;
            var skip = 0;
            for (var round = 0; round < rounds; round++)
            {
                foreach (var length in lengthsArray)
                {
                    var lengthFirst = current;
                    for (var i = 0; i < length; i++)
                        current = GetNextNode(current);
                    var lengthLast = GetPreviousNode(current);

                    for (var i = 0; i < length / 2; i++)
                    {
                        var temp = lengthFirst.Value;
                        lengthFirst.Value = lengthLast.Value;
                        lengthLast.Value = temp;

                        lengthFirst = GetNextNode(lengthFirst);
                        lengthLast = GetPreviousNode(lengthLast);
                    }

                    for (var i = 0; i < skip; i++)
                        current = GetNextNode(current);
                    skip++;
                }
            }
            return list;
        }

        private static LinkedListNode<int> GetNextNode(LinkedListNode<int> n) => n.Next ?? n.List.First;

        private static LinkedListNode<int> GetPreviousNode(LinkedListNode<int> n) => n.Previous ?? n.List.Last;

        protected override void RunPartTwo()
        {
            Console.WriteLine(string.Join(",", GetLengths("1,2,3")));
            Console.WriteLine(string.Join(",", GetDenseHash(new[] {65, 27, 9, 1, 4, 3, 40, 50, 91, 7, 6, 0, 2, 5, 68, 22})));
            Console.WriteLine(ToHexadecimal(new[] {64, 7, 255}));
            Console.WriteLine(GetKnotHash(""));
            Console.WriteLine(GetKnotHash("AoC 2017"));
            Console.WriteLine(GetKnotHash("1,2,3"));
            Console.WriteLine(GetKnotHash("1,2,4"));
            Console.WriteLine(GetKnotHash(LoadInput().First()));
        }

        public static string GetKnotHash(string input)
        {
            var lengths = GetLengths(input);
            var list = RunSimulation(256, lengths, 64);
            var denseHash = GetDenseHash(list);
            return ToHexadecimal(denseHash);
        }

        private static IEnumerable<int> GetLengths(string input) =>
            input.Select(c => (int) c).Concat(new[] {17, 31, 73, 47, 23});

        private static IEnumerable<int> GetDenseHash(IEnumerable<int> input)
        {
            var values = new List<int>();
            foreach (var i in input)
            {
                values.Add(i);
                if (values.Count != 16) continue;

                yield return values.Aggregate(0, (agg, k) => agg == 0 ? k : agg ^ k);
                values = new List<int>();
            }
        }

        private static string ToHexadecimal(IEnumerable<int> input) =>
            input.Aggregate(new StringBuilder(), (sb, i) => sb.Append(i.ToString("x2"))).ToString();
    }
}