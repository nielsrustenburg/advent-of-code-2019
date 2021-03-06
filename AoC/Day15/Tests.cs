﻿using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AoC.Day15
{
    class Tests
    {
        [TestCase("", "")]
        public void Test(string input, string expectedOutput)
        {
            var output = "";
            Assert.AreEqual(expectedOutput, output);
        }

        [TestCase("266", "274")]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }
    }
}
