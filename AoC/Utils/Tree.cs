using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC.Utils
{
    abstract class Tree<T> : IInspectableTree where T : TreeNode
    {
        protected Dictionary<string, T> nodes;
        protected string root;
        protected HashSet<string> leaves;

        protected Tree()
        {
            nodes = new Dictionary<string, T>();
            leaves = new HashSet<string>();
        }

        public string Parent(string child)
        {
            return nodes[child].Parent();
        }

        public IEnumerable<string> Children(string parent)
        {
            return nodes[parent].Children();
        }

        public bool ContainsNode(string name)
        {
            return nodes.ContainsKey(name);
        }

        protected void AddChild(string parent, string child)
        {
            nodes[parent].AddChild(child);
            nodes[child].SetParent(parent);
        }

        public void AddTreeAsChild(string parent, Tree<T> otherTree)
        {
            foreach(var kvp in otherTree.nodes)
            {
                nodes.Add(kvp.Key, CopyNode(kvp.Value));
            }
            AddChild(parent, otherTree.root);
            leaves.Remove(parent);
            leaves.UnionWith(otherTree.leaves);
        }

        public Dictionary<string, int> Depths()
        {
            int d = 0;
            Dictionary<string, int> depthDict = new Dictionary<string, int>();
            List<string> currentLayer = new List<string> { root };
            while (currentLayer.Any())
            {
                List<string> nextLayer = new List<string>();
                foreach (string node in currentLayer)
                {
                    depthDict.Add(node, d);
                    nextLayer.AddRange(Children(node));
                }
                d++;
                currentLayer = nextLayer;
            }
            return depthDict;
        }

        public IEnumerable<string> Ancestors(string from)
        {
            if (from != root)
            {
                var parent = Parent(from);
                foreach (var ancestor in Ancestors(parent))
                {
                    yield return ancestor;
                }
                yield return parent;
            }
        }

        protected abstract T CopyNode(T copyMe);

        public IEnumerable<string> Leaves()
        {
            return leaves.AsEnumerable();
        }

        public string Root()
        {
            return root;
        }
    }

    interface IInspectableTree
    {
        bool ContainsNode(string name);

        string Root();

        IEnumerable<string> Leaves();

        string Parent(string child);

        IEnumerable<string> Children(string parent);
    }

    interface ITreeNode : INode
    {
        string Parent();

        IEnumerable<string> Children();

        void AddChild(string child);

        bool IsLeaf();

        bool IsRoot();
    }

    abstract class TreeNode : ITreeNode
    {
        public string Name { get; }
        protected string parent;
        protected HashSet<string> children;

        protected TreeNode(string name)
        {
            Name = name;
            children = new HashSet<string>();
        }

        protected TreeNode(TreeNode copyMe) : this(copyMe.Name)
        {
            parent = copyMe.parent;
            children.UnionWith(copyMe.children);
        }

        public IEnumerable<string> Children()
        {
            return children.AsEnumerable();
        }

        public bool IsLeaf()
        {
            return !children.Any();
        }

        public bool IsRoot()
        {
            return parent == null;
        }

        public string Parent()
        {
            return parent;
        }

        public void AddChild(string child)
        {
            children.Add(child);
        }

        public void SetParent(string parent)
        {
            this.parent = parent;
        }
    }
}
