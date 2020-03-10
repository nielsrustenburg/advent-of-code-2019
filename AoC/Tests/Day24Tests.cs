using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

namespace AoC.Tests
{
    static class Day24Tests
    {
        public static void RunAll()
        {
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day24Tests Completed!");
        }

        static void TestSolvePartOne()
        {
            TestSuite.Test("32509983", Day24.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            TestSuite.Test(2012, Day24.SolvePartTwo);
        }
    }
}
