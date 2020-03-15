using System;
using System.Collections.Generic;
using System.Text;
using AoC.Utils;

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
            TestSuite.Test(2, 12, Day1.FuelForModule);
            TestSuite.Test(2, 14, Day1.FuelForModule);
            TestSuite.Test(654, 1969, Day1.FuelForModule);
            TestSuite.Test(33583, 100756, Day1.FuelForModule);
        }

        static void TestTotalFuelForModule()
        {
            int module_mass = 100756;
            int initial_fuel = Day1.FuelForModule(module_mass);
            TestSuite.Test(50346, initial_fuel, Day1.TotalFuelForModule);
        }

        static void TestSolvePartOne()
        {
            TestSuite.Test(3362507, Day1.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            TestSuite.Test(5040874, Day1.SolvePartTwo);
        }
    }
}
