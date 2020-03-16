using System;
using System.Collections.Generic;
using System.Text;

namespace AoC.common
{
    abstract class PuzzleSolver : IPuzzleSolver
    {
        protected string resultPartOne;
        protected string resultPartTwo;
        protected int dayNum;

        protected PuzzleSolver(int dayNum)
        {
            this.dayNum = dayNum;
        }

        public void WriteSolutions()
        {
            Console.WriteLine($"Day {dayNum} \n {resultPartOne} \n {resultPartTwo} \n");
        }

        protected abstract void SolvePartOne();

        protected abstract void SolvePartTwo();

        public string SolutionOne()
        {
            return resultPartOne;
        }

        public string SolutionTwo()
        {
            return resultPartTwo;
        }
    }
}
