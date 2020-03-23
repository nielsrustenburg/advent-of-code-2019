using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AoC.Day21
{
    class Tests
    {
        const string testCase1 = @"....#
#..#.
#..##
..#..
#....";

        [TestCase(testCase1, "2129920")]
        public void Test(string input, string expectedOutput)
        {
            var output = "";
            Assert.AreEqual(expectedOutput, output);
        }

        [TestCase(testCase1, 10, "99")]
        public void TestRecursive(string input,int steps, string expectedOutput)
        {
            var output = "";
            Assert.AreEqual(expectedOutput, output);
        }

        [TestCase("19353565", "1140612950")]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }
    }
}
