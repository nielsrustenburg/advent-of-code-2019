using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC
{
    static class Day20
    {
        public static int SolvePartOne()
        {
            string[] input = InputReader.StringsFromTxt("d20input.txt");
            (Maze<string> dmaze, DonutGraph dgraph) = BuildDonutMaze(input);

            return dgraph.BestFirstPath("AA:OUT", "ZZ:IN");
        }

        public static int SolvePartTwo()
        {
            string[] input = InputReader.StringsFromTxt("d20input.txt");
            (Maze<string> dmaze, DonutGraph dgraph) = BuildDonutMaze(input);

            return dgraph.BestFirstPath("AA:OUT", "ZZ:IN", true);
        }

        private static (Maze<string>, DonutGraph) BuildDonutMaze(string[] input)
        {

            IEnumerable<IEnumerable<string>> mazeInput = input.Reverse().Select(s => s.Select(c => c.ToString()));

            Maze<string> maze = new Maze<string>(mazeInput, " ", new List<string> { "." }, false);

            //Set up some properties of the maze
            int thickness = FindDonutThickness(input);
            int outerLeft = 2;
            int outerRight = input[0].Length - 3;
            int outerTop = input.Count() - 3;
            int outerBot = 2;

            int innerLeft = outerLeft + thickness;
            int innerRight = outerRight - thickness;
            int innerTop = outerTop - thickness;
            int innerBot = outerBot + thickness;

            HashSet<int> innerXBorder = new HashSet<int> { innerLeft, innerRight };
            HashSet<int> innerYBorder = new HashSet<int> { innerTop, innerBot };
            HashSet<int> outerXBorder = new HashSet<int> { outerLeft, outerRight };
            HashSet<int> outerYBorder = new HashSet<int> { outerTop, outerBot };

            //Locate the portals
            List<(int x, int y)> floors = maze.FindAllMatchingTiles(".");

            HashSet<string> nonPortalLabels = new HashSet<string> { ".", " ", "#" };
            HashSet<string> portals = new HashSet<string>();
            foreach ((int x, int y) in floors)
            {
                bool isOuterPortal = outerXBorder.Contains(x) || outerYBorder.Contains(y);
                bool isInnerPortal = (innerXBorder.Contains(x) && y < innerTop && y > innerBot) ||
                                     (innerYBorder.Contains(y) && x < innerRight && x > innerLeft);
                if (isInnerPortal || isOuterPortal)
                {
                    var part1 = maze.GetNeighbours(x, y).Where(kvp => !nonPortalLabels.Contains(kvp.Value)).First();
                    string portalName;
                    if (part1.Key == "N")
                    {
                        portalName = maze.GetTile(x, y + 2) + maze.GetTile(x, y + 1);
                        maze.SetTile(x, y + 2, " ");
                        maze.SetTile(x, y + 1, " ");
                    }
                    else if (part1.Key == "E")
                    {
                        portalName = maze.GetTile(x + 1, y) + maze.GetTile(x + 2, y);
                        maze.SetTile(x + 1, y, " ");
                        maze.SetTile(x + 2, y, " ");
                    }
                    else if (part1.Key == "S")
                    {
                        portalName = maze.GetTile(x, y - 1) + maze.GetTile(x, y - 2);
                        maze.SetTile(x, y - 2, " ");
                        maze.SetTile(x, y - 1, " ");
                    }
                    else if (part1.Key == "W")
                    {
                        portalName = maze.GetTile(x - 2, y) + maze.GetTile(x - 1, y);
                        maze.SetTile(x - 1, y, " ");
                        maze.SetTile(x - 2, y, " ");
                    }
                    else
                    {
                        throw new Exception("Shouldn't reach this");
                    }
                    portals.Add(portalName);
                    maze.SetTile(x, y, isInnerPortal ? portalName + ":IN" : portalName + ":OUT");
                }
            }

            //Create the graph
            DonutGraph dgraph = new DonutGraph(thickness);

            foreach (string label in portals)
            {
                dgraph.AddNode(label + ":IN");
                dgraph.AddNode(label + ":OUT");
            }

            //For every portal set up the reachable locations
            HashSet<string> existingPortals = maze.GetAllTileTypes();
            existingPortals.ExceptWith(nonPortalLabels); //discard non-portal-tiletypes;
            foreach (string portal in existingPortals)
            {
                (bool _, int x, int y) = maze.FindFirstMatchingTile(portal);
                (int _, List<(string tile, int distance)> reachablePortals) = maze.FloodFillDistanceFinder(x, y, existingPortals);
                foreach ((string reachable, int distance) in reachablePortals)
                {
                    //If you can walk from any portal to an out-portal then it should connect to the in-portal and vice versa
                    //Since there is no reason to walk to a portal and not take it
                    string[] split = reachable.Split(':');
                    string otherSide = split[0] + (split[1] == "IN" ? ":OUT" : ":IN");
                    dgraph.AddEdge(portal, otherSide, distance+1);
                }
            }


            return (maze, dgraph);
        }

        public static int FindDonutThickness(string[] input)
        {
            string halfWaySlice = input[input.Count() / 2];

            for (int i = 0; i < halfWaySlice.Length / 2; i++)
            {
                //Donut starts at index = 2
                if (halfWaySlice[i + 2] != '.' && halfWaySlice[i + 2] != '#')
                {
                    //Found the inside of the donut-ring, distance from one edge to other of donut is i-1, actual thickness is #i symbols, but we need the distance
                    return i - 1;
                }
            }
            throw new Exception("Should've found the inside before reaching half the donut thickness");
        }
    }

    class DonutGraph : AdjacencyDiGraph<DonutGraphNode>
    {
        readonly int donutThickness; //useful for A* heuristic

        public void AddNode(string name)
        {
            nodes.Add(name, new DonutGraphNode(name));
        }

        public bool IsInnerPortal(string name)
        {
            return nodes[name].IsInnerPortal;
        }

        public DonutGraph(int thickness) : base()
        {
            donutThickness = thickness;
        }

        public int BestFirstPath(string from, string to, bool depthCheck = false, int maxDist = int.MaxValue)
        {
            List<(string, int distance, int depth, int aStarDist)> priorityQueue = new List<(string, int, int, int)>();
            if (from == to) return 0;

            //make it so we end on depth 0 for my implementation of the graph 
            //(as we don't actually take ZZ:OUT to reach ZZ:IN, but still count it as using an OUT portal)
            int initialDepth = IsInnerPortal(to) ? 1 : -1;

            priorityQueue.Add((from, 0, initialDepth, donutThickness));
            while (priorityQueue.Any())
            {
                (string portal, int distance, int depth, int _) = priorityQueue[0];
                if (portal == to && (!depthCheck || depth == 0))
                {
                    return distance-1; //We don't take the last portal
                }

                priorityQueue.RemoveAt(0);

                foreach ((string nb, int weight) in OutNeighbours(portal))
                {
                    int newDist = distance + weight;
                    int newDepth = depth + (nb.Split(':')[1] == "OUT" ? 1 : -1);
                    int newaStarDist = newDist + Math.Abs(newDepth) * donutThickness;

                    //(potential) distance must be within positive integer range
                    if (newDist <= maxDist &&
                        newDist > 0 &&
                        (newDepth >= 0 || nb == to) &&
                        !(depthCheck && (newaStarDist > maxDist || newaStarDist < 0))
                        )
                    {

                        int insertAt;
                        if (depthCheck)
                        {
                            insertAt = priorityQueue.FindLastIndex(p => p.aStarDist <= newaStarDist);
                        }
                        else
                        {
                            insertAt = priorityQueue.FindLastIndex(p => p.distance <= newDist);
                        }
                        priorityQueue.Insert(insertAt+1, (nb, newDist, newDepth, newaStarDist));
                    }
                }
            }
            throw new Exception("couldn't find a path within the allowed maxDistance");
        }
    }

    class DonutGraphNode : AdjacencyDiGraphNode
    {
        public bool IsInnerPortal { get; }

        public DonutGraphNode(string name) : base(name)
        {
            IsInnerPortal = name.Split(':')[1] == "IN";
        }
    }

}

