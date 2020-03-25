using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AoC.Computers;
using AoC.Common;
using AoC.Utils;

namespace AoC.Day5
{
    class Solver : PuzzleSolver
    {
        List<int> program;
        List<int> output1;
        List<int> output2;

        public Solver() : this(Input.InputMode.Embedded, "input")
        {
        }

        public Solver(Input.InputMode mode, string input) : base(mode, input)
        {
        }

        protected override void ParseInput(string input)
        {
            program = InputParser<int>.ParseCSVLine(input, int.Parse).ToList();
        }

        protected override void PrepareSolution()
        {
            var ic1 = new IntCode(program);
            output1 = ic1.Run(new List<int> { 1 });
            var ic2 = new IntCode(program);
            output2 = ic2.Run(new List<int> { 5 });
        }

        protected override void SolvePartOne()
        {
            resultPartOne = output1.Last().ToString();
        }

        protected override void SolvePartTwo()
        {
            resultPartTwo = output2.Last().ToString();
        }
    }
}
