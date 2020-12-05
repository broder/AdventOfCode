using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2020
{
    internal class Day04 : BaseDay
    {
        private static String[] VALID_CREDENTIAL_KEYS = new[] {
            "byr",
            "iyr",
            "eyr",
            "hgt",
            "hcl",
            "ecl",
            "pid",
        };

        private static String[] VALID_EYE_COLOURS = new[] {
            "amb",
            "blu",
            "brn",
            "gry",
            "grn",
            "hzl",
            "oth",
        };

        protected override void RunPartOne()
        {
            Console.WriteLine(GetCredentials(LoadInput("practice.1")).Count(HasValidPassportKeys));
            Console.WriteLine(GetCredentials(LoadInput()).Count(HasValidPassportKeys));
        }

        private static IEnumerable<Dictionary<String, String>> GetCredentials(string[] input)
        {
            var currentCredentials = new Dictionary<String, String>();
            foreach (var line in input)
            {
                if (line.Length == 0)
                {
                    yield return currentCredentials;
                    currentCredentials = new Dictionary<String, String>();
                    continue;
                }

                foreach (var cred in line.Split(' '))
                {
                    var splitCred = cred.Split(':');
                    currentCredentials.Add(splitCred[0], splitCred[1]);
                }
            }
            if (currentCredentials.Count > 0)
                yield return currentCredentials;
        }

        private static bool HasValidPassportKeys(Dictionary<String, String> credentials)
        {
            return credentials.Keys.ToHashSet().IsSupersetOf(VALID_CREDENTIAL_KEYS);
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetCredentials(LoadInput("practice.2")).Count(IsValidPassport));
            Console.WriteLine(GetCredentials(LoadInput("practice.3")).Count(IsValidPassport));
            Console.WriteLine(GetCredentials(LoadInput()).Count(IsValidPassport));
        }

        private static bool IsValidPassport(Dictionary<String, String> credentials)
        {
            if (!HasValidPassportKeys(credentials))
                return false;

            try
            {
                var birthYear = int.Parse(credentials["byr"]);
                if (birthYear < 1920 || birthYear > 2002)
                    return false;

                var issueYear = int.Parse(credentials["iyr"]);
                if (issueYear < 2010 || issueYear > 2020)
                    return false;

                var expirationYear = int.Parse(credentials["eyr"]);
                if (expirationYear < 2020 || expirationYear > 2030)
                    return false;

                var height = credentials["hgt"];
                if (height.Length == 5)
                {
                    if (height.Substring(3) != "cm")
                        return false;
                    var heightInCm = int.Parse(height.Substring(0, 3));
                    if (heightInCm < 150 || heightInCm > 193)
                        return false;
                }
                else if (height.Length == 4)
                {
                    if (height.Substring(2) != "in")
                        return false;
                    var heightInInches = int.Parse(height.Substring(0, 2));
                    if (heightInInches < 59 || heightInInches > 76)
                        return false;
                }
                else
                {
                    return false;
                }

                var hairColour = credentials["hcl"];
                if (hairColour.Length != 7 || hairColour[0] != '#' || !hairColour.Skip(1).ToHashSet().IsSubsetOf(HexChars.ToList()))
                    return false;

                var eyeColour = credentials["ecl"];
                if (!VALID_EYE_COLOURS.Contains(eyeColour))
                    return false;

                var passportId = credentials["pid"];
                if (passportId.Length != 9 || !int.TryParse(passportId, out var pid))
                    return false;

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}