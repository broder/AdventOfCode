using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2020
{
    internal class Day21 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(CountSafeIngredients(LoadInput("practice")));
            Console.WriteLine(CountSafeIngredients(LoadInput()));
        }

        private static int CountSafeIngredients(string[] input)
        {
            var (foods, allergenIngredients) = ParseFoods(input);
            var unsafeIngredients = allergenIngredients.Values.SelectMany(i => i).ToHashSet();
            return foods.Sum(f => f.ingredients.Count(i => !unsafeIngredients.Contains(i)));
        }

        private static (List<(HashSet<string> ingredients, HashSet<string> allergens)> foods, Dictionary<string, HashSet<string>> allergenIngredients) ParseFoods(string[] input)
        {
            var foods = new List<(HashSet<string> ingredients, HashSet<string> allergens)>();
            var possibleAllergenIngredients = new Dictionary<string, HashSet<string>>();
            foreach (var line in input)
            {
                var s = line.Split(new[] { " (contains ", ", ", ")" }, StringSplitOptions.RemoveEmptyEntries);
                var f = (ingredients: new HashSet<string>(s[0].Split(' ')), allergens: new HashSet<string>(s.Skip(1)));
                foods.Add(f);

                foreach (var a in f.allergens)
                {
                    if (!possibleAllergenIngredients.ContainsKey(a))
                        possibleAllergenIngredients[a] = new HashSet<string>(f.ingredients);
                    else
                        possibleAllergenIngredients[a].IntersectWith(f.ingredients);
                }
            }

            while (possibleAllergenIngredients.Any(kvp => kvp.Value.Count() > 1))
            {
                foreach (var confirmedAllergen in possibleAllergenIngredients.Where(kvp => kvp.Value.Count() == 1))
                {
                    foreach (var possibleAllergen in possibleAllergenIngredients)
                    {
                        if (confirmedAllergen.Key == possibleAllergen.Key) continue;
                        possibleAllergen.Value.Remove(confirmedAllergen.Value.Single());
                    }
                }
            }

            return (foods, possibleAllergenIngredients);
        }

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetDangerousIngredients(LoadInput("practice")));
            Console.WriteLine(GetDangerousIngredients(LoadInput()));
        }

        private static string GetDangerousIngredients(string[] input) => string.Join(',', ParseFoods(input).allergenIngredients.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value.Single()));
    }
}