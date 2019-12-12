using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day12 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetTotalEnergy(LoadInput("practice.1"), 10));
            Console.WriteLine(GetTotalEnergy(LoadInput("practice.2"), 100));
            Console.WriteLine(GetTotalEnergy(LoadInput(), 1000));
        }

        private static int GetTotalEnergy(string[] scan, int ticks)
        {
            var moons = ParseMoons(scan);

            for (var t = 1; t <= ticks; t++)
            {
                for (var i = 0; i < moons.Length; i++)
                {
                    SimulateGravity(i, moons);
                    moons[i].Position = moons[i].Position.Add(moons[i].Velocity);
                }
            }

            return moons.Sum(m => m.GetTotalEnergy());
        }

        private static Moon[] ParseMoons(string[] scan) =>
            scan.Select(moon => moon
                    .Split(new[] {"<x=", ", y=", ", z=", ">"}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray())
                .Select(split => new Moon(split[0], split[1], split[2]))
                .ToArray();

        private static void SimulateGravity(int i, Moon[] moons)
        {
            for (var j = i + 1; j < moons.Length; j++)
            {
                if (moons[i].Position.X < moons[j].Position.X)
                {
                    moons[i].Velocity = moons[i].Velocity.Add(new Point(1, 0, 0));
                    moons[j].Velocity = moons[j].Velocity.Add(new Point(-1, 0, 0));
                }
                else if (moons[i].Position.X > moons[j].Position.X)
                {
                    moons[i].Velocity = moons[i].Velocity.Add(new Point(-1, 0, 0));
                    moons[j].Velocity = moons[j].Velocity.Add(new Point(1, 0, 0));
                }

                if (moons[i].Position.Y < moons[j].Position.Y)
                {
                    moons[i].Velocity = moons[i].Velocity.Add(new Point(0, 1, 0));
                    moons[j].Velocity = moons[j].Velocity.Add(new Point(0, -1, 0));
                }
                else if (moons[i].Position.Y > moons[j].Position.Y)
                {
                    moons[i].Velocity = moons[i].Velocity.Add(new Point(0, -1, 0));
                    moons[j].Velocity = moons[j].Velocity.Add(new Point(0, 1, 0));
                }

                if (moons[i].Position.Z < moons[j].Position.Z)
                {
                    moons[i].Velocity = moons[i].Velocity.Add(new Point(0, 0, 1));
                    moons[j].Velocity = moons[j].Velocity.Add(new Point(0, 0, -1));
                }
                else if (moons[i].Position.Z > moons[j].Position.Z)
                {
                    moons[i].Velocity = moons[i].Velocity.Add(new Point(0, 0, -1));
                    moons[j].Velocity = moons[j].Velocity.Add(new Point(0, 0, 1));
                }
            }
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetPeriodicity(LoadInput("practice.1")));
            Console.WriteLine(GetPeriodicity(LoadInput("practice.2")));
            Console.WriteLine(GetPeriodicity(LoadInput()));
        }
        
        private static long GetPeriodicity(string[] scan)
        {
            var moons = ParseMoons(scan);
            var coordinatePeriods = new long[3];

            for (var t = 1; coordinatePeriods.Any(p => p == 0); t++)
            {
                for (var i = 0; i < moons.Length; i++)
                {
                    SimulateGravity(i, moons);
                    moons[i].Position = moons[i].Position.Add(moons[i].Velocity);

                }

                if (coordinatePeriods[0] == 0 && moons.Select(m => m.Velocity.X).All(x => x == 0))
                    coordinatePeriods[0] = t;
                
                if (coordinatePeriods[1] == 0 && moons.Select(m => m.Velocity.Y).All(z => z == 0))
                    coordinatePeriods[1] = t;
                
                if (coordinatePeriods[2] == 0 && moons.Select(m => m.Velocity.Z).All(z => z == 0))
                    coordinatePeriods[2] = t;
            }

            return coordinatePeriods.Aggregate(GetLowestCommonMultiple) * 2;
        }

        private class Moon
        {
            public Point Position;
            public Point Velocity;

            public Moon(int x, int y, int z)
            {
                Position = new Point(x, y, z);
                Velocity = new Point(0, 0, 0);
            }

            public int GetPotentialEnergy() => Math.Abs(Position.X) + Math.Abs(Position.Y) + Math.Abs(Position.Z);

            public int GetKineticEnergy() => Math.Abs(Velocity.X) + Math.Abs(Velocity.Y) + Math.Abs(Velocity.Z);

            public int GetTotalEnergy() => GetPotentialEnergy() * GetKineticEnergy();
        }
    }
}