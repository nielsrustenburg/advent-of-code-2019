using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC
{
    class Doolhof<T> : Grid<T>
    {
        HashSet<T> passableTerrain;
        public Doolhof(T defaultTile, IEnumerable<T> passable) : base(defaultTile)
        {
            passableTerrain = passable.ToHashSet();
        }

        public (int totalSteps, List<(T tile, int distance)> poi) FloodFillDistanceFinder(int xStart, int yStart, IEnumerable<T> pointsOfInterest)
        {
            HashSet<T> poi = pointsOfInterest.ToHashSet();
            List<(T tile, int distance)> poiFound = new List<(T, int)>();

            List<(int x, int y)> frontier = new List<(int x, int y)> { (xStart, yStart) };
            Grid<bool> floodedGrid = new Grid<bool>(false);

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
                            var neighbourCoords = ModifyCoordinates(x, y, key);
                            if (poi.Contains(neighbours[key]))
                            {
                                poiFound.Add((neighbours[key], steps + 1));
                            }
                            if (passableTerrain.Contains(neighbours[key]))
                            {
                                nextFrontier.Add(neighbourCoords);
                            }
                            floodedGrid.SetTile(neighbourCoords.x, neighbourCoords.y, true);
                        }
                    }
                }
                frontier = nextFrontier;
                steps++;
            }
            return (steps-1, poiFound);
        }
    }
}
