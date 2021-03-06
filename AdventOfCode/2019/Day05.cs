﻿using System;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day05 : BaseIntcodeDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(new IntcodeVM(LoadInput().First()).SendInput(1).Run().GetOutputs().Last());
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(new IntcodeVM("3,9,8,9,10,9,4,9,99,-1,8").SendInput(7).Run().GetOutputs().First());
            Console.WriteLine(new IntcodeVM("3,9,8,9,10,9,4,9,99,-1,8").SendInput(8).Run().GetOutputs().First());

            Console.WriteLine(new IntcodeVM("3,9,7,9,10,9,4,9,99,-1,8").SendInput(8).Run().GetOutputs().First());
            Console.WriteLine(new IntcodeVM("3,9,7,9,10,9,4,9,99,-1,8").SendInput(7).Run().GetOutputs().First());

            Console.WriteLine(new IntcodeVM("3,3,1108,-1,8,3,4,3,99").SendInput(7).Run().GetOutputs().First());
            Console.WriteLine(new IntcodeVM("3,3,1108,-1,8,3,4,3,99").SendInput(8).Run().GetOutputs().First());

            Console.WriteLine(new IntcodeVM("3,3,1107,-1,8,3,4,3,99").SendInput(8).Run().GetOutputs().First());
            Console.WriteLine(new IntcodeVM("3,3,1107,-1,8,3,4,3,99").SendInput(7).Run().GetOutputs().First());

            Console.WriteLine(
                new IntcodeVM("3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9").SendInput(0).Run().GetOutputs().First());
            Console.WriteLine(
                new IntcodeVM("3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9").SendInput(1).Run().GetOutputs().First());

            Console.WriteLine(new IntcodeVM("3,3,1105,-1,9,1101,0,0,12,4,12,99,1").SendInput(0).Run().GetOutputs()
                .First());
            Console.WriteLine(new IntcodeVM("3,3,1105,-1,9,1101,0,0,12,4,12,99,1").SendInput(1).Run().GetOutputs()
                .First());

            Console.WriteLine(
                new IntcodeVM(
                        "3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99")
                    .SendInput(7).Run().GetOutputs().First());
            Console.WriteLine(
                new IntcodeVM(
                        "3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99")
                    .SendInput(8).Run().GetOutputs().First());
            Console.WriteLine(
                new IntcodeVM(
                        "3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99")
                    .SendInput(9).Run().GetOutputs().First());

            Console.WriteLine(new IntcodeVM(LoadInput().First()).SendInput(5).Run().GetOutputs().First());
        }
    }
}