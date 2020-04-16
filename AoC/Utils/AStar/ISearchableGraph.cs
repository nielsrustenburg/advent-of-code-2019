using System;
using System.Collections.Generic;
using System.Text;

namespace AoC.Utils.AStar
{
    public interface ISearchableGraph
    {
        IEnumerable<(string node, int weight)> GetReachableNodes(string from);
    }
}
