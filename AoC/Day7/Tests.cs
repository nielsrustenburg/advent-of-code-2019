using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using AoC.Utils;

namespace AoC.Day7
{
    class Tests
    {
        [TestCase("", 1)]
        [TestCase("1", 1)]
        [TestCase("1,2", 2)]
        [TestCase("1,2,3",6)]
        [TestCase("1,2,3,4",24)]
        [TestCase("1,2,3,4,5",120)]
        public void TestPermutations(string input,int expectedOutput)
        {
            var perms = SetHelper.Permutations(input.Split(',').ToList());
            Assert.AreEqual(expectedOutput, perms.Count);
        }

        [TestCase("567045", "39016654")]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }
    }
}
