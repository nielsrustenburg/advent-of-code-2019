using System;
using System.Collections.Generic;
using System.Text;

namespace AoC.Tests
{
    static class Day3Tests
    {
        public static void RunAll()
        {
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day3Tests Completed!");
        }

        static void TestSolvePartOne()
        {
            void Test(int expectedOutput)
            {
                int output = Day3.SolvePartOne();
                if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
            }
            Test(352);
        }

        static void TestSolvePartTwo()
        {
            void Test(int expectedOutput)
            {
                int output = Day3.SolvePartTwo();
                if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
            }
            Test(43848);
        }
    }
}
