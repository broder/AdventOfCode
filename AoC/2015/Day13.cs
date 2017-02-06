using System;
using System.Linq;
using Combinatorics.Collections;

namespace AoC._2015
{
    internal class Day13 : BaseDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(GetTotalHappiness(4, "practice"));
            Console.WriteLine(GetTotalHappiness(8));
        }

        private int GetTotalHappiness(int numberOfPeople, string fileVariant = null)
        {
            var people = new string[numberOfPeople];
            var happinesses = new int[numberOfPeople, numberOfPeople];
            foreach (var line in LoadInput(fileVariant))
            {
                var split = line.Split(new[] {" would ", " happiness units by sitting next to "},
                    StringSplitOptions.None);
                var person1 = GetPersonIndex(people, split[0]);
                var happiness = (split[1].StartsWith("gain") ? +1 : -1) * int.Parse(split[1].Substring(5));
                var person2 = GetPersonIndex(people, split[2].Trim('.'));

                happinesses[person1, person2] = happiness;
            }

            var maxHappiness = -1;
            foreach (var permutation in new Permutations<int>(Enumerable.Range(0, numberOfPeople).ToList()))
            {
                var happiness = 0;
                for (var i = 0; i < permutation.Count; i++)
                {
                    var person1 = permutation[i > 0 ? i - 1 : permutation.Count - 1];
                    var person2 = permutation[i];
                    var person3 = permutation[i < permutation.Count - 1 ? i + 1 : 0];
                    happiness += happinesses[person2, person1];
                    happiness += happinesses[person2, person3];
                }
                maxHappiness = maxHappiness == -1 ? happiness : Math.Max(maxHappiness, happiness);
            }
            return maxHappiness;
        }

        private int GetPersonIndex(string[] people, string location)
        {
            var index = Array.IndexOf(people, location);
            if (index != -1) return index;
            var nullIndex = Array.IndexOf(people, null);
            people[nullIndex] = location;
            return nullIndex;
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(GetTotalHappiness(9));
        }
    }
}