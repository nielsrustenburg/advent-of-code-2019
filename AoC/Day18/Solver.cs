using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AoC.Common;
using AoC.Utils;

namespace AoC.Day18
{
    class Solver : PuzzleSolver
    {
        const string alphabet = "abcdefghijklmnopqrstuvwxyz";
        Maze<string> maze;
        HashSet<string> keys;
        HashSet<string> doors;
        HashSet<string> walls;
        HashSet<string> floors;
        HashSet<string> pointsOfInterest;
        int xStart;
        int yStart;
        TritonGraph tGraph;

        public Solver() : this(Input.InputMode.Embedded, "input")
        {
        }

        public Solver(Input.InputMode mode, string input) : base(mode, input)
        {
        }

        protected override void ParseInput(string input)
        {
            var rows = InputParser.Split(input);

            keys = alphabet.ToCharArray().Select(c => c.ToString()).ToHashSet();
            doors = keys.Select(k => k.ToUpper()).ToHashSet();
            walls = new HashSet<string> { "#" };
            floors = new HashSet<string> { ".", "@", "1", "2", "3", "4" };//I've modified my input s.t. my center has a number in each of its corners

            //Set new List<string> { '.' } as passable floors for graphs with less edges
            //Use floors instead if you want to know what is actually reachable from each position
            var tilesByRowAndColumn = rows.Select(r => r.ToCharArray().Select(c => c.ToString()));
            maze = new Maze<string>(tilesByRowAndColumn, "#", new List<string> { "." });
        }

        protected override void PrepareSolution()
        {
            (_, xStart, yStart) = maze.FindFirstMatchingTile("@");
            maze.SetTile(xStart + 1, yStart + 1, "1");
            maze.SetTile(xStart + 1, yStart - 1, "2");
            maze.SetTile(xStart - 1, yStart - 1, "3");
            maze.SetTile(xStart - 1, yStart + 1, "4");

            //Not necessary but makes the maze easier to interpret
            maze.EliminateDeadEnds(doors.Union(floors).ToHashSet(), "#", walls);

            //Create distances-graph
            pointsOfInterest = floors.ToHashSet();
            pointsOfInterest.Remove(".");
            tGraph = new TritonGraph(maze, keys, doors, pointsOfInterest);
        }

        protected override void SolvePartOne()
        {
            string bestSequence = "@qcaiwesxtuvrzbmohjlfkdypgn"; //Found by hand
            resultPartOne = ValidateKeySequence(tGraph, bestSequence.ToCharArray().Select(x => x.ToString()).ToList()).ToString();
        }

        protected override void SolvePartTwo()
        {
            //Split the maze into four quadrants
            Maze<string> fourMaze = new Maze<string>(maze);
            fourMaze.SetTile(xStart + 1, yStart, "#");
            fourMaze.SetTile(xStart, yStart - 1, "#");
            fourMaze.SetTile(xStart - 1, yStart, "#");
            fourMaze.SetTile(xStart, yStart + 1, "#");

            //Create distances-graph
            TritonGraph fourGraph = new TritonGraph(fourMaze, keys, doors, pointsOfInterest);

            List<string> bestSequences = new List<string> { "1saiwe", "2qcvrtu", "3zmohjlfkdypgn", "4xb" }; //handmade
            resultPartTwo =  ValidateQuadraBotSequence(fourGraph, bestSequences).ToString();
        }

        private static int ValidateKeySequence(TritonGraph graph, List<string> sequence)
        {
            int totalDist = 0;
            HashSet<string> keysAcquired = new HashSet<string>();
            bool validPath = true;
            while (validPath && sequence.Count > 1)
            {
                (bool reachable, int distance) = graph.Distance(sequence[0], sequence[1], keysAcquired);
                validPath = reachable;
                if (reachable)
                {
                    keysAcquired.Add(sequence[1]);
                    totalDist += distance;
                    sequence.RemoveAt(0);
                }
            }

            return validPath ? totalDist : -1;
        }

