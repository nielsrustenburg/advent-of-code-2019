using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC.Tests
{
    static class Day18Tests
    {
        public static void RunAll()
        {
            TestEliminateDeadEnds();
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day18Tests Completed!");
        }

        static void TestSolvePartOne()
        {
            TestSuite.Test(3146, Day18.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            TestSuite.Test(2194, Day18.SolvePartTwo);
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

            Doolhof<char> maze = new Doolhof<char>(mazeIn, '#', new List<char> { '.' });
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
