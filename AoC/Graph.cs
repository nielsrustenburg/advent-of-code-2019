using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC
{
    class Graph
    {
        Dictionary<string, Node> nodes;

        public void AddConnection(Node nodeA, Node nodeB)
        {
            AddConnection(nodeA, nodeB, true);
        }

        public void AddConnection(Node nodeA, Node nodeB, bool sym)
        {
            Edge edge = new Edge(nodeA, nodeB, sym:sym);
            nodeA.AddOutEdge(edge);
            nodeB.AddInEdge(edge);
        }
    }

    class WeightedGraph
    {
        public Dictionary<string, Node> Nodes { get; private set; }

        public WeightedGraph()
        {
            Nodes = new Dictionary<string, Node>();
        }

        public void AddNode(Node n)
        {
            Nodes.Add(n.Label, n);
        }

        public void AddNode(string label)
        {
            Nodes.Add(label, new Node(label));
        }

        public void AddConnection(string a, string b, int weight, bool sym = true)
        {
            AddConnection(Nodes[a], Nodes[b], weight, sym);
        }

        public void AddConnection(Node nodeA, Node nodeB, int weight, bool sym = true)
        {
            Edge edge = new Edge(nodeA, nodeB, weight, sym);
            nodeA.AddOutEdge(edge);
            nodeB.AddInEdge(edge);
        }

        public bool Contains(string label)
        {
            return Nodes.ContainsKey(label);
        }

        public bool Contains(Node node)
        {
            return Nodes.ContainsValue(node);
        }

        public (List<Node>, int) ShortestPath(Node from, Node to, List<Node> path = null, int dist = 0)
        {
            throw new NotImplementedException();
        }
    }

    class DoorMazeGraph : WeightedGraph
    {
        //Weighted graph met functies om nodes met objecten samen te trekken/te taggen??

    }

    class Node
    {
        List<Edge> outEdges;
        List<Edge> inEdges;
        public string Label { get; private set; }

        public Node() : this("")
        {
        }

        public Node(string label)
        {
            outEdges = new List<Edge>();
            inEdges = new List<Edge>();
            Label = label;
        }

        public void AddOutEdge(Edge edge)
        {
            outEdges.Add(edge);
        }

        public void AddInEdge(Edge edge)
        {
            inEdges.Add(edge);
        }

        public IEnumerable<Edge> Outgoing()
        {
            return outEdges.Union(inEdges.Where(i => i.IsSymmetrical));
        }

        public IEnumerable<Edge> Incoming()
        {
            return inEdges.Union(outEdges.Where(o => o.IsSymmetrical));
        }

        public override string ToString()
        {
            return Label;
        }
    }

    class Edge
    {
        public Node From { get; private set; }
        public Node To { get; private set; }
        public bool IsSymmetrical { get; private set; }
        public int Weight { get; private set; }
        public List<string> Tags { get; private set; }

        public Edge(Node from, Node to, int weight = 0, bool sym = true) : this(from,to,new List<string>(), weight, sym)
        {

        }

        public Edge(Node from, Node to, IEnumerable<string> tags, int weight = 0, bool sym = true)
        {
            From = from;
            To = to;
            IsSymmetrical = sym;
            Weight = weight;
            Tags = new List<string>(tags);
        }

        public Node Other(Node n)
        {
            if (From == n) return To;
            if (To == n) return From;
            throw new Exception("input node does not belong to this edge");
        }

        public override string ToString()
        {
            return From.ToString() + (IsSymmetrical ? "==" : "=>") + To.ToString();
        }
    }
}
