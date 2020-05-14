using System;
using System.Collections.Generic;
using System.Text;
using AoC.Utils;
using AoC.Common;

namespace AoC.Common
{
    abstract class PuzzleSolver : IPuzzleSolver
    {
        protected string resultPartOne;
        protected string resultPartTwo;

        protected PuzzleSolver(Input.InputMode mode, string input)
        {
            var inputString = Input.GetInput(mode, input);
            ParseInput(inputString);
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
            Console.WriteLine($"{resultPartOne} \n{resultPartTwo} \n");
        }

        protected abstract void ParseInput(string input);

        protected abstract void PrepareSolution();

        protected abstract void SolvePartOne();

        protected abstract void SolvePartTwo();
    }
}

