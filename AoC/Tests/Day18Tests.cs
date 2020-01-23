using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC.Tests
{
    static class Day18Tests
    {
        public static void RunAll()
        {
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day18Tests Completed!");
        }

        public static void TestSolvePartOne()
        {
            void Test(int expectedOutput)
            {
                int output = Day18.SolvePartOne();
                if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
            }
            Test(0);
        }

        public static void TestSolvePartTwo()
        {
            void Test(int expectedOutput)
            {
                int output = Day18.SolvePartTwo();
                if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
            }
            Test(0);
        }
    }
}
