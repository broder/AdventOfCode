using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2020
{
    internal class Day01 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(ProcessExpenseReport(LoadInput("practice").Select(int.Parse).ToList()));
            Console.WriteLine(ProcessExpenseReport(LoadInput().Select(int.Parse).ToList()));
        }

        private static int ProcessExpenseReport(List<int> report)
        {
            for (var i = 0; i < report.Count; i++)
                for (var j = i + 1; j < report.Count; j++)
                    if (report[i] + report[j] == 2020)
                        return report[i] * report[j];

            return -1;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(ProcessExpenseReportThree(LoadInput("practice").Select(int.Parse).ToList()));
            Console.WriteLine(ProcessExpenseReportThree(LoadInput().Select(int.Parse).ToList()));
        }

        private static int ProcessExpenseReportThree(List<int> report)
        {
            for (var i = 0; i < report.Count; i++)
                for (var j = i + 1; j < report.Count; j++)
                    for (var k = j + 1; k < report.Count; k++)
                        if (report[i] + report[j] + report[k] == 2020)
                            return report[i] * report[j] * report[k];

            return -1;
        }
    }
}