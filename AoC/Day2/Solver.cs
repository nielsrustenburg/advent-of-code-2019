using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AoC.common;
using AoC.Utils;


namespace AoC.Day2
{
    class Solver : PuzzleSolver
    {
        List<int> backup;

        public Solver() : base(2)
        {
        }

        public Solver(IEnumerable<string> input) : base(input, 2)
        {
        }

        protected override void ParseInput(IEnumerable<string> input)
        {
            backup = InputParser.ParseCSVInts(input).First().ToList();
        }

        protected override void PrepareSolution()
        {
            //Requires no prep
        }

        protected override void SolvePartOne()
        {
            resultPartOne = RunIntCodewithNV(backup, 12, 2).ToString();
        }

        protected override void SolvePartTwo()
        {
            resultPartTwo = FindNounVerb(backup, 19690720).ToString();
        }

        //CHECK CODE SOMETIME? COULD BE SHORTER???
        public static int FindNounVerb(List<int> code, int target, int lower = 0, int upper = 99)
        {
            for (int noun = lower; noun <= upper; noun++)
            {
                for (int verb = lower; verb <= upper; verb++)
                {
                    try
                    {
                        int output = RunIntCodewithNV(new List<int>(code), noun, verb);
                        if (output == target)
                        {
                            return 100 * noun + verb;
                        }
                    }
                    catch
                    {
                    }
                }
            }
            throw new Exception($"Can't find target: {target} in range:({lower},{upper})");
        }

        public static int RunIntCodewithNV(List<int> code, int noun, int verb)
        {
            IntCode program = new IntCode(code);
            program.Write(1, noun);
            program.Write(2, verb);
            program.Run();
            int output = program.GetValAtMemIndex(0);
            return output;
        }
    }
}
