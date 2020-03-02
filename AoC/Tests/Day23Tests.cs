using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

namespace AoC.Tests
{
    static class Day23Tests
    {
        public static void RunAll()
        {
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day23Tests Completed!");
        }

        static void TestSolvePartOne()
        {
            TestSuite.Test("24555", Day23.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            TestSuite.Test("19463", Day23.SolvePartTwo);
        }
    }
}
