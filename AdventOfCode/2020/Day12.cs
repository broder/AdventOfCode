using System;

namespace AdventOfCode._2020
{
    internal class Day12 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetFinalShipDistance(LoadInput("practice")));
            Console.WriteLine(GetFinalShipDistance(LoadInput()));
        }

        private static int GetFinalShipDistance(string[] instructions)
        {
            var p = new Point(0, 0);
            var d = new Point(1, 0);
            foreach (var ins in instructions)
            {
                if (ins[0] == 'N')
                    p = p.Add(new Point(0, -int.Parse(ins.Substring(1))));
                else if (ins[0] == 'S')
                    p = p.Add(new Point(0, int.Parse(ins.Substring(1))));
                else if (ins[0] == 'E')
                    p = p.Add(new Point(int.Parse(ins.Substring(1)), 0));
                else if (ins[0] == 'W')
                    p = p.Add(new Point(-int.Parse(ins.Substring(1)), 0));
                else if (ins[0] == 'L' || ins[0] == 'R')
                {
                    var deg = int.Parse(ins.Substring(1)) % 360;
                    if (ins[0] == 'L') deg = -deg;
                    if (deg == 90 || deg == -270)
                        d = new Point(-d.Y, d.X);
                    else if (deg == 180 || deg == -180)
                        d = new Point(-d.X, -d.Y);
                    else if (deg == 270 || deg == -90)
                        d = new Point(d.Y, -d.X);
                }
                else if (ins[0] == 'F')
                {
                    var dist = int.Parse(ins.Substring(1));
                    p = p.Add(new Point(d.X * dist, d.Y * dist));
                }
            }
            return Math.Abs(p.X) + Math.Abs(p.Y);
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetFinalWaypointDistance(LoadInput("practice")));
            Console.WriteLine(GetFinalWaypointDistance(LoadInput()));
        }

        private static int GetFinalWaypointDistance(string[] instructions)
        {
            var ship = new Point(0, 0);
            var waypoint = new Point(10, -1);
            foreach (var ins in instructions)
            {
                if (ins[0] == 'N')
                    waypoint = waypoint.Add(new Point(0, -int.Parse(ins.Substring(1))));
                else if (ins[0] == 'S')
                    waypoint = waypoint.Add(new Point(0, int.Parse(ins.Substring(1))));
                else if (ins[0] == 'E')
                    waypoint = waypoint.Add(new Point(int.Parse(ins.Substring(1)), 0));
                else if (ins[0] == 'W')
                    waypoint = waypoint.Add(new Point(-int.Parse(ins.Substring(1)), 0));
                else if (ins[0] == 'L' || ins[0] == 'R')
                {
                    var d = int.Parse(ins.Substring(1)) % 360;
                    if (ins[0] == 'L') d = -d;
                    if (d == 90 || d == -270)
                        waypoint = new Point(-waypoint.Y, waypoint.X);
                    else if (d == 180 || d == -180)
                        waypoint = new Point(-waypoint.X, -waypoint.Y);
                    else if (d == 270 || d == -90)
                        waypoint = new Point(waypoint.Y, -waypoint.X);
                }
                else if (ins[0] == 'F')
                {
                    var d = int.Parse(ins.Substring(1));
                    ship = ship.Add(new Point(waypoint.X * d, waypoint.Y * d));
                }
            }
            return Math.Abs(ship.X) + Math.Abs(ship.Y);
        }
    }
}