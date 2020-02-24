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
            TestSolvePartTwo();
            Console.WriteLine("Day8Tests Completed!");
        }

        static void TestSolvePartOne()
        {
            TestSuite.Test(1441, Day8.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            List<string> expectedImage = new List<string>
            {
                "111  1  1 1111 111  111  ",
                "1  1 1  1    1 1  1 1  1 ",
                "1  1 1  1   1  111  1  1 ",
                "111  1  1  1   1  1 111  ",
                "1 1  1  1 1    1  1 1    ",
                "1  1  11  1111 111  1    "
            };//RUZBP

            TestSuite.TestSequence(expectedImage, 0,  (int _) => { return Day8.SolvePartTwo(); } );
        }
    }
}

