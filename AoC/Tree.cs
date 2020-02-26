using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC
{
    class Tree
    {
        protected TreeNode root;
        protected Dictionary<string, TreeNode> nodes;
        protected HashSet<string> leaves;

        public Tree(TreeNode root, Dictionary<string,TreeNode> allNodes)
        {
            this.root = root;
            nodes = new Dictionary<string, TreeNode>();
            leaves = new HashSet<string>();

            IQueue<TreeNode> queue = new NaiveQueue<TreeNode>();
            queue.Enqueue(root);

            while (queue.Any())
            {
                TreeNode currentNode = queue.Dequeue();
                nodes.Add(currentNode.Name, currentNode);

                List<string> children = currentNode.Children();
                if (children.Any())
                {
                    foreach (string child in children)
                    {
                        queue.Enqueue(allNodes[child]);
                    }
                }
                else
                {
                    leaves.Add(currentNode.Name);
                }
            }
        }

        public void AddChild(string parent, TreeNode child)
        {
            nodes.Add(child.Name, child);

            if (leaves.Contains(parent))
            {
                leaves.Remove(parent);
                leaves.Add(child.Name);
            }

            nodes[parent].AddChild(child);
        }

        public void UpdateRoot(TreeNode newRoot)
        {
            nodes.Add(newRoot.Name, newRoot);
            newRoot.AddChild(root);
            root = newRoot;
        }
    }

    class TreeNode : INode
    {
        public string Name { get; private set; }
        protected TreeNode parent;
        protected Dictionary<string, TreeNode> children;
        public int Depth { get; private set; }

        public TreeNode(string name)
        {
            Name = name;
            Depth = 0;
            children = new Dictionary<string, TreeNode>();
        }

        public void AddChild(TreeNode child)
        {
            if (children.ContainsKey(child.Name)) throw new Exception($"{Name} already contains a child with name: {child.Name}");

            children.Add(child.Name, child);
            child.SetParent(this);
            child.UpdateDepth(Depth + 1);
        }

        private void SetParent(TreeNode newParent)
        {
            parent = newParent;
        }

        public void UpdateParent(TreeNode newParent, bool allowReplacement = false)
        {
            if (!allowReplacement && parent != null) throw new Exception("Trying to update parent of a treenode which already has a parent without permission");

            newParent.AddChild(this);
        }

        private void UpdateDepth(int newDepth)
        {
            Depth = newDepth;
            foreach (TreeNode child in children.Values)
            {
                child.UpdateDepth(Depth + 1);
            }
        }

        public string Parent()
        {
            return parent.Name;
        }

        public List<string> Children()
        {
            return children.Keys.ToList();
        }
    }

    abstract class AbstractTreeBuilder<T,N> where T : Tree
                                            where N : TreeNode
    {
        public List<T> MakeTrees(IEnumerable<(string,string)> pcRelations)
        {
            List<(string, string)> relationsToAdd = pcRelations.ToList();
            Dictionary<string, N> nodes = new Dictionary<string, N>();

            foreach ((string parent, string child) in relationsToAdd)
            {
                if (!nodes.TryGetValue(parent, out N pNode))
                {
                    pNode = CreateNode(parent);
                    nodes[parent] = pNode;
                }

                if (!nodes.TryGetValue(child, out N cNode))
                {
                    cNode = CreateNode(child);
                    nodes[child] = cNode;
                }
            }

            HashSet<string> roots = new HashSet<string>();
            HashSet<string> nodesWithRelations = new HashSet<string>();

            foreach ((string parent, string child) in relationsToAdd)
            {
                if (roots.Contains(child)) roots.Remove(child);
                if (!nodesWithRelations.Contains(parent)) roots.Add(parent);

                nodes[parent].AddChild(nodes[child]);
                nodesWithRelations.Add(child);
                nodesWithRelations.Add(parent);
            }

            return roots.Select(r => CreateTree(nodes[r], nodes)).ToList();
        }

        abstract protected N CreateNode(string name);

        abstract protected T CreateTree(N root, Dictionary<string,N> nodes);
    }
}
