using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode._2018
{
    internal class Day23 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetStrongestNanobotNeighbours(LoadInput("practice.1")));
            Console.WriteLine(GetStrongestNanobotNeighbours(LoadInput()));
        }

        private static int GetStrongestNanobotNeighbours(IEnumerable<string> input)
        {
            var bots = ParseInput(input);

            var strongestBot = bots.OrderByDescending(b => b.Radius).First();

            var neighbourCount = 0;
            foreach (var bot in bots)
            {
                if (Math.Abs(bot.X - strongestBot.X) + Math.Abs(bot.Y - strongestBot.Y) +
                    Math.Abs(bot.Z - strongestBot.Z) <= strongestBot.Radius)
                    neighbourCount++;
            }

            return neighbourCount;
        }

        private static List<Nanobot> ParseInput(IEnumerable<string> input)
        {
            var bots = new List<Nanobot>();
            foreach (var line in input)
            {
                var split = line.Split(new[] {"pos=<", ",", ">, r="}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse).ToArray();
                bots.Add(new Nanobot {X = split[0], Y = split[1], Z = split[2], Radius = split[3]});
            }

            return bots;
        }

        protected override void RunPartTwo()
        {
            GenerateZ3File("practice.2");
            GenerateZ3File();
        }

        private void GenerateZ3File(string fileVariant = null)
        {
            var bots = ParseInput(LoadInput(fileVariant));

            var sb = new StringBuilder();

            sb.AppendLine(@"(declare-const x Int)
(declare-const y Int)
(declare-const z Int)

(define-fun abs ((v Int)) Int
    (ite (> v 0)
    v
    (- v)))

(define-fun dist ((x1 Int) (y1 Int) (z1 Int) (x2 Int) (y2 Int) (z2 Int)) Int
    (+ (abs (- x2 x1))
    (abs (- y2 y1))
    (abs (- z2 z1))))

(define-fun inrange ((x Int) (y Int) (z Int)) Int
    (+");

            foreach (var bot in bots)
            {
                sb.AppendLine($"        (if (<= (dist x y z {bot.X} {bot.Y} {bot.Z}) {bot.Radius}) 1 0)");
            }
            
            sb.AppendLine(@"    ))
(maximize (inrange x y z))
(minimize (dist 0 0 0 x y z))
(check-sat)
(get-model)");

            var fileName = $"2018.23{(fileVariant != null ? "." + fileVariant : "")}.z3";
            File.WriteAllText(fileName, sb.ToString());

            Console.WriteLine($"Run \"z3 {fileName}\"");
        }

        private struct Nanobot
        {
            public int X;
            public int Y;
            public int Z;
            public int Radius;
        }
    }
}