        private static int ValidateQuadraBotSequence(TritonGraph graph, List<string> sequences)
        {
            int totalDist = 0;
            List<int> botDists = new List<int> { 0, 0, 0, 0 };
            List<string> bots = new List<string> { "1", "2", "3", "4" };
            List<List<string>> botSequences = sequences.Select(s => s.ToCharArray().Select(x => x.ToString()).ToList()).ToList();

            HashSet<string> keysAcquired = new HashSet<string>();
            bool atleastOneMoved = true;
            while (atleastOneMoved && botSequences.Sum(s => s.Count) > 4)
            {
                atleastOneMoved = false;
                for (int i = 0; i < bots.Count; i++)
                {
                    if (botSequences[i].Count > 1)
                    {
                        (bool reachable, int distance) = graph.Distance(botSequences[i][0], botSequences[i][1], keysAcquired);
                        if (reachable)
                        {
                            keysAcquired.Add(botSequences[i][1]);
                            totalDist += distance;
                            botDists[i] += distance;
                            botSequences[i].RemoveAt(0);
                            atleastOneMoved = true;
                            break;
                        }
                    }
                }
            }

            return atleastOneMoved ? totalDist : -1;
        }
    }

    class TritonGraph : AdjacencyGraph<TritonNode>
    {
        public TritonGraph(Maze<string> maze, HashSet<string> keys, HashSet<string> doors, HashSet<string> otherPoIs)
        {
            foreach (string key in keys)
            {
                AddKeyNode(key);
            }

            foreach (string door in doors)
            {
                AddDoorNode(door);
            }

            foreach (string pointOfInterest in otherPoIs)
            {
                AddNode(pointOfInterest);
            }

            //add edges
            foreach (string node in nodes.Keys)
            {
                (bool found, int x, int y) = maze.FindFirstMatchingTile(node);
                if (found)
                {
                    (int totalSteps, List<(string tile, int distance)> poi) reachableNodes = maze.FloodFillDistanceFinder(x, y, nodes.Keys);
                    foreach ((string tile, int dist) in reachableNodes.poi)
                    {
                        AddEdge(node, tile, dist);
                    }
                }
            }

        }

        public void AddKeyNode(string name)
        {
            nodes.Add(name, new TritonNode(name));
        }

        public void AddNode(string name)
        {
            nodes.Add(name, new TritonNode(name));
        }

        public void AddDoorNode(string name)
        {
            nodes.Add(name, new TritonNode(name, new List<string> { name.ToLower() }));
        }

        public bool PassageAllowed(string node, HashSet<string> keys)
        {
            return nodes[node].PassageAllowed(keys);
        }

        public (bool reachable, int distance) Distance(string from, string to, HashSet<string> keys)
        {
            HashSet<string> visited = new HashSet<string>();
            List<(string name, int dist)> orderedNodes = new List<(string, int)>();
            orderedNodes.Add((from, 0));
            while (orderedNodes.Any())
            {
                var current = orderedNodes[0];
                orderedNodes.RemoveAt(0);
                if (current.name == to) return (true, current.dist);
                if (!visited.Contains(current.name))
                {
                    visited.Add(current.name);
                    foreach (var edge in Neighbours(current.name))
                    {
                        if (!visited.Contains(edge.nb) && PassageAllowed(edge.nb, keys))
                        {
                            int nextDist = current.dist + edge.weight;
                            int indexOfBetter = orderedNodes.FindLastIndex(n => n.dist <= nextDist);
                            orderedNodes.Insert(indexOfBetter + 1, (edge.nb, nextDist));
                        }
                    }
                }
            }
            return (false, int.MaxValue);
        }
    }

    class TritonNode : AdjacencyGraphNode
    {
        HashSet<string> requiredKeys;

        public TritonNode(string name) : base(name)
        {
            requiredKeys = new HashSet<string>();
        }

        public TritonNode(string name, IEnumerable<string> keyRequirements) : this(name)
        {
            requiredKeys.UnionWith(keyRequirements);
        }

        public bool PassageAllowed(IEnumerable<string> yourKeys)
        {
            return requiredKeys.Intersect(yourKeys).Count() == requiredKeys.Count;
        }
    }
}
