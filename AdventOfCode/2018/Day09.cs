using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018
{
    internal class Day09 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetHighScore(7, 32));
            Console.WriteLine(GetHighScore(10, 1618));
            Console.WriteLine(GetHighScore(13, 7999));
            Console.WriteLine(GetHighScore(17, 1104));
            Console.WriteLine(GetHighScore(21, 6111));
            Console.WriteLine(GetHighScore(30, 5807));
            Console.WriteLine(GetHighScore(458, 71307));
        }

        private static long GetHighScore(int playerCount, int finalMarbleValue)
        {
            var scores = new long[playerCount];
            var currentPlayer = 0;

            var marbles = new LinkedList<int>();
            var currentMarble = marbles.AddFirst(0);
            var nextMarbleValue = 1;
            while (nextMarbleValue < finalMarbleValue)
            {
                if (Mod(nextMarbleValue, 23) == 0)
                {
                    for (var i = 0; i < 7; i++)
                        currentMarble = GetPreviousCircularNode(currentMarble);
                    var removedMarble = currentMarble;
                    currentMarble = GetNextCircularNode(removedMarble);
                    marbles.Remove(removedMarble);
                    scores[currentPlayer] += removedMarble.Value + nextMarbleValue;
                }
                else
                {
                    currentMarble = GetNextCircularNode(currentMarble);
                    marbles.AddAfter(currentMarble, new LinkedListNode<int>(nextMarbleValue));
                    currentMarble = GetNextCircularNode(currentMarble);
                }


                currentPlayer = Mod(currentPlayer + 1, playerCount);
                nextMarbleValue++;
            }

            return scores.Max();
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetHighScore(458, 71307 * 100));
        }
    }
}