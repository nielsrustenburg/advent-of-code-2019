using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AoC.Day12
{
    class Tests
    {
        const string testCase1 = @"<x=-1, y=0, z=2>
<x=2, y=-10, z=-7>
<x=4, y=-8, z=8>
<x=3, y=5, z=-1>";

        const string testCase2 = @"<x=-8, y=-10, z=0>
<x=5, y=5, z=10>
<x=2, y=-7, z=3>
<x=9, y=-8, z=-3>";

        [TestCase(testCase1,10,179)]
        [TestCase(testCase2,100,1940)]
        public void TestSimulateStep(string input,int numSteps, int expectedOutput)
        {
            var solver = new Solver(Common.Input.InputMode.String, input);
            solver.ResetMoons();
            solver.SimulateSteps(numSteps);

            Assert.AreEqual(expectedOutput, solver.EnergyInSystem());
        }

        [TestCase(testCase1, "2772")]
        [TestCase(testCase2, "4686774924")]
        public void TestFindCycle(string input, string expectedOutput)
        {
            var solver = new Solver(Common.Input.InputMode.String, input);
            var output = solver.FindCycle().ToString();
            Assert.AreEqual(expectedOutput, output);
        }

        [TestCase("10635", "583523031727256")]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }
    }
}
