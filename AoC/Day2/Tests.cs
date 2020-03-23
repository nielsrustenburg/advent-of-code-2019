using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AoC.Day2
{
    class Tests
    {
        //phase out IntCode in favor of BigIntCode before making IntCode tests?

        [TestCase("1,9,10,3,2,3,11,0,99,30,40,50", "3500,9,10,70,2,3,11,0,99,30,40,50")]
        [TestCase("1,0,0,0,99", "2,0,0,0,99")]
        [TestCase("2,3,0,3,99", "2,3,0,6,99")]
        [TestCase("2,4,4,5,99,0", "2,4,4,5,99,9801")]
        [TestCase("1,1,1,4,99,5,6,0,99", "30,1,1,4,2,5,6,0,99")]
        public void TestAddAndMultiply(string input, string expectedOutput)
        {
            var output = "";
            Assert.AreEqual(expectedOutput, output);
        }

        [TestCase("3716293", "6429")]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }
    }
}
