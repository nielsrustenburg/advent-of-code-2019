using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AoC.Day11
{
    class Tests
    {
        const string expectedSolverImage = @"..##..###..#....###....##.####..##..#..#...
.#..#.#..#.#....#..#....#....#.#..#.#..#...
.#....###..#....#..#....#...#..#....#..#...
.#....#..#.#....###.....#..#...#....#..#...
.#..#.#..#.#....#....#..#.#....#..#.#..#...
..##..###..####.#.....##..####..##...##....";

        [TestCase("2276", expectedSolverImage)]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }
    }
}
