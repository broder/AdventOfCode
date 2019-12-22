using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode._2019
{
    internal class Day22 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(string.Join(",", ShuffleCards(LoadInput("practice.1"), 10)));
            Console.WriteLine(string.Join(",", ShuffleCards(LoadInput("practice.2"), 10)));
            Console.WriteLine(string.Join(",", ShuffleCards(LoadInput("practice.3"), 10)));
            Console.WriteLine(string.Join(",", ShuffleCards(LoadInput("practice.4"), 10)));
            Console.WriteLine(ShuffleCards(LoadInput(), 10007).TakeWhile(v => v != 2019).Count());
        }

        private static int[] ShuffleCards(string[] instructions, int deckSize)
        {
            var cards = new int[deckSize];
            for (var i = 0; i < cards.Length; i++)
                cards[i] = i;

            foreach (var instruction in instructions)
            {
                var split = instruction.Split(" ");
                var newCards = new int[cards.Length];

                if (split[0] == "cut")
                {
                    var position = int.Parse(split[1]);
                    for (var i = 0; i < cards.Length; i++)
                        newCards[Mod(i - position, newCards.Length)] = cards[i];
                }
                else if (split[2] == "increment")
                {
                    var increment = int.Parse(split[3]);
                    for (var i = 0; i < cards.Length; i++)
                        newCards[Mod(i * increment, newCards.Length)] = cards[i];
                }
                else if (split[3] == "stack")
                {
                    for (var i = 0; i < cards.Length; i++)
                        newCards[newCards.Length - 1 - i] = cards[i];
                }

                cards = newCards;
            }

            return cards;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(FindCard(LoadInput(), 119315717514047, 101741582076661, 2020));
        }

        private static BigInteger FindCard(string[] instructions, long deckSize, long repeats, long target)
        {
            var offsetDiff = new BigInteger(0);
            var incrementMultiplier = new BigInteger(1);
            foreach (var instruction in instructions)
            {
                var split = instruction.Split(" ");
                if (split[0] == "cut")
                {
                    offsetDiff = (offsetDiff + incrementMultiplier * int.Parse(split[1])) % deckSize;
                }
                else if (split[2] == "increment")
                {
                    incrementMultiplier = incrementMultiplier * BigInteger.ModPow(int.Parse(split[3]), deckSize - 2, deckSize);
                }
                else if (split[3] == "stack")
                {
                    incrementMultiplier = -incrementMultiplier;
                    offsetDiff = (offsetDiff + incrementMultiplier)% deckSize;
                }
            }

            var increment = BigInteger.ModPow(incrementMultiplier, repeats, deckSize);
            var offset = offsetDiff * (1 - increment) *
                         BigInteger.ModPow((1 - incrementMultiplier) % deckSize, deckSize - 2, deckSize);
            offset %= deckSize;

            return (offset + increment * target) % deckSize;
        }
    }
}