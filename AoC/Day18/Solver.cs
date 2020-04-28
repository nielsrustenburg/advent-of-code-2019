using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AoC.Common;
using AoC.Utils;
using AoC.Utils.AStar;

namespace AoC.Day18
{

    class Solver : PuzzleSolver
    {
        public const string alphabet = "abcdefghijklmnopqrstuvwxyz";
        public static readonly HashSet<char> keys = alphabet.ToHashSet();
        public static readonly HashSet<char> doors = alphabet.ToUpper().ToHashSet();
        public static readonly HashSet<char> walls = new HashSet<char> { '#' };
        public static readonly HashSet<char> floors = new HashSet<char> { '.', '@' };
        public static readonly HashSet<char> doorsAndKeys = doors.Union(keys).ToHashSet();

        IEnumerable<string> mazeRows;
        Maze<char> lockedDoorMaze;
        Maze<char> unlockedDoorMaze;
        Dictionary<char, HashSet<char>> keysRequiredToAccessKey;
        Dictionary<char, Dictionary<char, int>> distanceBetweenKeys;
        QuadrantTreeEdgeSet initialQTEdgeSet;

        public Solver() : this(Input.InputMode.Embedded, "input")
        {
        }

        public Solver(Input.InputMode mode, string input) : base(mode, input)
        {
        }

        protected override void ParseInput(string input)
        {
            mazeRows = InputParser.Split(input);
        }

        protected override void PrepareSolution()
        {
            var tilesByRowAndColumn = mazeRows.Select(row => row.ToCharArray());
            lockedDoorMaze = new Maze<char>(tilesByRowAndColumn, '#', floors.Concat(keys));
            unlockedDoorMaze = new Maze<char>(tilesByRowAndColumn, '#', floors.Concat(keys).Concat(doors));

            keysRequiredToAccessKey = FindKeysRequiredToAccessKey();
            distanceBetweenKeys = FindDistances();

            initialQTEdgeSet = CreateQuadrantTrees();
        }

        protected override void SolvePartOne()
        {
            var searchNode = new MazeSearchNode('@', new char[0], keysRequiredToAccessKey, distanceBetweenKeys, initialQTEdgeSet, 0);
            var result = Search<HeapPriorityQueue<SearchNode>>.Execute(searchNode);
            var resultPartOne = result.cost.ToString();
        }

        protected override void SolvePartTwo()
        {
            ////Split the maze into four quadrants
            //Maze<string> fourMaze = new Maze<string>(maze);
            //fourMaze.SetTile(xStart + 1, yStart, "#");
            //fourMaze.SetTile(xStart, yStart - 1, "#");
            //fourMaze.SetTile(xStart - 1, yStart, "#");
            //fourMaze.SetTile(xStart, yStart + 1, "#");

            ////Create distances-graph
            //TritonGraph fourGraph = new TritonGraph(fourMaze, keys, doors, pointsOfInterest);

            //List<string> bestSequences = new List<string> { "1saiwe", "2qcvrtu", "3zmohjlfkdypgn", "4xb" }; //handmade
            //resultPartTwo =  ValidateQuadraBotSequence(fourGraph, bestSequences).ToString();
        }

