using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2016
{
    internal class Day09 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetCharCount("ADVENT"));
            Console.WriteLine(GetCharCount("A(1x5)BC"));
            Console.WriteLine(GetCharCount("(3x3)XYZ"));
            Console.WriteLine(GetCharCount("A(2x2)BCD(2x2)EFG"));
            Console.WriteLine(GetCharCount("(6x1)(1x3)A"));
            Console.WriteLine(GetCharCount("X(8x2)(3x3)ABCY"));
            Console.WriteLine(GetCharCountFromFile());
        }

        private long GetCharCountFromFile(bool recurse = false)
        {
            return LoadInput().Sum(line => GetCharCount(line, recurse));
        }

        public readonly Regex Regex = new Regex(@"^\((.*?)x(.*?)\)");

        private long GetCharCount(string input, bool recurse = false)
        {
            long output = 0;
            var currentIndex = 0;
            while (currentIndex < input.Length)
            {
                var subInput = input.Substring(currentIndex);
                var match = Regex.Match(subInput);
                if (match.Success)
                {
                    var repeatInstructionLength = match.Groups[0].Value.Length;

                    var repeatCharacters = int.Parse(match.Groups[1].Value);
                    var repeatTimes = int.Parse(match.Groups[2].Value);

                    if (recurse)
                        output += GetCharCount(subInput.Substring(repeatInstructionLength, repeatCharacters), true) * repeatTimes;
                    else
                        output += repeatCharacters * repeatTimes;

                    currentIndex += repeatInstructionLength + repeatCharacters;
                }
                else
                {
                    output++;
                    currentIndex++;
                }
            }
            return output;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetCharCount("(3x3)XYZ", true));
            Console.WriteLine(GetCharCount("X(8x2)(3x3)ABCY", true));
            Console.WriteLine(GetCharCount("(27x12)(20x12)(13x14)(7x10)(1x12)A", true));
            Console.WriteLine(GetCharCount("(25x3)(3x3)ABC(2x3)XY(5x2)PQRSTX(18x9)(3x2)TWO(5x7)SEVEN", true));
            Console.WriteLine(GetCharCountFromFile(true));
        }
    }
}