using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC.Utils.AStar
{
    static class GraphSearch
    {
        public static (string[] path, int distance) Dijkstra(ISearchableGraph graph, string from, string to)
        {
            DijkstraNode startNode = new DijkstraNode(graph, from, 0, to);
            var result = Search<HeapPriorityQueue<SearchNode>>.Execute(startNode);

            var path = result.GetPath().Select(sn => {
                var gsn = sn as DijkstraNode;
                return gsn.graphNode;
            }).ToArray();

            return (path, result.cost);
        }
    }
    class DijkstraNode : SearchNode
    {
        public string graphNode;
        ISearchableGraph graph;
        string targetNode;

        public DijkstraNode(ISearchableGraph graph, string graphNode, int cost, string targetNode, DijkstraNode parent = null) : base(cost, parent)
        {
            this.graph = graph;
            this.graphNode = graphNode;
            this.targetNode = targetNode;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj == null) return false;
            var oth = obj as DijkstraNode;
            if (graph != oth.graph) return false;
            return graphNode == oth.graphNode;
        }

        public override int GetHashCode()
        {
            return graphNode.GetHashCode();
        }

        public override int GetHeuristicCost()
        {
            return 0;
        }

        public override SearchNode[] GetNeighbours()
        {
            return graph.GetReachableNodes(graphNode).Select(nb => new DijkstraNode(graph, nb.node, cost + nb.weight, targetNode, this)).ToArray();
        }

        public override bool IsAtTarget()
        {
            return graphNode == targetNode;
        }
    }
}
