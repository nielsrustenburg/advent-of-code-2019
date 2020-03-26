using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AoC.Day8
{
    class Tests
    {
        const string expectedImage1 = @"123
456

789
012";

        const string expectedImage2 = @"02
22

11
22

22
12

00
00";

        const string expectedImageSolverP2 = @"111  1  1 1111 111  111  
1  1 1  1    1 1  1 1  1 
1  1 1  1   1  111  1  1 
111  1  1  1   1  1 111  
1 1  1  1 1    1  1 1    
1  1  11  1111 111  1    ";

        [TestCase("123456789012", 3,2,expectedImage1)]
        [TestCase("0222112222120000", 2,2,expectedImage2)]
        public void TestImage(string input, int width, int height, string expectedOutput)
        {
            var img = new Image(width, height, input);
            Assert.AreEqual(expectedOutput, img.ShowLayers());
        }

        [TestCase("1441", expectedImageSolverP2)]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }
    }
}
