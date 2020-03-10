using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC.Tests
{
    static class Day21Tests
    {
        public static void RunAll()
        {
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day21Tests Completed!");
        }

        static void TestSolvePartOne()
        {
            TestSuite.Test(19353565, Day21.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            TestSuite.Test(1140612950, Day21.SolvePartTwo);
        }
    }
}
