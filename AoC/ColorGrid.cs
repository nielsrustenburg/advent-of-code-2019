using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC
{
    public class ColorGrid
    {
        public string DefaultColor { get; set; }
        int xMin;
        int xMax;
        int yMin;
        int yMax;
        public HashSet<string> allColors;
        Dictionary<int, Dictionary<int, string>> gridColors;

        public ColorGrid(string defaultColor)
        {
            xMin = xMax = yMin = yMax = 0;
            DefaultColor = defaultColor;
            allColors = new HashSet<string>();
            allColors.Add(DefaultColor);
            gridColors = new Dictionary<int, Dictionary<int, string>>();
        }

        public string GetColorAt(int x, int y)
        {
            if (IsWithinKnownEdges(x, y))
            {
                if (gridColors.ContainsKey(x))
                {
                    if (gridColors[x].ContainsKey(y))
                    {
                        return gridColors[x][y];
                    }
                }
            }
            return DefaultColor;
        }

        public (bool found, int x, int y) Find(string color)
        {
            foreach (var xKeyDictVal in gridColors)
            {
                foreach (var yKeyColorVal in xKeyDictVal.Value)
                {
                    if (yKeyColorVal.Value == color)
                    {
                        return (true, xKeyDictVal.Key, yKeyColorVal.Key);
                    }
                }
            }
            return (false, 0, 0);
        }

        public List<(int x, int y)> FindAll(List<string> colors)
        {
            List<(int, int)> results = new List<(int, int)>();
            foreach (var xKeyDictVal in gridColors)
            {
                foreach (var yKeyColorVal in xKeyDictVal.Value)
                {
                    if (colors.Contains(yKeyColorVal.Value))
                    {
                        results.Add((xKeyDictVal.Key, yKeyColorVal.Key));
                    }
                }
            }
            return results;
        }

        public List<(int x, int y)> FindAll(string color)
        {
            List<(int, int)> results = new List<(int, int)>();
            foreach (var xKeyDictVal in gridColors)
            {
                foreach (var yKeyColorVal in xKeyDictVal.Value)
                {
                    if (yKeyColorVal.Value == color)
                    {
                        results.Add((xKeyDictVal.Key, yKeyColorVal.Key));
                    }
                }
            }
            return results;
        }

        public bool HasNeighbour4(int x, int y, string color, int threshold = 1)
        {
            string north = GetColorAt(x, y + 1);
            string south = GetColorAt(x, y - 1);
            string west = GetColorAt(x - 1, y);
            string east = GetColorAt(x + 1, y);

            List<string> directions = new List<string> { north, south, west, east };
            int neighbourAmount = directions.Where(dir => dir == color).Count();
            return neighbourAmount >= threshold;
            //if (!all)
            //{
            //    return north == color ||
            //           south == color ||
            //           west == color ||
            //           east == color;
            //}
            //else
            //{
            //    return north == color &&
            //           south == color &&
            //           west == color &&
            //           east == color;
            //}
        }

        public Dictionary<string, string> Neighbours(int x, int y, bool diag = false)
        {
            Dictionary<string, string> nb = new Dictionary<string, string>();
            nb.Add("N", GetColorAt(x, y + 1));
            nb.Add("E", GetColorAt(x + 1, y));
            nb.Add("S", GetColorAt(x, y - 1));
            nb.Add("W", GetColorAt(x - 1, y));
            if (diag)
            {
                nb.Add("NE", GetColorAt(x + 1, y + 1));
                nb.Add("SE", GetColorAt(x + 1, y - 1));
                nb.Add("SW", GetColorAt(x - 1, y - 1));
                nb.Add("NW", GetColorAt(x - 1, y + 1));
            }
            return nb;
        }

        public ColorGrid CopyMe(){
            ColorGrid copy = new ColorGrid(DefaultColor);
            foreach(var kvp in gridColors){
                foreach(var kvp2 in kvp.Value){
                    copy.GetColorAt(kvp.Key, kvp2.Key);
                    copy.SetColorAt(kvp.Key, kvp2.Key, kvp2.Value);
                }
            }
            return copy;
        }

        public void RemoveDeadEnds(List<string> checkTiles)
        {
            List<(int x, int y)> tileLocs = FindAll(checkTiles);
            List<(int x, int y)> updated = new List<(int x, int y)>();
            do
            {
                foreach((int x, int y) loc in tileLocs)
                {
                    if(HasNeighbour4(loc.x, loc.y, DefaultColor, 3))
                    {
                        SetColorAt(loc.x, loc.y, DefaultColor);
                        updated.Add(loc);
                    }
                }
                foreach(var upd in updated)
                {
                    tileLocs.Remove(upd);
                }
            } while (updated.Any());
        }

        internal List<(int x, int y, int dist)> FloodFind((int x, int y) from, IEnumerable<string> targets, bool exceptTargets = false)
        {
            ColorGrid mapCopy = CopyMe();
            HashSet<string> targetList = targets.ToHashSet();

            HashSet<(int x, int y)> frontier = new HashSet<(int x, int y)> { from };

            List<(int x, int y, int dist)> result = new List<(int x, int y, int dist)>();
            int steps = 0;
            while (frontier.Any())
            {
                HashSet<(int x, int y)> nextFrontier = new HashSet<(int x, int y)>();
                foreach ((int x, int y) fTile in frontier)
                {
                    if (mapCopy.GetColorAt(fTile.x, fTile.y) == "%")
                    {
                        //Portal found, add other portals exits to next frontier
                        result.Add((fTile.x, fTile.y, steps));
                    }
                    mapCopy.SetColorAt(fTile.x, fTile.y, "&");
                    foreach (KeyValuePair<string, string> nbor in mapCopy.Neighbours(fTile.x, fTile.y))
                    {
                        if (nbor.Value == "%" || nbor.Value == ".")//Hardcoded for day20 requires refactoring
                        {
                            (int x, int y) nLoc = nbor.Key.Aggregate(fTile, (a, b) => (a.Item1 + (b == 'E' ? 1 : 0) - (b == 'W' ? 1 : 0)
                                                                            , a.Item2 + (b == 'N' ? 1 : 0) - (b == 'S' ? 1 : 0)));
                            nextFrontier.Add(nLoc);
                        }
                    }
                }
                steps++;
                frontier = nextFrontier;
            }
            return result;
        }

        internal (bool, int) DistanceBetween(string from, string to, List<string> passableTerrain)
        {
            ColorGrid mapCopy = CopyMe();
            List<(int x, int y)> passablePoints = mapCopy.FindAll(passableTerrain);
            (bool _, int x, int y) target = Find(to);
            bool hasMoved = true;
            int steps = 1;
            while (hasMoved)
            {
                if (mapCopy.HasNeighbour4(target.x, target.y, from))
                {
                    return (true,steps);
                }

                List<(int x, int y)> reached = new List<(int x, int y)>();
                foreach ((int x, int y) point in passablePoints)
                {
                    if (mapCopy.HasNeighbour4(point.x, point.y, from))
                    {
                        reached.Add(point);
                    }
                }

                foreach ((int x, int y) point in reached)
                {
                    mapCopy.SetColorAt(point.x, point.y, from);
                    passablePoints.RemoveAt(passablePoints.FindIndex(p => p.x == point.x && p.y == point.y));
                }

                steps++;
                hasMoved = reached.Any();
            }
            return (false, int.MaxValue);
        }

        public void SetColorAt(int x, int y, string color)
        {
            if (gridColors.TryGetValue(x, out Dictionary<int, string> yToCol))
            {
                yToCol.TryGetValue(y, out string col);
                yToCol[y] = color;
            }
            else
            {
                yToCol = new Dictionary<int, string>();
                gridColors[x] = yToCol;
                yToCol.Add(y, color);
            }
            allColors.Add(color);
        }

        private bool IsWithinKnownEdges(int x, int y)
        {
            bool inside = true;
            if (xMin > x)
            {
                xMin = x;
                inside = false;
            }
            if (xMax < x)
            {
                xMax = x;
                inside = false;
            }
            if (yMin > y)
            {
                yMin = y;
                inside = false;
            }
            if (yMax < y)
            {
                yMax = y;
                inside = false;
            }
            return inside;
        }

        public int PanelsPainted()
        {
            return gridColors.Values.Select(yDict => yDict.Values.Count).Sum();
        }

        public List<string> GetImageStrings(bool dense = false)
        {
            List<string> rows = new List<string>();
            int incrementBy = dense ? -1 : 1;
            int yStart = dense ? yMax : yMin; //Not sure if this is "dense" or the opposite is true... starting to think past-Me was a bit dense

            bool PastYLimit(int y)
            {
                return dense ? (y >= yMin) : (y <= yMax);
            }

            for (int y = yStart; PastYLimit(y); y += incrementBy)
            {
                string row = "";
                for (int x = xMin; x <= xMax; x++)
                {
                    row = row + GetColorAt(x, y);
                }
                rows.Add(row);
            }

            return rows;
        }

        public int Width()
        {
            return xMax - xMin + 1;
        }

        public int Height()
        {
            return yMax - yMin + 1;
        }
    }
}

