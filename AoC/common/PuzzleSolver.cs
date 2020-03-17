using System;
using System.Collections.Generic;
using System.Text;
using AoC.Utils;

namespace AoC.common
{
    abstract class PuzzleSolver : IPuzzleSolver
    {
        protected string resultPartOne;
        protected string resultPartTwo;
        protected int dayNum;

        protected abstract void ParseInput(IEnumerable<string> input);

        protected abstract void PrepareSolution();

        protected abstract void SolvePartOne();

        protected abstract void SolvePartTwo();

        protected PuzzleSolver(int dayNum)
        {
            this.dayNum = dayNum;
            SolveChallenge(ReadInput());
        }

        protected PuzzleSolver(IEnumerable<string> input, int dayNum)
        {
            this.dayNum = dayNum;
            SolveChallenge(input);
        }

        private IEnumerable<string> ReadInput()
        {
            return InputReader.StringsFromTxt($"d{dayNum}input.txt");
        }

        private void SolveChallenge(IEnumerable<string> input)
        {
            ParseInput(input);
            PrepareSolution();
            SolvePartOne();
            SolvePartTwo();
        }

        public string SolutionOne()
        {
            return resultPartOne;
        }

        public string SolutionTwo()
        {
            return resultPartTwo;
        }

        public void WriteSolutions()
        {
            Console.WriteLine($"Day {dayNum} \n{resultPartOne} \n{resultPartTwo} \n");
        }
    }
}

