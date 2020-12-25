using System;

namespace AdventOfCode._2020
{
    internal class Day25 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetEncryptionKey(5764801L, 17807724L));
            Console.WriteLine(GetEncryptionKey(6270530L, 14540258L));
        }

        private static long GetEncryptionKey(long cardKey, long doorKey)
        {
            var cardLoopSize = GetLoopSize(7, cardKey);
            return Transform(doorKey, cardLoopSize);
        }

        private static long GetLoopSize(long subject, long key)
        {
            var value = 1L;
            for (var t = 1; ; t++)
            {
                value *= subject;
                value %= 20201227;
                if (value == key)
                    return t;
            }
        }

        private static long Transform(long subject, long loopSize)
        {
            var value = 1L;
            for (var t = 0; t < loopSize; t++)
            {
                value *= subject;
                value %= 20201227;
            }
            return value;
        }

        protected override void RunPartTwo()
        {
        }
    }
}