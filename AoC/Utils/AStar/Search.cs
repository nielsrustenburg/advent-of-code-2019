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
            expanded.Add(rootNode, rootNode.LBCost);

            var prioQueue = new T();
            prioQueue.Enqueue(rootNode);

            while (!prioQueue.IsEmpty())
            {
                var currentNode = prioQueue.Dequeue();
                if (currentNode.IsAtTarget()) return currentNode;

                var haveNotFoundCheaper = expanded[currentNode] >= currentNode.LBCost;
                if (haveNotFoundCheaper)
                {
                    var neighbours = currentNode.GetNeighbours();
                    foreach (var neighbour in neighbours)
                    {
                        int bestKnownCost;
                        if (!expanded.TryGetValue(neighbour, out bestKnownCost) || bestKnownCost > neighbour.LBCost)
                        {
                            prioQueue.Enqueue(neighbour);
                            expanded[neighbour] = neighbour.LBCost;
                        }
                    }
                }
            }
            return null;
        }
    }
}
