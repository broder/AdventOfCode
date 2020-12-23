using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2020
{
    internal class Day22 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetWinningScore(LoadInput("practice")));
            Console.WriteLine(GetWinningScore(LoadInput()));
        }

        private static int GetWinningScore(string[] input)
        {
            var cards = ParseCards(input);
            while (cards.All(c => c.Count() > 0)) {
                var playedCards = cards.Select(c => c.Dequeue()).ToArray();
                var winningCard = playedCards.Max();
                var winningPlayer = Array.IndexOf(playedCards, winningCard);

                foreach (var c in playedCards.OrderByDescending(c => c))
                    cards.ElementAt(winningPlayer).Enqueue(c);
            }
            var winner = cards.Where(c => c.Count() > 0).Single();
            var totalCards = winner.Count();
            return winner.Select((c, i) => c * (totalCards - i)).Sum();
        }

        private static List<Queue<int>> ParseCards(string[] input) {
            var cards = new List<Queue<int>>();
            foreach (var player in string.Join("\n", input).Split("\n\n"))
                cards.Add(new Queue<int>(player.Split("\n").Skip(1).Select(int.Parse)));
            return cards;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetRecursiveWinningScore(LoadInput("practice")));
            Console.WriteLine(GetRecursiveWinningScore(LoadInput()));
        }

        private static int GetRecursiveWinningScore(string[] input)
        {
            var cards = ParseCards(input);

            var winningPlayer = GetWinner(cards);
            var winner = cards.ElementAt(winningPlayer);
            var totalCards = winner.Count();
            return winner.Select((c, i) => c * (totalCards - i)).Sum();
        }

        private static int GetWinner(List<Queue<int>> cards) {
            var seenStates = new HashSet<string>();
            while (cards.All(c => c.Count() > 0)) {
                var state = string.Join(",", cards.ElementAt(0));
                if (!seenStates.Add(state)) {
                    return 0;
                }

                var playedCards = cards.Select(c => c.Dequeue()).ToArray();

                int winningPlayer;
                if (playedCards.Select((c, i) => (c, i)).All((a) => a.c <= cards.ElementAt(a.i).Count())) {
                    var clonedCards = playedCards.Select((c, i) => (c, i)).Select(a => new Queue<int>(cards.ElementAt(a.i).Take(a.c))).ToList();
                    winningPlayer = GetWinner(clonedCards);
                } else {
                    var winningCard = playedCards.Max();
                    winningPlayer = Array.IndexOf(playedCards, winningCard);
                }

                cards.ElementAt(winningPlayer).Enqueue(playedCards.ElementAt(winningPlayer));
                cards.ElementAt(winningPlayer).Enqueue(playedCards.ElementAt(winningPlayer == 0 ? 1 : 0));
            }
            return cards.IndexOf(cards.Where(c => c.Count() > 0).Single());
        }
    }
}