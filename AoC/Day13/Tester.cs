using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AoC.common;
using AoC.Utils;
using AoC.Tests;
namespace AoC.Day13
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
