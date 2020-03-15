using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AoC.Utils;

namespace AoC.Tests
{
    static class Day11Tests
    {
        public static void RunAll()
        {
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day11Tests Completed!");
        }

        static void TestSolvePartOne()
        {
            TestSuite.Test(2276, Day11.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            List<string> expectedImage = new List<string>
            {
                "..##..###..#....###....##.####..##..#..#...",
                ".#..#.#..#.#....#..#....#....#.#..#.#..#...",
                ".#....###..#....#..#....#...#..#....#..#...",
                ".#....#..#.#....###.....#..#...#....#..#...",
                ".#..#.#..#.#....#....#..#.#....#..#.#..#...",
                "..##..###..####.#.....##..####..##...##...."
            };//CBLPJZCU

            TestSuite.TestSequence(expectedImage, 0, (int _) => { return Day11.SolvePartTwo(); });
        }
    }
}
