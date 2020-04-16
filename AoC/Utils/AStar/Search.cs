using System;
using System.Collections.Generic;
using System.Text;

namespace AoC.Utils.AStar
{
    public static class Search<T> where T : IPriorityQueue<SearchNode>, new()
    {
        public static SearchNode Execute(SearchNode rootNode)
        {
            var expanded = new Dictionary<SearchNode, int>();
            expanded.Add(rootNode, rootNode.LowerBoundCost);

            var prioQueue = new T();
            prioQueue.Enqueue(rootNode);

            while (!prioQueue.IsEmpty())
            {
                var currentNode = prioQueue.Dequeue();
                if (currentNode.IsAtTarget()) return currentNode;

                var haveNotFoundCheaper = expanded[currentNode] >= currentNode.LowerBoundCost;
                if (haveNotFoundCheaper)
                {
                    var neighbours = currentNode.GetNeighbours();
                    foreach (var neighbour in neighbours)
                    {
                        int bestKnownCost;
                        var alreadyFound = expanded.TryGetValue(neighbour, out bestKnownCost);
                        if (!alreadyFound || bestKnownCost > neighbour.LowerBoundCost)
                        {
                            prioQueue.Enqueue(neighbour);
                            expanded[neighbour] = neighbour.LowerBoundCost;
                        }
                    }
                }
            }
            return null;
        }
    }
}
