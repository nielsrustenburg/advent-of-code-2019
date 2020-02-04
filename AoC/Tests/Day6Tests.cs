using System;
using System.Collections.Generic;
using System.Text;

namespace AoC.Tests
{
    static class Day6Tests
    {
        public static void RunAll()
        {
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day6Tests Completed!");
        }

        static void TestSolvePartOne()
        {
            TestSuite.Test(158090, Day6.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            TestSuite.Test(241, Day6.SolvePartTwo);
        }
    }
}
