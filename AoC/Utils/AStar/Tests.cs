using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AoC.Utils.AStar
{
    class Tests
    {
        [Test]
        public void SingleStep()
        {
            var origin = new GridNode(0, 0, 0, 1);
            var result = AStar.Search<NaivePriorityQueue<SearchNode>>.Execute(origin) as GridNode;

            Assert.AreEqual(1, result.x);
            Assert.AreEqual(1, result.cost);
        }

        [Test]
        public void MediumDistance()
        {
            var origin = new GridNode(0, 0, 0, 1000);
            var result = AStar.Search<NaivePriorityQueue<SearchNode>>.Execute(origin) as GridNode;

            Assert.AreEqual(1000, result.x);
            Assert.AreEqual(1000, result.cost);
        }
    }

    class GridNode : SearchNode
    {
        public int x;
        public int y;
        public int target;

        public GridNode(int cost, int x, int y, int target, GridNode parent = null) : base(cost, parent)
        {
            this.x = x;
            this.y = y;
            this.target = target;
        }

        public override bool Equals(SearchNode other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other == null) return false;
            var oth = other as GridNode;
            return x == oth.x && y == oth.y && target == oth.target;
        }

        public override int GetHashCode(SearchNode obj)
        {
            unchecked
            {
                return x * 12345 + y;
            }
        }
        public override int GetHeuristicCost()
        {
            return Math.Abs(target - x) + Math.Abs(y);
        }

        public override SearchNode[] GetNeighbours()
        {
            return new SearchNode[] {
                new GridNode(cost + 1, x + 1, y, target, this),
                new GridNode(cost + 1, x - 1, y, target, this),
                new GridNode(cost + 1, x, y + 1, target, this),
                new GridNode(cost + 1, x, y - 1, target, this),
            };
        }

        public override bool IsAtTarget()
        {
            return x == target && y == 0;
        }
    }
}
