using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC.Tests
{
    static class Day25Tests
    {
        public static void RunAll()
        {
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day25Tests Completed!");
        }

        //public static void TestSolvePartOne()
        //{
        //    void Test(int expectedOutput)
        //    {
        //        int output = Day25.SolvePartOne();
        //        if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
        //    }
        //    Test(0);
        //}

        //public static void TestSolvePartTwo()
        //{
        //    void Test(int expectedOutput)
        //    {
        //        int output = Day25.SolvePartTwo();
        //        if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
        //    }
        //    Test(0);
        //}

        static void TestSolvePartOne()
        {
            TestSuite.Test(0, Day25.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            TestSuite.Test(0, Day25.SolvePartTwo);
        }
    }
}
