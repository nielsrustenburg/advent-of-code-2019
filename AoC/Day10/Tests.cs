using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AoC.Day10
{
    class Tests
    {
        [TestCase(0,1,0)]
        [TestCase(1,1,45)]
        [TestCase(1,0,90)]
        [TestCase(1,-1,135)]
        [TestCase(0,-1,180)]
        [TestCase(-1,-1,225)]
        [TestCase(-1,0,270)]
        [TestCase(-1,1,315)]
        public void TestAngleDegrees(int x, int y, int expectedAngle)
        {
            var output = AsteroidVisionSet.StepTo360Angle((x, y));
            Assert.AreEqual(expectedAngle, output);
        }

        [TestCase("282", "1008")]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }
    }
}
