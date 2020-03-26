using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AoC.Common;
using AoC.Utils;

namespace AoC.Day6
{
    class Solver : PuzzleSolver
    {
        List<(string, string)> orbitRelations;
        OrbitTree otree;

        public Solver() : this(Input.InputMode.Embedded, "input")
        {
        }

        public Solver(Input.InputMode mode, string input) : base(mode, input)
        {
        }

        protected override void ParseInput(string input)
        {
            orbitRelations = InputParser<(string,string)>.SplitAndParse(input, ParseOrbitRelation).ToList();

            (string, string) ParseOrbitRelation(string inp)
            {
                var sp = inp.Split(')');
                return (sp[0], sp[1]);
            }
        }

        protected override void PrepareSolution()
        {
            otree = OrbitTreeBuilder.MakeTrees(orbitRelations).First();
        }

        protected override void SolvePartOne()
        {
            resultPartOne = otree.CountAllOrbits().ToString();
        }

        protected override void SolvePartTwo()
        {
            resultPartTwo = (otree.DistanceBetweenNodes("YOU", "SAN")).ToString();
        }
    }

    class OrbitTree : Tree<OrbitNode>
    {
        public OrbitTree(string rootName) : base()
        {
            OrbitNode myRoot = new OrbitNode(rootName);
            nodes.Add(rootName, myRoot);
            root = rootName;
            leaves.Add(rootName);
        }

        protected override OrbitNode CopyNode(OrbitNode copyMe)
        {
            return new OrbitNode(copyMe);
        }

        public void AddNode(string parent, string child)
        {
            OrbitNode newChild = new OrbitNode(child);
            nodes.Add(child, newChild);
            AddChild(parent, child);
            leaves.Add(child);
            leaves.Remove(parent);
        }

        public int CountAllOrbits()
        {
            int total = 0;
            foreach (int depth in Depths().Values)
            {
                total += depth;
            }
            return total;
        }

        public int DistanceBetweenNodes(string nodeA, string nodeB)
        {
            //Find common ancestor, or the other node
            List<string> pathToRootA = Ancestors(nodeA).ToList();
            List<string> pathToRootB = Ancestors(nodeB).ToList();

            int shortestPathCount = pathToRootA.Count < pathToRootB.Count ? pathToRootA.Count : pathToRootB.Count;
            int distance = -1;

            for (int commonAncestors = 0; commonAncestors < shortestPathCount; commonAncestors++)
            {
                if (pathToRootA[commonAncestors] != pathToRootB[commonAncestors])
                {
                    distance = pathToRootA.Count + pathToRootB.Count - (2 * (commonAncestors));
                    break;
                }
            }
            return distance;
        }
    }

    class OrbitNode : TreeNode
    {
        public OrbitNode(string name) : base(name)
        {

        }

        public OrbitNode(OrbitNode copyMe) : base(copyMe)
        {

        }
    }

    static class OrbitTreeBuilder
    {
        public static List<OrbitTree> MakeTrees(IEnumerable<(string, string)> pcRelations)
        {
            List<(string, string)> relationsToAdd = pcRelations.ToList();
            Dictionary<string, OrbitTree> trees = new Dictionary<string, OrbitTree>();

            Dictionary<string, string> nodesToRoots = new Dictionary<string, string>();

            //Combine trees whenever we've encountered the parent or child before
            foreach ((string parent, string child) in relationsToAdd)
            {
                OrbitTree tree;
                if (nodesToRoots.ContainsKey(parent))
                {
                    string parentRoot = parent;
                    do
                    {
                        parentRoot = nodesToRoots[parentRoot];
                    } while (!trees.ContainsKey(parentRoot) && parentRoot != parent);
                    tree = trees[parentRoot];
                }
                else
                {
                    tree = new OrbitTree(parent);
                    trees.Add(parent, tree);
                    nodesToRoots.Add(parent, parent);
                }

                if (trees.ContainsKey(child))
                {
                    OrbitTree childTree = trees[child];
                    string childRoot = childTree.Root();
                    tree.AddTreeAsChild(parent, childTree);
                    nodesToRoots[childRoot] = nodesToRoots[parent];
                    trees.Remove(childRoot);
                }
                else
                {
                    tree.AddNode(parent, child);
                    nodesToRoots.Add(child, nodesToRoots[parent]);
                }
            }
            return trees.Values.ToList();
        }
    }
}
