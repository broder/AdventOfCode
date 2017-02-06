using System;
using System.Linq;

namespace AoC._2015
{
    internal class Day01 : BaseDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(GetFloor("(())"));
            Console.WriteLine(GetFloor("()()"));
            Console.WriteLine(GetFloor("((("));
            Console.WriteLine(GetFloor("(()(()("));
            Console.WriteLine(GetFloor("))((((("));
            Console.WriteLine(GetFloor("())"));
            Console.WriteLine(GetFloor("))("));
            Console.WriteLine(GetFloor(")))"));
            Console.WriteLine(GetFloor(")())())"));
            Console.WriteLine(GetFloorFromFile());
        }

        private int GetFloorFromFile()
        {
            var instructions = LoadInput().First();
            return GetFloor(instructions);
        }

        private int GetFloor(string instructions)
        {
            var floor = 0;
            foreach (var c in instructions)
            {
                if (c == '(')
                    floor++;
                else
                    floor--;
            }
            return floor;
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(GetBasementPosition(")"));
            Console.WriteLine(GetBasementPosition("()())"));
            Console.WriteLine(GetBasementPositionFromFile());
        }

        private int GetBasementPositionFromFile()
        {
            var instructions = LoadInput().First();
            return GetBasementPosition(instructions);
        }

        private int GetBasementPosition(string instructions)
        {
            var floor = 0;
            for (var i = 0; i < instructions.Length; i++)
            {
                var c = instructions[i];
                if (c == '(')
                    floor++;
                else
                    floor--;

                if (floor < 0)
                    return i + 1;
            }
            return -1;
        }
    }
}