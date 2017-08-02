using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2015
{
    internal class Day15 : BaseDay
    {
        public override void RunPartOne()
        {
            Console.WriteLine(GetHighestScore("practice"));
            Console.WriteLine(GetHighestScore());
        }

        private int GetHighestScore(int calorieCount)
        {
            return GetHighestScore(null, calorieCount);
        }

        private int GetHighestScore(string fileVariant = null, int calorieCount = 0)
        {
            var ingredients = GetIngredientProperties(fileVariant);

            var maxScore = 0;
            foreach (var ingredientSplit in DivideIngredients(ingredients.GetLength(0), 100))
            {
                if (calorieCount > 0)
                {
                    var currentCalories = ingredientSplit
                        .Select((value, i) => new {i, value})
                        .Sum(split => split.value * ingredients[split.i, 4]);

                    if (currentCalories != calorieCount) continue;
                }

                var score = 1;
                for (var j = 0; j < 4; j++)
                {
                    var property = ingredientSplit
                        .Select((value, i) => new {i, value})
                        .Sum(split => split.value * ingredients[split.i, j]);
                    score *= Math.Max(0, property);
                }
                maxScore = Math.Max(maxScore, score);
            }
            return maxScore;
        }

        private IEnumerable<IEnumerable<int>> DivideIngredients(int ingredientsLength, int numberOfTeaspoons)
        {
            if (ingredientsLength == 1)
                yield return new List<int> {numberOfTeaspoons};
            else
                for (var i = 0; i <= numberOfTeaspoons; i++)
                    foreach (var j in DivideIngredients(ingredientsLength - 1, numberOfTeaspoons - i))
                        yield return new List<int> {i}.Concat(j);
        }

        private int[,] GetIngredientProperties(string fileVariant)
        {
            var input = LoadInput(fileVariant);

            var ingredients = new int[input.Length, 5];
            for (var i = 0; i < input.Length; i++)
            {
                var line = input[i];
                var split = line.Split(new[] {": capacity ", ", durability ", ", flavor ", ", texture ", ", calories "},
                    StringSplitOptions.None);

                for (var j = 0; j < 5; j++)
                    ingredients[i, j] = int.Parse(split[j + 1]);
            }
            return ingredients;
        }

        public override void RunPartTwo()
        {
            Console.WriteLine(GetHighestScore("practice", 500));
            Console.WriteLine(GetHighestScore(500));
        }
    }
}