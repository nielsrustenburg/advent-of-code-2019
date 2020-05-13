using System;
using System.Collections.Generic;
using System.Text;

namespace AoC.Utils
{
    interface IGrid<T>
    {
        int Width { get; }
        int Height { get; }

        T this[int x, int y]
        {
            get;
            set;
        }

        IEnumerable<T> GetAllTileTypes();

        Dictionary<T, int> GetTileCounts();

        int Count(T tile);

        Dictionary<Direction, T> GetNeighbours(int x, int y, bool includeDiagonal);

        (int x, int y)? FindFirstMatchingTile(T findMe);

        IEnumerable<(int x, int y)> FindAllMatchingTiles(T tile);

        IEnumerable<(int x, int y)> FindAllMatchingTiles(HashSet<T> findUs);

        List<string> RowsAsStrings();
    }
}
