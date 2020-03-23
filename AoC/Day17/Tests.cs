using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AoC.Day17
{
    class Tests
    {
        const string testCase1 = @"..#..........
..#..........
#######...###
#.#...#...#.#
#############
..#...#...#..
..#####...^..";

        const string testCase2 = @"#######...#####
#.....#...#...#
#.....#...#...#
......#...#...#
......#...###.#
......#.....#.#
^########...#.#
......#.#...#.#
......#########
........#...#..
....#########..
....#...#......
....#...#......
....#...#......
....#####......";

        [TestCase("13580", "1063081")]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }
    }
}
