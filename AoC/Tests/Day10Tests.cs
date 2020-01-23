using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC.Tests
{
    static class Day10Tests
    {
        public static void RunAll()
        {
            TestAngleDegs();
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day10Tests Completed!");
        }

        public static void TestSolvePartOne()
        {
            void Test(int expectedOutput)
            {
                int output = Day10.SolvePartOne();
                if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
            }
            Test(282);
        }

        public static void TestSolvePartTwo()
        {
            void Test(int expectedOutput)
            {
                int output = Day10.SolvePartTwo();
                if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
            }
            Test(1008);
        }

        public static void TestAngleDegs()
        {
            void Test((int x, int y) p, double expectedOutput)
            {
                double output = Math.Floor(Day10.DifferenceToAngle(p));
                if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
            }
            Test((2, -6), 18);
            Test((0, -3), 0); //-90
            Test((-3, 0), 270); //180
            Test((0, 3), 180); //90
            Test((3, 0), 90);
            Test((2, 6), 161);
            Test((-2, 6), 198);
            Test((-2, -6), 341);
        }
    }
}
