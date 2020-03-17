using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AoC.common;
using AoC.Utils;
using AoC.Tests;
namespace AoC.Day18
{
    class Tester : PuzzleSolverTester
    {
        public Tester(IPuzzleSolver solver, string expectedOne, string expectedTwo) : base(solver, expectedOne, expectedTwo)
        {
        }

        public override void RunAll()
        {
            TestEliminateDeadEnds();
            TestSolver();
        }

        static void TestEliminateDeadEnds()
        {
            List<string> mazeIn = new List<string> {"########",
                                                    "###.####",
                                                    "###.####",
                                                    "###.####",
                                                    "#......#",
                                                    "#.####.#",
                                                    "#......#",
                                                    "########"};

            Maze<char> maze = new Maze<char>(mazeIn, '#', new List<char> { '.' });
            maze.EliminateDeadEnds(new HashSet<char> { '.' }, '#', new HashSet<char> { '#' });

            List<string> mazeOut = maze.RowsAsStrings();
            List<string> expectedMazeOut = new List<string> {"########",
                                                             "########",
                                                             "########",
                                                             "########",
                                                             "#......#",
                                                             "#.####.#",
                                                             "#......#",
                                                             "########"};

            TestSuite.TestSequence(expectedMazeOut, 0, (int _) => mazeOut);
        }
    }
}
