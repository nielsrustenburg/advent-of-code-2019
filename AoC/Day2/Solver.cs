using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;
using AoC.Computers;
using AoC.Common;
using AoC.Utils;


namespace AoC.Day2
{
    class Solver : PuzzleSolver
    {
        List<BigInteger> program;

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
            //Requires no prep
        }

        protected override void SolvePartOne()
        {
            resultPartOne = RunIntCodewithNounVerb(program, 12, 2).ToString();
        }

        protected override void SolvePartTwo()
        {
            resultPartTwo = FindNounVerb(program, 19690720).ToString();
        }

        public static BigInteger FindNounVerb(IEnumerable<BigInteger> code, int target, int lower = 0, int upper = 99)
        {
            for (int noun = lower; noun <= upper; noun++)
            {
                for (int verb = lower; verb <= upper; verb++)
                {
                    var output = RunIntCodewithNounVerb(code, noun, verb);
                    if (output == target) return 100 * noun + verb;
                }
            }
            throw new Exception($"Can't find target: {target} in range:({lower},{upper})");
        }

        public static BigInteger RunIntCodewithNounVerb(IEnumerable<BigInteger> code, int noun, int verb)
        {
            var program = new IntCode(code);
            program[1] = noun;
            program[2] = verb;
            program.Run();
            return program[0];
        }
    }
}
