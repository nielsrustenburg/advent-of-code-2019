using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils.AStar;
using System.Text;

namespace AoC.Utils
{
    abstract class AdjacencyDiGraph<N> : IInspectableDiGraph, ISearchableGraph
        where N : IDiGraphNode
    {
        protected Dictionary<string, N> nodes;

        protected AdjacencyDiGraph()
        {
            nodes = new Dictionary<string, N>();
        }

        public bool ContainsNode(string name)
        {
            return nodes.ContainsKey(name);
        }

        public void AddEdge(string from, string to, int dist)
        {
            nodes[from].AddOutNeighbour(to, dist);
            nodes[to].AddInNeighbour(from, dist);
        }

        public bool ContainsEdge(string from, string to)
        {
            return nodes[from].HasOutNeighbour(to);
        }

        public IEnumerable<string> Nodes()
        {
            return nodes.Keys;
        }

        public IEnumerable<(string from, string to, int weight)> Edges()
        {
            foreach(var kvp in nodes)
            {
                foreach(var (nb, weight) in kvp.Value.OutNeighbours())
                {
                    yield return (kvp.Key, nb, weight);
                }
            }
        }

        public IEnumerable<(string nb, int weight)> InNeighbours(string name)
        {
            return nodes[name].InNeighbours();
        }

        public IEnumerable<(string nb, int weight)> OutNeighbours(string name)
        {
            return nodes[name].OutNeighbours();
        }

        public IEnumerable<(string node, int weight)> GetReachableNodes(string from)
        {
            return OutNeighbours(from);
        }
    }

    abstract class AdjacencyDiGraphNode : IDiGraphNode
    {
        public string Name { get; }
        Dictionary<string, int> inNeighbours;
        Dictionary<string, int> outNeighbours;

        protected AdjacencyDiGraphNode(string name)
        {
            Name = name;
            inNeighbours = new Dictionary<string, int>();
            outNeighbours = new Dictionary<string, int>();
        }

        public void AddInNeighbour(string name, int weight)
        {
            inNeighbours.Add(name, weight);
        }

        public void AddOutNeighbour(string name, int weight)
        {
            outNeighbours.Add(name, weight);
        }

        public bool HasInNeighbour(string from)
        {
            return inNeighbours.ContainsKey(from);
        }

        public bool HasOutNeighbour(string to)
        {
            return inNeighbours.ContainsKey(to);
        }

        public IEnumerable<(string nb, int weight)> InNeighbours()
        {
            return inNeighbours.Select(kvp => (kvp.Key, kvp.Value));
        }

        public IEnumerable<(string nb, int weight)> OutNeighbours()
        {
            return outNeighbours.Select(kvp => (kvp.Key, kvp.Value));
        }
    }
}
