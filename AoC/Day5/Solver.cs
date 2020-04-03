using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;
using AoC.Computers;
using AoC.Common;
using AoC.Utils;

namespace AoC.Day5
{
    class Solver : PuzzleSolver
    {
        List<BigInteger> program;
        List<BigInteger> output1;
        List<BigInteger> output2;

        public Solver() : this(Input.InputMode.Embedded, "input")
        {
        }

        public Solver(Input.InputMode mode, string input) : base(mode, input)
        {
        }

        protected override void ParseInput(string input)
        {
            program = InputParser<BigInteger>.ParseCSVLine(input, BigInteger.Parse).ToList();
        }

        protected override void PrepareSolution()
        {
            var ic1 = new IntCode(program);
            output1 = ic1.Run(new List<BigInteger> { 1 });
            var ic2 = new IntCode(program);
            output2 = ic2.Run(new List<BigInteger> { 5 });
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
