using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC
{
    static class Day7
    {
        public static int SolvePartOne()
        {
            List<int> program = InputReader.IntsFromCSVLine("d7input.txt");
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

            return bestOutput;
        }

        public static int SolvePartTwo()
        {
            List<int> program = InputReader.IntsFromCSVLine("d7input.txt");
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
                    prevOutput = RunAmplifiersWithInstructions(amplifiers,intcodeInstructions,prevOutput);
                    intcodeInstructions = ampSetting.Select(_ => new List<int>()).ToList();
                }

                if (prevOutput > bestOutput)
                {
                    bestOutput = prevOutput;
                    bestSetting = ampSetting;
                }
            }
            return bestOutput;
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
