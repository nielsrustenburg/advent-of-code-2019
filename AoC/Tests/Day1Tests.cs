using System;
using System.Collections.Generic;
using System.Text;

namespace AoC.Tests
{
    static class Day1Tests
    {
        public static void RunAll()
        {
            TestFuelForModule();
            TestTotalFuelForModule();
            TestSolvePartOne();
            TestSolvePartTwo();

            Console.WriteLine("Day1Tests Completed!");
        }

        static void TestFuelForModule()
        {
            void Test(int input, int expectedOutput)
            {
                int output = Day1.FuelForModule(input);
                if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}({input}): {output}, expected {expectedOutput}");
            }
            Test(12, 2);
            Test(14, 2);
            Test(1969, 654);
            Test(100756, 33583);
        }

        static void TestTotalFuelForModule()
        {
            void Test(int input, int expectedOutput)
            {
                int output = Day1.TotalFuelForModule(input);
                if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}({input}): {output}, expected {expectedOutput}");
            }
            int module_mass = 100756;
            int initial_fuel = Day1.FuelForModule(module_mass);
            Test(initial_fuel, 50346);
        }

        static void TestSolvePartOne()
        {
            void Test(int expectedOutput)
            {
                int output = Day1.SolvePartOne();
                if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
            }
            Test(3362507);
        }

        static void TestSolvePartTwo()
        {
            void Test(int expectedOutput)
            {
                int output = Day1.SolvePartTwo();
                if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
            }
            Test(5040874);
        }
    }
}
