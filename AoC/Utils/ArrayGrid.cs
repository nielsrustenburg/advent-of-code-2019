using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Serialization;
using System.Threading.Tasks.Dataflow;

namespace AoC.Utils
{
    public class ArrayGrid<T> : IGrid<T>
    {
        T[,] grid;
        Dictionary<T, int> tileCounts;
        public readonly bool originAtBottom;
        T defaultTile;

        public ArrayGrid(T defaultTile, int width, int height, bool originAtBottom = true)
        {
            grid = new T[width, height];
            this.defaultTile = defaultTile;
            tileCounts = new Dictionary<T, int> { [defaultTile] = width * height };
            this.originAtBottom = originAtBottom;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = defaultTile;
                }
            }
        }

        public ArrayGrid(IEnumerable<IEnumerable<T>> filledGrid, T defaultTile, bool originAtBottom) : this(defaultTile, filledGrid.First().Count(), filledGrid.Count(), originAtBottom)
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

        public int Width { get { return grid.GetLength(0); } }
        public int Height { get { return grid.GetLength(1); } }

        public T this[int x, int y]
        {
            get
            {
                return grid[x, y];
            }

            set
            {
                UpdateCount(grid[x, y], false);
                grid[x, y] = value;
                UpdateCount(value, true);
            }
        }

        private void UpdateCount(T tile, bool add)
        {
            bool tileTypeExists = tileCounts.TryGetValue(tile, out int currentCount);

            if (add)
            {
                if (!tileTypeExists) tileCounts.Add(tile, 1);
                else tileCounts[tile]++;
            }
            else
            {
                if (currentCount == 1) tileCounts.Remove(tile);
                else tileCounts[tile]--;
            }
        }

        public IEnumerable<T> GetAllTileTypes()
        {
            return tileCounts.Keys;
        }

        public Dictionary<T, int> GetTileCounts()
        {
            return new Dictionary<T, int>(tileCounts);
        }

        public int Count(T tile)
        {
            return tileCounts.ContainsKey(tile) ? tileCounts[tile] : 0;
        }

        public Dictionary<Direction, T> GetNeighbours(int x, int y, bool includeDiagonal = false)
        {
            var directions = Enum.GetValues(typeof(Direction));
            var neighbours = new Dictionary<Direction, T>();
            foreach (Direction dir in directions)
            {
                if (includeDiagonal || ((int)dir % 2) == 0)
                {
                    var actualDirection = originAtBottom ? dir : dir.Opposite();
                    var (modX, modY) = DirectionHelper.StepInDirection(dir, x, y, 1);
                    if (modX >= 0 && modY >= 0 && modX < grid.GetLength(0) && modY < grid.GetLength(1))
                    {
                        neighbours.Add(actualDirection, grid[modX, modY]);
                    }
                    else
                    {
                        neighbours.Add(actualDirection, defaultTile); //Alternative: not adding to dictionary, downside, different behaviour from Grid
                    }
                }
            }
            return neighbours;
        }

        public (int x, int y)? FindFirstMatchingTile(T findMe)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    if (grid[x, y].Equals(findMe)) return (x, y);
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
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    if (findUs.Contains(grid[x, y])) yield return (x, y);
                }
            }
        }

        public List<string> RowsAsStrings()
        {
            List<string> rows = new List<string>();
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                var sb = new StringBuilder();
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    sb.Append(grid[x, y].ToString());
                }
                rows.Add(sb.ToString());
            }
            if (originAtBottom) rows.Reverse();
            return rows;
        }
    }
}
