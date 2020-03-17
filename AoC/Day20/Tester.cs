using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AoC.common;
using AoC.Utils;
using AoC.Tests;
namespace AoC.Day20
{
    class Tester : PuzzleSolverTester
    {
        public Tester(IPuzzleSolver solver, string expectedOne, string expectedTwo) : base(solver, expectedOne, expectedTwo)
        {
        }

        public override void RunAll()
        {
            TestFindDonutThickness();
            TestSolver();
        }

        static void TestFindDonutThickness()
        {
            string[] bbdonut = InputReader.StringsFromTxt("babydonut.txt");
            TestSuite.Test(4, bbdonut, Solver.FindDonutThickness);

            string[] donut20 = InputReader.StringsFromTxt("d20input.txt");
            TestSuite.Test(34, donut20, Solver.FindDonutThickness);
        }
    }
}
