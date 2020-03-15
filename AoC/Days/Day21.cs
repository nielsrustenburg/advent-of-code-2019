using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AoC.Utils;

namespace AoC
{
    static class Day21
    {
        public static int SolvePartOne()
        {
            string input = InputReader.FirstLineFromTxt("d21input.txt");
            List<BigInteger> program = input.Split(',').Select(x => BigInteger.Parse(x)).ToList();
            SpringDroid springdroid = new SpringDroid(program, false);

            //if any of ABC has a hole and D is safe, jump to D
            springdroid.AddInstruction("NOT A T");
            springdroid.AddInstruction("NOT B J");
            springdroid.AddInstruction("OR T J");
            springdroid.AddInstruction("NOT C T");
            springdroid.AddInstruction("OR T J");
            springdroid.AddInstruction("AND D J");
            return (int) springdroid.Walk();
        }

        public static int SolvePartTwo()
        {
            string input = InputReader.FirstLineFromTxt("d21input.txt");
            List<BigInteger> program = input.Split(',').Select(x => BigInteger.Parse(x)).ToList();
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
            return (int) springdroid.Run();
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
            if(outputToConsole)Console.Write(ASCIIHelper.ASCIIToString(output.Take(takeN)));
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

