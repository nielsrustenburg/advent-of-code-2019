using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;

namespace AoC.Day18
{
    class Tests
    {
        [TestCase("3146", "2194")]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }

        [TestCase(8, "8")]
        [TestCase(9, "1,8")]
        [TestCase(22, "2,4,16")]
        [TestCase(33554433, "1,33554432")]
        [TestCase(67108864, "")] //Should be too large so should return empty
        public void TestExtractKeys(int input, string expectedOutputAsString)
        {
            var output = Solver.ExtractKeys((uint) input);
            var outputAsString = string.Join(',', output.Select(k => k.ToString()));
            Assert.AreEqual(expectedOutputAsString, outputAsString);
        }
    }
}
