using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018
{
    internal class Day10 : BaseDay
    {
        protected override void RunPartOne()
        {
            PrintMessage(LoadInput("practice"));
            PrintMessage(LoadInput());
        }

        private static void PrintMessage(IEnumerable<string> initialPositions)
        {
            var lights = ParseLights(initialPositions);
            var boundingBoxArea = long.MaxValue;
            while (true)
            {

                var nextLights = new List<Light>();
                foreach (var light in lights)
                    nextLights.Add(new Light {Position= light.Position.Add(light.Velocity), Velocity = light.Velocity});
                
                var nextBoundingBoxArea = GetBoundingBoxArea(nextLights);

                if (nextBoundingBoxArea > boundingBoxArea)
                    break;
                
                lights = nextLights;
                boundingBoxArea = nextBoundingBoxArea;
            }
            Print(lights);
        }

        private static List<Light> ParseLights(IEnumerable<string> initialPositions)
        {
            var lights = new List<Light>();
            foreach (var initialPosition in initialPositions)
            {
                var split = initialPosition.Split(new[] {"position=<", ",", "> velocity=<", ">"},
                    StringSplitOptions.RemoveEmptyEntries);
                var xPos = int.Parse(split[0]);
                var yPos = int.Parse(split[1]);
                var xVel = int.Parse(split[2]);
                var yVel = int.Parse(split[3]);
                lights.Add(new Light {Position = new Point(xPos, yPos), Velocity = new Point(xVel, yVel)});
            }

            return lights;
        }

        private static long GetBoundingBoxArea(IEnumerable<Light> lights)
        {
            var (xMin, xMax, yMin, yMax) = GetLimitingCoordinates(lights);
            return (long) (xMax - xMin) * (yMax - yMin);
        }

        private static (int xMin, int xMax, int yMin, int yMax) GetLimitingCoordinates(IEnumerable<Light> lights)
        {
            var xMin = int.MaxValue;
            var xMax = int.MinValue;
            var yMin = int.MaxValue;
            var yMax = int.MinValue;
            foreach (var light in lights)
            {
                if (light.Position.X < xMin)
                    xMin = light.Position.X;
                if (light.Position.X > xMax)
                    xMax = light.Position.X;
                if (light.Position.Y < yMin)
                    yMin = light.Position.Y;
                if (light.Position.Y > yMax)
                    yMax = light.Position.Y;
            }

            return (xMin, xMax, yMin, yMax);
        }
        
        private static void Print(IReadOnlyCollection<Light> lights)
        {
            var (xMin, xMax, yMin, yMax) = GetLimitingCoordinates(lights);
            var lightSet = new HashSet<Point>(lights.Select(l => l.Position));
            for (var y = yMin; y <= yMax; y++)
            {
                for (var x = xMin; x <= xMax; x++)
                    Console.Write(lightSet.Contains(new Point(x, y)) ? "#" : ".");
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetMessageTime(LoadInput("practice")));
            Console.WriteLine(GetMessageTime(LoadInput()));
        }
        
        private static int GetMessageTime(IEnumerable<string> initialPositions)
        {
            var lights = ParseLights(initialPositions);
            var boundingBoxArea = long.MaxValue;
            var time = 0;
            while (true)
            {

                var nextLights = new List<Light>();
                foreach (var light in lights)
                    nextLights.Add(new Light {Position= light.Position.Add(light.Velocity), Velocity = light.Velocity});
                
                var nextBoundingBoxArea = GetBoundingBoxArea(nextLights);

                if (nextBoundingBoxArea > boundingBoxArea)
                    break;
                
                lights = nextLights;
                boundingBoxArea = nextBoundingBoxArea;
                time++;
            }

            return time;
        }

        private class Light
        {
            public Point Position;
            public Point Velocity;
        }
    }
}