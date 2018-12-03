using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018
{
    internal class Day03 : BaseDay
    {
        private const int SquareSize = 2000;

        protected override void RunPartOne()
        {
            Console.WriteLine(GetOverusedSquares(LoadInput("practice")));
            Console.WriteLine(GetOverusedSquares(LoadInput()));
        }

        private static int GetOverusedSquares(IEnumerable<string> claims)
        {
            var fabricUsages = new int[SquareSize, SquareSize];
            var parsedClaims = claims.Select(ParseClaim).ToArray();
            foreach (var parsedClaim in parsedClaims)
                for (var x = parsedClaim.StartX; x < parsedClaim.StartX + parsedClaim.Width; x++)
                for (var y = parsedClaim.StartY; y < parsedClaim.StartY + parsedClaim.Height; y++)
                    fabricUsages[x, y]++;

            return fabricUsages.Cast<int>().Count(usages => usages > 1);
        }

        private static Claim ParseClaim(string claim)
        {
            var splitClaim = claim.Split(new[] {"#", " @ ", ",", ": ", "x"}, StringSplitOptions.RemoveEmptyEntries);
            return new Claim
            {
                Id = int.Parse(splitClaim[0]),
                StartX = int.Parse(splitClaim[1]),
                StartY = int.Parse(splitClaim[2]),
                Width = int.Parse(splitClaim[3]),
                Height = int.Parse(splitClaim[4]),
            };
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetNonOverlappingClaim(LoadInput("practice")));
            Console.WriteLine(GetNonOverlappingClaim(LoadInput()));
        }

        private static int GetNonOverlappingClaim(IEnumerable<string> claims)
        {
            var fabricUsages = new List<int>[SquareSize, SquareSize];
            var parsedClaims = claims.Select(ParseClaim).ToArray();
            foreach (var parsedClaim in parsedClaims)
            {
                for (var x = parsedClaim.StartX; x < parsedClaim.StartX + parsedClaim.Width; x++)
                {
                    for (var y = parsedClaim.StartY; y < parsedClaim.StartY + parsedClaim.Height; y++)
                    {
                        if (fabricUsages[x, y] == null)
                            fabricUsages[x, y] = new List<int>();

                        fabricUsages[x, y].Add(parsedClaim.Id);
                    }
                }
            }

            var nonOverlappingClaims = new HashSet<int>(parsedClaims.Select(c => c.Id));
            foreach (var usage in fabricUsages)
            {
                if (usage == null || usage.Count < 2)
                    continue;

                foreach (var claimId in usage)
                    nonOverlappingClaims.Remove(claimId);
            }

            return nonOverlappingClaims.First();
        }

        private struct Claim
        {
            public int Id;
            public int StartX;
            public int StartY;
            public int Width;
            public int Height;
        }
    }
}