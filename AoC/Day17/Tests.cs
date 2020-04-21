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

        [TestCase(testCase2, "R,8,R,8,R,4,R,4,R,8,L,6,L,2,R,4,R,4,R,8,R,8,R,8,L,6,L,2")]
        public void TestFindGreedyPath(string layout, string expectedPath)
        {
            var layoutRows = layout.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var robot = new SimulationRobot(layoutRows);
            var path = string.Join(',', robot.FindGreedyPath());
            Assert.AreEqual(expectedPath, path);
        }

        [TestCase(testCase1)]
        [TestCase(testCase2)]
        public void TestGetImageStrings(string layout)
        {
            var layoutRows = layout.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var robot = new SimulationRobot(layoutRows);
            var output = robot.GetImageStrings();

            for(int i = 0; i < output.Length; i++)
            {
                Assert.AreEqual(layoutRows[i], output[i]);
            }
        }

        [TestCase(testCase2, "A,B,C,B,A,C", "R,8,R,8", "R,4,R,4,R,8", "L,6,L,2")]
        public void TestTryRoutine(string layout, string main, string a, string b, string c)
        {
            var layoutRows = layout.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var robot = new SimulationRobot(layoutRows);
            var routine = new MovementRoutine(main, a, b, c);
            Assert.IsTrue(routine.TryMainRoutine(robot));
        }

        [TestCase(testCase2)]
        public void TestFindValidRoutine(string layout)
        {
            var layoutRows = layout.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var robot = new SimulationRobot(layoutRows);
            var path = robot.FindGreedyPath();
            var routine = Solver.FindValidMovementRoutine(path, new List<string>[0], new int[0]);
            Assert.IsTrue(routine.TryMainRoutine(robot));
        }

        [TestCase("13580", "1063081")]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }
    }
}
