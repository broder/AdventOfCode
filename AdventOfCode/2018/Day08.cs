using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018
{
    internal class Day08 : BaseDay
    {
        protected override void RunPartOne()
        {
            Console.WriteLine(SumMetadata(LoadInput("practice").First()));
            Console.WriteLine(SumMetadata(LoadInput().First()));
        }

        private static int SumMetadata(string treeDefinition)
        {
            var (root, _) = CreateNode(treeDefinition.Split(' ').Select(int.Parse).ToArray(), 0);
            return GetNodeMetadataSum(root);
        }

        private static (Node node, int currentIndex) CreateNode(IReadOnlyList<int> treeDefinition, int startIndex)
        {
            var node = new Node();

            var childCount = treeDefinition[startIndex];
            var metadataCount = treeDefinition[startIndex + 1];
            var currentIndex = startIndex + 1;

            node.Children = new Node[childCount];
            for (var i = 0; i < childCount; i++)
            {
                var (child, nextIndex) = CreateNode(treeDefinition, currentIndex + 1);
                node.Children[i] = child;
                currentIndex = nextIndex;
            }

            for (var i = 0; i < metadataCount; i++)
            {
                currentIndex++;
                node.Metadata.Add(treeDefinition[currentIndex]);
            }

            return (node, currentIndex);
        }

        private static int GetNodeMetadataSum(Node node) =>
            node.Metadata.Sum() + node.Children.Select(GetNodeMetadataSum).Sum();

        protected override void RunPartTwo()
        {
            Console.WriteLine(GetRootValue(LoadInput("practice").First()));
            Console.WriteLine(GetRootValue(LoadInput().First()));
        }

        private static int GetRootValue(string treeDefinition)
        {
            var (root, _) = CreateNode(treeDefinition.Split(' ').Select(int.Parse).ToArray(), 0);
            return GetNodeValue(root);
        }

        private static int GetNodeValue(Node node) => node.Children.Length == 0
            ? node.Metadata.Sum()
            : node.Metadata.Where(m => m >= 1 && m <= node.Children.Length)
                .Select(m => GetNodeValue(node.Children[m - 1])).Sum();

        private class Node
        {
            public Node[] Children;
            public readonly List<int> Metadata = new List<int>();
        }
    }
}