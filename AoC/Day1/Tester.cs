using System;
using System.Collections.Generic;
using System.Text;
using AoC.Utils;
using AoC.common;
using AoC.Tests;

namespace AoC.Day1
{
    class Tester : PuzzleSolverTester
    {
        public Tester(IPuzzleSolver solver, string exp1, string exp2) : base(solver, exp1, exp2)
        {

        }

        public override void RunAll()
        {
            TestFuelForModule();
            TestTotalFuelForModule();
            TestSolver();
        }

        static void TestFuelForModule()
        {
            TestSuite.Test(2, 12, Solver.FuelForModule);
            TestSuite.Test(2, 14, Solver.FuelForModule);
            TestSuite.Test(654, 1969, Solver.FuelForModule);
            TestSuite.Test(33583, 100756, Solver.FuelForModule);
        }

        static void TestTotalFuelForModule()
        {
            int module_mass = 100756;
            int initial_fuel = Solver.FuelForModule(module_mass);
            TestSuite.Test(50346, initial_fuel, Solver.TotalFuelForModule);
        }
    }
}
