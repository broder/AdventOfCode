using System;
using System.Linq;
using System.Text;

namespace AoC._2016
{
    internal class Day06 : BaseDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(GetRepetitionCode(0, "practice"));
            Console.WriteLine(GetRepetitionCode(0));
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(GetRepetitionCode(-1, "practice"));
            Console.WriteLine(GetRepetitionCode(-1));
        }

        private string GetRepetitionCode(int rank, string fileVariant = null)
        {
            var lines = LoadInput(fileVariant);
            var message = new StringBuilder();
            for (var c = 0; c < lines[0].Length; c++)
            {
                var letters = Enumerable
                    .Range(0, lines.Length)
                    .Select(r => lines[r][c])
                    .GroupBy(ch => ch)
                    .OrderByDescending(gp => gp.Count())
                    .Select(gp => gp.Key)
                    .ToArray();

                message.Append(letters.ElementAt(Mod(rank, letters.Length)));
            }
            return message.ToString();
        }
    }
}