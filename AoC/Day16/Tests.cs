using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AoC.Day16
{
    class Tests
    {
        [TestCase("12345678", 4, "01029498")]
        [TestCase("80871224585914546619083218645595", 100, "24176176")]
        [TestCase("19617804207202209144916044189917", 100, "73745418")]
        [TestCase("69317163492948606335995924319873", 100, "52432133")]
        public void TestPartOne(string input, string expectedOutput)
        {
            var output = "";
            Assert.AreEqual(expectedOutput, output);
        }

        [TestCase("03036732577212944063491565474664", "84462026")]
        [TestCase("02935109699940807407585447034323", "78725270")]
        [TestCase("03081770884921959731165446850517", "53553731")]
        public void TestPartTwo(string input, string expectedOutput)
        {
            var output = "";
            Assert.AreEqual(expectedOutput, output);
        }

        [TestCase("34694616", "17069048")]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }
    }
}