        private Dictionary<char, HashSet<char>> FindKeysRequiredToAccessKey()
        {
            var result = new Dictionary<char, HashSet<char>>();

            var reachablePoIsFromTile = new Dictionary<char, HashSet<char>>();
            var (_, x, y) = lockedDoorMaze.FindFirstMatchingTile('@');
            var (_, reachablePoIs) = lockedDoorMaze.FloodFillDistanceFinder(x, y, doorsAndKeys);
            reachablePoIsFromTile.Add('@', reachablePoIs.Select(poi => poi.tile).ToHashSet());

            foreach (var door in doors)
            {
                (_, x, y) = lockedDoorMaze.FindFirstMatchingTile(door);
                (_, reachablePoIs) = lockedDoorMaze.FloodFillDistanceFinder(x, y, doorsAndKeys);
                reachablePoIsFromTile.Add(door, reachablePoIs.Select(poi => poi.tile).ToHashSet());
            }

            IdentifyAndRecordNewlyReachedKeys('@', new List<char>());

            return result;

            void IdentifyAndRecordNewlyReachedKeys(char current, List<char> path)
            {
                HashSet<char> newlyReached;
                if (!path.Any())
                {
                    newlyReached = reachablePoIsFromTile[current].ToHashSet();
                }
                else
                {
                    newlyReached = reachablePoIsFromTile[current].Except(reachablePoIsFromTile[path.Last()]).ToHashSet();
                    newlyReached.Remove(path.Last());
                }

                foreach (var key in newlyReached.Intersect(keys))
                {
                    var blockingSet = path.Select(door => char.ToLower(door)).ToHashSet();
                    blockingSet.Add(char.ToLower(current));
                    blockingSet.Remove('@');
                    result.Add(key, blockingSet);
                }

                foreach (var door in newlyReached.Intersect(doors))
                {
                    var nextPath = path.ToList();
                    nextPath.Add(current);
                    IdentifyAndRecordNewlyReachedKeys(door, nextPath);
                }
            }
        }

        private Dictionary<char, Dictionary<char, int>> FindDistances()
        {
            var result = new Dictionary<char, Dictionary<char, int>>();
            var sortedRelevantLocations = keys.Union(new char[] { '@' }).ToArray();
            var coordinates = sortedRelevantLocations.Select(key => unlockedDoorMaze.FindFirstMatchingTile(key)).Select(coords => (coords.x, coords.y)).ToArray();

            foreach (var key in sortedRelevantLocations)
            {
                result.Add(key, new Dictionary<char, int>());
            }

            for (int keyId = 0; keyId < sortedRelevantLocations.Length - 1; keyId++)
            {
                var (x, y) = coordinates[keyId];
                var (_, pois) = unlockedDoorMaze.FloodFillDistanceFinder(x, y, sortedRelevantLocations.Skip(keyId + 1));
                foreach (var (key, dist) in pois)
                {
                    result[sortedRelevantLocations[keyId]].Add(key, dist);
                    result[key].Add(sortedRelevantLocations[keyId], dist);
                }
            }
            return result;
        }

