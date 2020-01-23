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
            List<List<int>> ampSettingPermutations = Permutations(Enumerable.Range(0, 5).ToList());

            int bestOutput = 0;
            List<int> bestSettings = null;
            foreach(List<int> ampSettings in ampSettingPermutations)
            {
                List<IntCode> amplifiers = Enumerable.Range(0, 5).Select(_ => new IntCode(program)).ToList();
                int prevOutput = 0;
                for(int i = 0; i < ampSettings.Count; i++)
                {
                    prevOutput = amplifiers[i].Run(new List<int> { ampSettings[i], prevOutput }).Last();
                }
                if(prevOutput > bestOutput)
                {
                    bestOutput = prevOutput;
                    bestSettings = ampSettings;
                }
            }
            return bestOutput;
        }

        public static int SolvePartTwo()
        {
            //Read input
            List<int> program = InputReader.IntsFromCSVLine("d7input.txt");
            List<List<int>> ampSettingPermutations = Permutations(Enumerable.Range(5, 5).ToList());

            int bestOutput = 0;
            List<int> bestSettings = null;
            foreach (List<int> ampSettings in ampSettingPermutations)
            {
                List<IntCode> amplifiers = Enumerable.Range(0, 5).Select(_ => new IntCode(program)).ToList();
                int prevOutput = 0;
                bool first_iter = true;
                while (!amplifiers.Last().Halted)
                {
                    for (int i = 0; i < ampSettings.Count; i++)
                    {
                        var instructions = first_iter ? new List<int> { ampSettings[i], prevOutput } : new List<int> { prevOutput };
                        prevOutput = amplifiers[i].Run(instructions).Last();
                    }
                    first_iter = false;
                }
                if (prevOutput > bestOutput)
                {
                    bestOutput = prevOutput;
                    bestSettings = ampSettings;
                }
            }
            return bestOutput;
        }

        public static List<List<T>> Permutations<T>(List<T> values)
        {
            List<List<T>> result = values.Count == 0 ? new List<List<T>> {new List<T>()} : new List<List<T>>();
            foreach (T val in values)
            {
                List<List<T>> without = Permutations(values.Where(x => !x.Equals(val)).ToList());
                foreach(List<T> setWithout in without)
                {
                    setWithout.Add(val);
                    result.Add(setWithout);
                }
            }
            return result;
        }
    }
}
