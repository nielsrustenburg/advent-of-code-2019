using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC.Tests
{
    static class Day15Tests
    {
        public static void RunAll()
        {
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day15Tests Completed!");
        }

        static void TestSolvePartOne()
        {
            TestSuite.Test(266, Day15.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            TestSuite.Test(274, Day15.SolvePartTwo);
        }
    }
}
