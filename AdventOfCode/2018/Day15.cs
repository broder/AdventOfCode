using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018
{
    internal class Day15 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetResult(LoadInput("practice.1")).outcome);
            Console.WriteLine(GetResult(LoadInput("practice.2")).outcome);
            Console.WriteLine(GetResult(LoadInput("practice.3")).outcome);
            Console.WriteLine(GetResult(LoadInput("practice.4")).outcome);
            Console.WriteLine(GetResult(LoadInput("practice.5")).outcome);
            Console.WriteLine(GetResult(LoadInput("practice.6")).outcome);
            Console.WriteLine(GetResult(LoadInput()).outcome);
        }

        private static (bool elvesWin, int outcome) GetResult(IReadOnlyList<string> input, bool noElfLosses = false,
            int elfAttackPower = 3)
        {
            var walls = new HashSet<Point>();
            var goblins = new Dictionary<Point, Creature>();
            var elves = new Dictionary<Point, Creature>();

            var height = input.Count;
            var width = input[0].Length;
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
            {
                var p = new Point(x, y);
                switch (input[y][x])
                {
                    case '.':
                        break;
                    case '#':
                        walls.Add(p);
                        break;
                    case 'G':
                        goblins[p] = new Goblin {Position = p};
                        break;
                    default: // 'E'
                        elves[p] = new Elf {Position = p, AttackPower = elfAttackPower};
                        break;
                }
            }

            for (var time = 0;; time++)
            {
                var creatures = goblins.Values.Concat(elves.Values).OrderBy(c => c.Position.Y).ThenBy(c => c.Position.X)
                    .ToList();
                for (var i = 0; i < creatures.Count; i++)
                {
                    var creature = creatures[i];
                    var isElf = creature is Elf;

                    var allies = isElf ? elves : goblins;
                    var enemies = isElf ? goblins : elves;

                    if (!enemies.Any()) return (isElf, time * allies.Values.Sum(c => c.HealthPoints));

                    if (!Point.ManhattanDirections.Select(d => creature.Position.Add(d))
                        .Any(p => enemies.ContainsKey(p)))
                        Move(creature, allies, enemies, walls);

                    var target = Point.ManhattanDirections
                        .Select(d => creature.Position.Add(d))
                        .Select(p => enemies.ContainsKey(p) ? enemies[p] : null)
                        .Where(e => e != null)
                        .OrderBy(e => e.HealthPoints)
                        .ThenBy(e => e.Position.Y)
                        .ThenBy(e => e.Position.X)
                        .FirstOrDefault();

                    if (target == null) continue;

                    // attack
                    target.HealthPoints -= creature.AttackPower;

                    if (target.HealthPoints > 0) continue;

                    // die
                    var removeIndex = creatures.IndexOf(target);
                    creatures.RemoveAt(removeIndex);
                    if (i > removeIndex) i--;

                    if (isElf)
                    {
                        goblins.Remove(target.Position);
                    }
                    else
                    {
                        if (noElfLosses) return (false, -1);
                        elves.Remove(target.Position);
                    }
                }
            }
        }

        private static void Move(Creature creature, IDictionary<Point, Creature> allies,
            IReadOnlyDictionary<Point, Creature> enemies, ICollection<Point> walls)
        {
            var queue = new Queue<Tuple<Point, Point>>();
            var seen = new HashSet<Point> {creature.Position};

            foreach (var p in Point.ManhattanDirections.Select(d => creature.Position.Add(d)))
            {
                if (walls.Contains(p) || allies.ContainsKey(p)) continue;
                seen.Add(p);

                queue.Enqueue(new Tuple<Point, Point>(p, p));
            }

            var nextPositions = new List<Tuple<Point, Point>>();
            while (true)
            {
                var nextQueue = new Queue<Tuple<Point, Point>>();
                while (queue.Count > 0)
                {
                    var (currentPosition, firstPosition) = queue.Dequeue();

                    foreach (var p in Point.ManhattanDirections.Select(d => currentPosition.Add(d)))
                    {
                        if (seen.Contains(p)) continue;
                        seen.Add(p);

                        if (walls.Contains(p) || allies.ContainsKey(p)) continue;

                        if (enemies.ContainsKey(p))
                            nextPositions.Add(new Tuple<Point, Point>(currentPosition, firstPosition));

                        nextQueue.Enqueue(new Tuple<Point, Point>(p, firstPosition));
                    }
                }

                if (nextPositions.Count > 0 || nextQueue.Count == 0)
                    break;

                queue = nextQueue;
            }

            if (nextPositions.Count == 0) return;

            var (_, nextPosition) = nextPositions.OrderBy(t => t.Item1.Y).ThenBy(t => t.Item1.X).First();
            allies.Remove(creature.Position);
            creature.Position = nextPosition;
            allies[nextPosition] = creature;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetWinningElfOutcome(LoadInput("practice.1")));
            Console.WriteLine(GetWinningElfOutcome(LoadInput("practice.2")));
            Console.WriteLine(GetWinningElfOutcome(LoadInput("practice.3")));
            Console.WriteLine(GetWinningElfOutcome(LoadInput("practice.4")));
            Console.WriteLine(GetWinningElfOutcome(LoadInput("practice.5")));
            Console.WriteLine(GetWinningElfOutcome(LoadInput("practice.6")));
            Console.WriteLine(GetWinningElfOutcome(LoadInput()));
        }

        private static int GetWinningElfOutcome(IReadOnlyList<string> input)
        {
            int startAttackPower, endAttackPower;
            for (var attackPower = 1;; attackPower *= 2)
            {
                var (elfWin, _) = GetResult(input, true, attackPower);
                if (!elfWin) continue;
                startAttackPower = attackPower / 2;
                endAttackPower = attackPower;
                break;
            }

            while (startAttackPower != endAttackPower)
            {
                var midAttackPower = (startAttackPower + endAttackPower) / 2;
                var (elfWin, _) = GetResult(input, true, midAttackPower);

                if (elfWin)
                    endAttackPower = midAttackPower;
                else
                    startAttackPower = midAttackPower + 1;
            }

            return GetResult(input, true, endAttackPower).outcome;
        }

        private abstract class Creature
        {
            public Point Position;
            public int AttackPower = 3;
            public int HealthPoints = 200;
        }

        private class Goblin : Creature
        {
        }

        private class Elf : Creature
        {
        }
    }
}