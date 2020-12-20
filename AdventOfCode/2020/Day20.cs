using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2020
{
    internal class Day20 : BaseDay
    {
        private static char[][] Monster = @"
                  # 
#    ##    ##    ###
 #  #  #  #  #  #   ".Split("\n").Skip(1).Select(s => s.ToCharArray()).ToArray();
        protected override void RunPartOne()
        {
            Console.WriteLine(GetCornerIds(LoadInput("practice")));
            Console.WriteLine(GetCornerIds(LoadInput()));
        }

        private static long GetCornerIds(string[] input)
        {
            var board = GetBoard(input);
            var size = board.GetLength(0);
            return board[0, 0].ID * board[size - 1, 0].ID * board[size - 1, size - 1].ID * board[0, size - 1].ID;
        }

        private static Tile[,] GetBoard(string[] input)
        {
            var tiles = new Dictionary<long, List<Tile>>();
            foreach (var t in string.Join('\n', input).Split("\n\n"))
            {
                var s = t.Split('\n');
                var id = long.Parse(s[0].Split(new[] { "Tile ", ":" }, StringSplitOptions.RemoveEmptyEntries)[0]);
                var squares = s.Skip(1).ToArray();
                tiles[id] = new List<Tile>() {
                    new Tile(id, squares),
                    new Tile(id, squares).FlipSquaresHorizontal(),
                    new Tile(id, squares).FlipSquaresVertical(),
                    new Tile(id, squares).RotateSquares(),
                    new Tile(id, squares).RotateSquares().FlipSquaresHorizontal(),
                    new Tile(id, squares).RotateSquares().FlipSquaresVertical(),
                    new Tile(id, squares).RotateSquares().RotateSquares(),
                    new Tile(id, squares).RotateSquares().RotateSquares().RotateSquares(),
                };
            }

            var size = (int)Math.Sqrt(tiles.Count());
            var board = new Tile[size, size];

            FillBoard(tiles, new HashSet<long>(), board);
            return board;
        }

        private static bool FillBoard(Dictionary<long, List<Tile>> potentialTiles, HashSet<long> usedTiles, Tile[,] board)
        {
            var (x, y) = FindNonFilledSquare(board);
            if (x == -1) return true;

            foreach (var potentialTile in potentialTiles.Where(kvp => !usedTiles.Contains(kvp.Key)))
            {
                foreach (var potentialPermutation in potentialTile.Value)
                {
                    if (x > 0 && board[x - 1, y] != null && potentialPermutation.LeftEdge != board[x - 1, y].RightEdge) continue;
                    if (y > 0 && board[x, y - 1] != null && potentialPermutation.TopEdge != board[x, y - 1].BottomEdge) continue;

                    var t = usedTiles.Add(potentialTile.Key);
                    board[x, y] = potentialPermutation;
                    if (FillBoard(potentialTiles, usedTiles, board)) return true;

                    usedTiles.Remove(potentialTile.Key);
                    board[x, y] = null;
                }
            }

            return false;
        }

        private static (int x, int y) FindNonFilledSquare(Tile[,] board)
        {
            for (var y = 0; y < board.GetLength(1); y++)
                for (var x = 0; x < board.GetLength(0); x++)
                    if (board[x, y] == null) return (x, y);
            return (-1, -1);
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetRoughness(LoadInput("practice")));
            Console.WriteLine(GetRoughness(LoadInput()));
        }

        private static long GetRoughness(string[] input)
        {
            var board = GetBoard(input);
            var boardSize = board.GetLength(0);
            var tileSize = board[0, 0].Squares.Length;
            var gridSize = boardSize * (tileSize - 2);
            var grid = new char[gridSize][];

            for (var by = 0; by < boardSize; by++)
            {
                for (var ty = 1; ty < tileSize - 1; ty++)
                {
                    grid[by * (tileSize - 2) + ty - 1] = new char[gridSize];
                    for (var bx = 0; bx < boardSize; bx++)
                        for (var tx = 1; tx < tileSize - 1; tx++)
                            grid[by * (tileSize - 2) + ty - 1][bx * (tileSize - 2) + tx - 1] = board[bx, by].Squares[ty][tx];
                }
            }

            var grids = new List<char[][]>() {
                grid,
                FlipHorizontal(grid),
                FlipVertical(grid),
                Rotate(grid),
                FlipHorizontal(Rotate(grid)),
                FlipVertical(Rotate(grid)),
                Rotate(Rotate(grid)),
                Rotate(Rotate(Rotate(grid))),
            };

            foreach (var g in grids)
            {
                var hasMonsters = false;
                for (var gy = 0; gy < g.Length; gy++)
                {
                    for (var gx = 0; gx < g.Length; gx++)
                    {
                        if (IsMonster(g, gx, gy))
                        {
                            for (var my = 0; my < Monster.Length; my++)
                                for (var mx = 0; mx < Monster[my].Length; mx++)
                                    if (Monster[my][mx] == '#')
                                        g[gy + my][gx + mx] = 'O';
                            hasMonsters = true;
                        }
                    }
                }
                if (hasMonsters)
                    return g.Sum(r => r.Count(b => b == '#'));
            }
            return -1;
        }

        private static bool IsMonster(char[][] grid, int gx, int gy)
        {
            if (gy + Monster.Length >= grid.Length) return false;
            if (gx + Monster[0].Length >= grid[0].Length) return false;
            for (var my = 0; my < Monster.Length; my++)
                for (var mx = 0; mx < Monster[my].Length; mx++)
                    if (Monster[my][mx] == '#' && grid[gy + my][gx + mx] != '#') return false;
            return true;
        }

        private static char[][] FlipHorizontal(char[][] c)
        {
            var o = new char[c.Length][];
            for (var i = 0; i < c.Length; i++)
                o[i] = c[i].Reverse().ToArray();
            return o;
        }

        private static char[][] FlipVertical(char[][] c)
        {
            var o = new char[c.Length][];
            for (var i = 0; i < c.Length; i++)
                o[i] = c[c.Length - 1 - i].Select(a => a).ToArray();
            return o;
        }

        private static char[][] Rotate(char[][] c)
        {
            var o = new char[c.Length][];
            for (var i = 0; i < c.Length; i++)
                o[i] = new char[c.Length];
            for (var i = 0; i < c.Length / 2; i++)
            {
                for (var j = i; j < c.Length - i - 1; j++)
                {
                    o[i][j] = c[j][c.Length - 1 - i];
                    o[j][c.Length - 1 - i] = c[c.Length - 1 - i][c.Length - 1 - j];
                    o[c.Length - 1 - i][c.Length - 1 - j] = c[c.Length - 1 - j][i];
                    o[c.Length - 1 - j][i] = c[i][j];
                }
            }
            return o;
        }

        private class Tile
        {
            public long ID;
            public char[][] Squares;

            public string TopEdge;
            public string BottomEdge;
            public string LeftEdge;
            public string RightEdge;

            public Tile(long ID, string[] Squares)
            {
                this.ID = ID;
                this.Squares = Squares.Select(s => s.ToCharArray()).ToArray();
                GenerateEdges();
            }

            public Tile FlipSquaresHorizontal()
            {
                Squares = FlipHorizontal(Squares);
                GenerateEdges();
                return this;
            }

            public Tile FlipSquaresVertical()
            {
                Squares = FlipVertical(Squares);
                GenerateEdges();
                return this;
            }

            public Tile RotateSquares()
            {
                Squares = Rotate(Squares);
                GenerateEdges();
                return this;
            }

            private void GenerateEdges()
            {
                TopEdge = new string(Squares.First());
                BottomEdge = new string(Squares.Last());
                LeftEdge = string.Join("", Squares.Select(s => s.First()));
                RightEdge = string.Join("", Squares.Select(s => s.Last()));
            }
        }
    }
}