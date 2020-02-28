using System;
using System.Collections.Generic;
using System.Text;

namespace AoC
{
    public interface IEditableGraph
    {
        bool AddNode(string name);

        void CreateEdge(string from, string to, int weight);
    }
    public interface IInspectableBaseGraph
    {
        bool ContainsNode(string name);

        bool ContainsEdge(string from, string to);

        IEnumerable<string> Nodes();

        IEnumerable<(string from, string to, int weight)> Edges();
    }

    public interface IInspectableGraph : IInspectableBaseGraph
    {
        IEnumerable<(string nb, int weight)> Neighbours(string name);
    }

    public interface IInspectableDiGraph : IInspectableBaseGraph
    {
        IEnumerable<(string nb, int weight)> InNeighbours(string name);

        IEnumerable<(string nb, int weight)> OutNeighbours(string name);
    }

    interface IGraphNode : INode
    {
        void AddNeighbour(string name, int weight);

        bool HasNeighbour(string nb);

        IEnumerable<(string nb, int weight)> Neighbours();
    }

    interface IDiGraphNode : INode
    {
        void AddInNeighbour(string name, int weight);
        void AddOutNeighbour(string name, int weight);

        bool HasInNeighbour(string from);
        bool HasOutNeighbour(string to);

        IEnumerable<(string nb, int weight)> InNeighbours();
        IEnumerable<(string nb, int weight)> OutNeighbours();
    }
}
