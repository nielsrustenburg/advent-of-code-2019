using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AoC.Day1
{
    class Tests
    {
        [TestCase(12, 2)]
        [TestCase(14, 2)]
        [TestCase(1969, 654)]
        [TestCase(100756, 33583)]
        public void TestFuelForModule(int input, int expectedOutput)
        {
            Assert.AreEqual(expectedOutput, Solver.FuelForModule(input));
        }

        [TestCase(14,2)]
        [TestCase(1969,966)]
        [TestCase(100756, 50346)]
        public void TestRecursiveFuelForModule(int input, int expectedOutput)
        {
            Assert.AreEqual(expectedOutput, Solver.TotalFuelForModule(input)); //need to substract input for desired values... might be worth refactoring
        }

        [TestCase("3362507", "5040874")]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }
    }
}
