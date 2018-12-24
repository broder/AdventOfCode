using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018
{
    internal class Day24 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetResult(LoadInput("practice")).armySize);
            Console.WriteLine(GetResult(LoadInput()).armySize);
        }

        private static (bool immuneWin, int armySize) GetResult(IEnumerable<string> input, int immuneBoost = 0)
        {
            var (immuneArmy, infectionArmy) = ParseInput(input, immuneBoost);

            while (true)
            {
                var aliveImmuneArmy = immuneArmy.Where(g => g.Units > 0).ToList();
                var aliveInfectionArmy = infectionArmy.Where(g => g.Units > 0).ToList();
                var aliveGroups = aliveImmuneArmy.Union(aliveInfectionArmy).ToList();
                if (aliveImmuneArmy.Count == 0 || aliveInfectionArmy.Count == 0)
                    break;

                // target phase
                foreach (var army in new[] {aliveInfectionArmy, aliveImmuneArmy})
                {
                    foreach (var attacker in army.OrderByDescending(g => g.EffectivePower)
                        .ThenByDescending(g => g.Initiative))
                    {
                        attacker.Target = null;
                        attacker.Attacker = null;

                        Group target = null;
                        var damage = int.MinValue;
                        foreach (var potentialTarget in aliveGroups
                            .Where(g => g.ImmuneArmy != attacker.ImmuneArmy)
                            .Where(g => g.Attacker == null))
                        {
                            var potentialDamage = GetAttackDamage(attacker, potentialTarget);
                            if (potentialDamage > damage
                                || potentialDamage == damage
                                && potentialTarget.EffectivePower > target?.EffectivePower
                                || (potentialTarget.EffectivePower == target?.EffectivePower
                                    && potentialTarget.Initiative > target?.Initiative))
                            {
                                target = potentialTarget;
                                damage = potentialDamage;
                            }
                        }

                        if (target == null || damage == 0) continue;

                        attacker.Target = target;
                        target.Attacker = attacker;
                    }
                }

                // attack
                var totalKilledUnits = 0;
                foreach (var attacker in aliveGroups.OrderByDescending(g => g.Initiative))
                {
                    var target = attacker.Target;

                    if (target == null) continue;

                    var attackDamage = GetAttackDamage(attacker, target);

                    var killedUnits = attackDamage / target.UnitHitPoints;
                    target.Units = Math.Max(target.Units - killedUnits, 0);
                    totalKilledUnits += killedUnits;
                }

                // stalemate
                if (totalKilledUnits == 0)
                    break;

            }

            var infectionUnits = infectionArmy.Sum(g => g.Units);
            var immuneUnits = immuneArmy.Sum(g => g.Units);
            return (infectionUnits == 0, infectionUnits + immuneUnits);
        }

        private static (List<Group> immuneArmy, List<Group> infectionArmy) ParseInput(IEnumerable<string> input,
            int immuneBoost = 0)
        {
            var immuneArmy = new List<Group>();
            var infectionArmy = new List<Group>();

            var currentList = immuneArmy;
            foreach (var line in input)
            {
                if (line == "") continue;

                if (line.StartsWith("Immune"))
                {
                    currentList = immuneArmy;
                    continue;
                }

                if (line.StartsWith("Infection"))
                {
                    currentList = infectionArmy;
                    continue;
                }

                var split = line.Split(
                    new[]
                    {
                        " units each with ", " hit points ", "(", ") ", "with an attack that does ",
                        " damage at initiative "
                    }, StringSplitOptions.RemoveEmptyEntries);
                var group = new Group();
                group.ImmuneArmy = currentList == immuneArmy;
                group.Units = int.Parse(split[0]);
                group.UnitHitPoints = int.Parse(split[1]);

                if (split.Length == 5)
                {
                    foreach (var buff in split[2].Split("; "))
                    {
                        if (buff.StartsWith("immune"))
                        {
                            var buffSplit = buff.Split(new[] {"immune to ", ", "},
                                StringSplitOptions.RemoveEmptyEntries);
                            foreach (var b in buffSplit)
                                group.Immunities.Add(b);
                        }
                        else
                        {
                            var buffSplit = buff.Split(new[] {"weak to ", ", "}, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var b in buffSplit)
                                group.Weaknesses.Add(b);
                        }
                    }
                }

                var attackSplit = split[split.Length - 2].Split(' ');
                group.AttackDamage = int.Parse(attackSplit[0]);
                if (group.ImmuneArmy)
                    group.AttackDamage += immuneBoost;
                group.AttackType = attackSplit[1];

                group.Initiative = int.Parse(split[split.Length - 1]);

                currentList.Add(group);
            }

            return (immuneArmy, infectionArmy);
        }

        private static int GetAttackDamage(Group attacker, Group target)
        {
            if (target.Immunities.Contains(attacker.AttackType))
                return 0;

            var damage = attacker.EffectivePower;

            if (target.Weaknesses.Contains(attacker.AttackType))
                damage *= 2;

            return damage;
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetResult(LoadInput("practice"), 1570).armySize);
            Console.WriteLine(GetMinimalBoostArmySize(LoadInput()));
        }

        private static int GetMinimalBoostArmySize(string[] input)
        {
            int startBoost, endBoost;
            for (var boost = 1;; boost *= 2)
            {
                var (immuneWin, _) = GetResult(input, boost);
                if (!immuneWin) continue;
                startBoost = boost / 2;
                endBoost = boost;
                break;
            }

            while (startBoost != endBoost)
            {
                var midBoost = (startBoost + endBoost) / 2;
                var (immuneWin, _) = GetResult(input, midBoost);

                if (immuneWin)
                    endBoost = midBoost;
                else
                    startBoost = midBoost + 1;
            }

            return GetResult(input, endBoost).armySize;
        }


        private class Group
        {
            public bool ImmuneArmy;
            public int Units;
            public int UnitHitPoints;
            public readonly HashSet<string> Weaknesses = new HashSet<string>();
            public readonly HashSet<string> Immunities = new HashSet<string>();
            public int AttackDamage;
            public string AttackType;
            public int Initiative;

            public int EffectivePower => Units * AttackDamage;

            public Group Target;
            public Group Attacker;
        }
    }
}