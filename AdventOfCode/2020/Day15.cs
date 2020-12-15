using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2020
{
    internal class Day15 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetSpokenNumber("0,3,6", 2020));
            Console.WriteLine(GetSpokenNumber("1,3,2", 2020));
            Console.WriteLine(GetSpokenNumber("2,1,3", 2020));
            Console.WriteLine(GetSpokenNumber("1,2,3", 2020));
            Console.WriteLine(GetSpokenNumber("2,3,1", 2020));
            Console.WriteLine(GetSpokenNumber("3,2,1", 2020));
            Console.WriteLine(GetSpokenNumber("3,1,2", 2020));
            Console.WriteLine(GetSpokenNumber("14,8,16,0,1,17", 2020));
        }

        private static int GetSpokenNumber(string startingNumbers, int timeLimit)
        {
            var time = 0;
            var lastNumber = 0;
            var recentHistory = new Dictionary<int, int>();
            var secondRecentHistory = new Dictionary<int, int>();
            foreach (var n in startingNumbers.Split(',').Select(int.Parse))
            {
                time++;
                lastNumber = n;
                recentHistory[lastNumber] = time;
            }

            while (time < timeLimit)
            {
                time++;
                if (secondRecentHistory.ContainsKey(lastNumber))
                    lastNumber = recentHistory[lastNumber] - secondRecentHistory[lastNumber];
                else
                    lastNumber = 0;

                if (!recentHistory.ContainsKey(lastNumber))
                    recentHistory[lastNumber] = time;
                else
                {
                    secondRecentHistory[lastNumber] = recentHistory[lastNumber];
                    recentHistory[lastNumber] = time;
                }

            }
            return lastNumber;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetSpokenNumber("0,3,6", 30000000));
            Console.WriteLine(GetSpokenNumber("1,3,2", 30000000));
            Console.WriteLine(GetSpokenNumber("2,1,3", 30000000));
            Console.WriteLine(GetSpokenNumber("1,2,3", 30000000));
            Console.WriteLine(GetSpokenNumber("2,3,1", 30000000));
            Console.WriteLine(GetSpokenNumber("3,2,1", 30000000));
            Console.WriteLine(GetSpokenNumber("3,1,2", 30000000));
            Console.WriteLine(GetSpokenNumber("14,8,16,0,1,17", 30000000));
        }
    }
}