using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC.Tests
{
    static class Day19Tests
    {
        public static void RunAll()
        {
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day19Tests Completed!");
        }

        static void TestSolvePartOne()
        {
            TestSuite.Test(201, Day19.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            TestSuite.Test(6610984, Day19.SolvePartTwo);
        }
    }
}
