using System;
using System.Collections.Generic;
using System.Text;

namespace AoC
{
    abstract class Treee<N> where N : TreaNode<N>
    {
        protected Dictionary<string, N> nodes;

        protected Treee()
        {
            nodes = new Dictionary<string, N>();
        }

        public void AddRelation(string parent, string child)
        {
            N pNode = GetOrCreateNode(parent);
            N cNode = GetOrCreateNode(child);
            pNode.AddChild(cNode);
        }

        public abstract N CreateNode(string id);

        public N GetOrCreateNode(string id)
        {
            if (!nodes.TryGetValue(id, out N node))
            {
                node = CreateNode(id);
                nodes[id] = node;
            }
            return node;
        }
    }
}
