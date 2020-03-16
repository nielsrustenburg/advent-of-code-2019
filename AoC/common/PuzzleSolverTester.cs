using System;
using System.Collections.Generic;
using System.Text;
using AoC.Tests;

namespace AoC.common
{
    abstract class PuzzleSolverTester
    {
        protected IPuzzleSolver solver;
        string expectedOne;
        string expectedTwo;

        protected PuzzleSolverTester(IPuzzleSolver solver, string expectedOne, string expectedTwo)
        {
            this.solver = solver;
            this.expectedOne = expectedOne;
            this.expectedTwo = expectedTwo;
        }

        public void TestSolver()
        {
            TestSuite.Test(expectedOne, solver.SolutionOne);
            TestSuite.Test(expectedTwo, solver.SolutionTwo);
            solver.WriteSolutions();
        }
    }
}
