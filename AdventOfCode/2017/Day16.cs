using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2017
{
    internal class Day16 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetFinalPosition(5, new[] {"s1", "x3/4", "pe/b"}));
            Console.WriteLine(GetFinalPosition(16, LoadInput().First().Split(',')));
        }

        private static string GetFinalPosition(int size, string[] steps, int repetitions = 1)
        {
            var positions = new char[size];
            for (var i = 0; i < size; i++)
                positions[i] = (char) (97 + i);

            var seenPositions = new HashSet<string>();
            var history = new Dictionary<int, string>();
            
            var intialPosition = new string(positions);
            seenPositions.Add(intialPosition);
            history[0] = intialPosition;
            
            for (var r = 0; r < repetitions; r++)
            {
                foreach (var step in steps)
                {
                    switch (step[0])
                    {
                        case 's':
                            var spin = int.Parse(step.Substring(1));
                            var currentPositions = new string(positions);
                            for (var i = 0; i < size; i++)
                                positions[Mod(i + spin, size)] = currentPositions[i];
                            break;
                        case 'x':
                        {
                            var split = step.Substring(1).Split('/').Select(int.Parse).ToArray();
                            var a = split[0];
                            var b = split[1];
                            var t = positions[a];
                            positions[a] = positions[b];
                            positions[b] = t;
                            break;
                        }
                        case 'p':
                        {
                            var split = step.Substring(1).Split('/').ToArray();
                            var a = 0;
                            var b = 0;
                            for (var i = 0; i < size; i++)
                            {
                                if (positions[i] == split[0][0])
                                    a = i;
                                if (positions[i] == split[1][0])
                                    b = i;
                            }
                            var t = positions[a];
                            positions[a] = positions[b];
                            positions[b] = t;
                            break;
                        }
                    }
                }
                
                var newPosition = new string(positions);
                if (seenPositions.Contains(newPosition))
                    break;
                seenPositions.Add(newPosition);
                history[r + 1] = newPosition;
            }

            var length = seenPositions.Count;

            return history[repetitions % length];
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetFinalPosition(5, new[] {"s1", "x3/4", "pe/b"}, 1000000000));
            Console.WriteLine(GetFinalPosition(16, LoadInput().First().Split(','), 1000000000));
        }
    }
}