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
            Console.Clear();
            string inputString = "59762770781817719190459920638916297932099919336473880209100837309955133944776196290131062991588533604012789279722697427213158651963842941000227675363260513283349562674004015593737518754413236241876959840076372395821627451178924619604778486903040621916904575053141824939525904676911285446889682089563075562644813747239285344522507666595561570229575009121663303510763018855038153974091471626380638098740818102085542924937522595303462725145620673366476987473519905565346502431123825798174326899538349747404781623195253709212209882530131864820645274994127388201990754296051264021264496618531890752446146462088574426473998601145665542134964041254919435635";
            List<int> input = inputString.Select(x => (int)Char.GetNumericValue(x)).ToList();
            List<int> signal = input;
            List<int> pattern = new List<int>{ 0, 1, 0, -1 };
            for(int i = 0; i < 100; i++)
            {
                signal = FlawedFrequencyTransmission(signal,pattern);
                if(i % 10 == 0)
                {
                    Console.WriteLine(signal.Aggregate("", (a, b) => a + b.ToString()).Substring(0,20));
                }
            }
            int output = signal.Take(8).Select((x, i) => (int) Math.Pow(10,(7-i)) * x).Aggregate(0, (a, b) => a + b);
            return output;
        }

        public static int SolvePartTwo()
        {
            string inputString = "59762770781817719190459920638916297932099919336473880209100837309955133944776196290131062991588533604012789279722697427213158651963842941000227675363260513283349562674004015593737518754413236241876959840076372395821627451178924619604778486903040621916904575053141824939525904676911285446889682089563075562644813747239285344522507666595561570229575009121663303510763018855038153974091471626380638098740818102085542924937522595303462725145620673366476987473519905565346502431123825798174326899538349747404781623195253709212209882530131864820645274994127388201990754296051264021264496618531890752446146462088574426473998601145665542134964041254919435635";
            List<int> input = inputString.Select(x => (int)Char.GetNumericValue(x)).ToList();
            List<int> signal = input;
            //List<int> signal = new List<int> { 1, 2, 3, 4};
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
            Console.WriteLine();
            return signal;
        }


        public static int MultiplyByPattern(int num, int j, int i, List<int> pattern)
        {
            int patNum = pattern[(j / i) % pattern.Count];
            return patNum * num;
        }
    }
}

