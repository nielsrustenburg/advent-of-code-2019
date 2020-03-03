using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC
{
    public class Grid<T> : IGrid<T>
    {
        public T DefaultTile { get; private set; }
        int xMin, xMax, yMin, yMax;
        HashSet<T> allTileTypes;
        Dictionary<T, int> tileCount;
        Dictionary<int, Dictionary<int, T>> content;
        public int Width { get { return xMax - xMin + 1; } }
        public int Height { get { return yMax - yMin + 1; } }

        public Grid(T defaultTile)
        {
            xMin = xMax = yMin = yMax = 0;
            DefaultTile = defaultTile;
            allTileTypes = new HashSet<T> { DefaultTile };
            content = new Dictionary<int, Dictionary<int, T>>();
            tileCount = new Dictionary<T, int>();
        }

        public Grid(IEnumerable<IEnumerable<T>> filledGrid, T defaultTile) : this(defaultTile)
        {
            int y = 0;
            foreach (IEnumerable<T> row in filledGrid)
            {
                int x = 0;
                foreach (T tile in row)
                {
                    SetTile(x, y, tile);
                    x++;
                }
                y--;
            }
        }

        public Grid(Grid<T> copyMe) : this(copyMe.DefaultTile)
        {
            foreach (var kvp in copyMe.content)
            {
                foreach (var kvp2 in kvp.Value)
                {
                    SetTile(kvp.Key, kvp2.Key, kvp2.Value);
                }
            }
        }

        public T GetTile(int x, int y)
        {
            if (content.ContainsKey(y))
            {
                if (content[y].ContainsKey(x))
                {
                    return content[y][x];
                }
            }
            return DefaultTile;
        }

        public void SetTile(int x, int y, T tileType)
        {
            EnsureBordersContain(x, y);
            bool addingDefault = tileType.Equals(DefaultTile);

            if (!addingDefault)
            {
                UpdateTile(x, y, tileType);
            }
            else
            {
                RemoveTile(x, y);
            }

            allTileTypes.Add(tileType);
        }

        private void UpdateTile(int x, int y, T tileType)
        {
            if (!content.ContainsKey(y)) content.Add(y, new Dictionary<int, T>());
            if (content[y].TryGetValue(x, out T value))
            {
                UpdateTileCount(content[y][x], -1);
            }
            UpdateTileCount(tileType, 1);
            content[y][x] = tileType;
        }

        private void UpdateTileCount(T tileType, int incBy)
        {
            if (!tileType.Equals(DefaultTile))
            {
                tileCount.TryGetValue(tileType, out int countVal);
                countVal += incBy;
                tileCount[tileType] = countVal;
            }
        }

        private void RemoveTile(int x, int y)
        {
            //Potentially leaves empty columns in the Grid
            if (content.ContainsKey(y))
            {
                if (content[y].ContainsKey(x))
                {
                    UpdateTileCount(content[y][x], -1);
                    content[y].Remove(x);
                }
            }
        }

        private void EnsureBordersContain(int x, int y)
        {
            if (x < xMin) xMin = x;
            if (x > xMax) xMax = x;
            if (y < yMin) yMin = y;
            if (y > yMax) yMax = y;
        }

        public int CountNonDefault()
        {
            return tileCount.Values.Sum();
        }

        public int Count(T tile)
        {
            return tileCount.ContainsKey(tile) ? tileCount[tile] : 0;
        }

        public Dictionary<string, T> GetNeighbours(int x, int y, bool includeDiagonal = false)
        {
            Dictionary<string, T> nb = new Dictionary<string, T>
            {
                { "N", GetTile(x, y + 1) },
                { "E", GetTile(x + 1, y) },
                { "S", GetTile(x, y - 1) },
                { "W", GetTile(x - 1, y) }
            };
            if (includeDiagonal)
            {
                nb.Add("NE", GetTile(x + 1, y + 1));
                nb.Add("SE", GetTile(x + 1, y - 1));
                nb.Add("SW", GetTile(x - 1, y - 1));
                nb.Add("NW", GetTile(x - 1, y + 1));
            }
            return nb;
        }

        public (bool found, int x, int y) FindFirstMatchingTile(T findMe)
        {
            if (findMe.Equals(DefaultTile)) throw new ArgumentException("Trying to find the DefaultTile, which is every unfilled Tile");
            foreach (var row in content)
            {
                foreach (var tile in row.Value)
                {
                    if (findMe.Equals(tile.Value))
                    {
                        return (true, tile.Key, row.Key);
                    }
                }
            }
            return (false, 0, 0);
        }

        public List<(int x, int y)> FindAllMatchingTiles(T tile)
        {
            return FindAllMatchingTiles(new List<T> { tile });
        }

        public List<(int x, int y)> FindAllMatchingTiles(List<T> findUs)
        {
            if (findUs.Contains(DefaultTile)) throw new ArgumentException("Trying to find the DefaultTile, which is every unfilled Tile not this methods intended purpose");
            List<(int, int)> results = new List<(int, int)>();
            foreach (var row in content)
            {
                foreach (var tile in row.Value)
                {
                    if (findUs.Contains(tile.Value))
                    {
                        results.Add((tile.Key, row.Key));
                    }
                }
            }
            return results;
        }

        public List<string> RowsAsStrings(bool invert = false)
        {
            List<string> rows = new List<string>();
            List<int> xCoords = Enumerable.Range(xMin, Width).ToList();
            for (int y = yMax; y >= yMin; y--)
            {
                string row = string.Concat(xCoords.Select(x => GetTile(x, y).ToString()));
                rows.Add(row);
            }
            if (invert)
            {
                rows.Reverse();
            }
            return rows;
        }
    }
}
