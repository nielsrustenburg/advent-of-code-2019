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
        public static readonly HashSet<string> keys = alphabet.Select(c => c.ToString()).ToHashSet();
        public static readonly HashSet<string> doors = keys.Select(k => k.ToUpper()).ToHashSet();
        public static readonly HashSet<string> walls = new HashSet<string> { "#" };
        public static readonly HashSet<string> floors = new HashSet<string> { ".", "@" };

        Dictionary<string, uint> locationsToKeycodes;
        Dictionary<uint, string> keycodesToLocations;
        Dictionary<uint, uint> blockingsetPerKey; //for 0 <= x < 26,  maps 2^x to a uint < 2^26 representing which keys block picking up this key
        Dictionary<uint, int> keycodesToQuadrants;
        BasicGraph graph;

        IEnumerable<string> mazeRows;

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
            locationsToKeycodes = new Dictionary<string, uint>();
            locationsToKeycodes.Add("@", 0);
            for (int a = 0; a < alphabet.Length; a++)
            {
                locationsToKeycodes.Add(alphabet[a].ToString(), (uint)1 << a);
            }
            keycodesToLocations = locationsToKeycodes.ToDictionary((kvp) => kvp.Value, (kvp) => kvp.Key);
            locationsToKeycodes.Add("0", 0); //Making sure that only keys have non-zero values
            locationsToKeycodes.Add("1", 0);
            locationsToKeycodes.Add("2", 0);
            locationsToKeycodes.Add("3", 0);

            var stringTilesByRowAndColumn = mazeRows.Select(row => row.Select(tile => tile.ToString()));
            var maze = new Maze<string>(stringTilesByRowAndColumn, "#", floors.Concat(keys).Select(tile => tile.ToString()));
            var floorsAndDoors = floors.Concat(doors).Select(tile => tile.ToString()).ToHashSet();
            maze.EliminateDeadEnds(floorsAndDoors, "#", walls.Select(tile => tile.ToString()).ToHashSet());

            var remainingKeysAndDoors = maze.GetAllTileTypes().ToHashSet();
            remainingKeysAndDoors.ExceptWith(new string[] { "@", ".", "#" });

            var quadRoots = new Dictionary<string, (int, int)>();
            var centerTile = ((int x,int y)) maze.FindFirstMatchingTile("@");
            maze[centerTile.x, centerTile.y] = "#";
            int intersectionCounter = 0;

            //Mark the corners around the center as intersections
            foreach (var direction in Direction.North.GetAll())
            {
                var (x, y) = DirectionHelper.NeighbourInDirection(direction, centerTile.x, centerTile.y);
                if (x == centerTile.x || y == centerTile.y)
                {
                    maze[x, y] = "#";
                }
                else
                {
                    maze[x, y] = intersectionCounter.ToString();
                    quadRoots.Add(intersectionCounter.ToString(), (x, y));
                    intersectionCounter++;
                }
            }

            var walkableTiles = floors.Concat(doors).Concat(keys).Select(tile => tile.ToString());

            //Do a floodfill of the remaining maze, changing the tile-type every time we encounter a door/key/intersection
            //tile-types will be built as follows [].[].[] where each[] is a door/key/intersection number

            var intersectionBranches = new HashSet<string>();

            var frontiers = new List<(int, int)>(quadRoots.Values);
            while (frontiers.Any())
            {
                var nextFrontiers = new List<(int, int)>();

                foreach (var (x, y) in frontiers)
                {
                    var currentTileType = maze[x, y];
                    var relevantNeighbours = maze.GetNeighbours(x, y).Where(nb => walkableTiles.Contains(nb.Value));
                    var keysOrDoors = relevantNeighbours.Where(nb => keys.Contains(nb.Value) || doors.Contains(nb.Value)).ToList();

                    if (relevantNeighbours.Count() > 1)
                    {
                        //this is an intersection an edge ends here, and 2 or 3 new edges start
                        maze[x, y] = $"{currentTileType}.";
                        int branchCounter = 0;
                        foreach (var nb in relevantNeighbours)
                        {
                            var (nbx, nby) = DirectionHelper.NeighbourInDirection(nb.Key, x, y);
                            var intersectionBranch = $"{currentTileType}.{intersectionCounter}:{branchCounter}";
                            maze[nbx, nby] =  intersectionBranch;
                            branchCounter++;
                            nextFrontiers.Add((nbx, nby));
                            intersectionBranches.Add(intersectionBranch);
                        }
                        intersectionCounter++;
                    }
                    else
                    {
                        if (relevantNeighbours.Any())
                        {
                            var (nbx, nby) = DirectionHelper.NeighbourInDirection(relevantNeighbours.First().Key, x, y);
                            maze[nbx, nby] = currentTileType;
                            nextFrontiers.Add((nbx, nby));
                        }
                    }

                    foreach (var nb in keysOrDoors)
                    {
                        //next tile will be a key or door, meaning the edge of this frontier will end there
                        var (nbx, nby) = DirectionHelper.NeighbourInDirection(nb.Key, x, y);
                        var updatedCurrentTileType = maze[nbx, nby];
                        maze[nbx, nby] = $"{updatedCurrentTileType}.{nb.Value}";
                    }
                }
                frontiers = nextFrontiers;
            }

            //intersections are marked by a . as final character, this means each branch has a count of 1 less than it should be
            var tileCounts = new Dictionary<string, int>(maze.GetTileCounts().Where(tc => tc.Value > 0 && tc.Key.Last() != '.'));
            foreach (var intersectionBranch in intersectionBranches)
            {
                tileCounts.TryGetValue(intersectionBranch, out int value);
                tileCounts[intersectionBranch] = value + 1;
            }

            //Build a graph
            graph = new BasicGraph();
            //Create nodes
            graph.AddNode("@");
            foreach (var (tile, count) in tileCounts)
            {
                var splitTile = tile.Split('.');
                var endpoint = splitTile.Last().Split(':');
                if (!graph.ContainsNode(endpoint.First()))
                {
                    graph.AddNode(endpoint.First());
                }
            }

            keycodesToQuadrants = new Dictionary<uint, int>();
            //Create edges & blockingsets
            blockingsetPerKey = new Dictionary<uint, uint>();
            foreach (var (tile, _) in tileCounts)
            {
                var splitTile = tile.Split('.').Select(loc => loc.Split(':').First()).ToArray();
                if (splitTile.Length > 1)
                {
                    var from = splitTile[splitTile.Length - 2];
                    var to = splitTile[splitTile.Length - 1];
                    var lastPeriod = tile.LastIndexOf('.');
                    var edgeLength = tileCounts[tile.Substring(0, lastPeriod)];
                    graph.AddEdge(from, to, edgeLength);
                    if (keys.Contains(to))
                    {
                        var blockingKeys = splitTile.Take(splitTile.Length - 1).Select(nodeName => nodeName.ToLower()).Where(keyName => keys.Contains(keyName)).Distinct();
                        uint blockingCode = 0;
                        foreach (var blockingKey in blockingKeys)
                        {
                            blockingCode = blockingCode | locationsToKeycodes[blockingKey];
                        }
                        blockingsetPerKey.Add(locationsToKeycodes[to], blockingCode);
                        keycodesToQuadrants.Add(locationsToKeycodes[to], int.Parse(splitTile.First()));
                    }
                }
            }

            //Add edges between @,0,1,2,3
            foreach (var quadrantStart in Enumerable.Range(0, 4).Select(q => q.ToString()))
            {
                graph.AddEdge("@", quadrantStart, 2);
            }
            graph.AddEdge("0", "1", 2);
            graph.AddEdge("1", "2", 2);
            graph.AddEdge("2", "3", 2);
            graph.AddEdge("3", "0", 2);

        }

        protected override void SolvePartOne()
        {
            uint target = ~(uint)0 >> 6;
            var initialSearchNode = new MazeSearchNode(0, "@", target, this, 0);
            var result = Search<HeapPriorityQueue<SearchNode>>.Execute(initialSearchNode);
            resultPartOne = result.cost.ToString();
        }

        protected override void SolvePartTwo()
        {
            uint target = ~(uint)0 >> 6;
            var startLocations = new string[] { "0", "1", "2", "3" };
            var initialSearchNode = new QuadrantSearchNode(0, startLocations, target, this, 0);
            var result = Search<HeapPriorityQueue<SearchNode>>.Execute(initialSearchNode);
            resultPartTwo = result.cost.ToString();
        }

        public static IEnumerable<uint> ExtractKeys(uint keys, int maxKeys = 26)
        {
            for (int i = 0; i < maxKeys; i++)
            {
                if ((keys & 1) == 1)
                {
                    yield return (uint)1 << i;
                }
                keys = keys >> 1;
            }
        }

        class MazeSearchNode : SearchNode
        {
            uint keyring;
            uint missingKeys;
            string location;// not sure if int
            uint target;
            Solver environment;

            public MazeSearchNode(uint keyring, string location, uint target, Solver environment, int cost, MazeSearchNode parent = null) : base(cost, parent)
            {
                this.keyring = keyring;
                this.location = location;
                this.target = target;
                this.environment = environment;
                missingKeys = target ^ keyring;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(this, obj)) return true;
                if (obj == null) return false;
                var other = obj as MazeSearchNode;
                if (other.location != location) return false;
                return keyring == other.keyring;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hash = 179;
                    hash = hash * 167 + (int)environment.locationsToKeycodes[location];
                    hash = hash * 167 + (int)keyring;
                    return hash;
                }
            }

            public override int GetHeuristicCost()
            {
                return 0;
            }

            public override SearchNode[] GetNeighbours()
            {
                IEnumerable<SearchNode> CreateNeighbourNodes()
                {
                    //for every missing key check if keyring & requiredkeys == requiredkeys
                    foreach (var key in ExtractKeys(missingKeys))
                    {
                        if ((environment.blockingsetPerKey[key] & keyring) == environment.blockingsetPerKey[key])
                        {
                            var distance = environment.graph.GetDistance(location, environment.keycodesToLocations[key]);
                            var newkeyring = keyring | key;
                            var newlocation = environment.keycodesToLocations[key];
                            yield return new MazeSearchNode(newkeyring, newlocation, target, environment, cost + distance, this);
                        }
                    }
                }
                return CreateNeighbourNodes().ToArray();
            }

            public override bool IsAtTarget()
            {
                return keyring == target;
            }
        }

        class QuadrantSearchNode : SearchNode

        {
            uint keyring;
            uint missingKeys;
            string[] locations;
            uint target;
            Solver environment;

            public QuadrantSearchNode(uint keyring, string[] locations, uint target, Solver environment, int cost, QuadrantSearchNode parent = null) : base(cost, parent)
            {
                this.keyring = keyring;
                this.locations = locations;
                this.target = target;
                this.environment = environment;
                missingKeys = target ^ keyring;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(this, obj)) return true;
                if (obj == null) return false;
                var other = obj as QuadrantSearchNode;
                for(int i = 0; i < 4; i++)
                {
                    if (other.locations[i] != locations[i]) return false;
                }
                return keyring == other.keyring;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hash = 179;
                    for (int i = 0; i < 4; i++)
                    {
                        hash = hash * 167 + (int) environment.locationsToKeycodes[locations[i]];
                    }
                    hash = hash * 167 + (int)keyring;
                    return hash;
                }
            }

            public override int GetHeuristicCost()
            {
                return 0;
            }

            public override SearchNode[] GetNeighbours()
            {
                IEnumerable<SearchNode> CreateNeighbourNodes()
                {
                    //for every missing key check if keyring & requiredkeys == requiredkeys
                    foreach (var key in ExtractKeys(missingKeys))
                    {
                        if ((environment.blockingsetPerKey[key] & keyring) == environment.blockingsetPerKey[key])
                        {
                            var distance = environment.graph.GetDistance(locations[environment.keycodesToQuadrants[key]], environment.keycodesToLocations[key]);
                            var newkeyring = keyring | key;
                            var newlocations = locations.ToArray();
                            newlocations[environment.keycodesToQuadrants[key]] = environment.keycodesToLocations[key];
                            yield return new QuadrantSearchNode(newkeyring, newlocations, target, environment, cost + distance, this);
                        }
                    }
                }
                return CreateNeighbourNodes().ToArray();
            }

            public override bool IsAtTarget()
            {
                return keyring == target;
            }
        }
    }
}
