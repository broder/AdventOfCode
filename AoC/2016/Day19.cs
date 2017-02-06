using System;

namespace AoC._2016
{
    internal class Day19 : BaseDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(GetWinningElf(5));
            Console.WriteLine(GetWinningElf(3018458));
        }

        private int GetWinningElf(int elves, bool opposite = false)
        {
            var start = new Elf {Index = 0};
            var current = start;
            Elf toRemove = null;
            for (var i = 1; i <= elves; i++)
            {
                if (i == elves)
                    current.Left = start;
                else
                {
                    current.Left = new Elf {Index = i, Right = current};
                    current = current.Left;
                }

                if (opposite && i == elves / 2)
                    toRemove = current;
            }
            start.Right = current;

            current = start;
            if (!opposite)
                toRemove = current.Left;

            var hasPresents = elves;
            while (hasPresents > 1)
            {
                //Remove node
                toRemove.Right.Left = toRemove.Left;
                toRemove.Left.Right = toRemove.Right;

                //Move node pointers
                current = current.Left;
                if (opposite)
                    toRemove = hasPresents % 2 == 0 ? toRemove.Left : toRemove.Left.Left;
                else
                    toRemove = current.Left;
                hasPresents--;
            }
            return current.Index + 1;
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(GetWinningElf(5, true));
            Console.WriteLine(GetWinningElf(3018458, true));
        }

        private class Elf
        {
            public int Index;
            public Elf Right;
            public Elf Left;
        }
    }
}