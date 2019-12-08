using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day06 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(FindTotalOrbits(LoadInput("practice.1")));
            Console.WriteLine(FindTotalOrbits(LoadInput()));
        }

        private static int FindTotalOrbits(IEnumerable<string> orbitDefinitions) =>
            ConstructGraph(orbitDefinitions).Values.Sum(o => o.GetTotalOrbiters());

        private static Dictionary<string, OrbitObject> ConstructGraph(IEnumerable<string> orbitDefinitions)
        {
            var orbitalObjects = new Dictionary<string, OrbitObject>();
            foreach (var definition in orbitDefinitions)
            {
                var split = definition.Split(")");

                var orbiteeKey = split[0];
                if (!orbitalObjects.ContainsKey(orbiteeKey))
                    orbitalObjects[orbiteeKey] = new OrbitObject(orbiteeKey);
                var orbitee = orbitalObjects[orbiteeKey];

                var orbiterKey = split[1];
                if (!orbitalObjects.ContainsKey(orbiterKey))
                    orbitalObjects[orbiterKey] = new OrbitObject(orbiterKey);
                var orbiter = orbitalObjects[orbiterKey];

                orbitee.orbiters.Add(orbiter);
                orbiter.orbitee = orbitee;
            }

            return orbitalObjects;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(FindMinimumTransfers(LoadInput("practice.2")));
            Console.WriteLine(FindMinimumTransfers(LoadInput()));
        }

        private static int FindMinimumTransfers(IEnumerable<string> orbitDefinitions)
        {
            var orbitalObjects = ConstructGraph(orbitDefinitions);

            var youPath = GetPathToRoot(orbitalObjects, "YOU");
            var santaPath = GetPathToRoot(orbitalObjects, "SAN");

            var union = youPath.Union(santaPath);
            var intersection = youPath.Intersect(santaPath).ToHashSet();

            return union.Count(k => !intersection.Contains(k)) - 2;
        }

        private static HashSet<string> GetPathToRoot(IReadOnlyDictionary<string, OrbitObject> orbitalObjects,
            string startKey)
        {
            var path = new HashSet<string>();
            var currentNode = orbitalObjects[startKey];
            while (currentNode != null)
            {
                path.Add(currentNode.reference);
                currentNode = currentNode.orbitee;
            }

            return path;
        }

        private class OrbitObject
        {
            public readonly string reference;
            public OrbitObject orbitee;
            public HashSet<OrbitObject> orbiters = new HashSet<OrbitObject>();

            private int? totalOrbiters;

            public OrbitObject(string reference)
            {
                this.reference = reference;
            }

            public int GetTotalOrbiters()
            {
                if (!totalOrbiters.HasValue)
                    totalOrbiters = orbiters.Count + orbiters.Sum(c => c.GetTotalOrbiters());

                return totalOrbiters.Value;
            }
        }
    }
}