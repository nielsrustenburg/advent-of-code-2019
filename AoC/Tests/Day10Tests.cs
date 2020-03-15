using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AoC.Utils;

namespace AoC.Tests
{
    static class Day10Tests
    {
        public static void RunAll()
        {
            TestAngleDegs();
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day10Tests Completed!");
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

        static void TestSolvePartOne()
        {
            TestSuite.Test(282, Day10.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            TestSuite.Test(1008, Day10.SolvePartTwo);
        }
    }
}
