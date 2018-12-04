using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018
{
    internal class Day04 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetMostAsleepGuardChecksum(LoadInput("practice")));
            Console.WriteLine(GetMostAsleepGuardChecksum(LoadInput()));
        }

        private static int GetMostAsleepGuardChecksum(string[] records)
        {
            var parsedRecords = ParseRecords(records);

            var guardSleepTimes = parsedRecords.Aggregate(new Dictionary<int, int>(), (d, r) =>
            {
                if (!d.ContainsKey(r.GuardId))
                    d[r.GuardId] = 0;

                foreach (var t in r.TimesAsleep)
                    d[r.GuardId] += t.End - t.Start;

                return d;
            });

            var sleepiestGuardId = guardSleepTimes.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

            var sleepiestGuardRecords = parsedRecords.Where(r => r.GuardId == sleepiestGuardId).ToArray();
            var sleepiestMinute = 0;
            var sleepiestMinuteCount = 0;

            for (var minute = 0; minute < 60; minute++)
            {
                var minuteCount = sleepiestGuardRecords.SelectMany(day => day.TimesAsleep)
                    .Count(times => times.Start <= minute && minute < times.End);

                if (minuteCount <= sleepiestMinuteCount)
                    continue;

                sleepiestMinuteCount = minuteCount;
                sleepiestMinute = minute;
            }

            return sleepiestGuardId * sleepiestMinute;
        }

        private static List<Record> ParseRecords(string[] records)
        {
            Array.Sort(records);
            var parsedRecords = new List<Record>();
            foreach (var record in records)
            {
                var splitRecord = record.Split(new[] {"[", "-", " ", ":", "]", "#"},
                    StringSplitOptions.RemoveEmptyEntries);
                switch (splitRecord[5])
                {
                    case "falls":
                        parsedRecords.Last().TimesAsleep.Add(new Range {Start = int.Parse(splitRecord[4])});
                        break;
                    case "wakes":
                        parsedRecords.Last().TimesAsleep.Last().End = int.Parse(splitRecord[4]);
                        break;
                    default:
                        parsedRecords.Add(new Record {GuardId = int.Parse(splitRecord[6])});
                        break;
                }
            }

            return parsedRecords;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetMostAsleepMinuteChecksum(LoadInput("practice")));
            Console.WriteLine(GetMostAsleepMinuteChecksum(LoadInput()));
        }

        private static int GetMostAsleepMinuteChecksum(string[] records)
        {
            var guardSleepCountPerMinute = new Dictionary<int, int>[60];
            foreach (var record in ParseRecords(records))
                foreach (var timeAsleep in record.TimesAsleep)
                    for (var minute = timeAsleep.Start; minute < timeAsleep.End; minute++)
                    {
                        if (guardSleepCountPerMinute[minute] == null)
                            guardSleepCountPerMinute[minute] = new Dictionary<int, int>();

                        if (!guardSleepCountPerMinute[minute].ContainsKey(record.GuardId))
                            guardSleepCountPerMinute[minute][record.GuardId] = 0;

                        guardSleepCountPerMinute[minute][record.GuardId]++;
                    }

            var maxSleeps = 0;
            var maxGuard = 0;
            var maxMinute = 0;

            for (var minute = 0; minute < 60; minute++)
            {
                if (guardSleepCountPerMinute[minute] == null)
                    continue;

                var sleepiestGuardCount = guardSleepCountPerMinute[minute].Aggregate((l, r) => l.Value > r.Value ? l : r);
                if (sleepiestGuardCount.Value < maxSleeps)
                    continue;

                maxSleeps = sleepiestGuardCount.Value;
                maxGuard = sleepiestGuardCount.Key;
                maxMinute = minute;
            }

            return maxGuard * maxMinute;
        }

        private class Record
        {
            public int GuardId;
            public readonly List<Range> TimesAsleep = new List<Range>();
        }

        private class Range
        {
            public int Start;
            public int End;
        }
    }
}