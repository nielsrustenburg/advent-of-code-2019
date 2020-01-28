using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC.Tests
{
    static class Day14Tests
    {
        public static void RunAll()
        {
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day14Tests Completed!");
        }

        //public static void TestSolvePartOne()
        //{
        //    void Test(int expectedOutput)
        //    {
        //        int output = Day14.SolvePartOne();
        //        if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
        //    }
        //    Test(483766);
        //}

        //public static void TestSolvePartTwo()
        //{
        //    void Test(int expectedOutput)
        //    {
        //        int output = Day14.SolvePartTwo();
        //        if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
        //    }
        //    Test(3061522);
        //}

        static void TestSolvePartOne()
        {
            TestSuite.Test(483766, Day14.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            TestSuite.Test(3061522, Day14.SolvePartTwo);
        }
    }
}
