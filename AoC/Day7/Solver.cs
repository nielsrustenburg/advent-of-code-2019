using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AoC.common;
using AoC.Utils;

namespace AoC.Day7
{
    class Solver : PuzzleSolver
    {
        List<int> program;

        public Solver() : base(7)
        {
        }

        public Solver(IEnumerable<string> input) : base(input, 7)
        {
        }

        protected override void ParseInput(IEnumerable<string> input)
        {
            program = InputParser.ParseCSVInts(input).First().ToList();
        }

        protected override void PrepareSolution()
        {
            //No common preparation for this one
        }

        //Still quite a bit of overlap in code between part 1 and 2, refactor sometime
        protected override void SolvePartOne()
        {
            List<int> ampOptions = Enumerable.Range(0, 5).ToList();
            List<List<int>> ampSettingPermutations = SetHelper.Permutations(ampOptions);

            int bestOutput = 0;
            List<int> bestSetting = null;

            foreach (List<int> ampSetting in ampSettingPermutations)
            {
                List<IntCode> amplifiers = Enumerable.Range(0, 5).Select(_ => new IntCode(program)).ToList();

                List<List<int>> intcodeInstructions = SetHelper.AsSingletons(ampSetting);
                int output = RunAmplifiersWithInstructions(amplifiers, intcodeInstructions, 0);

                if (output > bestOutput)
                {
                    bestOutput = output;
                    bestSetting = ampSetting;
                }
            }

            resultPartOne = bestOutput.ToString();
        }

        protected override void SolvePartTwo()
        {
            List<int> ampOptions = Enumerable.Range(5, 5).ToList(); //6-10
            List<List<int>> ampSettingPermutations = SetHelper.Permutations(ampOptions);

            int bestOutput = 0;
            List<int> bestSetting = null;

            foreach (List<int> ampSetting in ampSettingPermutations)
            {
                List<IntCode> amplifiers = Enumerable.Range(0, 5).Select(_ => new IntCode(program)).ToList();

                int prevOutput = 0;
                List<List<int>> intcodeInstructions = SetHelper.AsSingletons(ampSetting);
                while (!amplifiers.Last().Halted)
                {
                    prevOutput = RunAmplifiersWithInstructions(amplifiers, intcodeInstructions, prevOutput);
                    intcodeInstructions = ampSetting.Select(_ => new List<int>()).ToList();
                }

                if (prevOutput > bestOutput)
                {
                    bestOutput = prevOutput;
                    bestSetting = ampSetting;
                }
            }
            resultPartTwo = bestOutput.ToString();
        }

        public static int RunAmplifiersWithInstructions(List<IntCode> amplifiers, List<List<int>> instructions, int previousOutput)
        {
            for (int i = 0; i < amplifiers.Count; i++)
            {
                instructions[i].Add(previousOutput);
                previousOutput = amplifiers[i].Run(instructions[i]).Last();
            }
            return previousOutput;
        }
    }
}
