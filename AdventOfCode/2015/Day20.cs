using System;

namespace AdventOfCode._2015
{
    internal class Day20 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetFirstHouseWithPresents(30));
            Console.WriteLine(GetFirstHouseWithPresents(40));
            Console.WriteLine(GetFirstHouseWithPresents(70));
            Console.WriteLine(GetFirstHouseWithPresents(60));
            Console.WriteLine(GetFirstHouseWithPresents(120));
            Console.WriteLine(GetFirstHouseWithPresents(80));
            Console.WriteLine(GetFirstHouseWithPresents(150));
            Console.WriteLine(GetFirstHouseWithPresents(130));
            Console.WriteLine(GetFirstHouseWithPresents(36000000));
        }

        private int GetFirstHouseWithPresents(int presents, int stopAt = -1)
        {
            var housesToConsider = presents / 10;
            var houses = new int[housesToConsider];

            for (var elfNumber = 1; elfNumber < housesToConsider; elfNumber++)
            {
                var lastHouseVisited = stopAt == -1 ? housesToConsider : Math.Min(elfNumber * stopAt + 1, housesToConsider);
                for (var houseNumber = elfNumber; houseNumber < lastHouseVisited; houseNumber += elfNumber)
                {
                    houses[houseNumber] += elfNumber * (10 + (stopAt == -1 ? 0 : 1));
                }
            }

            for (var i = 0; i < housesToConsider; i++)
                if (houses[i] >= presents)
                    return i;
            return -1;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetFirstHouseWithPresents(36000000, 50));
        }
    }
}