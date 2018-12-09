using System;
using System.Collections.Generic;

namespace AdventOfCode._2017
{
    internal class Day17 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetSubsequentValue(9, 3));
            Console.WriteLine(GetSubsequentValue(2017, 3));
            Console.WriteLine(GetSubsequentValue(2017, 349));
        }

        private static int GetSubsequentValue(int size, int step)
        {
            var list = new LinkedList<int>(new int[1]);
            var current = list.First;

            for (var i = 1; i <= size; i++)
            {
                for (var j = 0; j < step; j++)
                    current = GetNextCircularNode(current);

                var next = new LinkedListNode<int>(i);
                list.AddAfter(current, next);
                current = next;
            }
            return GetNextCircularNode(current).Value;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetSubsequentZeroValue(50000000, 349));
        }

        private static int GetSubsequentZeroValue(int size, int step)
        {
            var currentIndex = 0;
            var value = 0;
            for (var i = 1; i <= size; i++)
            {
                currentIndex = Mod(currentIndex + step, i) + 1;
                if (currentIndex == 1)
                    value = i;
            }
            return value;
        }
    }
}