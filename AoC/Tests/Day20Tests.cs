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
            TestFindDonutThickness();
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day20Tests Completed!");
        }

        static void TestFindDonutThickness()
        {
            string[] bbdonut = InputReader.StringsFromTxt("babydonut.txt");
            TestSuite.Test(4, bbdonut, Day20.FindDonutThickness);

            string[] donut20 = InputReader.StringsFromTxt("d20input.txt");
            TestSuite.Test(34, donut20, Day20.FindDonutThickness);
        }

        static void TestSolvePartOne()
        {
            TestSuite.Test(668, Day20.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            TestSuite.Test(7778, Day20.SolvePartTwo);
        }
    }
}
