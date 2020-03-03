using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC.Tests
{
    static class Day17Tests
    {
        public static void RunAll()
        {
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day17Tests Completed!");
        }

        static void TestSolvePartOne()
        {
            TestSuite.Test(13580, Day17.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            TestSuite.Test(1063081, Day17.SolvePartTwo);
        }
    }
}
