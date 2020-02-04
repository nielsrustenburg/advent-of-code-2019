using System;
using System.Collections.Generic;
using System.Text;

namespace AoC.Tests
{
    static class Day4Tests
    {
        public static void RunAll()
        {
            TestIncreasingDigits();
            TestMatchingDigits();
            TestDoubleDigits();
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day4Tests Completed!");
        }

        public static void TestIncreasingDigits()
        {
            TestSuite.Test(false, 223450, Day4.IncreasingDigits);
            TestSuite.Test(true, 123789, Day4.IncreasingDigits);
        }

        public static void TestMatchingDigits()
        {
            TestSuite.Test(true, 223450, Day4.MatchingDigits);
            TestSuite.Test(false, 123789, Day4.MatchingDigits);
        }

        public static void TestDoubleDigits()
        {
            TestSuite.Test(true, 112233, Day4.DoubleDigits);
            TestSuite.Test(false, 123444, Day4.DoubleDigits);
            TestSuite.Test(true, 111122, Day4.DoubleDigits);
            TestSuite.Test(true, 778888, Day4.DoubleDigits);
        }

        static void TestSolvePartOne()
        {
            TestSuite.Test(544, Day4.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            TestSuite.Test(334, Day4.SolvePartTwo);
        }

    }
}
