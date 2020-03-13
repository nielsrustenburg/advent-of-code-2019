using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC
{
    static class Day16
    {
        public static int SolvePartOne()
        {
            string inputString = InputReader.StringsFromTxt("d16input.txt")[0];
            List<int> input = inputString.Select(x => (int)Char.GetNumericValue(x)).ToList();
            List<int> signal = input;
            List<int> pattern = new List<int>{ 0, 1, 0, -1 };
            for(int i = 0; i < 100; i++)
            {
                signal = FlawedFrequencyTransmission(signal,pattern);
            }
            //Turn digitsequence into int
            int output = signal.Take(8).Select((x, i) => (int) Math.Pow(10,(7-i)) * x).Aggregate(0, (a, b) => a + b);
            return output;
        }

        public static int SolvePartTwo()
        {
            string inputString = InputReader.StringsFromTxt("d16input.txt")[0];
            List<int> input = inputString.Select(x => (int)Char.GetNumericValue(x)).ToList();
            List<int> signal = input;
            int messageOffset = int.Parse(inputString.Substring(0,7));
            signal = CheatMode(signal, 10000, messageOffset);
            int output = signal.Take(8).Select((x, i) => (int)Math.Pow(10, (7 - i)) * x).Aggregate(0, (a, b) => a + b);
            return output;
        }

        public static List<int> FlawedFrequencyTransmission(List<int> signal, List<int> pattern)
        {
            List<int> outputSignal = new List<int>();
            for(int i = 0; i < signal.Count; i++)
            {
                int outcome = 0;
                //Multiply entire list by i*repeat pattern
                for(int j = 0; j < signal.Count; j++)
                {
                    int num = MultiplyByPattern(signal[j], j+1, i+1, pattern);
                    outcome += num;
                }
                outputSignal.Add(Math.Abs(outcome % 10));
            }
            return outputSignal;
        }

        public static List<int> CheatMode(List<int> ogSig, int reps, int messageOffset)
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

            for(int i = 0; i < remainingReps; i++)
            {
                signal.AddRange(ogSig);
            }

            for (int n = 0; n < 100; n++)
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

