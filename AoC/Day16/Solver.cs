using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AoC.Common;
using AoC.Utils;

namespace AoC.Day16
{
    class Solver : PuzzleSolver
    {
        List<int> signal;
        public Solver() : this(Input.InputMode.Embedded, "input")
        {
        }

        public Solver(Input.InputMode mode, string input) : base(mode, input)
        {
        }

        protected override void ParseInput(string input)
        {
            signal = InputParser.Split(input).First().Select(digit => (int)char.GetNumericValue(digit)).ToList();
        }

        protected override void PrepareSolution()
        {
            //no common prep
        }

        protected override void SolvePartOne()
        {
            var digits = Solver.RunFFT(signal, 100).Take(8);
            resultPartOne = string.Join("", digits.Select(d => d.ToString()));
        }

        protected override void SolvePartTwo()
        {
            int messageOffset = signal.Take(7).Aggregate(0, (a, b) => a * 10 + b);
            var newSignal = CheatMode(signal, 10000, 100, messageOffset);
            var digits = newSignal.Take(8);
            resultPartTwo = string.Join("", digits.Select(d => d.ToString()));
        }

        public static List<int> RunFFT(IEnumerable<int> signal, int phases)
        {
            var pattern = new List<int> { 0, 1, 0, -1 };
            var newSignal = new List<int>(signal);
            for (int i = 0; i < phases; i++)
            {
                newSignal = FlawedFrequencyTransmission(newSignal, pattern);
            }
            return newSignal;
        }

        public static List<int> FlawedFrequencyTransmission(List<int> signal, List<int> pattern)
        {
            List<int> outputSignal = new List<int>();
            for (int i = 0; i < signal.Count; i++)
            {
                int outcome = 0;
                //Multiply entire list by i*repeat pattern
                for (int j = 0; j < signal.Count; j++)
                {
                    int num = MultiplyByPattern(signal[j], j + 1, i + 1, pattern);
                    outcome += num;
                }
                outputSignal.Add(Math.Abs(outcome % 10));
            }
            return outputSignal;
        }

        public static List<int> CheatMode(List<int> ogSig, int reps, int phases, int messageOffset)
        {
            //My solution works under the assumption that our message offset is past the halfway point of the signal digits
            //This makes our sum easy to compute (everything before becomes 0, everything after becomes 1)
            //If this was not the case we'd have to deal with the second half of the pattern (0, -1)'s 
            //Or even with repetitions of the pattern for messageOffsets very early in the signal
            //Because this last part seems like a hassle, and I assume everyone has a messageOffset past the halfway point 
            //I will leave my limited solution as is
            if (ogSig.Count * reps > 2 * messageOffset) throw new Exception("cheatmode is not fit for this messageOffset, requires offset to be at least half the inputsize");
            List<int> signal = ogSig.Skip(messageOffset % ogSig.Count).ToList();
            int remainingReps = reps - (messageOffset / ogSig.Count + 1);

            for (int i = 0; i < remainingReps; i++)
            {
                signal.AddRange(ogSig);
            }

            for (int n = 0; n < phases; n++)
            {
                List<int> newSignal = new List<int>();
                int numSum = signal.Sum();
                for (int j = 0; j < signal.Count; j++)
                {
                    newSignal.Add(numSum % 10);
                    numSum -= signal[j];
                }
                signal = newSignal;
            }
            return signal;
        }

        public static int MultiplyByPattern(int num, int j, int i, List<int> pattern)
        {
            int patNum = pattern[(j / i) % pattern.Count];
            return patNum * num;
        }
    }
}
