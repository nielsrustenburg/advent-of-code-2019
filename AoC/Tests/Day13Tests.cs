using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC.Tests
{
    static class Day13Tests
    {
        public static void RunAll()
        {
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day13Tests Completed!");
        }

        static void TestSolvePartOne()
        {
            TestSuite.Test(173, Day13.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            TestSuite.Test(8942, Day13.SolvePartTwo);
        }
    }
}
