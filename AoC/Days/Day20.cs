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
            (Doolhof<string> dmaze, DonutGraph dgraph) = BuildDonutMaze(input);

            return dgraph.BestFirstPath("AA:OUT", "ZZ:IN");
        }

        public static int SolvePartTwo()
        {
            string[] input = InputReader.StringsFromTxt("d20input.txt");
            (Doolhof<string> dmaze, DonutGraph dgraph) = BuildDonutMaze(input);

            return dgraph.BestFirstPath("AA:OUT", "ZZ:IN", true);
        }

        private static (Doolhof<string>, DonutGraph) BuildDonutMaze(string[] input)
        {

            IEnumerable<IEnumerable<string>> mazeInput = input.Reverse().Select(s => s.Select(c => c.ToString()));

            Doolhof<string> maze = new Doolhof<string>(mazeInput, " ", new List<string> { "." }, false);

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
        int donutThickness; //useful for A* heuristic

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


    static class Day20Old
    {
        public static int SolvePartOne()
        {
            string[] input = InputReader.StringsFromTxt("d20input.txt");
            DonutMaze dm = new DonutMaze(input, new List<string> { "#", " " }, new List<string> { "." });
            dm.PrintInternalRepresentation();
            return dm.ShortestPath("AA", "ZZ");
        }

        public static int SolvePartTwo()
        {
            string[] input = InputReader.StringsFromTxt("d20input.txt");
            DonutMaze dm = new DonutMaze(input, new List<string> { "#", " " }, new List<string> { "." });
            return dm.ShortestRecursivePathAAZZ();
        }
    }

    class DonutMaze
    {
        List<string> originalRep;
        ColorGrid internalRep;
        List<string> wallSymbols;
        List<string> floorSymbols;
        Dictionary<string, List<(int x, int y)>> portalLocations;
        Dictionary<int, Dictionary<int, string>> portalLabels;
        HashSet<(int x, int y)> outerPortals;
        HashSet<(int x, int y)> innerPortals;
        (int up, int down, int left, int right) edge;

        public DonutMaze(IEnumerable<string> image, IEnumerable<string> walls, IEnumerable<string> floors)
        {
            List<string> originalRep = new List<string>(image);
            wallSymbols = new List<string>(walls);
            floorSymbols = new List<string>(floors);

            internalRep = new ColorGrid(wallSymbols.First());
            List<(int, int)> labelLocations = InitInternalRepresentation();

            edge = LocateDonutEdges();

            portalLocations = new Dictionary<string, List<(int x, int y)>>();
            portalLabels = new Dictionary<int, Dictionary<int, string>>();
            outerPortals = new HashSet<(int x, int y)>();
            innerPortals = new HashSet<(int x, int y)>();
            IdentifyPortals(labelLocations);


            List<(int x, int y)> InitInternalRepresentation()
            {
                List<(int x, int y)> unidentifiedSymbols = new List<(int x, int y)>();

                for (int y = 0; y < originalRep.Count; y++)
                {
                    string row = originalRep[originalRep.Count - 1 - y];
                    for (int x = 0; x < row.Length; x++)
                    {
                        internalRep.GetColorAt(x, y); //a lil hack to make sure the grid xyminmax is acceptable (plz fix when refactoring ColorGrid)
                        string sym = row[x].ToString();
                        if (!wallSymbols.Contains(sym))
                        {
                            internalRep.SetColorAt(x, y, sym);
                            if (!floorSymbols.Contains(sym))
                            {
                                unidentifiedSymbols.Add((x, y));
                            }
                        }
                    }
                }
                return unidentifiedSymbols;
            }

            (int up, int down, int left, int right) LocateDonutEdges()
            {
                bool upperFound = false;
                int up, down, left, right;
                up = down = left = right = 0;
                for (int y = originalRep.Count - 1; y >= 0; y--)
                {
                    if (!upperFound)
                    {
                        if (originalRep[originalRep.Count - 1 - y].Contains('#'))
                        {
                            upperFound = true;
                            up = y;
                            left = originalRep[originalRep.Count - 1 - y].IndexOf('#');
                            right = originalRep[originalRep.Count - 1 - y].LastIndexOf('#');
                        }
                    }
                    else
                    {
                        if (!originalRep[originalRep.Count - 1 - y].Contains('#'))
                        {
                            down = y + 1;
                            return (up, down, left, right);
                        }
                    }
                }
                throw new Exception("Shouldn't reach this");
            }

            void IdentifyPortals(IEnumerable<(int, int)> labelLocs)
            {
                foreach ((int x, int y) in labelLocs)
                {
                    Dictionary<string, string> nbours = internalRep.Neighbours(x, y);
                    var floor = nbours.FirstOrDefault(kvp => kvp.Value == ".");
                    if (!floor.Equals(default(KeyValuePair<string, string>)))
                    {
                        string label = "";
                        if (nbours["N"] == ".")
                        {
                            AddPortal(x, y + 1, internalRep.GetColorAt(x, y) + nbours["S"]);
                        }
                        if (nbours["E"] == ".")
                        {
                            AddPortal(x + 1, y, nbours["W"] + internalRep.GetColorAt(x, y));
                        }
                        if (nbours["S"] == ".")
                        {
                            AddPortal(x, y - 1, nbours["N"] + internalRep.GetColorAt(x, y));
                        }
                        if (nbours["W"] == ".")
                        {
                            AddPortal(x - 1, y, internalRep.GetColorAt(x, y) + nbours["E"]);
                        }
                    }
                }
            }
        }

        public int ShortestPath(string from, string to)
        {
            //Assume uniquely labelled "portals"
            return ShortestPath(portalLocations[from].First(), portalLocations[to].First());
        }

        public int ShortestPath((int x, int y) from, (int x, int y) to, bool print = false)
        {
            //Floodfill until we reach our destination
            ColorGrid maze = internalRep.CopyMe();

            if (print) Console.Clear();

            HashSet<(int x, int y)> frontier = new HashSet<(int x, int y)> { from };
            int steps = 0;
            while (true)
            {
                HashSet<(int x, int y)> nextFrontier = new HashSet<(int x, int y)>();
                foreach ((int x, int y) fTile in frontier)
                {
                    if (fTile.x == to.x && fTile.y == to.y)//Sad, no tuple equality in C# 7.0 gotta upgrade!!!
                    {
                        return steps;
                    }
                    if (maze.GetColorAt(fTile.x, fTile.y) == "%")
                    {
                        //Portal found, add other portals exits to next frontier
                        string label = portalLabels[fTile.x][fTile.y];
                        IEnumerable<(int x, int y)> exits = portalLocations[label].Where(pos => pos.x != fTile.x || pos.y != fTile.y);
                        nextFrontier.UnionWith(exits);
                    }
                    maze.SetColorAt(fTile.x, fTile.y, "&");
                    foreach (KeyValuePair<string, string> nbor in maze.Neighbours(fTile.x, fTile.y))
                    {
                        if (nbor.Value == "%" || nbor.Value == ".")
                        {
                            (int x, int y) nLoc = nbor.Key.Aggregate(fTile, (a, b) => (a.Item1 + (b == 'E' ? 1 : 0) - (b == 'W' ? 1 : 0)
                                                                            , a.Item2 + (b == 'N' ? 1 : 0) - (b == 'S' ? 1 : 0)));
                            nextFrontier.Add(nLoc);
                        }
                    }
                }
                if (print)
                {
                    Console.SetCursorPosition(0, 0);
                    foreach (string line in maze.GetImageStrings(true))
                    {
                        Console.WriteLine(line);
                    }
                    System.Threading.Thread.Sleep(100);
                }
                steps++;
                frontier = nextFrontier;
            }
        }

        public WeightedGraph CreateGraph()
        {
            //Create a graph representation
            HashSet<(int, int)> portals = outerPortals.Union(innerPortals).ToHashSet();
            HashSet<(int, int)> coveredPortals = new HashSet<(int, int)>();
            WeightedGraph wgraph = new WeightedGraph();
            foreach ((int x, int y) portal in portals)
            {
                string pName = PortalName(portal);
                //Add Node if necessary
                if (!wgraph.Contains(pName))
                {
                    wgraph.AddNode(pName);
                }
                //Add Opposite-Portal node if necessary
                IEnumerable<(int x, int y)> potentialOpposites = portalLocations[portalLabels[portal.x][portal.y]].Where(pos => pos.x != portal.x || pos.y != portal.y);
                foreach (var oPortal in potentialOpposites)
                {
                    string opName = PortalName(oPortal);
                    if (!wgraph.Contains(opName))
                    {
                        wgraph.AddNode(opName);
                    }
                    //Add Edge to Opposite-Portal node if necessary
                    if (!coveredPortals.Contains(oPortal))
                    {
                        wgraph.AddConnection(pName, opName, 1);
                    }
                }

                //Find all reachable other portals
                List<(int x, int y, int dist)> reached = internalRep.FloodFind(portal, new List<string> { "%" });

                //Add Edges for all reachable uncovered Portals
                foreach (var pXYDist in reached)
                {
                    var rLoc = (pXYDist.x, pXYDist.y);
                    if (pXYDist.dist > 0 && !coveredPortals.Contains(rLoc))
                    {
                        string rName = PortalName(rLoc);
                        if (!wgraph.Contains(rName))                //Add Node if necessary
                        {
                            wgraph.AddNode(rName);
                        }
                        wgraph.AddConnection(pName, rName, pXYDist.dist);
                    }
                }

                coveredPortals.Add(portal);
            }
            return wgraph;
        }

        public int ShortestRecursivePathAAZZ()
        {
            WeightedGraph wgraph = CreateGraph();

            RecursiveDonutTreeNode root = new RecursiveDonutTreeNode(0, wgraph.Nodes["AA:OUT"], 0);
            RecursiveDonutTree tree = new RecursiveDonutTree(root, "ZZ:OUT");
            RecursiveDonutTreeNode result = tree.BestFirstSearch();
            RecursiveDonutTreeNode next = result;
            while (next.Parent != null)
            {
                Console.WriteLine(next);
                next = (RecursiveDonutTreeNode)next.Parent;
            }
            return result.Value;
        }

        private string PortalName((int x, int y) pLoc, bool opposite = false)
        {
            return portalLabels[pLoc.x][pLoc.y] + (opposite ^ innerPortals.Contains(pLoc) ? ":IN" : ":OUT");
        }

        public void PrintInternalRepresentation()
        {
            List<string> rows = internalRep.GetImageStrings(true);
            foreach (string row in rows)
            {
                Console.WriteLine(row);
            }
        }

        private void AddPortal(int xPos, int yPos, string pLabel)
        {
            AddPortalLabel(xPos, yPos, pLabel);
            AddPortalLocation(xPos, yPos, pLabel);
            internalRep.SetColorAt(xPos, yPos, "%");//Hardcoded, maybe should allow choosing portalSymbols
            if (yPos == edge.up || yPos == edge.down || xPos == edge.left || xPos == edge.right)
            {
                outerPortals.Add((xPos, yPos));
            }
            else
            {
                innerPortals.Add((xPos, yPos));
            }
            void AddPortalLabel(int x, int y, string label)
            {
                Dictionary<int, string> yToLabels;
                if (!portalLabels.TryGetValue(x, out yToLabels))
                {
                    yToLabels = new Dictionary<int, string>();
                    portalLabels.Add(x, yToLabels);
                }
                yToLabels.Add(y, label);
            }
            void AddPortalLocation(int x, int y, string label)
            {
                List<(int, int)> pLocs;
                if (!portalLocations.TryGetValue(label, out pLocs))
                {
                    pLocs = new List<(int, int)>();
                    portalLocations.Add(label, pLocs);
                }
                pLocs.Add((x, y));
            }
        }


    }
}

