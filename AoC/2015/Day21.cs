using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC._2015
{
    internal class Day21 : BaseDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(PlayerWins(new Person {HitPoints = 8, Damage = 5, Armour = 5},
                new Person {HitPoints = 12, Damage = 7, Armour = 2}));
            Console.WriteLine(GetMinimumGoldSpend());
        }

        private readonly Regex ItemRegex = new Regex(@"(.*?) [\s]+ ([\d]+)[\s]+([\d]+)[\s]+([\d]+)");

        private int GetMinimumGoldSpend(bool dodgyShop = false)
        {
            var player = new Person {HitPoints = 100};
            var boss = LoadBoss();

            var emptyItem = new Item();
            var weaponItems = new List<Item>();
            var armourItems = new List<Item> {emptyItem};
            var ringItems = new List<Item> {emptyItem};
            var current = weaponItems;

            foreach (var line in LoadInput("shop"))
            {
                if (line.StartsWith("Weapons"))
                {
                    current = weaponItems;
                    continue;
                }
                if (line.StartsWith("Armor"))
                {
                    current = armourItems;
                    continue;
                }
                if (line.StartsWith("Rings"))
                {
                    current = ringItems;
                    continue;
                }

                var item = new Item();
                if (!ItemRegex.IsMatch(line)) continue;
                var m = ItemRegex.Match(line);
                item.Cost = int.Parse(m.Groups[2].Value);
                item.DamageModifier = int.Parse(m.Groups[3].Value);
                item.ArmourModifier = int.Parse(m.Groups[4].Value);
                current.Add(item);
            }

            var winningCosts = new List<int>();
            var losingCosts = new List<int>();
            foreach (var weapon in weaponItems)
            {
                foreach (var armour in armourItems)
                {
                    foreach (var firstRing in ringItems)
                    {
                        foreach (var secondRing in ringItems)
                        {
                            if (firstRing.Equals(secondRing)) continue;
                            var currentPlayer = player.Clone();
                            currentPlayer.Equip(weapon);
                            currentPlayer.Equip(armour);
                            currentPlayer.Equip(firstRing);
                            currentPlayer.Equip(secondRing);
                            var currentBoss = boss.Clone();

                            if (PlayerWins(currentPlayer, currentBoss))
                                winningCosts.Add(weapon.Cost + armour.Cost + firstRing.Cost + secondRing.Cost);
                            else
                                losingCosts.Add(weapon.Cost + armour.Cost + firstRing.Cost + secondRing.Cost);
                        }
                    }
                }
            }
            return dodgyShop ? losingCosts.Max() : winningCosts.Min();
        }

        private Person LoadBoss()
        {
            var enemy = new Person();
            foreach (var line in LoadInput())
            {
                var value = int.Parse(line.Split(new[] {": "}, StringSplitOptions.None)[1]);
                if (line.StartsWith("H"))
                    enemy.HitPoints = value;
                else if (line.StartsWith("D"))
                    enemy.Damage = value;
                else
                    enemy.Armour = value;
            }
            return enemy;
        }

        private bool PlayerWins(Person player, Person enemy)
        {
            var playersTurn = true;
            while (player.HitPoints > 0 && enemy.HitPoints > 0)
            {
                if (playersTurn)
                    enemy.HitPoints -= player.Damage - enemy.Armour;
                else
                    player.HitPoints -= enemy.Damage - player.Armour;
                playersTurn = !playersTurn;
            }
            return player.HitPoints > 0;
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(GetMinimumGoldSpend(true));
        }

        private struct Person
        {
            public int HitPoints;
            public int Damage;
            public int Armour;

            public Person Clone()
            {
                return new Person {HitPoints = HitPoints, Damage = Damage, Armour = Armour};
            }

            public void Equip(Item i)
            {
                Damage += i.DamageModifier;
                Armour += i.ArmourModifier;
            }
        }

        private struct Item
        {
            public int Cost;
            public int DamageModifier;
            public int ArmourModifier;

            public bool Equals(Item i)
            {
                return Cost != 0 && Cost == i.Cost && DamageModifier == i.DamageModifier &&
                       ArmourModifier == i.ArmourModifier;
            }
        }
    }
}