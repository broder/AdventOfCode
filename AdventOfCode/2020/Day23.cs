using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2020
{
    internal class Day23 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetCupString("389125467", 10));
            Console.WriteLine(GetCupString("389125467", 100));
            Console.WriteLine(GetCupString("487912365", 100));
        }

        private static string GetCupString(string input, int moves)
        {
            var cupMap = SimulateCups(input, moves);

            var o = "";
            var currentCup = GetNextCircularNode(cupMap[1]);
            while (currentCup.Value != 1)
            {
                o += currentCup.Value;
                currentCup = GetNextCircularNode(currentCup);
            }

            return o;
        }

        private static Dictionary<int, LinkedListNode<int>> SimulateCups(string input, int moves, int totalCups = 0)
        {
            var cups = new LinkedList<int>(input.Select(c => int.Parse(c.ToString())));
            for (var i = cups.Max() + 1; cups.Count < totalCups; i++)
                cups.AddLast(i);

            var cupMap = new Dictionary<int, LinkedListNode<int>>();
            var currentCup = cups.First;
            while (currentCup != null)
            {
                cupMap[currentCup.Value] = currentCup;
                currentCup = currentCup.Next;
            }
            currentCup = cups.First;

            var min = cupMap.Keys.Min();
            var max = cupMap.Keys.Max();
            for (var t = 0; t < moves; t++)
            {
                var removedCups = new List<LinkedListNode<int>>();
                for (var i = 0; i < 3; i++)
                {
                    var nextNode = GetNextCircularNode(currentCup);
                    removedCups.Add(nextNode);
                    cups.Remove(nextNode);
                }

                var insertValue = currentCup.Value;
                LinkedListNode<int> insertCup = null;
                while (insertCup == null)
                {
                    insertValue -= 1;
                    if (insertValue < min)
                        insertValue = max;

                    if (cupMap.ContainsKey(insertValue) && removedCups.Count(n => n.Value == insertValue) == 0)
                        insertCup = cupMap[insertValue];
                }

                foreach (var c in removedCups)
                {
                    cups.AddAfter(insertCup, c);
                    insertCup = c;
                }

                currentCup = GetNextCircularNode(currentCup);
            }

            return cupMap;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetStarCups("389125467"));
            Console.WriteLine(GetStarCups("487912365"));
        }

        private static long GetStarCups(string input)
        {
            var cupMap = SimulateCups(input, 10000000, 1000000);
            return (long)GetNextCircularNode(cupMap[1]).Value * (long)GetNextCircularNode(GetNextCircularNode(cupMap[1])).Value;
        }
    }
}