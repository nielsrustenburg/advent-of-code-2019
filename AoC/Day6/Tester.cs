using System;
using System.Collections.Generic;
using System.Text;
using AoC.common;
using AoC.Tests;

namespace AoC.Day6
{
    class Tester : PuzzleSolverTester
    {
        public Tester(IPuzzleSolver solver, string expectedOne, string expectedTwo) : base(solver, expectedOne, expectedTwo)
        {
        }

        public override void RunAll()
        {
            TestSolver();
        }
    }
}
