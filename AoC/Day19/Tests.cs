using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AoC.Day19
{
    class Tests
    {
        [TestCase("201", "6610984")]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }
    }
}
