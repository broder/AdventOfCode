using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2017
{
    internal class Day18 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(RunSimulation(LoadInput("practice")));
            Console.WriteLine(RunSimulation(LoadInput()));
        }

        private static long RunSimulation(IReadOnlyList<string> steps)
        {
            var registers = new Dictionary<string, long>();
            long lastSound = 0;

            for (var i = 0; i < steps.Count; i++)
            {
                var step = steps[i];

                var command = step.Substring(0, 3);
                var parameters = step.Substring(4);

                switch (command)
                {
                    case "snd":
                        lastSound = GetValue(registers, parameters);
                        break;
                    case "rcv":
                        if (GetValue(registers, parameters) != 0)
                            return lastSound;
                        break;
                    case "set":
                    case "add":
                    case "mul":
                    case "mod":
                        var split = parameters.Split(' ').ToArray();
                        var x = split[0];
                        var y = GetValue(registers, split[1]);

                        if (!registers.ContainsKey(x))
                            registers[x] = 0;

                        if (command == "set")
                            registers[x] = y;
                        else if (command == "add")
                            registers[x] += y;
                        else if (command == "mul")
                            registers[x] *= y;
                        else if (command == "mod")
                            registers[x] = Mod(registers[x], y);
                        break;
                    case "jgz":
                        var jgzSplit = parameters.Split(' ').ToArray();
                        var jgzX = GetValue(registers, jgzSplit[0]);
                        var jgzY = GetValue(registers, jgzSplit[1]);
                        if (jgzX > 0)
                            i += (int) jgzY - 1;
                        break;
                }
            }
            return -1;
        }

        private static long GetValue(IDictionary<string, long> registers, string input)
        {
            if (long.TryParse(input, out var output))
                return output;

            if (!registers.ContainsKey(input))
                registers[input] = 0;

            return registers[input];
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(RunMultipleSimulations(LoadInput()));
        }

        private static int RunMultipleSimulations(string[] input)
        {
            var p0 = new Simulation(input, 0);
            var p1 = new Simulation(input, 1);
            var p1Values = new Queue<long>();

            var sendCount = 0;

            while (true)
            {
                var p0Values = p0.Run(p1Values);
                p1Values = p1.Run(p0Values);
                sendCount += p1Values.Count;

                if (p0Values.Count == 0 && p1Values.Count == 0)
                    return sendCount;
            }
        }

        private class Simulation
        {
            private readonly string[] steps;
            private int location = 0;
            private readonly Dictionary<string, long> registers = new Dictionary<string, long>();

            public Simulation(string[] input, int id)
            {
                steps = input;
                registers["p"] = id;
            }

            public Queue<long> Run(Queue<long> receivedValues)
            {
                var sentValues = new Queue<long>();

                while (true)
                {
                    if (location < 0 || location >= steps.Length)
                        return sentValues;
                    
                    var step = steps[location];

                    var command = step.Substring(0, 3);
                    var parameters = step.Substring(4);

                    switch (command)
                    {
                        case "snd":
                            sentValues.Enqueue(GetValue(registers, parameters));
                            break;
                        case "rcv":
                            var rcvX = parameters;
                            if (!receivedValues.Any())
                                return sentValues;
                            registers[rcvX] = receivedValues.Dequeue();
                            break;
                        case "set":
                        case "add":
                        case "mul":
                        case "mod":
                            var split = parameters.Split(' ').ToArray();
                            var x = split[0];
                            var y = GetValue(registers, split[1]);

                            if (!registers.ContainsKey(x))
                                registers[x] = 0;

                            if (command == "set")
                                registers[x] = y;
                            else if (command == "add")
                                registers[x] += y;
                            else if (command == "mul")
                                registers[x] *= y;
                            else if (command == "mod")
                                registers[x] = Mod(registers[x], y);
                            break;
                        case "jgz":
                            var jgzSplit = parameters.Split(' ').ToArray();
                            var jgzX = GetValue(registers, jgzSplit[0]);
                            var jgzY = GetValue(registers, jgzSplit[1]);
                            if (jgzX > 0)
                                location += (int) jgzY - 1;
                            break;
                    }
                    
                    location++;
                }
            }
        }
    }
}