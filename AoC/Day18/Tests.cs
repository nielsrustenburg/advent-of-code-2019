using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AoC.Day18
{
    class Tests
    {
        [TestCase("", "")]
        public void Test(string input, string expectedOutput)
        {
            var output = "";
            Assert.AreEqual(expectedOutput, output);
        }

        //List<string> mazeIn = new List<string> {"########",
        //                                        "###.####",
        //                                        "###.####",
        //                                        "###.####",
        //                                        "#......#",
        //                                        "#.####.#",
        //                                        "#......#",
        //                                        "########"};

        //List<string> expectedMazeOut = new List<string> {"########",
        //                                                 "########",
        //                                                 "########",
        //                                                 "########",
        //                                                 "#......#",
        //                                                 "#.####.#",
        //                                                 "#......#",
        //                                                 "########"};

        [TestCase("3146", "2194")]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }
    }
}
