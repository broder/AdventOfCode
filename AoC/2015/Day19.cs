using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC._2015
{
    internal class Day19 : BaseDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(GetDistinctMoleculesCount("HOH", "practice"));
            Console.WriteLine(GetDistinctMoleculesCount("HOHOHO", "practice"));
            Console.WriteLine(GetDistinctMoleculesCount(
                "ORnPBPMgArCaCaCaSiThCaCaSiThCaCaPBSiRnFArRnFArCaCaSiThCaCaSiThCaCaCaCaCaCaSiRnFYFArSiRnMgArCaSiRnPTiTiBFYPBFArSiRnCaSiRnTiRnFArSiAlArPTiBPTiRnCaSiAlArCaPTiTiBPMgYFArPTiRnFArSiRnCaCaFArRnCaFArCaSiRnSiRnMgArFYCaSiRnMgArCaCaSiThPRnFArPBCaSiRnMgArCaCaSiThCaSiRnTiMgArFArSiThSiThCaCaSiRnMgArCaCaSiRnFArTiBPTiRnCaSiAlArCaPTiRnFArPBPBCaCaSiThCaPBSiThPRnFArSiThCaSiThCaSiThCaPTiBSiRnFYFArCaCaPRnFArPBCaCaPBSiRnTiRnFArCaPRnFArSiRnCaCaCaSiThCaRnCaFArYCaSiRnFArBCaCaCaSiThFArPBFArCaSiRnFArRnCaCaCaFArSiRnFArTiRnPMgArF"));
        }

        private int GetDistinctMoleculesCount(string molecule, string fileVariant = null)
        {
            return GetDistinctMolecules(molecule, fileVariant).Count();
        }

        private IEnumerable<string> GetDistinctMolecules(string molecule, string fileVariant = null)
        {
            var molecules = new List<string>();
            foreach (var transformation in LoadInput(fileVariant))
            {
                var split = transformation.Split(new[] {" => "}, StringSplitOptions.None);
                var find = split[0];
                var replace = split[1];
                var i = molecule.IndexOf(find);
                while (i != -1)
                {
                    var start = molecule.Substring(0, i);
                    var endIndex = i + find.Length;
                    var end = molecule.Substring(endIndex);
                    molecules.Add(start + replace + end);
                    i = end.IndexOf(find);
                    if (i < 0) break;
                    i += endIndex;
                }
            }

            return molecules.Distinct();
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(GetMinimumSteps("HOH", "practice"));
            Console.WriteLine(GetMinimumSteps("HOHOHO", "practice"));
            Console.WriteLine(GetMinimumStepsAfterAnalysis(
                "ORnPBPMgArCaCaCaSiThCaCaSiThCaCaPBSiRnFArRnFArCaCaSiThCaCaSiThCaCaCaCaCaCaSiRnFYFArSiRnMgArCaSiRnPTiTiBFYPBFArSiRnCaSiRnTiRnFArSiAlArPTiBPTiRnCaSiAlArCaPTiTiBPMgYFArPTiRnFArSiRnCaCaFArRnCaFArCaSiRnSiRnMgArFYCaSiRnMgArCaCaSiThPRnFArPBCaSiRnMgArCaCaSiThCaSiRnTiMgArFArSiThSiThCaCaSiRnMgArCaCaSiRnFArTiBPTiRnCaSiAlArCaPTiRnFArPBPBCaCaSiThCaPBSiThPRnFArSiThCaSiThCaSiThCaPTiBSiRnFYFArCaCaPRnFArPBCaCaPBSiRnTiRnFArCaPRnFArSiRnCaCaCaSiThCaRnCaFArYCaSiRnFArBCaCaCaSiThFArPBFArCaSiRnFArRnCaCaCaFArSiRnFArTiRnPMgArF"));
        }

        private int GetMinimumSteps(string targetMolecule, string fileVariant = null)
        {
            var transformations = LoadInput(fileVariant)
                .Select(t => t.Split(new[] {" => "}, StringSplitOptions.None))
                .OrderByDescending(t => t[1].Length);

            var states = new Stack<Tuple<int, string>>();
            states.Push(new Tuple<int, string>(0, targetMolecule));
            while (states.Count > 0)
            {
                var state = states.Pop();
                var currentCount = state.Item1;
                var currentMolecule = state.Item2;

                if (currentMolecule == "e")
                    return currentCount;

                var nextCount = currentCount + 1;

                foreach (var transformation in transformations)
                {
                    var r = new Regex(transformation[1]);
                    if (r.IsMatch(currentMolecule))
                        states.Push(new Tuple<int, string>(nextCount,
                            r.Replace(currentMolecule, transformation[0], 1)));
                }
            }
            return 0;
        }

        private int GetMinimumStepsAfterAnalysis(string targetMolecule)
        {
            var elements = targetMolecule.Count(char.IsUpper);
            var radon = (targetMolecule.Length - targetMolecule.Replace("Rn", "").Length) / 2;
            var argon = (targetMolecule.Length - targetMolecule.Replace("Ar", "").Length) / 2;
            var ytterbium = (targetMolecule.Length - targetMolecule.Replace("Y", "").Length);
            return elements - radon - argon - 2 * ytterbium - 1;
        }
    }
}