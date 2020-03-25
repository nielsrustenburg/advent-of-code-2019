using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AoC.Day3
{
    class Tests
    {
        const string testCase1 = "R8,U5,L5,D3\n"+
                                 "U7,R6,D4,L4";

        const string testCase2 = @"R75,D30,R83,U83,L12,D49,R71,U7,L72
U62,R66,U55,R34,D71,R55,D58,R83";

        const string testCase3 = @"R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51
U98,R91,D20,R16,D67,R40,U7,R15,U6,R7";


        [TestCase(testCase1, "6")]
        [TestCase(testCase2, "159")]
        [TestCase(testCase3, "135")]
        public void TestPartOne(string input, string expectedOutput)
        {
            Solver solver = new Solver(Common.Input.InputMode.String, input);
            Assert.AreEqual(expectedOutput, solver.SolutionOne());
        }

        [TestCase(testCase1, "30")]
        [TestCase(testCase2, "610")]
        [TestCase(testCase3, "410")]
        public void TestPartTwo(string input, string expectedOutput)
        {
            Solver solver = new Solver(Common.Input.InputMode.String, input);
            Assert.AreEqual(expectedOutput, solver.SolutionTwo());
        }

        [TestCase("352", "43848")]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }
    }
}
