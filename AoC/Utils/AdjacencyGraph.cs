using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoC.Utils.AStar;

namespace AoC.Utils
{
    abstract class AdjacencyGraph<N> : IInspectableGraph, ISearchableGraph
        where N : IGraphNode
    {
        protected Dictionary<string, N> nodes;

        protected AdjacencyGraph()
        {
            nodes = new Dictionary<string, N>();
        }

        public bool ContainsEdge(string from, string to)
        {
            return nodes[from].HasNeighbour(to);
        }

        public bool ContainsNode(string name)
        {
            return nodes.ContainsKey(name);
        }

        public void AddEdge(string from ,string to, int dist)
        {
            nodes[from].AddNeighbour(to, dist);
            nodes[to].AddNeighbour(from, dist);
        }

        public IEnumerable<(string from, string to, int weight)> Edges()
        {
            //Let C# decide some arbitrary order, use it to avoid returning duplicate edges
            Dictionary<string,int> orderedNodes = nodes.Select((kvp, i) => (kvp, i)).ToDictionary(a => a.kvp.Key, b => b.i); 

            foreach (var kvp in nodes)
            {
                foreach (var (nb, weight) in kvp.Value.Neighbours())
                {
                    if (orderedNodes[kvp.Key] < orderedNodes[nb])
                    {
                        yield return (kvp.Key, nb, weight);
                    }
                }
            }
        }

        public IEnumerable<(string nb, int weight)> Neighbours(string name)
        {
            return nodes[name].Neighbours();
        }

        public IEnumerable<string> Nodes()
        {
            return nodes.Keys;
        }

        public IEnumerable<(string node, int weight)> GetReachableNodes(string from)
        {
            return Neighbours(from);
        }
    }

    abstract class AdjacencyGraphNode : IGraphNode
    {
        public string Name { get; }
        Dictionary<string, int> neighbours;

        protected AdjacencyGraphNode(string name)
        {
            Name = name;
            neighbours = new Dictionary<string, int>();
        }

        public void AddNeighbour(string name, int weight)
        {
            neighbours.TryAdd(name, weight);
        }

        public bool HasNeighbour(string from)
        {
            return neighbours.ContainsKey(from);
        }

        public IEnumerable<(string nb, int weight)> Neighbours()
        {
            return neighbours.Select(kvp => (kvp.Key, kvp.Value));
        }
    }
}
