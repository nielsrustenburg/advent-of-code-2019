using System;
using System.Collections.Generic;
using System.Text;
using AoC.common;
using AoC.Tests;
using AoC.Utils;

namespace AoC.Day7
{
    class Tester : PuzzleSolverTester
    {
        public Tester(IPuzzleSolver solver, string expectedOne, string expectedTwo) : base(solver, expectedOne, expectedTwo)
        {
        }

        private static void TestPermutations()
        {
            TestSuite.TestCount(6, new List<int> { 1, 2, 3 }, SetHelper.Permutations);
            TestSuite.TestCount(24, new List<int> { 1, 2, 3, 4 }, SetHelper.Permutations);
            TestSuite.TestCount(120, new List<char> { 'a', 'b', 'c', 'd', 'e' }, SetHelper.Permutations);
        }

        public override void RunAll()
        {
            TestPermutations();
            TestSolver();
        }
    }
}
