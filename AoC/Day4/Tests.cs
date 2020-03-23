using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AoC.Day4
{
    class Tests
    {
        [TestCase(111111,true)]
        [TestCase(923456,false)]
        [TestCase(223450, false)]
        [TestCase(123789, true)]
        [TestCase(1234, true)]
        [TestCase(-1234, true)]
        public void TestIncreasingDigits(int input, bool expectedOutput)
        {
            var output = Solver.IncreasingDigits(input);
            Assert.AreEqual(expectedOutput, output);
        }

        [TestCase(111111, true)]
        [TestCase(923456, false)]
        [TestCase(223450, true)]
        [TestCase(123789, false)]
        [TestCase(12378, false)]
        [TestCase(2344,true)]
        public void TestMatchingDigits(int input, bool expectedOutput)
        {
            var output = Solver.MatchingDigits(input);
            Assert.AreEqual(expectedOutput, output);
        }

        [TestCase(112233, true)]
        [TestCase(443544, true)]
        [TestCase(123444, false)]
        [TestCase(111122, true)]
        [TestCase(778888, true)]
        public void TestContainsDigitPair(int input, bool expectedOutput)
        {
            var output = Solver.ContainsDigitPair(input);
            Assert.AreEqual(expectedOutput, output);
        }

        [TestCase("544", "334")]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }
    }
}
