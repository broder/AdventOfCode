using System;
using System.Linq;
using System.Text;

namespace AdventOfCode._2016
{
    internal class Day16 : BaseDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(GetChecksumForData("10000", 20));
            Console.WriteLine(GetChecksumForData("01000100010010111", 272));
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(GetChecksumForData("01000100010010111", 35651584));
        }

        private string GetChecksumForData(string seed, int targetLength)
        {
            var data = GetData(seed, targetLength);
            return GetCheckSum(data);
        }

        private string GetData(string seed, int targetLength)
        {
            var a = seed;
            while (a.Length < targetLength)
            {
                var b = a;
                b = new string(b.Reverse().ToArray());
                b = b.Replace('0', '2');
                b = b.Replace('1', '0');
                b = b.Replace('2', '1');
                a = a + "0" + b;
            }
            return a.Substring(0, targetLength);
        }

        private string GetCheckSum(string data)
        {
            var output = new StringBuilder();
            for (var i = 0; i < data.Length; i += 2)
            {
                output.Append(data[i] == data[i + 1] ? '1' : '0');
            }
            return output.Length % 2 == 0 ? GetCheckSum(output.ToString()) : output.ToString();
        }
    }
}