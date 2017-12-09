using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdventOfCode._2015
{
    internal class Day12 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(SumJson("[1,2,3]"));
            Console.WriteLine(SumJson("{\"a\":2,\"b\":4}"));
            Console.WriteLine(SumJson("[[[3]]]"));
            Console.WriteLine(SumJson("{\"a\":{\"b\":4},\"c\":-1}"));
            Console.WriteLine(SumJson("{\"a\":[-1,1]}"));
            Console.WriteLine(SumJson("[-1,{\"a\":1}]"));
            Console.WriteLine(SumJson("[]"));
            Console.WriteLine(SumJson("{}"));
            Console.WriteLine(SumJsonFromFile());
        }

        private long SumJsonFromFile(string ignore = null)
        {
            return LoadInput().Sum(line => SumJson(line, ignore));
        }

        private long SumJson(string jsonString, string ignore = null)
        {
            return GetSum((dynamic) JsonConvert.DeserializeObject(jsonString), ignore);
        }

        private long GetSum(JObject obj, string ignore = null)
        {
            return obj.Properties().Select(p => p.Value).OfType<JValue>().Select(v => v.Value).Contains(ignore)
                ? 0L
                : obj.Properties().Sum((dynamic a) => (long) GetSum(a.Value, ignore));
        }

        private long GetSum(JArray arr, string ignore = null)
        {
            return arr.Sum((dynamic a) => (long) GetSum(a, ignore));
        }

        private long GetSum(JValue val, string ignore = null)
        {
            return val.Type == JTokenType.Integer ? (long) val.Value : 0L;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(SumJson("[1,2,3]", "red"));
            Console.WriteLine(SumJson("[1,{\"c\":\"red\",\"b\":2},3]", "red"));
            Console.WriteLine(SumJson("{\"d\":\"red\",\"e\":[1,2,3,4],\"f\":5}", "red"));
            Console.WriteLine(SumJson("[1,\"red\",5]", "red"));
            Console.WriteLine(SumJsonFromFile("red"));
        }
    }
}