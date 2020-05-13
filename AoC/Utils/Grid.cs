using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net.NetworkInformation;

namespace AoC.Utils
{
    public class Grid<T> : IGrid<T>
    {
        int xMin, xMax, yMin, yMax;
        HashSet<T> allTileTypes;
        Dictionary<T, int> tileCount;
        Dictionary<int, Dictionary<int, T>> content;
        public readonly bool originAtBottom;

        public Grid(T defaultTile, bool originAtBottom)
        {
            xMin = xMax = yMin = yMax = 0;
            this.originAtBottom = originAtBottom;
            DefaultTile = defaultTile;
            allTileTypes = new HashSet<T> { DefaultTile };
            content = new Dictionary<int, Dictionary<int, T>>();
            tileCount = new Dictionary<T, int>();
        }

        public Grid(IEnumerable<IEnumerable<T>> filledGrid, T defaultTile, bool originAtBottom) : this(defaultTile, originAtBottom)
        {
            if (originAtBottom) filledGrid = filledGrid.Reverse();
            int y = 0;
            foreach (IEnumerable<T> row in filledGrid)
            {
                int x = 0;
                foreach (T tile in row)
                {
                    this[x, y] = tile;
                    x++;
                }
                y++;
            }
        }

        public Grid(Grid<T> copyMe) : this(copyMe.DefaultTile, copyMe.originAtBottom)
        {
            foreach (var kvp in copyMe.content)
            {
                foreach (var kvp2 in kvp.Value)
                {
                    this[kvp.Key, kvp2.Key] = kvp2.Value;
                }
            }
        }

        public T DefaultTile { get; private set; }
        public int Width { get { return xMax - xMin + 1; } }
        public int Height { get { return yMax - yMin + 1; } }

        public T this[int x, int y]
        {
            get
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
            set
            {
                EnsureBordersContain(x, y);
                bool addingDefault = value.Equals(DefaultTile);

                if (!addingDefault)
                {
                    UpdateTile(x, y, value);
                }
                else
                {
                    RemoveTile(x, y);
                }

                allTileTypes.Add(value);
            }
        }

        public IEnumerable<T> GetAllTileTypes()
        {
            return allTileTypes;
        }

        public Dictionary<T,int> GetTileCounts()
        {
            return new Dictionary<T, int>(tileCount);
        }

        public int CountNonDefault()
        {
            return tileCount.Values.Sum();
        }

        public int Count(T tile)
        {
            return tileCount.ContainsKey(tile) ? tileCount[tile] : 0;
        }

        public Dictionary<Direction, T> GetNeighbours(int x, int y, bool includeDiagonal = false)
        {
            var directions = Enum.GetValues(typeof(Direction));
            var neighbours = new Dictionary<Direction, T>();
            foreach(Direction dir in directions)
            {
                if(includeDiagonal || ((int) dir % 2) == 0)
                {
                    var (modX, modY) = DirectionHelper.StepInDirection(dir, x, y, 1);
                    neighbours.Add(dir, this[modX, modY]);
                }
            }
            return neighbours;
        }

        public (int x, int y)? FindFirstMatchingTile(T findMe)
        {
            if (findMe.Equals(DefaultTile)) throw new ArgumentException("Trying to find the DefaultTile, which is every unfilled Tile");
            foreach (var row in content)
            {
                foreach (var tile in row.Value)
                {
                    if (findMe.Equals(tile.Value))
                    {
                        return (tile.Key, row.Key);
                    }
                }
            }
            return null;
        }

        public IEnumerable<(int x, int y)> FindAllMatchingTiles(T tile)
        {
            return FindAllMatchingTiles(new HashSet<T> { tile });
        }

        public IEnumerable<(int x, int y)> FindAllMatchingTiles(HashSet<T> findUs)
        {
            if (findUs.Contains(DefaultTile)) throw new ArgumentException("Trying to find the DefaultTile, which is every unfilled Tile not this methods intended purpose");
            foreach (var row in content)
            {
                foreach (var tile in row.Value)
                {
                    if (findUs.Contains(tile.Value))
                    {
                        yield return (tile.Key, row.Key);
                    }
                }
            }
        }

        public List<string> RowsAsStrings()
        {
            List<string> rows = new List<string>();
            List<int> xCoords = Enumerable.Range(xMin, Width).ToList();
            for (int y = yMax; y >= yMin; y--)
            {
                string row = string.Concat(xCoords.Select(x => this[x, y].ToString()));
                rows.Add(row);
            }
            if (!originAtBottom)
            {
                rows.Reverse();
            }
            return rows;
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
    }
}
