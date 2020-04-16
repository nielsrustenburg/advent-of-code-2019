using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Linq;

namespace AoC.Utils.Tests
{
    class SetHelperTests
    {
        [TestCase("", 1)]
        [TestCase("1", 1)]
        [TestCase("1,2", 2)]
        [TestCase("1,2,3", 6)]
        [TestCase("1,2,3,4", 24)]
        [TestCase("1,2,3,4,5", 120)]
        public void TestPermutations(string input, int expectedPermutationsCount)
        {
            var perms = SetHelper.Permutations(input.Split(',').ToList());
            Assert.AreEqual(expectedPermutationsCount, perms.Count);
        }

        [TestCase("")]
        [TestCase("1")]
        [TestCase("1,2")]
        [TestCase("1,2,3")]
        [TestCase("1,2,3,4")]
        [TestCase("1,2,3,4,5")]
        public void TestSubsets(string input)
        {
            var originalSet = input.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
            int expectedSubsetsCount = 1;
            foreach(var _ in originalSet)
            {
                expectedSubsetsCount = expectedSubsetsCount << 1;
            }
            var subsets = SetHelper.Subsets(originalSet);
            Assert.AreEqual(expectedSubsetsCount, subsets.Count());
        }
    }
}
