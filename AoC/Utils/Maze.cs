using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Utils
{
    class Maze<T> : Grid<T>
    {
        HashSet<T> passableTerrain;
        public Maze(T defaultTile, IEnumerable<T> passable, bool originAtBottom) : base(defaultTile, originAtBottom)
        {
            passableTerrain = passable.ToHashSet();
        }

        public Maze(IEnumerable<IEnumerable<T>> filledGrid, T defaultTile, IEnumerable<T> passable, bool yDecreasing = true) : base(filledGrid,defaultTile, yDecreasing)
        {
            passableTerrain = passable.ToHashSet();
        }

        public Maze(Maze<T> copyMe) : base(copyMe)
        {
            passableTerrain = copyMe.passableTerrain.ToHashSet();
        }

        public (int totalSteps, List<(T tile, int distance)> poi) FloodFillDistanceFinder(int xStart, int yStart, IEnumerable<T> pointsOfInterest)
        {
            HashSet<T> poi = pointsOfInterest.ToHashSet();
            List<(T tile, int distance)> poiFound = new List<(T, int)>();

            List<(int x, int y)> frontier = new List<(int x, int y)> { (xStart, yStart) };
            Grid<bool> floodedGrid = new Grid<bool>(false, this.originAtBottom);
            floodedGrid[xStart, yStart] = true;

            int steps = 0;

            while (frontier.Any())
            {
                List<(int x, int y)> nextFrontier = new List<(int x, int y)>();
                foreach ((int x, int y) in frontier)
                {
                    var neighbours = GetNeighbours(x, y);
                    var floodNeighbours = floodedGrid.GetNeighbours(x, y);
                    foreach(var key in neighbours.Keys)
                    {
                        if (!floodNeighbours[key])
                        {
                            var neighbourCoords = DirectionHelper.StepInDirection(key, x, y, 1); 
                            if (poi.Contains(neighbours[key]))
                            {
                                poiFound.Add((neighbours[key], steps + 1));
                            }
                            if (passableTerrain.Contains(neighbours[key]))
                            {
                                nextFrontier.Add(neighbourCoords);
                            }
                            floodedGrid[neighbourCoords.x, neighbourCoords.y] = true;
                        }
                    }
                }
                frontier = nextFrontier;
                steps++;
            }
            return (steps-1, poiFound);
        }

        public void EliminateDeadEnds(HashSet<T> checkTilesOfType, T turnInto, HashSet<T> killerTiles)
        {
            IEnumerable<(int, int)> positionsToCheck = FindAllMatchingTiles(checkTilesOfType);
            EliminateDeadEnds(positionsToCheck, turnInto, killerTiles);
        }

        public void EliminateDeadEnds(IEnumerable<(int x, int y)> checkPositions, T turnInto, HashSet<T> killerTiles)
        {
            List<(int x, int y)> originalCandidates = checkPositions.ToList();
            bool requiresDoubleCheck = killerTiles.Contains(turnInto);

            List<(int x, int y)> eliminationCandidates = new List<(int x, int y)>(originalCandidates);

            while (eliminationCandidates.Any())
            {
                List<(int x, int y)> tilesToEliminate = new List<(int x, int y)>();
                List<(int, int)> potentialNewDeadEnds = new List<(int, int)>();

                foreach ((int x, int y) in eliminationCandidates)
                {
                    var neighbours = GetNeighbours(x, y);
                    if (neighbours.Values.Count(t => killerTiles.Contains(t)) >= 3)
                    {
                        tilesToEliminate.Add((x, y));
                        if (requiresDoubleCheck)
                        {
                            foreach (var nb in neighbours)
                            {
                                if (!nb.Value.Equals(turnInto))
                                {
                                    var nbCoords = DirectionHelper.StepInDirection(nb.Key, x, y, 1);
                                    if (originalCandidates.Any(t => t.x == nbCoords.x && t.y == nbCoords.y))
                                    {
                                        potentialNewDeadEnds.Add(nbCoords);
                                    }
                                }
                            }
                        }
                    }
                }

                foreach ((int x, int y) in tilesToEliminate)
                {
                    this[x, y] = turnInto;
                }

                eliminationCandidates = potentialNewDeadEnds;
            }
        }
    }
}
