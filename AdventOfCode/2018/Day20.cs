using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018
{
    internal class Day20 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetFurthestRoom("^WNE$"));
            Console.WriteLine(GetFurthestRoom("^ENWWW(NEEE|SSE(EE|N))$"));
            Console.WriteLine(GetFurthestRoom("^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|)NNN$"));
            Console.WriteLine(GetFurthestRoom("^ESSWWN(E|NNENN(EESS(WNSE|)SSS|WWWSSSSE(SW|NNNE)))$"));
            Console.WriteLine(GetFurthestRoom("^WSSEESWWWNW(S|NENNEEEENN(ESSSSW(NWSW|SSEN)|WSWWN(E|WWS(E|SS))))$"));
            Console.WriteLine(GetFurthestRoom(LoadInput().First()));
        }

        private static int GetFurthestRoom(string directions) => GenerateRooms(directions).Values.Max();

        private static Dictionary<Point, int> GenerateRooms(string directions)
        {
            var rooms = new Dictionary<Point, int>();
            var currentLocation = new Point(0, 0);
            rooms[currentLocation] = 0;

            var locations = new Stack<Point>();
            foreach (var c in directions)
            {
                var previousDistance = rooms[currentLocation];
                switch (c)
                {
                    case 'N':
                        currentLocation = currentLocation.Add(new Point(0, -1));
                        break;
                    case 'E':
                        currentLocation = currentLocation.Add(new Point(1, 0));
                        break;
                    case 'S':
                        currentLocation = currentLocation.Add(new Point(0, 1));
                        break;
                    case 'W':
                        currentLocation = currentLocation.Add(new Point(-1, 0));
                        break;
                    case '(':
                        locations.Push(currentLocation);
                        break;
                    case '|':
                        currentLocation = locations.Peek();
                        break;
                    case ')':
                        currentLocation = locations.Pop();
                        break;
                }

                if (!rooms.ContainsKey(currentLocation))
                    rooms[currentLocation] = previousDistance + 1;
            }

            return rooms;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetDistantRooms(LoadInput().First()));
        }

        private static int GetDistantRooms(string directions) => GenerateRooms(directions).Values.Count(d => d >= 1000);
    }
}