using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;
using AoC.Common;
using AoC.Computers;
using AoC.Utils;

namespace AoC.Day25
{
    class Solver : PuzzleSolver
    {
        List<BigInteger> program;

        public Solver() : base(25)
        {
        }

        public Solver(IEnumerable<string> input) : base(input, 25)
        {
        }

        protected override void ParseInput(IEnumerable<string> input)
        {
            program = InputParser.ParseCSVBigIntegers(input).First().ToList();
        }

        protected override void PrepareSolution()
        {
            //not even a part 2 this time :D
        }

        protected override void SolvePartOne()
        {
            //Did this exercise manually
            //Strategy is to find all items that are safe to pick up (some end your game, or put your IntCode computer in an endless loop)
            //Bring all safe items to the checkpoint
            //Try each individual item to single out items which are too heavy to be part of a good solution
            //Put all remaining items together, remove individual items to find which items are essential to a solution (too light when removed)
            //Remaining items are the items which are not essential & not useless, try combinations of these
            //If your search-space is still quite big I recommend some A-Priori-like algorithm to eliminate combinations of items
            resultPartOne = "34095120";
        }

        public void PlayTheGame()
        {
            Console.Clear();
            ManualASCIIComputer rd22 = new ManualASCIIComputer(program);
            rd22.RunManual();
        }

        protected override void SolvePartTwo()
        {
            resultPartTwo = "no part two";
        }
    }
}
