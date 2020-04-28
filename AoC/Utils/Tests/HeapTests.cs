using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;

namespace AoC.Utils.Tests
{
    class HeapTests
    {
        [Test]
        public void TestHeaps()
        {
            var minHeap = new MinHeap<int>();
            var maxHeap = new MaxHeap<int>();
            var numbers = new List<int> { 14, 9, 2, 3, 8, 11, 22, 34, 17, 24, 1, 5, 6, 3 };
            foreach(var num in numbers)
            {
                minHeap.Insert(num);
                maxHeap.Insert(num);
            }

            var minSorted = numbers.OrderBy(n => n).ToList();
            var maxSorted = numbers.OrderByDescending(n => n).ToList();

            for(int i = 0; i < minSorted.Count; i++)
            {
                Assert.AreEqual(minSorted[i], minHeap.Delete());
                Assert.AreEqual(maxSorted[i], maxHeap.Delete());
            }
        }
    }
}
