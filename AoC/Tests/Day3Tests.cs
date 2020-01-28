using System;
using System.Collections.Generic;
using System.Text;

namespace AoC.Tests
{
    static class Day3Tests
    {
        public static void RunAll()
        {
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day3Tests Completed!");
        }

        static void TestSolvePartOne()
        {
            TestSuite.Test(352, Day3.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            TestSuite.Test(43848, Day3.SolvePartTwo);
        }
    }
}
