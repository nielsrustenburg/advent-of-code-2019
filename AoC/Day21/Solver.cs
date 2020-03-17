using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;
using AoC.common;
using AoC.Utils;

namespace AoC.Day21
{
    class Solver : PuzzleSolver
    {
        List<BigInteger> program;

        public Solver() : base(21)
        {
        }

        public Solver(IEnumerable<string> input) : base(input, 21)
        {
        }

        protected override void ParseInput(IEnumerable<string> input)
        {
            program = InputParser.ParseCSVBigIntegers(input).First().ToList();
        }

        protected override void PrepareSolution()
        {
            //no common prep
        }

        protected override void SolvePartOne()
        {
            SpringDroid springdroid = new SpringDroid(program, false);

            //if any of ABC has a hole and D is safe, jump to D
            springdroid.AddInstruction("NOT A T");
            springdroid.AddInstruction("NOT B J");
            springdroid.AddInstruction("OR T J");
            springdroid.AddInstruction("NOT C T");
            springdroid.AddInstruction("OR T J");
            springdroid.AddInstruction("AND D J");
            resultPartOne = springdroid.Walk().ToString();
        }

        protected override void SolvePartTwo()
        {
            SpringDroid springdroid = new SpringDroid(program, false);
            springdroid.AddInstruction("NOT A T");
            springdroid.AddInstruction("NOT B J");
            springdroid.AddInstruction("OR T J");
            springdroid.AddInstruction("NOT C T");
            springdroid.AddInstruction("OR T J");
            springdroid.AddInstruction("AND D J");
            springdroid.AddInstruction("NOT H T");
            springdroid.AddInstruction("AND E T");
            springdroid.AddInstruction("OR H T");
            springdroid.AddInstruction("AND T J");
            resultPartTwo = springdroid.Run().ToString();
        }
    }

    class SpringDroid
    {
        ASCIIComputer brain;
        bool outputToConsole;

        public SpringDroid(List<BigInteger> programming, bool toConsole)
        {
            outputToConsole = toConsole;
            brain = new ASCIIComputer(programming);
            string prompt = brain.RunString();
            if (outputToConsole) Console.Write(prompt);
        }

        public void AddInstruction(string instruction)
        {
            string output = brain.RunString(instruction);
            if (outputToConsole) Console.Write(output);
        }

        public BigInteger Walk()
        {
            List<BigInteger> output = brain.Run("WALK");
            int takeN = output.Count;
            if (output.Last() > 255)
            {
                takeN--;
            }
            if (outputToConsole) Console.Write(ASCIIHelper.ASCIIToString(output.Take(takeN)));
            return output.Last();
        }

        public BigInteger Run()
        {
            List<BigInteger> output = brain.Run("RUN");
            int takeN = output.Count;
            if (output.Last() > 255)
            {
                takeN--;
            }
            if (outputToConsole) Console.Write(ASCIIHelper.ASCIIToString(output.Take(takeN)));
            return output.Last();
        }
    }
}
