using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;
using AoC.Common;
using AoC.Computers;
using AoC.Utils;

namespace AoC.Day9
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
            //No prep required
        }

        protected override void SolvePartOne()
        {
            BigIntCode bic = new BigIntCode(program);
            output1 = bic.Run(new List<BigInteger> { 1 });
            resultPartOne = output1.Last().ToString();
        }

        protected override void SolvePartTwo()
        {
            BigIntCode bic = new BigIntCode(program);
            output2 = bic.Run(new List<BigInteger> { 2 });
            resultPartTwo = output2.Last().ToString();
        }
    }
}
