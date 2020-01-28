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

        //public static void TestSolvePartOne()
        //{
        //    void Test(int expectedOutput)
        //    {
        //        int output = Day17.SolvePartOne();
        //        if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
        //    }
        //    Test(13580);
        //}

        //public static void TestSolvePartTwo()
        //{
        //    void Test(int expectedOutput)
        //    {
        //        int output = Day17.SolvePartTwo();
        //        if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
        //    }
        //    Test(0);
        //}

        static void TestSolvePartOne()
        {
            TestSuite.Test(13580, Day17.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            TestSuite.Test(0, Day17.SolvePartTwo);
        }
    }
}
