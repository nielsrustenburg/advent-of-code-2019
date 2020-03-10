using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC
{
    static class Day18
    {
        public static int SolvePartOne()
        {
            List<string> mazeInput = InputReader.StringsFromTxt("d18input.txt").ToList();
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            HashSet<string> keys = alphabet.ToCharArray().Select(c => c.ToString()).ToHashSet();
            HashSet<string> doors = keys.Select(k => k.ToUpper()).ToHashSet();
            HashSet<string> walls = new HashSet<string> { "#" };
            HashSet<string> floors = new HashSet<string> { ".", "@", "1", "2", "3", "4" };//I've modified my input s.t. my center has a number in each of its corners

            //Set new List<string> { '.' } as passable floors for graphs with less edges
            //Use floors instead if you want to know what is actually reachable from each position
            Maze<string> maze = new Maze<string>(mazeInput.Select(r => r.ToCharArray().Select(c => c.ToString())), "#", new List<string> { "." });

            //Use for visual clarity
            maze.EliminateDeadEnds(doors.Union(floors).ToHashSet(), "#", walls);

            //Create distances-graph
            HashSet<string> pointsOfInterest = floors.ToHashSet();
            pointsOfInterest.Remove(".");
            TritonGraph tGraph = new TritonGraph(maze, keys, doors, pointsOfInterest);

            string bestSequence = "@qcaiwesxtuvrzbmohjlfkdypgn"; //Found by hand

            return ValidateKeySequence(tGraph, bestSequence.ToCharArray().Select(x => x.ToString()).ToList());
        }

        public static int ValidateKeySequence(TritonGraph graph, List<string> sequence)
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

        public static int SolvePartTwo()
        {
            List<string> mazeInput = InputReader.StringsFromTxt("d18inputP2.txt").ToList();
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            HashSet<string> keys = alphabet.ToCharArray().Select(c => c.ToString()).ToHashSet();
            HashSet<string> doors = keys.Select(k => k.ToUpper()).ToHashSet();
            HashSet<string> walls = new HashSet<string> { "#" };
            HashSet<string> floors = new HashSet<string> { ".", "@", "1", "2", "3", "4" };//I've modified my input s.t. my center has a number in each of its corners

            //Set new List<string> { '.' } as passable floors for graphs with less edges
            //Use floors instead if you want to know what is actually reachable from each position
            Maze<string> graphMaze = new Maze<string>(mazeInput.Select(r => r.ToCharArray().Select(c => c.ToString())), "#", new List<string> { "." });

            Maze<string> analysisMaze = new Maze<string>(mazeInput.Select(r => r.ToCharArray().Select(c => c.ToString())), "#", floors.Union(doors).Union(keys));

            Dictionary<string, List<string>> keysPerSubgraph = new Dictionary<string, List<string>>();
            foreach(string start in new List<string> { "1", "2", "3", "4" })
            {
                (bool _, int x, int y) = analysisMaze.FindFirstMatchingTile(start);
                keysPerSubgraph.Add(start, analysisMaze.FloodFillDistanceFinder(x, y, keys).poi.Select(p => p.tile).ToList());
            }

            //Create distances-graph
            HashSet<string> pointsOfInterest = floors.ToHashSet();
            pointsOfInterest.Remove(".");
            TritonGraph tGraph = new TritonGraph(graphMaze, keys, doors, pointsOfInterest);

            List<string> bestSequences = new List<string> { "1saiwe", "2qcvrtu", "3zmohjlfkdypgn", "4xb" }; //handmade
            return ValidateQuadraBotSequence(tGraph, bestSequences);
        }

        public static int ValidateQuadraBotSequence(TritonGraph graph, List<string> sequences)
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
                for(int i = 0; i < bots.Count; i++)
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

