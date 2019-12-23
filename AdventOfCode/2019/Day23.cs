using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day23 : BaseIntcodeDay
    {
        protected override void RunPartOne()
        {
            Console.Write(SimulateNetwork(LoadInput().First()));
        }

        private static long SimulateNetwork(string opcodeString, bool returnFirstNatValue = true)
        {
            var routers = new List<IntcodeVM>();
            for (var i = 0; i < 50; i++)
                routers.Add(new IntcodeVM(opcodeString, 4000).SendInput(i).Run());

            Tuple<long, long> nat = null;
            var lastY = -1L;

            var t = 0;

            while (true)
            {
                var inputs = new Dictionary<long, List<long>>();
                foreach (var router in routers)
                {
                    var outputs = router.GetOutputs();

                    for (var i = 0; i < outputs.Count; i += 3)
                    {
                        if (!inputs.ContainsKey(outputs[i]))
                            inputs[outputs[i]] = new List<long>();
                        inputs[outputs[i]].Add(outputs[i + 1]);
                        inputs[outputs[i]].Add(outputs[i + 2]);
                    }

                    router.GetOutputs().Clear();
                }

                if (inputs.Count == 0 && nat != null)
                {
                    inputs[0] = new List<long> {nat.Item1, nat.Item2};
                    if (nat.Item2 == lastY)
                        return nat.Item2;

                    lastY = nat.Item2;
                }

                if (inputs.ContainsKey(255))
                {
                    if (returnFirstNatValue) return inputs[255][1];
                    nat = new Tuple<long, long>(inputs[255][inputs[255].Count - 2], inputs[255][inputs[255].Count - 1]);
                }

                for (var i = 0; i < routers.Count; i++)
                {
                    var input = inputs.GetValueOrDefault(i, new List<long> {-1L});
                    foreach (var inp in input)
                        routers[i].SendInput(inp).Run();
                }

                t++;
            }
        }

        protected override void RunPartTwo()
        {
            Console.Write(SimulateNetwork(LoadInput().First(), false));
        }
    }
}