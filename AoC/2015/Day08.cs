using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC._2015
{
    internal class Day08 : BaseDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(GetCharacterDifference(Decode, "practice"));
            Console.WriteLine(GetCharacterDifference(Decode));
        }

        private int GetCharacterDifference(Func<string, string> method, string fileVariant = null)
        {
            return LoadInput(fileVariant).Sum(s => Math.Abs(method(s).Length - s.Length));
        }

        private string Decode(string line)
        {
            var parsedLine = line.Trim('"');
            parsedLine = parsedLine.Replace("\\\\", "a");
            parsedLine = parsedLine.Replace("\\\"", "b");
            return new Regex(@"\\x[a-f0-9]{2}").Replace(parsedLine, "c");
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(GetCharacterDifference(Encode, "practice"));
            Console.WriteLine(GetCharacterDifference(Encode));
        }

        private string Encode(string line)
        {
            var parsedLine = line.Replace("\\", "aa");
            parsedLine = parsedLine.Replace("\"", "bb");
            return "\"" + parsedLine + "\"";
        }
    }
}