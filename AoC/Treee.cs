using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC
{
    abstract class Treee<N> where N : TreaNode<N>
    {
        protected Dictionary<string, N> nodes;
        protected N root;
        protected HashSet<N> leaves;

        protected Treee()
        {
            nodes = new Dictionary<string, N>();
        }

        protected Treee(IEnumerable<(string,string)> relations) : this()
        {
            List<(string, string)> relationsToAdd = relations.ToList();
            HashSet<string> roots = new HashSet<string>();
            foreach((string parent, string child) in relations)
            {
                if (roots.Contains(child)) roots.Remove(child);
                if (!nodes.ContainsKey(parent)) roots.Add(parent);
                AddInitialRelation(parent, child);
            }
            if (roots.Count != 1) throw new Exception($"This set of relations contains {roots.Count} roots, please provide relations that form a single tree");

            root = nodes[roots.Select(x => x).First()]; //Not sure if there's a cleaner way to get the only item in the HashSet
            InitializeLeaves();
        }

        public void AddChildNode(string parent, string child)
        {
            if (!nodes.ContainsKey(parent)) throw new Exception($"Parent node {parent} not found");
        }

        public void CreateNewRoot(string newRootId)
        {
            N newRoot = CreateNode(newRootId);
            newRoot.AddChild(root);
            root = newRoot;
        }

        private void AddInitialRelation(string parent, string child)
        {
            N pNode = GetOrCreateNode(parent);
            N cNode = GetOrCreateNode(child);
            pNode.AddChild(cNode);
        }

        protected abstract N CreateNode(string id);

        private N GetOrCreateNode(string id)
        {
            if (!nodes.TryGetValue(id, out N node))
            {
                node = CreateNode(id);
                nodes[id] = node;
            }
            return node;
        }

        private void InitializeLeaves()
        {
            leaves = new HashSet<N>();
            List<N> queue = new List<N> { root }; //Not an actual queue sadly
            while(queue.Any())
            {
                List<N> childNodes = queue.First().Children();
                if(childNodes.Count == 0)
                {
                    leaves.Add(queue.First());
                } else
                {
                    queue.AddRange(childNodes);
                }
                queue.RemoveAt(0);
            }
        }
    }
}
