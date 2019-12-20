using System;
using System.Collections.Generic;
using Priority_Queue;

namespace AdventOfCode._2019
{
    internal class Day18 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetShortestPathThroughAllKeys(LoadInput("practice.1")));
            Console.WriteLine(GetShortestPathThroughAllKeys(LoadInput("practice.2")));
            Console.WriteLine(GetShortestPathThroughAllKeys(LoadInput("practice.3")));
            Console.WriteLine(GetShortestPathThroughAllKeys(LoadInput("practice.4")));
            Console.WriteLine(GetShortestPathThroughAllKeys(LoadInput("1")));
        }

        private static int GetShortestPathThroughAllKeys(string[] map, string startingPositions = "@")
        {
            var keys = new Dictionary<char, Point>();
            for (var y = 0; y < map.Length; y++)
            for (var x = 0; x < map[y].Length; x++)
                if (Alphabet.Contains(map[y][x]) || startingPositions.Contains(map[y][x]))
                    keys[map[y][x]] = new Point(x, y);

            var graph = new Dictionary<char, Dictionary<char, Tuple<int, int>>>();
            foreach (var (startKey, startPosition) in keys)
                graph[startKey] = GetKeyGraph(map, startPosition);

            return GetShortestPathThroughAllKeys(graph, startingPositions);
        }

        private static Dictionary<char, Tuple<int, int>> GetKeyGraph(string[] map, Point start)
        {
            var reachableKeys = new Dictionary<char, Tuple<int, int>>();

            var q = new Queue<Tuple<Point, int, int>>();
            q.Enqueue(new Tuple<Point, int, int>(start, 0, 0));
            var seen = new HashSet<Point>();
            while (q.Count > 0)
            {
                var (currentPosition, distance, doors) = q.Dequeue();
                seen.Add(currentPosition);

                var currentChar = map[currentPosition.Y][currentPosition.X];

                if (!currentPosition.Equals(start) && Alphabet.Contains(currentChar))
                    reachableKeys[currentChar] = new Tuple<int, int>(distance, doors);

                if (Alphabet.ToUpper().Contains(currentChar))
                    doors |= 1 << (currentChar - 'A');

                foreach (var d in Point.ManhattanDirections)
                {
                    var nextPosition = currentPosition.Add(d);

                    if (seen.Contains(nextPosition)) continue;
                    if (map[nextPosition.Y][nextPosition.X] == '#') continue;

                    q.Enqueue(new Tuple<Point, int, int>(nextPosition, distance + 1, doors));
                }
            }

            return reachableKeys;
        }

        private static int GetShortestPathThroughAllKeys(Dictionary<char, Dictionary<char, Tuple<int, int>>> graph,
            string startingPositions)
        {
            var q = new SimplePriorityQueue<Tuple<string, int, int>>();
            q.Enqueue(new Tuple<string, int, int>(startingPositions, 0, 0), 0);
            var seen = new Dictionary<Tuple<string, int>, int>();
            while (q.Count > 0)
            {
                var (currentPositions, currentDistance, currentKeys) = q.Dequeue();

                if (CountBits(currentKeys) == graph.Keys.Count - currentPositions.Length) return currentDistance;

                var seenKey = new Tuple<string, int>(currentPositions, currentKeys);
                if (seen.ContainsKey(seenKey) && seen[seenKey] <= currentDistance) continue;
                seen[seenKey] = currentDistance;

                for (var i = 0; i < currentPositions.Length; i++)
                {
                    foreach (var (nextKey, (nextKeyDistance, nextKeyDoors)) in graph[currentPositions[i]])
                    {
                        if ((1 << (nextKey - 'a') & currentKeys) != 0) continue; // we already have this key

                        if ((nextKeyDoors & currentKeys) != nextKeyDoors) continue; // we don't have the right keys

                        var nextKeys = currentKeys | (1 << (nextKey - 'a'));

                        q.Enqueue(
                            new Tuple<string, int, int>(currentPositions.Remove(i, 1).Insert(i, nextKey.ToString()),
                                currentDistance + nextKeyDistance, nextKeys),
                            currentDistance + nextKeyDistance);
                    }
                }
            }

            throw new ArgumentException();
        }

        private static int CountBits(int value)
        {
            var count = 0;
            while (value != 0)
            {
                count++;
                value &= value - 1;
            }

            return count;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetShortestPathThroughAllKeys(LoadInput("practice.5"), "1234"));
            Console.WriteLine(GetShortestPathThroughAllKeys(LoadInput("practice.6"), "1234"));
            Console.WriteLine(GetShortestPathThroughAllKeys(LoadInput("practice.7"), "1234"));
            Console.WriteLine(GetShortestPathThroughAllKeys(LoadInput("practice.8"), "1234"));
            Console.WriteLine(GetShortestPathThroughAllKeys(LoadInput("2"), "1234"));
        }
    }
}