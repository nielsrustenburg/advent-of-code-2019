using System;
using System.Collections.Generic;
using System.Text;

namespace AoC.Tests
{
    static class Day8Tests
    {
        public static void RunAll()
        {
            TestSolvePartOne();
            Console.WriteLine("Day8Tests Completed!");
        }

        //public static void TestSolvePartOne()
        //{
        //    void Test(int expectedOutput)
        //    {
        //        int output = Day8.SolvePartOne();
        //        if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
        //    }
        //    Test(1441);
        //}

        static void TestSolvePartOne()
        {
            TestSuite.Test(1441, Day8.SolvePartOne);
        }

        //static void TestSolvePartTwo()
        //{
        //    TestSuite.Test(, Day8.SolvePartTwo);
        //}
    }
}

