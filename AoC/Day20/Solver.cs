using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AoC.Common;
using AoC.Utils;
using AoC.Utils.AStar;

namespace AoC.Day20
{
    class Solver : PuzzleSolver
    {
        string[] mazeRows;
        Maze<string> maze;
        DonutGraph graph;

        public Solver() : this(Input.InputMode.Embedded, "input")
        {
        }

        public Solver(Input.InputMode mode, string input) : base(mode, input)
        {
        }

        protected override void ParseInput(string input)
        {
            mazeRows = InputParser.Split(input).ToArray();
        }

        protected override void PrepareSolution()
        {
            (maze, graph) = BuildDonutMaze(mazeRows);
        }

        protected override void SolvePartOne()
        {
            var (path, dist) = GraphSearch.Dijkstra(graph, "AA:OUT", "ZZ:OUT");
            resultPartOne = dist.ToString(); 
        }

        protected override void SolvePartTwo()
        {
            var aaStart = new RecursiveDonutSearchNode(graph, "AA:OUT", 0, "ZZ:OUT", 0);
            var zzEnd = Search<NaivePriorityQueue<SearchNode>>.Execute(aaStart);
            resultPartTwo = zzEnd.cost.ToString();
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
                    if (part1.Key == Direction.North)
                    {
                        portalName = maze.GetTile(x, y + 2) + maze.GetTile(x, y + 1);
                        maze.SetTile(x, y + 2, " ");
                        maze.SetTile(x, y + 1, " ");
                    }
                    else if (part1.Key == Direction.East)
                    {
                        portalName = maze.GetTile(x + 1, y) + maze.GetTile(x + 2, y);
                        maze.SetTile(x + 1, y, " ");
                        maze.SetTile(x + 2, y, " ");
                    }
                    else if (part1.Key == Direction.South)
                    {
                        portalName = maze.GetTile(x, y - 1) + maze.GetTile(x, y - 2);
                        maze.SetTile(x, y - 2, " ");
                        maze.SetTile(x, y - 1, " ");
                    }
                    else if (part1.Key == Direction.West)
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
            var graph = new DonutGraph(thickness);
            var existingPortals = maze.GetAllTileTypes().Where(tile => tile.Length > 1).ToHashSet();

            foreach (string portal in existingPortals)
            {
                graph.AddNode(portal);
            }

            foreach (var innerPortal in graph.Nodes().Where(portal => graph.IsInnerPortal(portal)))
            {
                var otherSide = graph.GetLabel(innerPortal) + ":OUT";
                if (graph.ContainsNode(otherSide))
                {
                    graph.AddEdge(innerPortal, otherSide, 1);
                }
            }

            //For every portal set up the reachable locations
            foreach (string portal in existingPortals)
            {
                (bool _, int x, int y) = maze.FindFirstMatchingTile(portal);
                (int _, List<(string tile, int distance)> reachablePortals) = maze.FloodFillDistanceFinder(x, y, existingPortals);
                foreach ((string reachable, int distance) in reachablePortals)
                {
                    graph.AddEdge(portal, reachable, distance);
                }
            }


            return (maze, graph);
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
            throw new Exception("Should've found the inside before reaching half the donut width");
        }
    }

    class DonutGraph : AdjacencyGraph<DonutGraphNode>
    {
        public readonly int donutThickness; //useful for A* heuristic

        public DonutGraph(int thickness) : base()
        {
            donutThickness = thickness;
        }

        public void AddNode(string name)
        {
            nodes.Add(name, new DonutGraphNode(name));
        }

        public bool IsInnerPortal(string name)
        {
            return nodes[name].IsInnerPortal;
        }

        public string GetLabel(string name)
        {
            return nodes[name].label;
        }
    }
    class DonutGraphNode : AdjacencyGraphNode
    {
        public bool IsInnerPortal { get; }
        public readonly string label;

        public DonutGraphNode(string name) : base(name)
        {
            var split = name.Split(':');
            label = split[0];
            IsInnerPortal = split[1] == "IN";
        }
    }
    class RecursiveDonutSearchNode : SearchNode
    {
        string portal;
        string target;
        int layer;
        DonutGraph graph;

        public RecursiveDonutSearchNode(DonutGraph graph, string portal, int layer, string target, int cost, RecursiveDonutSearchNode parent = null) : base(cost, parent)
        {
            this.graph = graph;
            this.portal = portal;
            this.layer = layer;
            this.target = target;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj == null) return false;
            var other = obj as RecursiveDonutSearchNode;
            return layer == other.layer && portal == other.portal && target == other.target;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 99877;
                hash = hash * 90371 + layer.GetHashCode();
                hash = hash * 90371 + portal.GetHashCode();
                hash = hash * 90371 + target.GetHashCode();
                return hash;
            }
        }

        public override int GetHeuristicCost()
        {
            return graph.donutThickness * layer;
        }

        public override SearchNode[] GetNeighbours()
        {
            var neighbours = graph.GetReachableNodes(portal);
            return GenerateSearchNodes().ToArray();

            IEnumerable<SearchNode> GenerateSearchNodes()
            {
                foreach (var neighbour in neighbours)
                {
                    var nbLayer = layer;
                    if (graph.GetLabel(portal) == graph.GetLabel(neighbour.node))
                    {
                        nbLayer = nbLayer + (graph.IsInnerPortal(portal) ? 1 : -1);
                    }

                    if (nbLayer >= 0)
                    {
                        yield return new RecursiveDonutSearchNode(graph, neighbour.node, nbLayer, target, cost + neighbour.weight, this);
                    }
                }
            }
        }

        public override bool IsAtTarget()
        {
            return layer == 0 && portal == target;
        }
    }
}
