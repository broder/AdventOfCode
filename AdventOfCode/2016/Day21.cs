using System;
using System.Linq;
using System.Text.RegularExpressions;
using Combinatorics.Collections;

namespace AdventOfCode._2016
{
    internal class Day21 : BaseDay
    {
        private readonly Regex SwapPositionRegex = new Regex(@"swap position (.*?) with position (.*?)$");
        private readonly Regex SwapLetterRegex = new Regex(@"swap letter (.*?) with letter (.*?)$");
        private readonly Regex RotateStepRegex = new Regex(@"rotate (.*?) (.*?) step");
        private readonly Regex RotateLetterRegex = new Regex(@"rotate based on position of letter (.*?)$");
        private readonly Regex ReverseRegex = new Regex(@"reverse positions (.*?) through (.*?)$");
        private readonly Regex MoveRegex = new Regex(@"move position (.*?) to position (.*?)$");

        protected override void RunPartOne()
        {
            Console.WriteLine(GetPassword("abcde", "practice"));
            Console.WriteLine(GetPassword("abcdefgh"));
        }

        private string GetPassword(string password, string fileVariant = null)
        {
            foreach (var line in LoadInput(fileVariant))
            {
                if (SwapPositionRegex.IsMatch(line))
                {
                    var swapPositionMatch = SwapPositionRegex.Match(line);
                    var x = int.Parse(swapPositionMatch.Groups[1].Value);
                    var y = int.Parse(swapPositionMatch.Groups[2].Value);
                    password = SwapPosition(password, x, y);
                }
                else if (SwapLetterRegex.IsMatch(line))
                {
                    var swapLetterMatch = SwapLetterRegex.Match(line);
                    var x = swapLetterMatch.Groups[1].Value;
                    var y = swapLetterMatch.Groups[2].Value;
                    password = SwapLetter(password, x, y);
                }
                else if (RotateStepRegex.IsMatch(line))
                {
                    var rotateStepMatch = RotateStepRegex.Match(line);
                    var left = rotateStepMatch.Groups[1].Value.Equals("left");
                    var x = int.Parse(rotateStepMatch.Groups[2].Value);
                    password = RotateStep(password, x, left);
                }
                else if (RotateLetterRegex.IsMatch(line))
                {
                    var rotateLetterMatch = RotateLetterRegex.Match(line);
                    var letter = rotateLetterMatch.Groups[1].Value;
                    password = RotateLetter(password, letter);
                }
                else if (ReverseRegex.IsMatch(line))
                {
                    var reverseMatch = ReverseRegex.Match(line);
                    var x = int.Parse(reverseMatch.Groups[1].Value);
                    var y = int.Parse(reverseMatch.Groups[2].Value);
                    password = Reverse(password, x, y);
                }
                else if (MoveRegex.IsMatch(line))
                {
                    var moveMatch = MoveRegex.Match(line);
                    var x = int.Parse(moveMatch.Groups[1].Value);
                    var y = int.Parse(moveMatch.Groups[2].Value);
                    password = Move(password, x, y);
                }
            }
            return password;
        }

        private string Move(string password, int start, int end)
        {
            var letter = password.Substring(start, 1);
            var output = password.Remove(start, 1);
            output = output.Insert(end, letter);
            return output;
        }

        private string Reverse(string password, int start, int end)
        {
            var arr = password.ToCharArray();
            Array.Reverse(arr, start, end - start + 1);
            return new string(arr);
        }

        private string RotateLetter(string password, string letter)
        {
            var steps = password.IndexOf(letter);
            if (steps >= 4) steps++;
            return RotateStep(password, 1 + steps, false);
        }

        private string RotateStep(string password, int steps, bool left)
        {
            var arr = password.ToCharArray();
            var width = arr.Length;
            for (var s = 0; s < steps; s++)
            {
                if (left)
                {
                    var first = arr[0];
                    for (var i = 1; i < width; i++)
                        arr[i - 1] = arr[i];
                    arr[width - 1] = first;
                }
                else
                {
                    var last = arr[width - 1];
                    for (var i = width - 1; i > 0; i--)
                        arr[i] = arr[i - 1];
                    arr[0] = last;
                }
            }
            return new string(arr);
        }

        private string SwapLetter(string password, string x, string y)
        {
            return SwapPosition(password, password.IndexOf(x), password.IndexOf(y));
        }

        private string SwapPosition(string password, int x, int y)
        {
            var arr = password.ToCharArray();
            arr[x] ^= arr[y];
            arr[y] ^= arr[x];
            arr[x] ^= arr[y];
            return new string(arr);
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetUnscrambledInput("fbgdceah"));
        }

        private string GetUnscrambledInput(string target)
        {
            foreach (var perm in new Permutations<char>("abcdefgh".ToList()))
            {
                var permStr = new string(perm.ToArray());
                if (GetPassword(permStr) == target)
                    return permStr;
            }
            return "";
        }
    }
}