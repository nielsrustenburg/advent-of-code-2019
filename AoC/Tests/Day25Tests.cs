using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC.Tests
{
    static class Day25Tests
    {
        public static void RunAll()
        {
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day25Tests Completed!");
        }

        static void TestSolvePartOne()
        {
            TestSuite.Test(34095120, Day25.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            Day25.SolvePartTwo();
        }
    }
}
