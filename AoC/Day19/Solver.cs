﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;
using AoC.Computers;
using AoC.Common;
using AoC.Utils;

namespace AoC.Day19
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
            //no common prep
        }

        protected override void SolvePartOne()
        {
            resultPartOne = TractorBeamSlice(program, 0, 50, 0, 50).ToString();
        }

        protected override void SolvePartTwo()
        {
            int targetWidth = 100;

            int[] lefts = new int[targetWidth], rights = new int[targetWidth];
            int pointer = 0;

            int xLeft = 0, xRight = 0, y = 50;
            while (true)
            {
                xLeft = FindTractorBeamEdge(program, xLeft, y, 0);

                if (xRight < xLeft) xRight = xLeft; //make sure we start inside the tractorbeam (should only be necessary for first iteration)
                xRight = FindTractorBeamEdge(program, xRight, y, 1) - 1; //-1 because we want the last x where the tractor was turned on

                int topPointer = (pointer + 1) % targetWidth;
                if (xLeft + targetWidth - 1 <= rights[topPointer])
                {
                    resultPartTwo = (10000 * xLeft + y - targetWidth + 1).ToString();
                    break;
                }

                lefts[pointer] = xLeft;
                rights[pointer] = xRight;
                pointer = (pointer + 1) % targetWidth;
                y++;
            }
        }

        public static int TractorBeamSlice(List<BigInteger> program, int xStart, int xCount, int yStart, int yCount, bool draw = false)
        {
            int pulls = 0;
            List<string> rows = new List<string>();
            for (int y = 0; y < yCount; y++)
            {
                string row = "";
                for (int x = 0; x < xCount; x++)
                {
                    BigIntCode bot = new BigIntCode(program);
                    List<BigInteger> output = bot.Run(new List<BigInteger> { x + xStart, y + yStart });
                    pulls += (int)output[0];
                    row = row + (output[0] == 0 ? "." : "#");
                }
                rows.Add(row);
            }
            if (draw)
            {
                Console.Clear();
                foreach (var row in rows)
                {
                    Console.WriteLine(row);
                }
            }
            return pulls;
        }

        public static int FindTractorBeamEdge(IEnumerable<BigInteger> program, int initialXval, int y, BigInteger whileStatus)
        {
            //Finds the x at which the tractorstatus does not match whileStatus
            int x = initialXval;
            BigInteger tractorStatus;
            do
            {
                BigIntCode EdgeFinder = new BigIntCode(program);
                tractorStatus = EdgeFinder.Run(new List<BigInteger> { x, y }).First();
                x++;
            } while (tractorStatus == whileStatus);
            return x - 1;
        }
    }
}