        private QuadrantTreeEdgeSet CreateQuadrantTrees()
        {
            //Make a separate maze for heavy abuse (one time use only)
            var stringTilesByRowAndColumn = mazeRows.Select(row => row.Select(tile => tile.ToString()));
            var maze = new Maze<string>(stringTilesByRowAndColumn, "#", floors.Concat(keys).Select(tile => tile.ToString()));
            var floorsAndDoors = floors.Concat(doors).Select(tile => tile.ToString()).ToHashSet();
            maze.EliminateDeadEnds(floorsAndDoors, "#", walls.Select(tile => tile.ToString()).ToHashSet());

            var quadRoots = new Dictionary<string, (int, int)>();
            var centerTile = maze.FindFirstMatchingTile("@");
            maze.SetTile(centerTile.x, centerTile.y, "#");
            int cornerCounter = 0;
            foreach (var direction in Direction.North.GetAll())
            {
                var (x, y) = DirectionHelper.NeighbourInDirection(direction, centerTile.x, centerTile.y);
                if (x == centerTile.x || y == centerTile.y)
                {
                    maze.SetTile(x, y, "#");
                }
                else
                {
                    maze.SetTile(x, y, cornerCounter.ToString());
                    quadRoots.Add(cornerCounter.ToString(), (x, y));
                    cornerCounter++;
                }
            }

            maze.RowsAsStrings().ForEach(row => Console.WriteLine(row));
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            var walkableTiles = floors.Concat(doors).Concat(keys).Select(tile => tile.ToString());

            var frontiers = new List<(int, int)>(quadRoots.Values);
            while (frontiers.Any())
            {
                var nextFrontiers = new List<(int, int)>();

                foreach (var (x, y) in frontiers)
                {
                    var currentTileType = maze.GetTile(x, y);
                    var relevantNeighbours = maze.GetNeighbours(x, y).Where(nb => walkableTiles.Contains(nb.Value));
                    var keyNeighbours = relevantNeighbours.Where(nb => keys.Contains(nb.Value.Last())).ToList();

                    if (relevantNeighbours.Count() > 1)
                    {
                        //this is an intersection an edge ends here, and 2 or 3 new edges start
                        maze.SetTile(x, y, $"{currentTileType}.");
                        int branchCounter = 0;
                        foreach (var nb in relevantNeighbours)
                        {
                            var (nbx, nby) = DirectionHelper.NeighbourInDirection(nb.Key, x, y);
                            maze.SetTile(nbx, nby, $"{currentTileType}.{branchCounter}");
                            branchCounter++;
                            nextFrontiers.Add((nbx, nby));
                        }
                    }
                    else
                    {
                        if (relevantNeighbours.Any())
                        {
                            var (nbx, nby) = DirectionHelper.NeighbourInDirection(relevantNeighbours.First().Key, x, y);
                            maze.SetTile(nbx, nby, currentTileType);
                            nextFrontiers.Add((nbx, nby));
                        }
                    }

                    foreach (var nb in keyNeighbours)
                    {
                        //next tile will be a key, meaning the edge of this frontier will end there
                        var (nbx, nby) = DirectionHelper.NeighbourInDirection(nb.Key, x, y);
                        maze.SetTile(nbx, nby, $"{currentTileType}.{nb.Value}");
                    }
                }
                frontiers = nextFrontiers;
            }
            var rawTileCounts = new Dictionary<string, int>(maze.GetTileCounts().Where(tc => tc.Value > 0));
            var updatedTileCounts = new Dictionary<string, int>();
            var endPoints = new List<string>();
            foreach (var (tile, count) in rawTileCounts)
            {
                if (tile.Last() != '.')
                {
                    var potentialParentIntersection = tile.Substring(0, tile.Length - 1);
                    if (rawTileCounts.ContainsKey(potentialParentIntersection))
                    {
                        if (rawTileCounts[potentialParentIntersection] > 1) System.Diagnostics.Debugger.Break();
                        updatedTileCounts.Add(tile, count + 1);
                    }
                    else
                    {
                        if (count > 1) //if == 1 it is an endpoint, won't need an edge
                        {
                            updatedTileCounts.Add(tile, count);
                        }
                    }
                    endPoints.Add(tile);
                }
            }

            var edges = new Dictionary<string, QuadrantTreeEdge>();
            foreach (var (tile, count) in updatedTileCounts.OrderBy(tc => tc.Key.Length))
            {
                var largestEndPointBelow = endPoints.Aggregate(tile, (a, b) => (b.Length > a.Length && tile == b.Substring(0, tile.Length)) ? b : a);
                if (largestEndPointBelow != tile)
                {
                    var keysAbove = tile.Where(c => keys.Contains(c)).ToHashSet();
                    var keysBelow = largestEndPointBelow.Where(c => keys.Contains(c) && !keysAbove.Contains(c));
                    var name = $"{tile}:{largestEndPointBelow.Substring(0, tile.Length + 2)}";
                    var parent = tile.Length > 2 ? edges[tile.Substring(0, tile.Length - 2)] : null;
                    var edge = new QuadrantTreeEdge(name, count, parent);
                    edges.Add(tile, edge);
                }
            }

            return new QuadrantTreeEdgeSet(new QuadrantTreeEdge[] { edges["0"], edges["1"], edges["2"], edges["3"] });
        }

