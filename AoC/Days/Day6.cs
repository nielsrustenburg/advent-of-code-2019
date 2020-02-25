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
            OrbitTreeTwo otree = new OrbitTreeTwo();
            foreach ((string parent, string child) in input)
            {
                otree.AddRelation(parent, child);
            }
            return otree.CountAllOrbits();
        }

        public static int SolvePartTwo()
        {
            List<(string, string)> input = InputReader.ReadOrbitRelationsFromTxt("d6input.txt");
            OrbitTreeTwo otree = new OrbitTreeTwo();
            foreach ((string parent, string child) in input)
            {
                otree.AddRelation(parent, child);
            }
            return otree.DistanceBetweenNodes("YOU", "SAN") - 2; 
        }
    }

    class OrbitTreeTwo : Treee<OrbitNodeTwo>
    {
        public override OrbitNodeTwo CreateNode(string id)
        {
            return new OrbitNodeTwo(id);
        }

        public int CountAllOrbits()
        {
            int total = 0;
            foreach(OrbitNodeTwo node in nodes.Values)
            {
                total += node.Depth();
            }
            return total;
        }

        public int DistanceBetweenNodes(string nodeA, string nodeB)
        {            
            //Find common parent, or the other node
            List<OrbitNodeTwo> aAncestors = new List<OrbitNodeTwo> ();
            List<OrbitNodeTwo> bAncestors = new List<OrbitNodeTwo> ();
            OrbitNodeTwo currentA = nodes[nodeA];
            OrbitNodeTwo currentB = nodes[nodeB];
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

    class OrbitNodeTwo : TreaNode<OrbitNodeTwo>
    {
        public OrbitNodeTwo(string id) : base(id)
        {

        }
    }
}
