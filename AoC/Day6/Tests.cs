using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AoC.Day6
{
    class Tests
    {
        [TestCase("158090", "241")]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }
    }
}
