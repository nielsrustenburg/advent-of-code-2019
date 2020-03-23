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

        public Solver() : base(5)
        {
        }
        public Solver(IEnumerable<string> input) : base(input, 5)
        {
        }

        protected override void ParseInput(IEnumerable<string> input)
        {
            program = InputParser.ParseCSVInts(input).First().ToList();
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