        private List<string> MarkIntersections(Grid<string> maze)
        {
            var potentialIntersections = maze.FindAllMatchingTiles(".");
            var walkable = floors.Concat(doors).Concat(keys).Select(tile => tile.ToString()).ToHashSet();
            int intersectionCounter = 0;
            var intersectionTiles = new List<string>();
            foreach (var (x, y) in potentialIntersections)
            {
                var neighbours = maze.GetNeighbours(x, y);
                var walkableNeighbourCount = neighbours.Values.Count(nb => walkable.Contains(nb));
                if (walkableNeighbourCount >= 3)
                {
                    intersectionCounter++;
                    var intersectionTile = $":{intersectionCounter}:";
                    maze.SetTile(x, y, intersectionTile);
                    intersectionTiles.Add(intersectionTile);
                    if (walkableNeighbourCount == 4) System.Diagnostics.Debugger.Break();
                }
            }
            return intersectionTiles;
        }



        //private static int ValidateKeySequence(TritonGraph graph, List<string> sequence)
        //{
        //    int totalDist = 0;
        //    HashSet<string> keysAcquired = new HashSet<string>();
        //    bool validPath = true;
        //    while (validPath && sequence.Count > 1)
        //    {
        //        (bool reachable, int distance) = graph.Distance(sequence[0], sequence[1], keysAcquired);
        //        validPath = reachable;
        //        if (reachable)
        //        {
        //            keysAcquired.Add(sequence[1]);
        //            totalDist += distance;
        //            sequence.RemoveAt(0);
        //        }
        //    }

        //    return validPath ? totalDist : -1;
        //}

        //private static int ValidateQuadraBotSequence(TritonGraph graph, List<string> sequences)
        //{
        //    int totalDist = 0;
        //    List<int> botDists = new List<int> { 0, 0, 0, 0 };
        //    List<string> bots = new List<string> { "1", "2", "3", "4" };
        //    List<List<string>> botSequences = sequences.Select(s => s.ToCharArray().Select(x => x.ToString()).ToList()).ToList();

        //    HashSet<string> keysAcquired = new HashSet<string>();
        //    bool atleastOneMoved = true;
        //    while (atleastOneMoved && botSequences.Sum(s => s.Count) > 4)
        //    {
        //        atleastOneMoved = false;
        //        for (int i = 0; i < bots.Count; i++)
        //        {
        //            if (botSequences[i].Count > 1)
        //            {
        //                (bool reachable, int distance) = graph.Distance(botSequences[i][0], botSequences[i][1], keysAcquired);
        //                if (reachable)
        //                {
        //                    keysAcquired.Add(botSequences[i][1]);
        //                    totalDist += distance;
        //                    botDists[i] += distance;
        //                    botSequences[i].RemoveAt(0);
        //                    atleastOneMoved = true;
        //                    break;
        //                }
        //            }
        //        }
        //    }

        //    return atleastOneMoved ? totalDist : -1;
        //}
    }

    class QuadrantTreeEdgeSet
    {
        List<QuadrantTreeEdge> activeRootEdges;
        HashSet<QuadrantTreeEdge> activeLeafEdges;

        Dictionary<QuadrantTreeEdge, bool> includedEdges;
        Dictionary<char, QuadrantTreeEdge> keyLocationsToEdgeBelow;
        int _totalEdgeValues;

        public int TotalEdgeValues { get { return _totalEdgeValues - activeRootEdges.Max(le => le.Length); } }

