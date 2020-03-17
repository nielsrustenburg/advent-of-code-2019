using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AoC.common;
using AoC.Utils;
using AoC.Tests;
namespace AoC.Day10
{
    class Tester : PuzzleSolverTester
    {
        public Tester(IPuzzleSolver solver, string expectedOne, string expectedTwo) : base(solver, expectedOne, expectedTwo)
        {
        }

        public static void TestAngleDegs()
        {
            TestSuite.Test(180, (0, -1), AsteroidVisionSet.StepTo360Angle);
            TestSuite.Test(270, (-1, 0), AsteroidVisionSet.StepTo360Angle);
            TestSuite.Test(0, (0, 1), AsteroidVisionSet.StepTo360Angle);
            TestSuite.Test(90, (1, 0), AsteroidVisionSet.StepTo360Angle);
            TestSuite.Test(45, (1, 1), AsteroidVisionSet.StepTo360Angle);
            TestSuite.Test(225, (-1, -1), AsteroidVisionSet.StepTo360Angle);
            TestSuite.Test(315, (-1, 1), AsteroidVisionSet.StepTo360Angle);
            TestSuite.Test(135, (1, -1), AsteroidVisionSet.StepTo360Angle);
        }

        public override void RunAll()
        {
            TestAngleDegs();
            TestSolver();
        }
    }
}
