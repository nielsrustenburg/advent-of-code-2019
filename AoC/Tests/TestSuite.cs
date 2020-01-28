using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace AoC.Tests
{
    static class TestSuite
    {
        public static void RunTests()
        {
            Day1Tests.RunAll();
            Day2Tests.RunAll();
            Day3Tests.RunAll();
            Day4Tests.RunAll();
            Day5Tests.RunAll();
            Day6Tests.RunAll();
            Day7Tests.RunAll();
            Day8Tests.RunAll();
            Day9Tests.RunAll();
            Day10Tests.RunAll();
            Day11Tests.RunAll();
            Day12Tests.RunAll();
            Day13Tests.RunAll();
            Day14Tests.RunAll();
            //Day15Tests.RunAll();
            //Day16Tests.RunAll();
            //Day17Tests.RunAll();
            //Day18Tests.RunAll();
            Day19Tests.RunAll();
            Day20Tests.RunAll();
            Day21Tests.RunAll();
            Day22Tests.RunAll();
            Day23Tests.RunAll();
            Day24Tests.RunAll();
            //Day25Tests.RunAll();

            GridTests.TestGrid();
        }

        public static string GetCurrentMethod()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }
    }
}
