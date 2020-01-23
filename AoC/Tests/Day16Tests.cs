using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC.Tests
{
    static class Day16Tests
    {
        public static void RunAll()
        {
            TestMultiplyByPattern();
            TestFlawedFrequencyTransmission();
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day16Tests Completed!");
        }

        public static void TestMultiplyByPattern()
        {
            List<int> signal = Enumerable.Range(1, 8).ToList();
            List<int> pattern = new List<int> { 0, 1, 0, -1 };
            List<int> outcome = new List<int>();
            List<int> expOutcome = new List<int> { 1, 0, -3, 0, 5, 0, -7, 0 };
            foreach(int i in signal)
            {
                outcome.Add(Day16.MultiplyByPattern(i, i, 1, pattern));
            }
            for(int j = 0; j < signal.Count; j++)
            {
                if (outcome[j] != expOutcome[j]) throw new Exception($"Expected: {expOutcome[j]} found: {outcome[j]}");
            }
        }

        public static void TestFlawedFrequencyTransmission()
        {
            List<int> signal = Enumerable.Range(1, 8).ToList();
            List<int> pattern = new List<int> { 0, 1, 0, -1 };
            List<int> expOutcome = new List<int> { 4, 8, 2, 2, 6, 1, 5, 8 };
            List<int> outcome = Day16.FlawedFrequencyTransmission(signal, pattern);
            for (int j = 0; j < signal.Count; j++)
            {
                if (outcome[j] != expOutcome[j]) throw new Exception($"Expected: {expOutcome[j]} found: {outcome[j]}");
            }
        }

        public static void TestSolvePartOne()
        {
            void Test(int expectedOutput)
            {
                int output = Day16.SolvePartOne();
                if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
            }
            Test(34694616);
        }

        public static void TestSolvePartTwo()
        {
            void Test(int expectedOutput)
            {
                int output = Day16.SolvePartTwo();
                if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
            }
            Test(17069048);
        }
    }
}
