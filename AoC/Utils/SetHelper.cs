using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Utils
{
    static class SetHelper
    {
        public static List<List<T>> AsSingletons<T>(List<T> originalSet)
        {
            List<List<T>> singletons = originalSet.Select(element => new List<T> { element }).ToList();
            return singletons;
        }

        public static List<List<T>> Permutations<T>(List<T> values)
        {
            List<List<T>> result = values.Count == 0 ? new List<List<T>> { new List<T>() } : new List<List<T>>();
            foreach (T val in values)
            {
                List<List<T>> without = Permutations(values.Where(x => !x.Equals(val)).ToList());
                foreach (List<T> setWithout in without)
                {
                    setWithout.Add(val);
                    result.Add(setWithout);
                }
            }
            return result;
        }
    }
}
