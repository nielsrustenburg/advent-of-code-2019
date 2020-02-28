using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC.Tests
{
    static class Day14Tests
    {
        public static void RunAll()
        {
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day14Tests Completed!");
        }

        static void TestSolvePartOne()
        {
            TestSuite.Test(483766, Day14.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            TestSuite.Test(3061522, Day14.SolvePartTwo);
        }
    }
}
