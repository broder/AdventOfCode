using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2017
{
    internal class Day20 : BaseDay
    {
        private const float LongTime = 1000;
        
        protected override void RunPartOne()
        {
            Console.WriteLine(GetClosestParticle(LoadInput("practice.1")));
            Console.WriteLine(GetClosestParticle(LoadInput()));
        }

        private static int GetClosestParticle(IReadOnlyList<string> input)
        {
            var min = double.MaxValue;
            var minIndex = 0;

            for (var i = 0; i < input.Count; i++)
            {
                var posVelAcc = input[i].Split(new[] {"p=<", ">, v=<", ">, a=<", ">"},
                    StringSplitOptions.RemoveEmptyEntries);
                
                var pos = posVelAcc[0].Split(',').Select(double.Parse).ToArray();
                var vel = posVelAcc[1].Split(',').Select(double.Parse).ToArray();
                var acc = posVelAcc[2].Split(',').Select(double.Parse).ToArray();

                var xPos = pos[0] + vel[0] * LongTime + acc[0] * LongTime * LongTime / 2.0;
                var yPos = pos[1] + vel[1] * LongTime + acc[1] * LongTime * LongTime / 2.0;
                var zPos = pos[2] + vel[2] * LongTime + acc[2] * LongTime * LongTime / 2.0;

                var dist = Math.Abs(xPos) + Math.Abs(yPos) + Math.Abs(zPos);
                if (dist >= min) continue;

                min = dist;
                minIndex = i;
            }

            return minIndex;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetRemainingParticles(LoadInput("practice.2")));
            Console.WriteLine(GetRemainingParticles(LoadInput()));
        }

        private static int GetRemainingParticles(IEnumerable<string> input)
        {
            var particles = new List<Particle>();
            foreach (var line in input)
            {
                var posVelAcc = line.Split(new[] {"p=<", ">, v=<", ">, a=<", ">"},
                    StringSplitOptions.RemoveEmptyEntries);
                
                var pos = posVelAcc[0].Split(',').Select(double.Parse).ToArray();
                var vel = posVelAcc[1].Split(',').Select(double.Parse).ToArray();
                var acc = posVelAcc[2].Split(',').Select(double.Parse).ToArray();
                
                particles.Add(new Particle
                {
                    Position = pos,
                    Velocity = vel,
                    Acceleration = acc,
                });
            }

            for (var t = 0; t < LongTime; t++)
            {
                var seenPositions = new Dictionary<string, int>();
                for (var i = 0; i < particles.Count; i++)
                {
                    var particle = particles[i];
                    if (particle.Dead) continue;

                    particle.Velocity[0] += particle.Acceleration[0];
                    particle.Velocity[1] += particle.Acceleration[1];
                    particle.Velocity[2] += particle.Acceleration[2];
                    
                    particle.Position[0] += particle.Velocity[0];
                    particle.Position[1] += particle.Velocity[1];
                    particle.Position[2] += particle.Velocity[2];

                    var posKey = string.Join(";", particle.Position);
                    if (seenPositions.ContainsKey(posKey))
                    {
                        particles[seenPositions[posKey]].Dead = true;
                        particle.Dead = true;
                    }
                    else
                        seenPositions[posKey] = i;
                }
            }

            return particles.Count(p => !p.Dead);
        }

        private class Particle
        {
            public double[] Position;
            public double[] Velocity;
            public double[] Acceleration;
            public bool Dead;
        }
    }
}