using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC
{
    static class Day6
    {
        public static int SolvePartOne()
        {
            List<(string, string)> input = InputReader.ReadOrbitRelationsFromTxt("d6input.txt");
            OrbitTree otree = new OrbitTree();
            foreach((string parent, string child) in input)
            {
                otree.AddOrbitRelation(parent, child);
            }
            otree.SetDistancesFromRoot();
            return otree.GetDirectOrbits() + otree.GetIndirectOrbits();
        }

        public static int SolvePartTwo()
        {
            List<(string, string)> input = InputReader.ReadOrbitRelationsFromTxt("d6input.txt");
            OrbitTree otree = new OrbitTree();
            foreach ((string parent, string child) in input)
            {
                otree.AddOrbitRelation(parent, child);
            }
            otree.SetDistancesFromRoot();

            return otree.DistanceBetweenNodes("YOU", "SAN") - 2;
        }
    }

    class OrbitTree
    {
        Dictionary<string, OrbitNode> nodes;
        OrbitNode root;

        public OrbitTree()
        {
            nodes = new Dictionary<string, OrbitNode>();
        }

        public void FindAndSetRoot()
        {
            OrbitNode current = nodes.Values.ToList().First();
            while (current.HasParent())
            {
                current = current.GetParent();
            }
            root = current;
        }

        public int GetDirectOrbits()
        {
            return nodes.Count - 1;
        }

        public int GetIndirectOrbits()
        {
            int total = 0; 
            foreach(OrbitNode node in nodes.Values)
            {
                total += node.IndirectOrbits();
            }
            return total;
        }

        public int DistanceBetweenNodes(string nodeA, string nodeB)
        {
            //Find common parent, or the other node
            List<OrbitNode> aAncestors = new List<OrbitNode> { nodes[nodeA] };
            List<OrbitNode> bAncestors = new List<OrbitNode> { nodes[nodeB] };
            bool unfinished = true;
            while (unfinished)
            {
                int adist = aAncestors.Last().GetRootDist();
                int bdist = bAncestors.Last().GetRootDist();
                if (adist > bdist)
                {
                    aAncestors.Add(aAncestors.Last().GetParent());
                } else
                {
                    bAncestors.Add(bAncestors.Last().GetParent());
                }

                unfinished = aAncestors.Last() != bAncestors.Last();
            }

            return aAncestors.Count + bAncestors.Count - 2;
        }

        public void SetDistancesFromRoot()
        {
            FindAndSetRoot();
            root.SetDistanceFromRoot(0);
        }

        public void AddOrbitRelation(string orbitee, string orbiter)
        {
            OrbitNode parent = TryCreateNode(orbitee);
            OrbitNode child = TryCreateNode(orbiter);
            parent.AddChild(child);
            child.AddParent(parent);
        }

        public OrbitNode TryCreateNode(string name)
        {
            OrbitNode node;
            if (!nodes.ContainsKey(name))
            {
                node = new OrbitNode(name);
                nodes.Add(name, node);
            } else
            {
                node = nodes[name];
            }
            return node;
        }

        public void AddNode(OrbitNode node)
        {
            nodes.Add(node.name, node);
        }

    }

    class OrbitNode
    {
        public string name;
        OrbitNode parent;
        List<OrbitNode> children;
        int distanceFromRoot;

        public OrbitNode(string name)
        {
            this.name = name;
            children = new List<OrbitNode>();
        }

        public void SetDistanceFromRoot(int distance)
        {
            distanceFromRoot = distance;
            foreach(OrbitNode child in children)
            {
                child.SetDistanceFromRoot(distance + 1);
            }
        }

        public int GetRootDist()
        {
            return distanceFromRoot;
        }

        public int IndirectOrbits()
        {
            //Indirect Orbits per node = distance from root - 1, except for rootnode
            if (HasParent())
            {
                return distanceFromRoot - 1;
            }
            return 0;
        }

        public bool HasParent()
        {
            return parent != null;
        }

        public OrbitNode GetParent()
        {
            return parent;
        }

        public void AddParent(OrbitNode parent)
        {
            if (this.parent != null) throw new Exception($"OrbitNode {name} already has a parent node {this.parent}, can't add{parent}");
            this.parent = parent;
        }

        public void AddChild(OrbitNode child)
        {
            children.Add(child);
        }

        public override string ToString()
        {
            return name;
        }
    }
}
