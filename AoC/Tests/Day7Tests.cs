using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoC.Utils;

namespace AoC.Tests
{
    static class Day7Tests
    {

        public static void RunAll()
        {
            TestSolvePartOne();
            TestSolvePartTwo();
            TestPermutations();

            Console.WriteLine("Day7Tests Completed!");
        }

        private static void TestPermutations()
        {
            TestSuite.TestCount(6, new List<int> { 1, 2, 3 }, SetHelper.Permutations);
            TestSuite.TestCount(24, new List<int> { 1, 2, 3 , 4}, SetHelper.Permutations);
            TestSuite.TestCount(120, new List<char> { 'a', 'b', 'c', 'd', 'e' }, SetHelper.Permutations);
        }

        static void TestSolvePartOne()
        {
            TestSuite.Test(567045, Day7.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            TestSuite.Test(39016654, Day7.SolvePartTwo);
        }
    }
}
