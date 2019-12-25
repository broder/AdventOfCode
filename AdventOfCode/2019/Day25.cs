using System;
using System.Linq;
using Combinatorics.Collections;

namespace AdventOfCode._2019
{
    internal class Day25 : BaseIntcodeDay
    {
        protected override void RunPartOne()
        {
            FindPassword(LoadInput().First());
        }

        private const string AllItemPath = "north\n" + "north\n" + "take sand\n" + "south\n" + "south\n" + "south\n" +
                                           "take space heater\n" + "south\n" + "east\n" + "take loom\n" + "west\n" +
                                           "north\n" + "west\n" + "take wreath\n" + "south\n" +
                                           "take space law space brochure\n" + "south\n" + "take pointer\n" +
                                           "north\n" + "north\n" + "east\n" + "north\n" + "west\n" + "south\n" +
                                           "take planetoid\n" + "north\n" + "west\n" + "take festive hat\n" +
                                           "south\n" + "west\n";

        private static void FindPassword(string opcodeString)
        {
            var vm = new IntcodeVM(opcodeString, 8000).Run();

            vm.SendInput(AllItemPath).Run().GetOutputs().Clear();

            var output = vm.SendInput("inv\n").Run().GetOutputs();
            var allItems = string.Join("", output.Select(i => (char) i)).Split('\n').Where(s => s.StartsWith("- "))
                .Select(s => s.Replace("- ", "")).ToList();
            Console.WriteLine(string.Join(",", allItems));
            output.Clear();

            for (var i = 1; i <= allItems.Count; i++)
            {
                foreach (var combination in new Combinations<string>(allItems, i))
                {
                    foreach (var item in allItems)
                        vm.SendInput($"drop {item}\n").Run().GetOutputs().Clear();
                    foreach (var item in combination)
                        vm.SendInput($"take {item}\n").Run().GetOutputs().Clear();
                    var o = vm.SendInput($"north\n").Run().GetOutputs();
                    var response = string.Join("", output.Select(c => (char) c));
                    if (!response.Contains("lighter") && !response.Contains("heavier"))
                    {
                        Console.WriteLine(response);
                        return;
                    }

                    o.Clear();
                }
            }
        }

        private static void RunGame(string opcodeString)
        {
            var vm = new IntcodeVM(opcodeString, 8000).Run();
            var output = vm.GetOutputs();
            Console.WriteLine(string.Join("", output.Select(i => (char) i)));
            output.Clear();

            while (!vm.IsFinished())
            {
                vm.SendInput(Console.ReadLine()).SendInput('\n').Run();

                output = vm.GetOutputs();
                Console.WriteLine(string.Join("", output.Select(i => (char) i)));
                output.Clear();
            }
        }

        protected override void RunPartTwo()
        {
        }
    }
}