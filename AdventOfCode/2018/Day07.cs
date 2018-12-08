using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode._2018
{
    internal class Day07 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetStepOrder(LoadInput("practice")));
            Console.WriteLine(GetStepOrder(LoadInput()));
        }

        private static string GetStepOrder(IEnumerable<string> stepInstructions)
        {
            var steps = ParseInstructions(stepInstructions);

            var stepList = new SortedList<char, Step>(steps.Where(pair => pair.Value.PreviousSteps.Count == 0)
                .ToDictionary(p => p.Key, p => p.Value));
            var output = new StringBuilder();
            var seen = new HashSet<char>();
            while (stepList.Count > 0)
            {
                var step = stepList.First().Value;
                stepList.RemoveAt(0);
                output.Append(step.Id);
                seen.Add(step.Id);
                foreach (var nextNode in step.NextSteps)
                {
                    if (!nextNode.PreviousSteps.All(n => seen.Contains(n.Id)))
                        continue;

                    stepList.Add(nextNode.Id, nextNode);
                }
            }

            return output.ToString();
        }

        private static Dictionary<char, Step> ParseInstructions(IEnumerable<string> stepInstructions)
        {
            var steps = new Dictionary<char, Step>();
            foreach (var stepInstruction in stepInstructions)
            {
                var split = stepInstruction.Split(new[] {"Step ", " must be finished before step ", " can begin."},
                    StringSplitOptions.RemoveEmptyEntries);

                var startId = split[0][0];
                if (!steps.ContainsKey(startId))
                    steps[startId] = new Step {Id = startId};
                var start = steps[startId];

                var endId = split[1][0];
                if (!steps.ContainsKey(endId))
                    steps[endId] = new Step {Id = endId};
                var end = steps[endId];

                start.NextSteps.Add(end);
                end.PreviousSteps.Add(start);
            }

            return steps;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetStepCompletionTime(LoadInput("practice"), 2, 0));
            Console.WriteLine(GetStepCompletionTime(LoadInput(), 5, 60));
        }

        private static int GetStepCompletionTime(IEnumerable<string> stepInstructions, int workerCount, int baseSeconds)
        {
            var steps = ParseInstructions(stepInstructions);

            var stepList = new SortedList<char, Step>(steps.Where(pair => pair.Value.PreviousSteps.Count == 0)
                .ToDictionary(p => p.Key, p => p.Value));

            var workers = new Worker[workerCount];
            for (var i = 0; i < workerCount; i++)
                workers[i] = new Worker();

            var completedSteps = new HashSet<char>();
            var time = 0;
            while (stepList.Count > 0 || workers.Any(w => w.CurrentJob != null))
            {
                while (stepList.Count > 0 && workers.Any(w => w.CurrentJob == null))
                {
                    var step = stepList.First().Value;
                    stepList.RemoveAt(0);
                    var freeWorker = workers.First(w => w.CurrentJob == null);
                    freeWorker.CurrentJob = step;
                    freeWorker.CompletionTime = time + baseSeconds + (step.Id + 1 - 65);
                }

                time++;

                foreach (var worker in workers.Where(w => w.CurrentJob != null && w.CompletionTime <= time))
                {
                    var completedStep = worker.CurrentJob;
                    completedSteps.Add(completedStep.Id);
                    foreach (var nextStep in completedStep.NextSteps)
                    {
                        if (!nextStep.PreviousSteps.All(n => completedSteps.Contains(n.Id)))
                            continue;

                        stepList.Add(nextStep.Id, nextStep);
                    }

                    worker.CurrentJob = null;
                }
            }

            return time;
        }

        private class Step
        {
            public char Id;
            public readonly List<Step> NextSteps = new List<Step>();
            public readonly List<Step> PreviousSteps = new List<Step>();
        }

        private class Worker
        {
            public Step CurrentJob;
            public int CompletionTime;
        }
    }
}