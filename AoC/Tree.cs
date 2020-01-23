using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC
{
    class Tree<T>
    {
        public TreeNode<T> Root { get; private set; }
        public List<TreeNode<T>> Leaves { get; private set; }

        public Tree(T rootValue) : this(new TreeNode<T>(rootValue))
        {
        }

        public Tree(TreeNode<T> root)
        {
            Root = root;
            Leaves = new List<TreeNode<T>> { root };
        }

        public void AddChild(TreeNode<T> parent, T value)
        {
            TreeNode<T> child = new TreeNode<T>(value);
            AddChild(parent, child);
        }

        public void AddChild(TreeNode<T> parent, TreeNode<T> child)
        {
            if (!parent.Children.Any())
            {
                Leaves.Remove(parent);
            }
            parent.AddChild(child);
        }
    }

    class TreeNode<T>
    {
        public T Value { get; private set; }
        public TreeNode<T> Parent { get; private set; }
        public List<TreeNode<T>> Children { get; private set; }

        public TreeNode(T value) : this(value, null)
        {
        }

        public TreeNode(T value, TreeNode<T> parent)
        {
            Value = value;
            Parent = parent;
            Children = new List<TreeNode<T>>();
            if (parent != null)
            {
                parent.AddChild(this);
            }
        }

        public void AddChild(TreeNode<T> child)
        {
            Children.Add(child);
        }
    }

    class RecursiveDonutTree : Tree<int>
    {
        public string TargetLabel { get; private set; }

        public RecursiveDonutTree(int rootValue) : this(new RecursiveDonutTreeNode(rootValue)) { }

        public RecursiveDonutTree(RecursiveDonutTreeNode root, string targetLabel = ":") : base(root)
        {
            TargetLabel = targetLabel;
        }

        public void Expand(RecursiveDonutTreeNode parent)
        {
            List<RecursiveDonutTreeNode> children = parent.Expand(TargetLabel);
            if (children.Any())
            {
                if (parent.Children.Any())
                {
                    throw new Exception("Trying to expand a node which already has children, are you sure about this?");
                }
                else
                {
                    Leaves.Remove(parent);
                }
            }
        }

        public RecursiveDonutTreeNode BestFirstSearch()
        {
            //TODO Implement Priority Queue to speed up?
            List<RecursiveDonutTreeNode> CandidatesByBestValue = Leaves.Select(x => (RecursiveDonutTreeNode)x).OrderBy(x => x.Value).ToList();
            string targetName = TargetLabel.Split(':')[0];
            int pointer = 0;
            while (CandidatesByBestValue.Any()) //Maybe add a limit to the maximum tree/leaf size before giving up
            {
                //RecursiveDonutTreeNode current = CandidatesByBestValue.First();
                RecursiveDonutTreeNode current = CandidatesByBestValue[pointer];
                if (current.TargetAcquired(TargetLabel)) return current;

                var newNodes = current.Expand(targetName);

                //CandidatesByBestValue.RemoveAt(0);
                pointer++;
                foreach (var newNode in newNodes)
                {
                    //int indexOfFirstWorse = CandidatesByBestValue.FindIndex(x => x.Value > newNode.Value);
                    int indexOfFirstWorse = CandidatesByBestValue.FindIndex(pointer, x => x.Value > newNode.Value);
                    indexOfFirstWorse = indexOfFirstWorse == -1 ? indexOfFirstWorse = CandidatesByBestValue.Count : indexOfFirstWorse;
                    CandidatesByBestValue.Insert(indexOfFirstWorse, newNode);
                }
            }

            return null;
        }
    }

    class RecursiveDonutTreeNode : TreeNode<int>
    {
        public Node GNode { get; private set; }
        public int Depth { get; private set; }
        public bool UsedPortal { get; private set; }

        public RecursiveDonutTreeNode(int value, RecursiveDonutTreeNode parent = null) : this(value, null, 0) { }

        public RecursiveDonutTreeNode(int value, Node gNode, int depth, RecursiveDonutTreeNode parent = null, bool usedPortal = true) : base(value, parent)
        {
            GNode = gNode;
            Depth = depth;
            UsedPortal = usedPortal;
        }

        public bool TargetAcquired(string targetLabel)
        {
            return Depth == 0 && targetLabel == GNode.Label;
        }

        internal List<RecursiveDonutTreeNode> Expand(string targetName)
        {
            if (Depth < 0) throw new ArgumentOutOfRangeException("Depth should be positive, expansion implemented incorrectly");
            if (Depth > 57) return new List<RecursiveDonutTreeNode>(); //Someone on the interwebs claims you shouldn't need more portal-visits than portals
            string[] myLabel = GNode.Label.Split(':');

            IEnumerable<Edge> candidates = GNode.Outgoing().Where(x => IsValidCandidate(x));
            List<RecursiveDonutTreeNode> result = new List<RecursiveDonutTreeNode>();
            foreach (Edge candidate in candidates)
            {
                //Create new node
                int newDepth = Depth;
                if (!UsedPortal)
                {
                    newDepth += myLabel[1] == "OUT" ? -1 : 1;
                }
                RecursiveDonutTreeNode newNode = new RecursiveDonutTreeNode(Value + candidate.Weight, candidate.Other(GNode), newDepth, this, !UsedPortal);
                result.Add(newNode);
            }
            return result;

            bool IsValidCandidate(Edge edge)
            {
                Node node = edge.Other(GNode);
                string[] label = node.Label.Split(':');
                if (UsedPortal)
                {
                    if (Depth == 0 && label[1] == "OUT")
                    {
                        return targetName == label[0]; //Only outer "portal" we are allowed to move towards at depth 0 should be our target
                    }
                    else
                    {
                        return label[0] != myLabel[0];
                    }
                }
                else
                {
                    //We must either use the portal we are on
                    return label[0] == myLabel[0];
                }
            }
        }

        public override string ToString()
        {
            return GNode.Label + " Depth:" + Depth + " Steps:" + Value;
        }
    }
}
