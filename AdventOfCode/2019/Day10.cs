using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day10 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetMaximumAsteroidDetections(LoadInput("practice.1")).visibleAsteroids);
            Console.WriteLine(GetMaximumAsteroidDetections(LoadInput("practice.2")).visibleAsteroids);
            Console.WriteLine(GetMaximumAsteroidDetections(LoadInput("practice.3")).visibleAsteroids);
            Console.WriteLine(GetMaximumAsteroidDetections(LoadInput("practice.4")).visibleAsteroids);
            Console.WriteLine(GetMaximumAsteroidDetections(LoadInput("practice.5")).visibleAsteroids);
            Console.WriteLine(GetMaximumAsteroidDetections(LoadInput()).visibleAsteroids);
        }

        private static (int visibleAsteroids, Point location) GetMaximumAsteroidDetections(string[] asteroidMap) =>
            GetMaximumAsteroidDetections(ParseAsteroidMap(asteroidMap));

        private static HashSet<Point> ParseAsteroidMap(string[] asteroidMap)
        {
            var asteroids = new HashSet<Point>();
            for (var y = 0; y < asteroidMap.Length; y++)
            for (var x = 0; x < asteroidMap[y].Length; x++)
                if (asteroidMap[y][x] == '#')
                    asteroids.Add(new Point(x, y));
            return asteroids;
        }

        private static (int visibleAsteroids, Point location) GetMaximumAsteroidDetections(HashSet<Point> asteroids)
        {
            var maxVisibleAsteroids = 0;
            var maxLocation = new Point(-1, -1);

            foreach (var asteroid in asteroids)
            {
                var visible = asteroids.Count(otherAsteroid => IsVisible(asteroids, asteroid, otherAsteroid));
                if (visible <= maxVisibleAsteroids) continue;
                maxVisibleAsteroids = visible;
                maxLocation = asteroid;
            }

            return (maxVisibleAsteroids, maxLocation);
        }

        private static bool IsVisible(HashSet<Point> asteroids, Point asteroid, Point otherAsteroid)
        {
            if (asteroid.Equals(otherAsteroid)) return false;

            var diff = otherAsteroid.Subtract(asteroid);

            if (diff.X == 0)
            {
                for (var y = 1; y < Math.Abs(diff.Y); y++)
                    if (asteroids.Contains(asteroid.Add(new Point(0, y * Math.Sign(diff.Y)))))
                        return false;
            }
            else if (diff.Y == 0)
            {
                for (var x = 1; x < Math.Abs(diff.X); x++)
                    if (asteroids.Contains(asteroid.Add(new Point(x * Math.Sign(diff.X), 0))))
                        return false;
            }
            else
            {
                var gcd = GetGreatestCommonDivisor(diff.X, diff.Y);
                var xStep = diff.X / gcd;
                var yStep = diff.Y / gcd;
                for (var i = 1; i < Math.Abs(gcd); i++)
                    if (asteroids.Contains(asteroid.Add(new Point(i * Math.Sign(gcd) * xStep,
                        i * Math.Sign(gcd) * yStep))))
                        return false;
            }

            return true;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetChecksumForNthDestroyedAsteroid(LoadInput("practice.5"), 200));
            Console.WriteLine(GetChecksumForNthDestroyedAsteroid(LoadInput(), 200));
        }

        private static int GetChecksumForNthDestroyedAsteroid(string[] asteroidMap, int n)
        {
            var asteroids = ParseAsteroidMap(asteroidMap);

            var laserLocation = GetMaximumAsteroidDetections(asteroids).location;
            asteroids.Remove(laserLocation);

            var destroyedAsteroids = new Stack<Point>();
            var visibleAsteroids = new Queue<Point>();

            while (destroyedAsteroids.Count < n && (visibleAsteroids.Count > 0 || asteroids.Count > 0))
            {
                if (visibleAsteroids.Count == 0)
                {
                    foreach (var visibleAsteroid in asteroids
                        .Where(a => IsVisible(asteroids, laserLocation, a))
                        .OrderBy(a => -Math.Atan2(a.X - laserLocation.X, a.Y - laserLocation.Y)))
                    {
                        visibleAsteroids.Enqueue(visibleAsteroid);
                        asteroids.Remove(visibleAsteroid);
                    }
                }

                destroyedAsteroids.Push(visibleAsteroids.Dequeue());
            }

            var nthDestroyedAsteroid = destroyedAsteroids.Pop();
            return nthDestroyedAsteroid.X * 100 + nthDestroyedAsteroid.Y;
        }
    }
}