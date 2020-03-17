using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;
using AoC.common;
using AoC.Utils;

namespace AoC.Day9
{
    class Solver : PuzzleSolver
    {
        List<BigInteger> program;
        List<BigInteger> output1;
        List<BigInteger> output2;

        public Solver() : base(9)
        {
        }

        public Solver(IEnumerable<string> input) : base(input, 9)
        {
        }

        protected override void ParseInput(IEnumerable<string> input)
        {
            program = InputParser.ParseCSVBigIntegers(input).First().ToList();
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
