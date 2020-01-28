using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

namespace AoC.Tests
{
    static class Day23Tests
    {
        public static void RunAll()
        {
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day23Tests Completed!");
        }

        //public static void TestSolvePartOne()
        //{
        //    void Test(string expected)
        //    {
        //        BigInteger expectedOutput = BigInteger.Parse(expected);
        //        BigInteger output = Day23.SolvePartOne();
        //        if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
        //    }
        //    Test("24555");
        //}

        //public static void TestSolvePartTwo()
        //{
        //    void Test(string expected)
        //    {
        //        BigInteger expectedOutput = BigInteger.Parse(expected);
        //        BigInteger output = Day23.SolvePartTwo();
        //        if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
        //    }
        //    Test("19463");
        //}

        static void TestSolvePartOne()
        {
            TestSuite.Test("24555", Day23.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            TestSuite.Test("19463", Day23.SolvePartTwo);
        }
    }
}
