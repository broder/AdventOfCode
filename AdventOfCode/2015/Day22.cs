using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2015
{
    internal class Day22 : BaseDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(GetMinimumManaSpend(new Player {HitPoints = 10, Mana = 250},
                new Boss {HitPoints = 13, Damage = 8}));
            Console.WriteLine(GetMinimumManaSpend(new Player {HitPoints = 10, Mana = 250},
                new Boss {HitPoints = 14, Damage = 8}));
            Console.WriteLine(GetMinimumManaSpend(new Player {HitPoints = 50, Mana = 500},
                new Boss {HitPoints = 58, Damage = 9}));
        }

        private int GetMinimumManaSpend(Player player, Boss boss, bool hard = false)
        {
            var spells = new List<Spell>
            {
                new Spell {Name = "Magic Missile", ManaCost = 53, Damage = 4},
                new Spell {Name = "Drain", ManaCost = 73, Damage = 2, Heal = 2},
                new Spell {Name = "Shield", ManaCost = 113, TurnsLeft = 6, Armour = 7},
                new Spell {Name = "Poison", ManaCost = 173, TurnsLeft = 6, DamageTick = 3},
                new Spell {Name = "Recharge", ManaCost = 229, TurnsLeft = 5, ManaTick = 101}
            };

            var states = new Queue<State>();
            states.Enqueue(new State
            {
                TotalManaCost = 0,
                PlayersTurn = true,
                Player = player,
                Boss = boss,
                ActiveSpells = new List<Spell>()
            });
            while (states.Count > 0)
            {
                var currentState = states.Dequeue();
                var totalManaCost = currentState.TotalManaCost;
                var playersTurn = currentState.PlayersTurn;
                var currentPlayer = currentState.Player;
                var currentBoss = currentState.Boss;
                var activeSpells = currentState.ActiveSpells;

                if (hard)
                {
                    currentPlayer.HitPoints--;
                    if (currentPlayer.HitPoints <= 0)
                        continue;
                }

                for (var i = 0; i < activeSpells.Count; i++)
                {
                    var spell = activeSpells[i];
                    currentPlayer.Mana += spell.ManaTick;
                    currentBoss.HitPoints -= spell.DamageTick;

                    spell.TurnsLeft = spell.TurnsLeft -= 1;

                    activeSpells.RemoveAt(i);
                    if (spell.TurnsLeft > 0)
                    {
                        activeSpells.Insert(i, spell);
                    }
                    else
                    {
                        currentPlayer.Armour -= spell.Armour;
                        i--;
                    }
                }
                if (currentBoss.HitPoints <= 0)
                    return totalManaCost;

                if (playersTurn)
                {
                    foreach (var spell in spells)
                    {
                        if (currentPlayer.Mana < spell.ManaCost || activeSpells.Any(s => s.Equals(spell))) continue;
                        var nextTotalManaCost = totalManaCost + spell.ManaCost;
                        var nextPlayer = currentPlayer.Clone();
                        nextPlayer.Mana -= spell.ManaCost;
                        var nextBoss = currentBoss.Clone();
                        var nextActiveSpells = new List<Spell>(activeSpells);

                        if (spell.TurnsLeft == 0)
                        {
                            nextBoss.HitPoints -= spell.Damage;
                            nextPlayer.HitPoints += spell.Heal;
                            if (nextBoss.HitPoints <= 0)
                                return nextTotalManaCost;
                        }
                        else
                        {
                            nextPlayer.Armour += spell.Armour;
                            nextActiveSpells.Add(spell);
                        }

                        states.Enqueue(new State
                        {
                            TotalManaCost = nextTotalManaCost,
                            PlayersTurn = false,
                            Player = nextPlayer,
                            Boss = nextBoss,
                            ActiveSpells = nextActiveSpells
                        });
                    }
                }
                else
                {
                    currentPlayer.HitPoints -= currentBoss.Damage - currentPlayer.Armour;
                    if (currentPlayer.HitPoints <= 0)
                        continue;

                    states.Enqueue(new State
                    {
                        TotalManaCost = totalManaCost,
                        PlayersTurn = true,
                        Player = currentPlayer,
                        Boss = currentBoss,
                        ActiveSpells = activeSpells
                    });
                }
            }
            return -1;
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(GetMinimumManaSpend(new Player {HitPoints = 50, Mana = 500},
                new Boss {HitPoints = 58, Damage = 9}, true));
        }

        private struct State
        {
            public int TotalManaCost;
            public bool PlayersTurn;
            public Player Player;
            public Boss Boss;
            public List<Spell> ActiveSpells;
        }

        private struct Player
        {
            public int HitPoints;
            public int Mana;
            public int Armour;

            public Player Clone()
            {
                return new Player
                {
                    HitPoints = HitPoints,
                    Mana = Mana,
                    Armour = Armour
                };
            }
        }

        private struct Boss
        {
            public int HitPoints;
            public int Damage;

            public Boss Clone()
            {
                return new Boss {HitPoints = HitPoints, Damage = Damage};
            }
        }

        private struct Spell
        {
            public string Name;
            public int TurnsLeft;
            public int ManaCost;
            public int Damage;
            public int Heal;
            public int Armour;
            public int ManaTick;
            public int DamageTick;

            public bool Equals(Spell s)
            {
                return s.Name == Name;
            }
        }
    }
}