using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC.Tests
{
    static class Day20Tests
    {
        public static void RunAll()
    {
        //TestSolvePartOne();
        TestSolvePartTwo();
        Console.WriteLine("Day20Tests Completed!");
    }

    public static void TestSolvePartOne()
    {
        void Test(int expectedOutput)
        {
            int output = Day20.SolvePartOne();
            if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
        }
        Test(668);
    }

    public static void TestSolvePartTwo()
    {
        void Test(int expectedOutput)
        {
            int output = Day20.SolvePartTwo();
            if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
        }
        Test(7778);
    }
}
}
