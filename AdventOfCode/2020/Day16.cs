using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2020
{
    internal class Day16 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(SumInvalidValues(LoadInput("practice.1")));
            Console.WriteLine(SumInvalidValues(LoadInput()));
        }

        private static (Dictionary<string, List<Tuple<int, int>>> fields, int[] myTicket, int[][] otherTickets) ParseTickets(string[] input)
        {
            var i = 0;
            var fields = new Dictionary<string, List<Tuple<int, int>>>();
            while (input[i].Length > 0)
            {
                var s = input[i].Split(": ");
                fields[s[0]] = s[1].Split(" or ").Select(s => s.Split('-').Select(int.Parse).ToArray()).Select(i => new Tuple<int, int>(i[0], i[1])).ToList();
                i++;
            }

            i += 2;

            var myTicket = input[i].Split(',').Select(int.Parse).ToArray();

            i += 3;

            var otherTickets = input.Skip(i).Select(t => t.Split(',').Select(int.Parse).ToArray()).ToArray();

            return (fields, myTicket, otherTickets);
        }

        private static int SumInvalidValues(string[] input)
        {
            var (fields, _, otherTickets) = ParseTickets(input);
            return otherTickets.Aggregate(new List<int>(), (l, t) => l.Concat(t).ToList()).Where(f => IsInvalidField(fields, f)).Sum();
        }

        private static bool IsInvalidField(Dictionary<string, List<Tuple<int, int>>> fields, int field) => !GetPossibleFields(fields, field).Any();

        private static IEnumerable<string> GetPossibleFields(Dictionary<string, List<Tuple<int, int>>> fields, int n)
        {
            foreach (var field in fields)
                foreach (var range in field.Value)
                    if (range.Item1 <= n && n <= range.Item2)
                        yield return field.Key;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(PrintFieldValues(GetFieldValues(LoadInput("practice.2"))));
            Console.WriteLine(MultiplyDepartureFieldValues(GetFieldValues(LoadInput())));
        }

        private static Dictionary<string, int> GetFieldValues(string[] input)
        {
            var (fields, myTicket, otherTickets) = ParseTickets(input);

            var validFields = new HashSet<string>[myTicket.Length];
            for (var j = 0; j < validFields.Length; j++)
                validFields[j] = new HashSet<string>(fields.Keys);

            foreach (var t in otherTickets.Where(t => !IsInvalidTicket(fields, t)))
                for (var j = 0; j < t.Length; j++)
                    validFields[j].IntersectWith(GetPossibleFields(fields, t[j]));

            var remainingFields = new HashSet<string>(fields.Keys);
            while (remainingFields.Count() > 0)
                foreach (var f in validFields)
                    if (f.Count() == 1)
                        remainingFields.Remove(f.First());
                    else
                        f.IntersectWith(remainingFields);

            var fieldValues = new Dictionary<string, int>();
            for (var j = 0; j < myTicket.Length; j++)
                fieldValues[validFields[j].First()] = myTicket[j];
            return fieldValues;
        }

        private static bool IsInvalidTicket(Dictionary<string, List<Tuple<int, int>>> fields, int[] ticket) => ticket.Any(t => IsInvalidField(fields, t));

        private static string PrintFieldValues(Dictionary<string, int> fields) => string.Join('\n', fields.Select(v => $"{v.Key}:{v.Value}"));

        private static long MultiplyDepartureFieldValues(Dictionary<string, int> fields) => fields.Where(f => f.Key.StartsWith("departure")).Aggregate(1L, (m, f) => m * f.Value);
    }
}