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

        //public static void TestSolvePartOne()
        //{
        //    void Test(int expectedOutput)
        //    {
        //        int output = Day21.SolvePartOne();
        //        if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
        //    }
        //    Test(19353565);
        //}

        //public static void TestSolvePartTwo()
        //{
        //    void Test(int expectedOutput)
        //    {
        //        int output = Day21.SolvePartTwo();
        //        if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
        //    }
        //    Test(1140612950);
        //}

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
