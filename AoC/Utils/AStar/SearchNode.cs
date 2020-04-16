using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC.Utils.AStar
{
    public abstract class SearchNode : IEquatable<SearchNode>, IComparable<SearchNode>
    {
        public int cost;
        public SearchNode parent;

        public abstract bool IsAtTarget();
        public abstract int GetHeuristicCost();
        public abstract SearchNode[] GetNeighbours();
        public abstract bool Equals(SearchNode other);
        public abstract int GetHashCode(SearchNode obj);

        public SearchNode(int cost)
        {
            this.cost = cost;
        }

        public SearchNode(int cost, SearchNode parent) : this(cost)
        {
            this.parent = parent;
        }

        public int LBCost
        {
            get
            {
                return cost + GetHeuristicCost();
            }
        }

        public List<SearchNode> GetPath()
        {
            if (parent == null) return new List<SearchNode>();
            var path = parent.GetPath();
            path.Add(this);
            return path;
        }

        int IComparable<SearchNode>.CompareTo(SearchNode other)
        {
            if (LBCost != other.LBCost) return LBCost.CompareTo(other.LBCost);
            return GetHeuristicCost().CompareTo(other.GetHeuristicCost());
        }
    }
}
