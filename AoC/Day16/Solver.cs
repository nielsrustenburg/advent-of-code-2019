using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AoC.Common;
using AoC.Utils;
using System.Xml.Schema;

namespace AoC.Day16
{
    class Solver : PuzzleSolver
    {
        internal static readonly int[] pattern = new int[] { 0, 1, 0, -1 };
        int[] signal;
        public Solver() : this(Input.InputMode.Embedded, "input")
        {
        }

        public Solver(Input.InputMode mode, string input) : base(mode, input)
        {
        }

        protected override void ParseInput(string input)
        {
            signal = InputParser.Split(input).First().Select(digit => (int)char.GetNumericValue(digit)).ToArray();
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

        public static int[] RunFFT(int[] signal, int phases)
        {
            var pattern = new List<int> { 0, 1, 0, -1 };
            var newSignal = signal;
            for (int i = 0; i < phases; i++)
            {
                newSignal = FlawedFrequencyTransmission(newSignal);
            }
            return newSignal;
        }

        public static int[] FlawedFrequencyTransmission(int[] signal)
        {
            var cumulativeSums = new int[signal.Length];
            int previousSum = 0;
            for (int i = 1; i <= cumulativeSums.Length; i++)
            {
                var index = cumulativeSums.Length - i;
                cumulativeSums[index] = signal[index] + previousSum;
                previousSum = cumulativeSums[index];
            }

            var outputSignal = Enumerable.Range(0, signal.Length).Select(index => ApplyPattern(index)).ToArray();
            return outputSignal;

            int ApplyPattern(int index)
            {
                int value = 0;
                int chunkFirst = index;
                int chunkLast = index * 2;

                int patternPart = 1;
                while (chunkFirst < signal.Length)
                {
                    int excessCumulativeSum;
                    if (chunkLast >= signal.Length - 1)
                    {
                        excessCumulativeSum = 0;
                    }
                    else
                    {
                        excessCumulativeSum = cumulativeSums[chunkLast + 1];
                    }
                    value += pattern[patternPart] * (cumulativeSums[chunkFirst] - excessCumulativeSum);

                    patternPart = (patternPart + 1) % 4;
                    chunkFirst += index + 1;
                    chunkLast += index + 1;
                }
                return Math.Abs(value) % 10;
            }
        }

        public static IEnumerable<int> CheatMode(int[] originalSignal, int repetitions, int phases, int messageOffset)
        {
            //My solution works under the assumption that our message offset is past the halfway point of the signal digits
            //This makes our sum easy to compute (everything before becomes 0, everything after becomes 1)
            //If this was not the case we'd have to deal with the second half of the pattern (0, -1)'s 
            //Or even with repetitions of the pattern for messageOffsets very early in the signal
            //Because this last part seems like a hassle, and I assume everyone has a messageOffset past the halfway point 
            //I will leave my limited solution as is
            if (originalSignal.Length * repetitions > 2 * messageOffset) throw new Exception("cheatmode is not fit for this messageOffset, requires offset to be at least half the inputsize");

            var firstPart = originalSignal.Skip(messageOffset % originalSignal.Length);
            int remainingReps = repetitions - (messageOffset / originalSignal.Length + 1);
            var signal = firstPart.Concat(Enumerable.Repeat(originalSignal, remainingReps).SelectMany(d => d)).ToArray();

            for (int n = 0; n < phases; n++)
            {
                int sum = 0;
                var newSignal = new int[signal.Length];
                for (int index = signal.Length - 1; index >= 0; index--)
                {
                    sum += signal[index];
                    newSignal[index] = sum % 10;
                }
                signal = newSignal;
            }
            return signal;
        }

    }
}
