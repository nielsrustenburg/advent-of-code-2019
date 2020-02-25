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
            OrbitTree otree = new OrbitTree(input);
            return otree.CountAllOrbits();
        }

        public static int SolvePartTwo()
        {
            List<(string, string)> input = InputReader.ReadOrbitRelationsFromTxt("d6input.txt");
            OrbitTree otree = new OrbitTree(input);
            return otree.DistanceBetweenNodes("YOU", "SAN") - 2; 
        }
    }

    class OrbitTree : Treee<OrbitNode>
    {
        public OrbitTree(IEnumerable<(string,string)> relations) : base(relations)
        {

        }

        protected override OrbitNode CreateNode(string id)
        {
            return new OrbitNode(id);
        }

        public int CountAllOrbits()
        {
            int total = 0;
            foreach(OrbitNode node in nodes.Values)
            {
                total += node.Depth();
            }
            return total;
        }

        public int DistanceBetweenNodes(string nodeA, string nodeB)
        {            
            //Find common parent, or the other node
            List<OrbitNode> aAncestors = new List<OrbitNode> ();
            List<OrbitNode> bAncestors = new List<OrbitNode> ();
            OrbitNode currentA = nodes[nodeA];
            OrbitNode currentB = nodes[nodeB];
            bool unfinished = true;
            while (unfinished)
            {
                if (currentA.Depth() > currentB.Depth())
                {
                    currentA = currentA.Parent();
                    aAncestors.Add(currentA);
                }
                else
                {
                    currentB = currentB.Parent();
                    bAncestors.Add(currentB);
                }

                unfinished = currentA != currentB;
            }
            return aAncestors.Count + bAncestors.Count;
        }
    }

    class OrbitNode : TreaNode<OrbitNode>
    {
        public OrbitNode(string id) : base(id)
        {

        }
    }
}
