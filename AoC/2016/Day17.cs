using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2016
{
    internal class Day17 : BaseDay
    {
        private static readonly char[] DoorCharacters = {'b', 'c', 'd', 'e', 'f'};

        public override void RunPartOne()
        {
            Console.WriteLine(GetShortestPath("ihgpwlah"));
            Console.WriteLine(GetShortestPath("kglvqrro"));
            Console.WriteLine(GetShortestPath("ulqzkmiv"));
            Console.WriteLine(GetShortestPath("yjjvjgan"));
        }

        private string GetShortestPath(string passcode)
        {
            var states = new Queue<State>();
            states.Enqueue(new State {Path = passcode, Xposition = 0, Yposition = 0});
            while (states.Count > 0)
            {
                var currentState = states.Dequeue();
                if (currentState.Xposition == 3 && currentState.Yposition == 3)
                    return currentState.Path.Substring(passcode.Length);

                AddNextStates(currentState, states);
            }
            return "";
        }

        private void AddNextStates(State currentState, Queue<State> states)
        {
            var hash = CalculateMd5Hash(currentState.Path);

            if (DoorCharacters.Contains(hash[0]) && currentState.Yposition - 1 >= 0)
            {
                states.Enqueue(new State
                {
                    Path = currentState.Path + 'U',
                    Xposition = currentState.Xposition,
                    Yposition = currentState.Yposition - 1
                });
            }

            if (DoorCharacters.Contains(hash[1]) && currentState.Yposition + 1 < 4)
            {
                states.Enqueue(new State
                {
                    Path = currentState.Path + 'D',
                    Xposition = currentState.Xposition,
                    Yposition = currentState.Yposition + 1
                });
            }

            if (DoorCharacters.Contains(hash[2]) && currentState.Xposition - 1 >= 0)
            {
                states.Enqueue(new State
                {
                    Path = currentState.Path + 'L',
                    Xposition = currentState.Xposition - 1,
                    Yposition = currentState.Yposition
                });
            }

            if (DoorCharacters.Contains(hash[3]) && currentState.Xposition + 1 < 4)
            {
                states.Enqueue(new State
                {
                    Path = currentState.Path + 'R',
                    Xposition = currentState.Xposition + 1,
                    Yposition = currentState.Yposition
                });
            }
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(GetLongestPathLength("ihgpwlah"));
            Console.WriteLine(GetLongestPathLength("kglvqrro"));
            Console.WriteLine(GetLongestPathLength("ulqzkmiv"));
            Console.WriteLine(GetLongestPathLength("yjjvjgan"));
        }

        private int GetLongestPathLength(string passcode)
        {
            var states = new Queue<State>();
            states.Enqueue(new State {Path = passcode, Xposition = 0, Yposition = 0});
            var max = 0;
            while (states.Count > 0)
            {
                var currentState = states.Dequeue();
                if (currentState.Xposition == 3 && currentState.Yposition == 3)
                    max = Math.Max(max, currentState.Path.Length);
                else
                    AddNextStates(currentState, states);
            }
            return max - passcode.Length;
        }

        private struct State
        {
            public string Path;
            public int Xposition;
            public int Yposition;
        }
    }
}