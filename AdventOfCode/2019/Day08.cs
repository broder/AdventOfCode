using System;
using System.Linq;

namespace AdventOfCode._2019
{
    internal class Day08 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(GetChecksum(LoadInput("practice.1").First().Select(c => c - '0').ToArray(), 3, 2));
            Console.WriteLine(GetChecksum(LoadInput().First().Select(c => c - '0').ToArray(), 25, 6));
        }

        private static int GetChecksum(int[] pixels, int width, int height)
        {
            var imageSize = width * height;
            var layerCount = pixels.Length / imageSize;

            var minZeroes = imageSize;
            var checksum = 0;

            for (var layer = 0; layer < layerCount; layer++)
            {
                var offset = layer * imageSize;
                var zeroCount = Enumerable.Range(offset, imageSize).Count(i => pixels[i] == 0);

                if (zeroCount >= minZeroes) continue;

                minZeroes = zeroCount;
                
                var oneCount = Enumerable.Range(offset, imageSize).Count(i => pixels[i] == 1);
                var twoCount = Enumerable.Range(offset, imageSize).Count(i => pixels[i] == 2);
                checksum = oneCount * twoCount;
            }

            return checksum;
        }

        protected override void RunPartTwo()
        {
            PrintImage(LoadInput("practice.2").First().Select(c => c - '0').ToArray(), 2, 2);
            PrintImage(LoadInput().First().Select(c => c - '0').ToArray(), 25, 6);
        }
        
        private static void PrintImage(int[] pixels, int width, int height)
        {
            var imageSize = width * height;
            var layerCount = pixels.Length / imageSize;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    for (var layer = 0; layer < layerCount; layer++)
                    {
                        var pixel = pixels[layer * imageSize + y * width + x];
                        switch (pixel)
                        {
                            case 0:
                                Console.Write(" ");
                                layer = layerCount;
                                break;
                            case 1:
                                Console.Write("0");
                                layer = layerCount;
                                break;
                            case 2:
                                continue;
                        }
                    }
                }
                Console.WriteLine();
            }
        }
    }
}