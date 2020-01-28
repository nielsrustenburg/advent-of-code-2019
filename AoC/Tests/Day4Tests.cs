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
            int t1 = 223450;
            int t2 = 123789;

            if (Day4.IncreasingDigits(t1)) throw new Exception($"{t1} should not succeed IncreasingDigits test");
            if (!Day4.IncreasingDigits(t2)) throw new Exception($"{t1} should've succeeded IncreasingDigits test");
        }
        public static void TestMatchingDigits()
        {
            int t1 = 223450;
            int t2 = 123789;

            if(!Day4.MatchingDigits(t1)) throw new Exception($"{t1} should've succeeded MatchingDigits test");
            if(Day4.MatchingDigits(t2)) throw new Exception($"{t2} should not succeed MatchingDigits test");
        }
        public static void TestDoubleDigits()
        {
            int t1 = 112233;
            int t2 = 123444;
            int t3 = 111122;
            int t4 = 778888;

            if (!Day4.DoubleDigits(t1)) throw new Exception($"{t1} should've succeeded DoubleDigits test");
            if (Day4.DoubleDigits(t2)) throw new Exception($"{t2} should've failed DoubleDigits test");
            if (!Day4.DoubleDigits(t3)) throw new Exception($"{t3} should've succeeded DoubleDigits test");
            if (!Day4.DoubleDigits(t4)) throw new Exception($"{t4} should've succeeded DoubleDigits test");
        }

        //static void TestSolvePartOne()
        //{
        //    void Test(int expectedOutput)
        //    {
        //        int output = Day4.SolvePartOne();
        //        if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
        //    }
        //    Test(544);
        //}

        //static void TestSolvePartTwo()
        //{
        //    void Test(int expectedOutput)
        //    {
        //        int output = Day4.SolvePartTwo();
        //        if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
        //    }
        //    Test(334);
        //}

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
