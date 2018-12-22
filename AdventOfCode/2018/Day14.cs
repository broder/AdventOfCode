using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode._2018
{
    internal class Day14 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetNextTenScoresAfter(9));
            Console.WriteLine(GetNextTenScoresAfter(5));
            Console.WriteLine(GetNextTenScoresAfter(18));
            Console.WriteLine(GetNextTenScoresAfter(2018));
            Console.WriteLine(GetNextTenScoresAfter(890691));
        }

        private static string GetNextTenScoresAfter(int recipes)
        {
            var scoreboard = new LinkedList<short>(new short[] {3, 7});
            var firstElf = scoreboard.First;
            var secondElf = scoreboard.Last;

            while (scoreboard.Count < recipes + 10)
                GetNextIteration(scoreboard, ref firstElf, ref secondElf);

            var output = new StringBuilder();
            var score = scoreboard.First;
            for (var i = 0; i < recipes + 10; i++)
            {
                if (i >= recipes)
                    output.Append(score.Value);
                score = score.Next;
            }

            return output.ToString();
        }

        private static void GetNextIteration(LinkedList<short> scoreboard, ref LinkedListNode<short> firstElf,
            ref LinkedListNode<short> secondElf)
        {
            var nextScore = firstElf.Value + secondElf.Value;

            if (nextScore == 0)
                scoreboard.AddLast(0);
            else
            {
                LinkedListNode<short> node = null;
                while (nextScore != 0)
                {
                    node = node == null
                        ? scoreboard.AddLast((short) (nextScore % 10))
                        : scoreboard.AddBefore(node, (short) (nextScore % 10));

                    nextScore /= 10;
                }
            }

            var stop = firstElf.Value + 1;
            for (var i = 0; i < stop; i++)
                firstElf = GetNextCircularNode(firstElf);

            stop = secondElf.Value + 1;
            for (var i = 0; i < stop; i++)
                secondElf = GetNextCircularNode(secondElf);
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetFirstAppearanceOf("51589"));
            Console.WriteLine(GetFirstAppearanceOf("01245"));
            Console.WriteLine(GetFirstAppearanceOf("92510"));
            Console.WriteLine(GetFirstAppearanceOf("59414"));
            Console.WriteLine(GetFirstAppearanceOf("890691"));
        }

        private static int GetFirstAppearanceOf(string score)
        {
            var scoreboard = new LinkedList<short>(new short[] {3, 7});
            var firstElf = scoreboard.First;
            var secondElf = scoreboard.Last;
            var targetScore = score.Reverse().Select(c => short.Parse(c.ToString())).ToArray();

            while (true)
            {
                GetNextIteration(scoreboard, ref firstElf, ref secondElf);

                if (EndsWith(scoreboard.Last, targetScore))
                    return scoreboard.Count - targetScore.Length;

                if (EndsWith(scoreboard.Last.Previous, targetScore))
                    return scoreboard.Count - targetScore.Length - 1;
            }
        }

        private static bool EndsWith(LinkedListNode<short> lastScore, IEnumerable<short> targetScore)
        {
            foreach (var lastTargetScore in targetScore)
            {
                if (lastScore.Value != lastTargetScore)
                    return false;
                lastScore = lastScore.Previous;
            }

            return true;
        }
    }
}