using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AoC.Day11
{
    class Tests
    {
        [TestCase("", "")]
        public void Test(string input, string expectedOutput)
        {
            var output = "";
            Assert.AreEqual(expectedOutput, output);
        }

        const string p2expOut = "..##..###..#....###....##.####..##..#..#...\n" +
                                ".#..#.#..#.#....#..#....#....#.#..#.#..#...\n" +
                                ".#....###..#....#..#....#...#..#....#..#...\n" +
                                ".#....#..#.#....###.....#..#...#....#..#...\n" +
                                ".#..#.#..#.#....#....#..#.#....#..#.#..#...\n" +
                                "..##..###..####.#.....##..####..##...##....";

        [TestCase("2276", p2expOut)]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }
    }
}