        public QuadrantTreeEdgeSet(IEnumerable<QuadrantTreeEdge> qtrees)
        {
            _totalEdgeValues = 0;
            includedEdges = new Dictionary<QuadrantTreeEdge, bool>();
            keyLocationsToEdgeBelow = new Dictionary<char, QuadrantTreeEdge>();
            activeLeafEdges = new HashSet<QuadrantTreeEdge>();

            var edges = qtrees.ToList();
            while (edges.Any())
            {
                var nextEdges = new List<QuadrantTreeEdge>();
                foreach (var edge in edges)
                {
                    includedEdges.Add(edge, true);
                    _totalEdgeValues += edge.Length;
                    nextEdges.AddRange(edge.GetEdgesBelow());
                    if (Solver.keys.Contains(edge.name.Last()))
                    {
                        keyLocationsToEdgeBelow.Add(edge.name.Last(), edge);
                    }
                    if (!edge.GetEdgesBelow().Any())
                    {
                        activeLeafEdges.Add(edge);
                    }
                }
                edges = nextEdges;
            }

            activeRootEdges = qtrees.ToList();
        }

        public QuadrantTreeEdgeSet(QuadrantTreeEdgeSet copyMe, char previousLocation, char location, HashSet<char> keys)
        {
            _totalEdgeValues = copyMe._totalEdgeValues;
            keyLocationsToEdgeBelow = copyMe.keyLocationsToEdgeBelow;
            includedEdges = new Dictionary<QuadrantTreeEdge, bool>(copyMe.includedEdges);
            keyLocationsToEdgeBelow.TryGetValue(previousLocation, out var relevantOuterEdge);
            activeRootEdges = copyMe.activeRootEdges.ToList();
            activeLeafEdges = copyMe.activeLeafEdges.ToHashSet();

            while (relevantOuterEdge != null && relevantOuterEdge.IsObsoleteShouldUpdateAbove(location, keys))
            {
                _totalEdgeValues -= relevantOuterEdge.Length;
                includedEdges[relevantOuterEdge] = false;
                activeLeafEdges.Remove(relevantOuterEdge);
                var parentEdge = relevantOuterEdge.GetEdgeAbove();
                if (parentEdge == null)
                {
                    activeRootEdges.Remove(relevantOuterEdge);
                }
                else
                {
                    activeLeafEdges.Add(parentEdge);
                }
                relevantOuterEdge = parentEdge;
            }

            bool potentialToEliminateRoot = activeRootEdges.Count == 1;
            while (potentialToEliminateRoot)
            {
                //once 3 of the 4 quadrant trees have been eliminated we can also eliminate edges from the center
                var activeRootEdgeChildren = activeRootEdges[0].GetEdgesBelow().Where(e => includedEdges[e]).ToArray();
                if (activeRootEdgeChildren.Length == 1)
                {
                    _totalEdgeValues -= activeRootEdges[0].Length;
                    includedEdges[activeRootEdges[0]] = false;
                    activeRootEdges[0] = activeRootEdgeChildren[0];
                    activeLeafEdges.Remove(activeRootEdges[0]); //Not sure if necessary
                }
                else
                {
                    potentialToEliminateRoot = false;
                }
            }
        }
    }

    class QuadrantTreeEdge : IComparable<QuadrantTreeEdge>
    {
        public readonly string name;
        public int Length { get; private set; }
        HashSet<char> keysAbove;
        HashSet<char> keysBelow;
        QuadrantTreeEdge edgeAbove;
        List<QuadrantTreeEdge> edgesBelow;

        public QuadrantTreeEdge(string name, int length, QuadrantTreeEdge edgeAbove = null)
        {
            this.name = name;
            this.Length = length;
            edgesBelow = new List<QuadrantTreeEdge>();

            keysBelow = new HashSet<char>();           //filled when adding edges below
            keysAbove = Solver.keys.ToHashSet();       //keys will be removed when adding edges below

            var maybeKeyName = name.Last();
            if (Solver.keys.Contains(maybeKeyName))
            {
                keysBelow.Add(maybeKeyName);
                keysAbove.Remove(maybeKeyName);
            }

            this.edgeAbove = edgeAbove;
            if (edgeAbove != null) edgeAbove.AddEdgeBelow(this);
        }

        public void AddEdgeBelow(QuadrantTreeEdge below)
        {
            edgesBelow.Add(below);
            UpdateKeys(below.keysBelow);
        }

