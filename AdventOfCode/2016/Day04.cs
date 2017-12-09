using System;
using System.Linq;
using System.Text;

namespace AdventOfCode._2016
{
    internal class Day04 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(new Room("aaaaa-bbb-z-y-x-123[abxyz]").IsRealRoom());
            Console.WriteLine(new Room("a-b-c-d-e-f-g-h-987[abcde]").IsRealRoom());
            Console.WriteLine(new Room("not-a-real-room-404[oarel]").IsRealRoom());
            Console.WriteLine(new Room("totally-real-room-200[decoy]").IsRealRoom());
            Console.WriteLine(SumRealRoomSectorIdsFromFile());
        }

        private int SumRealRoomSectorIdsFromFile()
        {
            return LoadInput().Select(s => new Room(s)).Where(r => r.IsRealRoom()).Sum(r => r.GetSectorId());
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(new Room("qzmt-zixmtkozy-ivhz-343[zimth]").GetDecryptedName());
            Console.WriteLine(GetDecryptedNamesFromFile());
        }

        private string GetDecryptedNamesFromFile()
        {
            return string.Join("\n", LoadInput()
                .Select(s => new Room(s))
                .Where(r => r.IsRealRoom())
                .Select(r => $"{r.GetSectorId()}: {r.GetDecryptedName()}"));
        }

        private class Room
        {

            private readonly string _encryptedName;
            private readonly int _sectorId;
            private readonly string _checksum;

            public Room(string instruction)
            {
                var sectorIdStart = instruction.LastIndexOf('-');
                var checksumStart = instruction.LastIndexOf('[');
                _encryptedName = instruction.Substring(0, sectorIdStart);
                _sectorId = int.Parse(instruction.Substring(sectorIdStart, checksumStart - sectorIdStart).Trim('-'));
                _checksum = instruction.Substring(checksumStart).Trim('[', ']');
            }

            public bool IsRealRoom()
            {
                var computedCheckSum = Alphabet
                    .Select(c => new {Char = c, Count = _encryptedName.Count(i => i == c)})
                    .OrderByDescending(value => value.Count)
                    .ThenBy(value => value.Char)
                    .Take(5)
                    .Aggregate(new StringBuilder(), (sb, value) => sb.Append(value.Char))
                    .ToString();

                return computedCheckSum == _checksum;
            }

            public string GetDecryptedName()
            {
                var output = new StringBuilder();
                foreach (var letter in _encryptedName)
                {
                    output.Append(letter == '-' ? ' ' : Alphabet[Mod(letter - 97 + _sectorId, 26)]);
                }
                return output.ToString();
            }

            public int GetSectorId()
            {
                return _sectorId;
            }
        }
    }
}