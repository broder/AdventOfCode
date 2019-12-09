using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day09 : BaseOpcodeDay
    {
        protected override void RunPartOne()
        {
            new OpcodeVM("109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99").Run().PrintOutputs();
            new OpcodeVM("1102,34915192,34915192,7,4,7,99,0").Run().PrintOutputs();
            new OpcodeVM("104,1125899906842624,99").Run().PrintOutputs();
            new OpcodeVM(LoadInput().First()).SendInput(1).Run().PrintOutputs();
        }

        protected override void RunPartTwo()
        {
            new OpcodeVM(LoadInput().First()).SendInput(2).Run().PrintOutputs();
        }
    }
}