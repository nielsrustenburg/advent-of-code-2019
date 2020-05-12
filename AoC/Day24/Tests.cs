using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using AoC.Common;
using AoC.Utils;

namespace AoC.Day24
{
    class Tests
    {

        const string initialState = @"....#
#..#.
#..##
..#..
#....";

        const string stateAfterOneStep = @"#..#.
####.
###.#
##.##
.##..";

        const string firstRepeatedState = @".....
.....
.....
#....
.#...";

        [TestCase(firstRepeatedState, 2129920)]
        public void TestBiodiversity(string input, int expectedOutput)
        {
            var inputWithoutNewLines = new string(input.Where(c => c == '#' || c == '.').ToArray());
            var grid = new Solver.ErisGrid(inputWithoutNewLines, false);
            Assert.AreEqual(expectedOutput, grid.GetBiodiversity());
        }

        [TestCase(initialState, stateAfterOneStep)]
        public void TestUpdate(string input, string expectedState)
        {
            var inputWithoutNewLines = new string(input.Where(c => c == '#' || c == '.').ToArray());
            var expectedWithoutNewLines = new string(expectedState.Where(c => c == '#' || c == '.').ToArray());
            var expectedGrid = new Solver.ErisGrid(expectedWithoutNewLines, false);
            var inputGrid = new Solver.ErisGrid(inputWithoutNewLines, false);
            inputGrid.Step();
            inputGrid.GetPrintableState().ToList().ForEach(s => System.Diagnostics.Trace.WriteLine(s));

            Assert.AreEqual(expectedGrid.GetBiodiversity(), inputGrid.GetBiodiversity());
        }

        [TestCase(initialState, "2129920")]
        public void TestPartOne(string input, string expectedOutput)
        {
            var solver = new Solver(Input.InputMode.String, input);
            var output = solver.SolutionOne();
            Assert.AreEqual(expectedOutput, output);
        }

        [TestCase(initialState, 10, 99)]
        public void TestRecursive(string input, int steps, int expectedOutput)
        {
            var inputWithoutNewLines = new string(input.Where(c => c == '#' || c == '.').ToArray());
            var grid = new Solver.ErisGrid(inputWithoutNewLines, true);
            grid.DoTimeSteps(steps);
            var output = grid.CountAllBugs(true);
            Assert.AreEqual(expectedOutput, output);
        }

        [TestCase("32509983", "2012")]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }
    }
}
