using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using AoC.Common;
using AoC.Utils;

namespace AoC.Day24
{
    class Tests
    {
        const string testCase1 = @"....#
#..#.
#..##
..#..
#....";

        [TestCase(testCase1, "2129920")]
        public void TestPartOne(string input, string expectedOutput)
        {
            var solver = new Solver(Input.InputMode.String, input);
            var output = solver.SolutionOne();
            Assert.AreEqual(expectedOutput, output);
        }

        [TestCase(testCase1, 10, 99)]
        public void TestRecursive(string input, int steps, int expectedOutput)
        {
            var inputString = Input.GetInput(Input.InputMode.String, input);
            var rows = InputParser.Split(inputString);
            var recursiveGoE = new RecursiveGameOfEris(rows);
            var output = recursiveGoE.Run(10);
            Assert.AreEqual(expectedOutput, output);
        }

        [TestCase("32509983", "2012")]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }
    }
}
