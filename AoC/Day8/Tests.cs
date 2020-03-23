using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AoC.Day8
{
    class Tests
    {
        const string p2expout = "111  1  1 1111 111  111  \n" +
                                "1  1 1  1    1 1  1 1  1 \n" +
                                "1  1 1  1   1  111  1  1 \n" +
                                "111  1  1  1   1  1 111  \n" +
                                "1 1  1  1 1    1  1 1    \n" +
                                "1  1  11  1111 111  1    ";

        [TestCase("1441", p2expout)]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }
    }
}
