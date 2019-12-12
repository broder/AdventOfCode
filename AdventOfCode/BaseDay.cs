using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode
{
    internal abstract class BaseDay
    {
        protected const string Alphabet = "abcdefghijklmnopqrstuvwxyz";

        private readonly string CurrentYear;
        private readonly string CurrentDay;

        protected BaseDay()
        {
            CurrentYear = GetType().Namespace?.Split('.').Last().Replace("_", "");
            CurrentDay = GetType().Name;
            Run();
        }

        private void Run()
        {
            Console.WriteLine($"{CurrentYear}.{CurrentDay}.1");
            RunPartOne();
            Console.WriteLine();

            Console.WriteLine($"{CurrentYear}.{CurrentDay}.2");
            RunPartTwo();
            Console.WriteLine();
            Console.WriteLine("END");
        }

        protected abstract void RunPartOne();
        protected abstract void RunPartTwo();

        protected string[] LoadInput(string variant = null)
        {
            var filenameParts = new List<string> {CurrentDay.ToLower(), ".txt"};
            if (!string.IsNullOrWhiteSpace(variant))
                filenameParts.Insert(1, $".{variant}");
            var filename = string.Join("", filenameParts);
            return File.ReadAllLines($"{Directory.GetCurrentDirectory()}/{CurrentYear}/Input/{filename}");
        }

        protected static int Mod(int x, int m) => (x % m + m) % m;

        protected static long Mod(long x, long m) => (x % m + m) % m;

        private readonly MD5 Md5 = MD5.Create();

        protected string CalculateMd5Hash(string input)
        {
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = Md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();

            foreach (var t in hash)
                sb.Append(t.ToString("x2"));

            return sb.ToString();
        }

        protected struct Point
        {
            public static readonly Point[] ManhattanDirections =
                {new Point(0, -1), new Point(-1, 0), new Point(1, 0), new Point(0, 1)};

            public Point(int x, int y, int z = 0)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public readonly int X;
            public readonly int Y;
            public readonly int Z;

            public bool Equals(Point p)
            {
                return X == p.X && Y == p.Y && Z == p.Z;
            }

            public override bool Equals(object obj)
            {
                return obj?.GetType() == GetType() && Equals((Point) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (((X * 397) ^ Y) * 397) ^ Z;
                }
            }

            public override string ToString()
            {
                return $"{X},{Y},{Z}";
            }

            public Point Add(Point p)
            {
                return new Point(X + p.X, Y + p.Y, Z + p.Z);
            }

            public Point Subtract(Point p)
            {
                return new Point(X - p.X, Y - p.Y, Z - p.Z);
            }
        }

        protected static LinkedListNode<T> GetNextCircularNode<T>(LinkedListNode<T> n) => n.Next ?? n.List.First;

        protected static LinkedListNode<T> GetPreviousCircularNode<T>(LinkedListNode<T> n) => n.Previous ?? n.List.Last;

        protected static long GetGreatestCommonDivisor(long a, long b) => b == 0 ? a : GetGreatestCommonDivisor(b, a % b);

        protected static long GetLowestCommonMultiple(long a, long b) => (a / GetGreatestCommonDivisor(a, b)) * b;
    }
}