        public void UpdateKeys(HashSet<char> newBelow)
        {
            keysBelow.UnionWith(newBelow);
            keysAbove.ExceptWith(newBelow);
            if (edgeAbove != null) edgeAbove.UpdateKeys(newBelow);
        }

        public bool IsObsoleteShouldUpdateAbove(char location, HashSet<char> keys)
        {
            return !keysBelow.Except(keys).Any() && keysAbove.Contains(location);
        }

        public bool IsObsoleteShouldUpdateBelow(char location, HashSet<char> keys)
        {
            return !keysAbove.Except(keys).Any() && keysBelow.Contains(location);
        }

        public QuadrantTreeEdge GetEdgeAbove()
        {
            return edgeAbove;
        }

        public IEnumerable<QuadrantTreeEdge> GetEdgesBelow()
        {
            foreach (var edge in edgesBelow)
            {
                yield return edge;
            }
        }

        public bool IsRoot()
        {
            return edgeAbove == null;
        }

        public bool IsLeaf()
        {
            return !edgesBelow.Any();
        }

        public override string ToString()
        {
            return name;
        }

        public int CompareTo(QuadrantTreeEdge other)
        {
            return Length.CompareTo(other.Length);
        }
    }

    class MazeSearchNode : SearchNode
    {
        char location; //a key or the '@' starting location
        HashSet<char> keys;
        SortedSet<char> sortedKeys; //need this for GetHashCode
        QuadrantTreeEdgeSet qteSet;

        HashSet<char> target;
        Dictionary<char, HashSet<char>> keysRequiredToAccessKey;
        Dictionary<char, Dictionary<char, int>> distanceBetweenKeys;


        public MazeSearchNode(char location, IEnumerable<char> carryingKeys, Dictionary<char, HashSet<char>> keysRequiredToAccessKey, Dictionary<char, Dictionary<char, int>> distanceBetweenKeys, QuadrantTreeEdgeSet qteSet, int distance, MazeSearchNode parent = null) : base(distance, parent)
        {
            this.location = location;
            keys = carryingKeys.ToHashSet();
            sortedKeys = new SortedSet<char>(carryingKeys);
            target = Solver.keys;
            this.keysRequiredToAccessKey = keysRequiredToAccessKey;
            this.distanceBetweenKeys = distanceBetweenKeys;
            this.qteSet = qteSet;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj == null) return false;
            var other = obj as MazeSearchNode;
            if (other.location != location) return false;
            return keys.SetEquals(other.keys);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 179;
                hash = hash * 167 + location.GetHashCode();
                foreach (var key in sortedKeys)
                {
                    hash = hash * 167 + key.GetHashCode();
                }
                return hash;
            }
        }

        //return 0; //dijkstra
        //return 10 * (26-keys.Count); //mindist between keys, not very good heuristic
        public override int GetHeuristicCost()
        {
            return qteSet.TotalEdgeValues;
            //return 10 * (26-keys.Count); mindist between keys, not very good heuristic
        }

        public override SearchNode[] GetNeighbours()
        {
            IEnumerable<SearchNode> CreateNeighbourNodes()
            {
                foreach (var key in target.Except(keys))
                {
                    if (!keysRequiredToAccessKey[key].Except(keys).Any())
                    {
                        var distance = distanceBetweenKeys[location][key];
                        var newKeys = keys.Concat(new char[] { key }).ToHashSet();
                        var newQteSet = new QuadrantTreeEdgeSet(qteSet, location, key, newKeys);
                        yield return new MazeSearchNode(key, newKeys, keysRequiredToAccessKey, distanceBetweenKeys, newQteSet, cost + distance, this);
                    }
                }
            }

            return CreateNeighbourNodes().ToArray();
        }

        public override bool IsAtTarget()
        {
            return !target.Except(keys).Any();
        }
    }
}
