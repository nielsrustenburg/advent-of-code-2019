using System;
using System.Collections.Generic;
using System.Text;
using AoC.common;
using AoC.Tests;

namespace AoC.Day3
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

        static void TestSmallerInstances()
        {
            Wire w1 = new Wire("R8,U5,L5,D3".Split(','));
            Wire w2 = new Wire("U7,R6,D4,L4".Split(','));

            TestSuite.Test(6, () => Solver.IntersectionDistClosestToOrigin(w1, w2));
            TestSuite.Test(30, () => Solver.ShortestIntersectionDist(w1, w2));

            Wire w3 = new Wire("R75,D30,R83,U83,L12,D49,R71,U7,L72".Split(','));
            Wire w4 = new Wire("U62,R66,U55,R34,D71,R55,D58,R83".Split(','));

            TestSuite.Test(159, () => Solver.IntersectionDistClosestToOrigin(w3, w4));
            TestSuite.Test(610, () => Solver.ShortestIntersectionDist(w3, w4));

            Wire w5 = new Wire("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51".Split(','));
            Wire w6 = new Wire("U98,R91,D20,R16,D67,R40,U7,R15,U6,R7".Split(','));

            TestSuite.Test(135, () => Solver.IntersectionDistClosestToOrigin(w5, w6));
            TestSuite.Test(410, () => Solver.ShortestIntersectionDist(w5, w6));
        }
    }
}
