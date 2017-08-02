using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode._2016
{
    internal class Day11 : BaseDay
    {
        private readonly Regex FloorRegex = new Regex(@"^The ([a-z]*) floor");
        private readonly Regex MicrochipRegex = new Regex(@"an? ([a-z]*)-compatible microchip");
        private readonly Regex GeneratorRegex = new Regex(@"an? ([a-z]*) generator");

        public override void RunPartOne()
        {
            Console.WriteLine(GetMinimumSteps("practice"));
            Console.WriteLine(GetMinimumSteps("1"));
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(GetMinimumSteps("2"));
        }

        private int GetMinimumSteps(string fileVariant)
        {
            var initialMap = GenerateMap(fileVariant);

            var queue = new Queue<State>();
            queue.Enqueue(new State(0, initialMap, 0));
            var seen = new HashSet<State>();

            while (queue.Count > 0)
            {
                var currentState = queue.Dequeue();

                for (short dir = -1; dir < 2; dir += 2)
                {
                    if (currentState.Floor == 0 && dir == -1 || currentState.Floor == 3 && dir == 1) continue;

                    for (var item2 = -1; item2 < 16; item2++)
                    {
                        for (var item1 = item2 + 1; item1 < 16; item1++)
                        {
                            var nextState = currentState.Move(dir, item1, item2);
                            if (!nextState.HasValue || !seen.Add(nextState.Value)) continue;

                            if (nextState.Value.IsComplete()) return nextState.Value.Steps;

                            queue.Enqueue(nextState.Value);
                        }
                    }
                }
            }
            return -1;
        }

        private ulong GenerateMap(string fileVariant)
        {
            var elements = new List<string>();
            var map = 0UL;
            foreach (var line in LoadInput(fileVariant))
            {
                var floorMatch = FloorRegex.Match(line);
                if (!floorMatch.Success) continue;

                var floor = GetFloor(floorMatch.Groups[1].Value);

                var chipMatch = MicrochipRegex.Match(line);
                while (chipMatch.Success)
                {
                    var element = chipMatch.Groups[1].Value;
                    map |= 1ul << ((floor - 1) * 16 + GetIndex(elements, element));
                    chipMatch = chipMatch.NextMatch();
                }

                var genMatch = GeneratorRegex.Match(line);
                while (genMatch.Success)
                {
                    var element = genMatch.Groups[1].Value;
                    map |= 1ul << ((floor - 1) * 16 + GetIndex(elements, element) + 8);
                    genMatch = genMatch.NextMatch();
                }
            }
            return map;
        }

        private int GetIndex(List<string> elements, string element)
        {
            var index = elements.IndexOf(element);
            if (index >= 0) return index;

            index = elements.Count;
            elements.Add(element);
            return index;

        }

        private int GetFloor(string str)
        {
            switch (str)
            {
                case "first":
                    return 1;
                case "second":
                    return 2;
                case "third":
                    return 3;
                case "fourth":
                    return 4;
                default:
                    return 0;
            }
        }

        private struct State
        {
            public State(short floor, ulong map, short steps)
            {
                Floor = floor;
                Map = map;
                Steps = steps;
            }

            public readonly short Floor;
            public readonly ulong Map;
            public readonly short Steps;

            public bool IsComplete()
            {
                return (Map & 0xFFFFFFFFFFFF) == 0;
            }

            public override bool Equals(object obj)
            {
                if (obj?.GetType() != GetType()) return false;

                var s = (State) obj;
                return Floor == s.Floor && Map == s.Map;
            }

            public override int GetHashCode()
            {
                return unchecked((Floor * 397) ^ Map.GetHashCode());
            }

            public State? Move(short dir, int item1, int item2)
            {
                if (item2 >= 0 && AreMatchingItems(item1, item2)) return null;

                var oldBit1 = 1ul << (Floor * 16 + item1);
                var oldBit2 = item2 < 0 ? 0 : 1ul << (Floor * 16 + item2);

                if ((Map & oldBit1) == 0 || item2 >= 0 && (Map & oldBit2) == 0) return null;

                var nextFloor = (short) (Floor + dir);
                var nextMap = Map;

                nextMap &= ~oldBit1;
                if (item2 >= 0)
                    nextMap &= ~oldBit2;

                var newBit1 = 1ul << (nextFloor * 16 + item1);
                var newBit2 = item2 < 0 ? 0 : 1ul << (nextFloor * 16 + item2);

                nextMap |= newBit1;
                if (item2 >= 0)
                    nextMap |= newBit2;


                for (var i = 0; i < 4; i++)
                {
                    var microchips = (int) ((nextMap >> (i * 16)) & 0xFF);
                    var generators = (int) ((nextMap >> (i * 16 + 8)) & 0xFF);

                    if ((microchips & ~generators) != 0 && generators != 0) return null;
                }

                return new State(nextFloor, nextMap, (short) (Steps + 1));
            }

            private bool AreMatchingItems(int item1, int item2)
            {
                return item1 >= 8 && item2 < 8 && item2 != item1 - 8 && item2 >= 8 && item1 < 8 && item1 != item2 - 8;
            }
        }
    }
}