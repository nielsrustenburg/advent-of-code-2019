using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Utils
{
    static class SetHelper
    {
        public static List<List<T>> Permutations<T>(List<T> values)
        {
            var result = values.Count == 0 ? new List<List<T>> { new List<T>() } : new List<List<T>>();
            foreach (T val in values)
            {
                var without = Permutations(values.Where(x => !x.Equals(val)).ToList());
                foreach (List<T> setWithout in without)
                {
                    setWithout.Add(val);
                    result.Add(setWithout);
                }
            }
            return result;
        }

        //Builds subsets in order of cardinality (more elegant approaches exist if we don't care about the order of subsets)
        public static IEnumerable<IEnumerable<T>> Subsets<T>(IEnumerable<T> values)
        {
            var originalSet = values.Distinct().ToList();
            var subsetsGroupedBySize = new List<List<T>>[originalSet.Count + 1];

            subsetsGroupedBySize[0] = new List<List<T>> { new List<T>() };
            yield return subsetsGroupedBySize[0].First();

            if (originalSet.Any())
            {
                subsetsGroupedBySize[1] = new List<List<T>>();
                foreach (var singleton in originalSet)
                {
                    var subset = new List<T> { singleton };
                    subsetsGroupedBySize[1].Add(subset);
                    yield return subset;
                }

                var singletonRankings = originalSet.Select((singleton, index) => (singleton, index)).ToDictionary(tuple => tuple.singleton,
                                                                                                               tuple => tuple.index);
                for (int k = 2; k < originalSet.Count + 1; k++)
                {
                    subsetsGroupedBySize[k] = new List<List<T>>();
                    foreach (var singletonRanking in singletonRankings)
                    {
                        foreach (var prevIterSubset in subsetsGroupedBySize[k - 1])
                        {
                            var subsetRanking = singletonRankings[prevIterSubset.Last()];
                            if (subsetRanking < singletonRanking.Value)
                            {
                                var subset = prevIterSubset.Append(singletonRanking.Key).ToList();
                                subsetsGroupedBySize[k].Add(subset);
                                yield return subset;
                            }
                        }
                    }
                }
            }
        }
    }
}
