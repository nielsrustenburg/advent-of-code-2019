using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using AoC.Common;

namespace AoC.Day21
{
    class Tests
    {
        [TestCase("19353565", "1140612950")]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }
    }
}
