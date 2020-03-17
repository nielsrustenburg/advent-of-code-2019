using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AoC.common;
using AoC.Utils;
using AoC.Tests;
namespace AoC.Day16
{
    class Tester : PuzzleSolverTester
    {
        public Tester(IPuzzleSolver solver, string expectedOne, string expectedTwo) : base(solver, expectedOne, expectedTwo)
        {
        }

        public override void RunAll()
        {
            TestFlawedFrequencyTransmission();
            TestMultiplyByPattern();
            TestSolver();
        }

        public static void TestMultiplyByPattern()
        {
            List<int> signal = Enumerable.Range(1, 8).ToList();
            List<int> pattern = new List<int> { 0, 1, 0, -1 };
            List<int> outcome = new List<int>();
            List<int> expOutcome = new List<int> { 1, 0, -3, 0, 5, 0, -7, 0 };
            foreach (int i in signal)
            {
                outcome.Add(Solver.MultiplyByPattern(i, i, 1, pattern));
            }
            for (int j = 0; j < signal.Count; j++)
            {
                if (outcome[j] != expOutcome[j]) throw new Exception($"Expected: {expOutcome[j]} found: {outcome[j]}");
            }
        }

        public static void TestFlawedFrequencyTransmission()
        {
            List<int> signal = Enumerable.Range(1, 8).ToList();
            List<int> pattern = new List<int> { 0, 1, 0, -1 };
            List<int> expOutcome = new List<int> { 4, 8, 2, 2, 6, 1, 5, 8 };
            List<int> outcome = Solver.FlawedFrequencyTransmission(signal, pattern);
            for (int j = 0; j < signal.Count; j++)
            {
                if (outcome[j] != expOutcome[j]) throw new Exception($"Expected: {expOutcome[j]} found: {outcome[j]}");
            }
        }
    }
}
