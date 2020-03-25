using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;
using AoC.Computers;
using AoC.Utils;
using NUnit.Framework;

namespace AoC.Day9
{
    class Tests
    {
        [TestCase("109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99", "109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99")]
        [TestCase("104,1125899906842624,99", "1125899906842624")]
        public void TestOutputs(string input, string expectedOutput)
        {
            var program = InputParser<BigInteger>.ParseCSVLine(input, BigInteger.Parse);
            var intcode = new BigIntCode(program);
            var output = string.Join(',', intcode.Run().Select(x => x.ToString()));
            Assert.AreEqual(expectedOutput, output);
        }

        [TestCase("1102,34915192,34915192,7,4,7,99,0", 16)]
        public void TestOutputDigitCount(string input, int digits)
        {
            var program = InputParser<BigInteger>.ParseCSVLine(input, BigInteger.Parse);
            var intcode = new BigIntCode(program);
            var output = intcode.Run().First().ToString();
            Assert.AreEqual(digits, output.Length);
        }

        [TestCase("2453265701", "80805")]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }
    }
}
