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
            OrbitTreeBuilder otb = new OrbitTreeBuilder();
            OrbitTree otree = otb.MakeTrees(input).First();
            return otree.CountAllOrbits();
        }

        public static int SolvePartTwo()
        {
            List<(string, string)> input = InputReader.ReadOrbitRelationsFromTxt("d6input.txt");
            OrbitTreeBuilder otb = new OrbitTreeBuilder();
            OrbitTree otree = otb.MakeTrees(input).First();
            return otree.DistanceBetweenNodes("YOU", "SAN") - 2; 
        }
    }

    class OrbitTreeBuilder : AbstractTreeBuilder<OrbitTree,TreeNode>
    {
        protected override TreeNode CreateNode(string name)
        {
            return new TreeNode(name);
        }

        protected override OrbitTree CreateTree(TreeNode root, Dictionary<string,TreeNode> nodes)
        {
            return new OrbitTree(root, nodes);
        }
    }

    class OrbitTree : Tree
    {
        public OrbitTree(TreeNode root, Dictionary<string,TreeNode> nodes) : base(root, nodes)
        {

        }

        public int CountAllOrbits()
        {
            int total = 0;
            foreach(TreeNode node in nodes.Values)
            {
                total += node.Depth;
            }
            return total;
        }

        public int DistanceBetweenNodes(string nodeA, string nodeB)
        {            
            //Find common ancestor, or the other node
            List<TreeNode> aAncestors = new List<TreeNode> ();
            List<TreeNode> bAncestors = new List<TreeNode> ();
            TreeNode currentA = nodes[nodeA];
            TreeNode currentB = nodes[nodeB];
            bool unfinished = true;
            while (unfinished)
            {
                if (currentA.Depth > currentB.Depth)
                {
                    currentA = nodes[currentA.Parent()];
                    aAncestors.Add(currentA);
                }
                else
                {
                    currentB = nodes[currentB.Parent()];
                    bAncestors.Add(currentB);
                }

                unfinished = currentA != currentB;
            }
            return aAncestors.Count + bAncestors.Count;
        }
    }
}
