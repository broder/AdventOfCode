using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2020
{
    internal class Day18 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(Evaluate("1 + 2 * 3 + 4 * 5 + 6"));
            Console.WriteLine(Evaluate("1 + (2 * 3) + (4 * (5 + 6))"));
            Console.WriteLine(Evaluate("2 * 3 + (4 * 5)"));
            Console.WriteLine(Evaluate("5 + (8 * 3 + 9 + 3 * 4 * 3)"));
            Console.WriteLine(Evaluate("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))"));
            Console.WriteLine(Evaluate("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2"));
            Console.WriteLine(LoadInput().Select(Evaluate).Sum());
        }

        private static long Evaluate(string exp)
        {
            exp = exp.Replace("(", "( ");
            exp = exp.Replace(")", " )");
            var s = exp.Split(' ');
            return Evaluate(s, 0).ans;
        }

        private static (long ans, int i) Evaluate(string[] exp, int i)
        {
            var ans = 0L;
            var op = "+";

            while (i < exp.Length)
            {
                if (exp[i] == "(")
                {
                    var o = Evaluate(exp, i + 1);
                    if (op == "+")
                        ans += o.ans;
                    else
                        ans *= o.ans;
                    i = o.i;
                }
                else if (exp[i] == ")")
                    break;
                else if (exp[i] == "+")
                    op = "+";
                else if (exp[i] == "*")
                    op = "*";
                else
                {
                    var v = long.Parse(exp[i]);
                    if (op == "+")
                        ans += v;
                    else
                        ans *= v;
                }

                i++;
            }

            return (ans, i);
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(AdvancedEvaluate("1 + 2 * 3 + 4 * 5 + 6"));
            Console.WriteLine(AdvancedEvaluate("1 + (2 * 3) + (4 * (5 + 6))"));
            Console.WriteLine(AdvancedEvaluate("2 * 3 + (4 * 5)"));
            Console.WriteLine(AdvancedEvaluate("5 + (8 * 3 + 9 + 3 * 4 * 3)"));
            Console.WriteLine(AdvancedEvaluate("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))"));
            Console.WriteLine(AdvancedEvaluate("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2"));
            Console.WriteLine(LoadInput().Select(AdvancedEvaluate).Sum());
        }

        private static long AdvancedEvaluate(string exp)
        {
            var stack = new Stack<int>();
            for (var i = 0; i < exp.Length; i++)
            {
                if (exp[i] == '(')
                    stack.Push(i);
                else if (exp[i] == ')')
                {
                    var start = stack.Pop();
                    var subexpression = exp.Substring(start + 1, i - start - 1);
                    exp = exp.Substring(0, start) + AdvancedEvaluate(subexpression) + exp.Substring(i + 1);
                    i = start;
                }
            }

            var ans = 1L;
            foreach (var m in exp.Split(" * "))
                ans *= m.Split(" + ").Select(long.Parse).Aggregate(0L, (o, v) => o + v);

            return ans;
        }
    }
}