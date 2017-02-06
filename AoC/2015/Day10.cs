using System;
using System.Text;

namespace AoC._2015
{
    internal class Day10 : BaseDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(GetResultLength(1, 1));
            Console.WriteLine(GetResultLength(1, 2));
            Console.WriteLine(GetResultLength(1, 3));
            Console.WriteLine(GetResultLength(1, 4));
            Console.WriteLine(GetResultLength(1, 5));
            Console.WriteLine(GetResultLength(1113122113, 40));
        }

        private int GetResultLength(int input, int steps)
        {
            var value = input.ToString();
            for (var i = 0; i < steps; i++)
            {
                var nextValue = new StringBuilder();
                var sequenceCount = -1;
                var sequenceCharacter = -1;
                foreach (var l in value)
                {
                    var c = int.Parse(l.ToString());
                    if (c == sequenceCharacter)
                    {
                        sequenceCount++;
                        continue;
                    }

                    if (sequenceCharacter != -1)
                    {
                        nextValue.Append(sequenceCount);
                        nextValue.Append(sequenceCharacter);
                    }
                    sequenceCharacter = c;
                    sequenceCount = 1;
                }
                nextValue.Append(sequenceCount);
                nextValue.Append(sequenceCharacter);
                value = nextValue.ToString();
            }
            return value.Length;
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(GetResultLength(1113122113, 50));
        }
    }
}