using System;
using System.Collections.Generic;
using System.Text;

namespace AoC.Tests
{
    static class Day6Tests
    {
        public static void RunAll()
        {
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day6Tests Completed!");
        }

        //public static void TestSolvePartOne()
        //{
        //    void Test(int expectedOutput)
        //    {
        //        int output = Day6.SolvePartOne();
        //        if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
        //    }
        //    Test(158090);
        //}

        //public static void TestSolvePartTwo()
        //{
        //    void Test(int expectedOutput)
        //    {
        //        int output = Day6.SolvePartTwo();
        //        if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
        //    }
        //    Test(241);
        //}

        static void TestSolvePartOne()
        {
            TestSuite.Test(158090, Day6.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            TestSuite.Test(241, Day6.SolvePartTwo);
        }
    }
}
