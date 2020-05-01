using System;
using System.Collections.Generic;
using System.Text;
using AoC.Utils.AStar;

namespace AoC.Utils
{
    class BasicGraph : AdjacencyGraph<BasicGraphNode>
    {
        Dictionary<string, Dictionary<string, int>> distanceMatrix;
        public BasicGraph() : base()
        {
            distanceMatrix = new Dictionary<string, Dictionary<string, int>>();
        }

        public void AddNode(string name)
        {
            nodes.Add(name, new BasicGraphNode(name));
        }

        public int GetDistance(string from, string to)
        {
            if (!distanceMatrix.TryGetValue(from, out var nextDict))
            {
                distanceMatrix[from] = new Dictionary<string, int>();
            }
            if (!distanceMatrix[from].TryGetValue(to, out var distance))
            {
                var (_, dist) = GraphSearch.Dijkstra(this, from, to);
                distanceMatrix[from][to] = dist;
                return dist;
            }
            else
            {
                return distance;
            }
        }
    }

    class BasicGraphNode : AdjacencyGraphNode
    {
        public BasicGraphNode(string name) : base(name) { }
    }
}
