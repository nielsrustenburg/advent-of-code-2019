using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;
using AoC.Common;
using AoC.Computers;
using AoC.Utils;

namespace AoC.Day7
{
    class Solver : PuzzleSolver
    {
        List<BigInteger> program;

        public Solver() : this(Input.InputMode.Embedded, "input")
        {
        }

        public Solver(Input.InputMode mode, string input) : base(mode, input)
        {
        }

        protected override void ParseInput(string input)
        {
            program = InputParser<BigInteger>.ParseCSVLine(input, BigInteger.Parse).ToList();
        }

        protected override void PrepareSolution()
        {
            //No common preparation for this one
        }

        protected override void SolvePartOne()
        {
            resultPartOne = RunAmplifiersWithRoutine(RunAmplifiersOnce, 0).ToString();
        }

        protected override void SolvePartTwo()
        {
            resultPartTwo = RunAmplifiersWithRoutine(RunAmplifiersUntilHalted, 5).ToString();
        }

        public BigInteger RunAmplifiersWithRoutine(Func<(IntCode amplifier, List<BigInteger> instructions)[], BigInteger> Routine, int firstAmplifierId, int amountOfAmplifiers = 5)
        {
            var amplifierOptions = Enumerable.Range(firstAmplifierId, amountOfAmplifiers).Select(n => (BigInteger)n).ToList();
            var amplifierOptionPermutations = SetHelper.Permutations(amplifierOptions);

            BigInteger bestOutput = 0;
            List<BigInteger> bestConfiguration = null;

            foreach (List<BigInteger> amplifiersConfiguration in amplifierOptionPermutations)
            {
                var amplifiersAndInstructions = amplifiersConfiguration.Select(phaseSetting => (new IntCode(program), new List<BigInteger> { phaseSetting })).ToArray();

                var output = Routine(amplifiersAndInstructions);

                if (output > bestOutput)
                {
                    bestOutput = output;
                    bestConfiguration = amplifiersConfiguration;
                }
            }
            return bestOutput;
        }

        private BigInteger RunAmplifiersOnce((IntCode amplifier, List<BigInteger> instructions)[] amplifiersAndInstructions)
        {
            return RunAmplifiersWithInstructions(amplifiersAndInstructions, 0);
        }

        private BigInteger RunAmplifiersUntilHalted((IntCode amplifier, List<BigInteger> instructions)[] amplifiersAndInstructions)
        {
            BigInteger prevOutput = 0;
            while (!amplifiersAndInstructions.Last().amplifier.Halted)
            {
                prevOutput = (int)RunAmplifiersWithInstructions(amplifiersAndInstructions, prevOutput);
                amplifiersAndInstructions = amplifiersAndInstructions.Select(ani => (ani.amplifier, new List<BigInteger>())).ToArray();
            }
            return prevOutput;
        }

        private BigInteger RunAmplifiersWithInstructions((IntCode amplifier, List<BigInteger> instructions)[] amplifiersAndInstructions, BigInteger initialInput)
        {
            BigInteger previousOutput = initialInput;
            foreach ((var amplifier, var instruction) in amplifiersAndInstructions)
            {
                instruction.Add(previousOutput);
                previousOutput = amplifier.Run(instruction).Last();
            }
            return previousOutput;
        }
    }
}
