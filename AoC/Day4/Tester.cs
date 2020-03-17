using System;
using System.Collections.Generic;
using System.Text;
using AoC.common;
using AoC.Tests;

namespace AoC.Day4
{
    class Tester : PuzzleSolverTester
    {
        public Tester(IPuzzleSolver solver, string expectedOne, string expectedTwo) : base(solver, expectedOne, expectedTwo)
        {
        }

        public override void RunAll()
        {
            TestIncreasingDigits();
            TestMatchingDigits();
            TestContainsDigitPair();
            TestSolver();
        }

        public static void TestIncreasingDigits()
        {
            TestSuite.Test(false, 923456, Solver.IncreasingDigits);
            TestSuite.Test(false, 223450, Solver.IncreasingDigits);
            TestSuite.Test(true, 123789, Solver.IncreasingDigits);
            TestSuite.Test(true, 1234, Solver.IncreasingDigits);
            TestSuite.Test(true, -1234, Solver.IncreasingDigits);
        }

        public static void TestMatchingDigits()
        {
            TestSuite.Test(true, 223450, Solver.MatchingDigits);
            TestSuite.Test(true, 2344, Solver.MatchingDigits);
            TestSuite.Test(false, 123789, Solver.MatchingDigits);
            TestSuite.Test(false, 12378, Solver.MatchingDigits);
        }

        public static void TestContainsDigitPair()
        {
            TestSuite.Test(true, 112233, Solver.ContainsDigitPair);
            TestSuite.Test(true, 443544, Solver.ContainsDigitPair); //Fails on increasing digits but should hold here
            TestSuite.Test(false, 123444, Solver.ContainsDigitPair);
            TestSuite.Test(true, 111122, Solver.ContainsDigitPair);
            TestSuite.Test(true, 778888, Solver.ContainsDigitPair);
        }
    }
}
