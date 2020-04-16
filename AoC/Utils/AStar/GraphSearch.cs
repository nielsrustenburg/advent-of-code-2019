using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC.Utils.AStar
{
    static class GraphSearch
    {
        public static (string[] path, int distance) Dijkstra(IInspectableGraph graph, string from, string to)
        {
            DijkstraNode startNode = new DijkstraNode(graph, from, 0, to);
            var result = Search<NaivePriorityQueue<SearchNode>>.Execute(startNode);

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
        IInspectableGraph graph;
        string targetNode;

        public DijkstraNode(IInspectableGraph graph, string graphNode, int cost, string targetNode, DijkstraNode parent = null) : base(cost, parent)
        {
            this.graph = graph;
            this.graphNode = graphNode;
            this.targetNode = targetNode;
        }

        public override bool Equals(SearchNode other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other == null) return false;
            var oth = other as DijkstraNode;
            if (graph != oth.graph) return false;
            return graphNode == oth.graphNode;
        }

        public override int GetHashCode(SearchNode obj)
        {
            return graphNode.GetHashCode();
        }

        public override int GetHeuristicCost()
        {
            return 0;
        }

        public override SearchNode[] GetNeighbours()
        {
            return graph.Neighbours(graphNode).Select(nb => new DijkstraNode(graph, nb.nb, cost + nb.weight, targetNode, this)).ToArray();
        }

        public override bool IsAtTarget()
        {
            return graphNode == targetNode;
        }
    }
}
