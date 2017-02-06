using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC._2016
{
    internal class Day07 : BaseDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(IsTlsIp("abba[mnop]qrst"));
            Console.WriteLine(IsTlsIp("abcd[bddb]xyyx"));
            Console.WriteLine(IsTlsIp("aaaa[qwer]tyui"));
            Console.WriteLine(IsTlsIp("ioxxoj[asdfgh]zxcvbn"));
            Console.WriteLine(CountTlsIps());
        }

        private int CountTlsIps()
        {
            return LoadInput().Count(IsTlsIp);
        }

        private bool IsTlsIp(string ipString)
        {
            var ip = new IP(ipString);
            var abbas = Alphabet.Select(c1 => Alphabet
                    .Where(c2 => c1 != c2)
                    .Select(c2 => $"{c1}{c2}{c2}{c1}"))
                .SelectMany(t => t);
            return ip.Hypernets.Any(s => abbas.Any(s.Contains)) &&
                   !ip.Supernets.Any(s => abbas.Any(s.Contains));
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(IsSslIp("aba[bab]xyz"));
            Console.WriteLine(IsSslIp("xyx[xyx]xyx"));
            Console.WriteLine(IsSslIp("aaa[kek]eke"));
            Console.WriteLine(IsSslIp("zazbz[bzb]cdb"));
            Console.WriteLine(CountSslIps());
        }

        private int CountSslIps()
        {
            return LoadInput().Count(IsSslIp);
        }

        private bool IsSslIp(string ipString)
        {
            var ip = new IP(ipString);
            var abas = Alphabet.Select(c1 => Alphabet
                    .Where(c2 => c1 != c2)
                    .Select(c2 => new Tuple<string, string>($"{c1}{c2}{c1}", $"{c2}{c1}{c2}")))
                .SelectMany(t => t);
            return abas.Any(t => ip.Supernets.Any(s => s.Contains(t.Item1)) && ip.Hypernets.Any(s => s.Contains(t.Item2)));
        }

        private class IP
        {
            public List<string> Hypernets = new List<string>();
            public List<string> Supernets = new List<string>();

            public IP(string ip)
            {
                var splitIp = ip.Split('[', ']');
                for (var i = 0; i < splitIp.Length; i++)
                {
                    var s = splitIp[i];
                    if (i % 2 == 0)
                        Hypernets.Add(s);
                    else
                        Supernets.Add(s);
                }
            }
        }
    }
}