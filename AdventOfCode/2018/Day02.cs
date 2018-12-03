using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode._2018
{
    internal class Day02 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(CalculateChecksum(LoadInput("1.practice")));
            Console.WriteLine(CalculateChecksum(LoadInput()));
        }

        private static int CalculateChecksum(IEnumerable<string> boxIds)
        {
            var doubleCount = 0;
            var tripleCount = 0;

            foreach (var boxId in boxIds)
            {
                var counts = boxId.GroupBy(c => c).Select(g => g.Count()).ToArray();
                if (counts.Contains(2))
                    doubleCount++;
                if (counts.Contains(3))
                    tripleCount++;
            }

            return doubleCount * tripleCount;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetCommonLetters(LoadInput("2.practice")));
            Console.WriteLine(GetCommonLetters(LoadInput()));
        }

        private static string GetCommonLetters(IReadOnlyList<string> boxIds)
        {
            int i;
            for (i = 0; i < boxIds.Count; i++)
            {
                int j;
                for (j = i; j < boxIds.Count; j++)
                {
                    var diffs = new List<int>();
                    for (var k = 0; k < boxIds[i].Length; k++)
                    {
                        if (boxIds[i][k] != boxIds[j][k])
                            diffs.Add(k);

                        if (diffs.Count > 1)
                            break;
                    }

                    if (diffs.Count != 1)
                        continue;
                    
                    var output = new StringBuilder(boxIds[i]);
                    output.Remove(diffs[0], 1);
                    return output.ToString();
                }
            }

            return "NOT FOUND";
        }
    }
}