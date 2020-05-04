using System;
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
        TractorBeamChecker tractorBeam;

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
            tractorBeam = new TractorBeamChecker(program);
        }

        protected override void SolvePartOne()
        {
            resultPartOne = FindTractorBeamSurface(0, 50, 0, 50).ToString();
        }

        protected override void SolvePartTwo()
        {
            resultPartTwo = ExponentialSearch(100).ToString();
        }

        public int FindTractorBeamSurface(int xStart, int xCount, int yStart, int yCount)
        {
            int xLeft = 0, xRight = 0, surface = 0;
            int xLimit = xStart + xCount;
            int yLimit = yStart + yCount;
            for(int y = yStart; y < yLimit; y++)
            {
                xLeft = tractorBeam.FindEdge(xLeft, y, false, xLimit);
                if (tractorBeam[xLeft, y]) //Tractorbeam has some gaps early on, if we can't find the left edge there is no right edge
                {
                    xRight = xRight > xLeft ? xRight : xLeft;
                    xRight = tractorBeam.FindEdge(xRight, y, true, xLimit);
                    surface += xRight - xLeft; //xLeft is part of the tractorbeam xRight isn't
                }
            }
            return surface;
        }

        public int ExponentialSearch(int targetWidth)
        {
            int previousX = 0, x = 0, bound = 1;
            bool potentialTargetFound = false;

            while (!potentialTargetFound)
            {
                previousX = x;
                int y = bound - 1;
                x = tractorBeam.FindEdge(x, y + (targetWidth - 1), false);
                potentialTargetFound = tractorBeam[x + (targetWidth - 1), y];
                if (!potentialTargetFound) bound *= 2;
            }

            var lowerBound = bound / 2;
            return BinarySearch(lowerBound - 1, lowerBound, previousX, x, targetWidth);
        }

        //Call with y and stepsize s.t. y fails while y+stepSize succeeds
        private int BinarySearch(int y, int stepSize, int topX, int botX, int targetWidth)
        {
            if (stepSize == 1) return botX * 10000 + y + 1;

            int newStepSize = stepSize / 2;

            int middleY = y + newStepSize;
            int middleX = tractorBeam.FindEdge(topX, middleY+(targetWidth-1), false);
            if(tractorBeam[middleX+(targetWidth-1), middleY])
            {
                return BinarySearch(y, newStepSize, topX, middleX, targetWidth);
            } else
            {
                return BinarySearch(middleY, newStepSize, middleX, botX, targetWidth);
            }
        }

        class TractorBeamChecker
        {
            int droidsUsed;
            int previousResultsUsed;
            Dictionary<string, bool> tractorBeamLocations;
            List<BigInteger> droidProgram;

            public TractorBeamChecker(IEnumerable<BigInteger> droidProgram)
            {
                this.droidProgram = droidProgram.ToList();
                droidsUsed = 0;
                previousResultsUsed = 0;
                tractorBeamLocations = new Dictionary<string, bool>();
            }

            public bool this[int x, int y]
            {
                get
                {
                    var coordString = $"{x},{y}";
                    if (!tractorBeamLocations.TryGetValue(coordString, out bool tractorPresence))
                    {
                        tractorPresence = SendDroid(x, y);
                        tractorBeamLocations[coordString] = tractorPresence;
                    }
                    else
                    {
                        previousResultsUsed++;
                    }
                    return tractorPresence;
                }
            }

            //Finds the first x (starting at xStart) for which the beam status is opposite to beamWasActive
            public int FindEdge(int xStart, int y, bool beamWasActive, int xLimit = int.MaxValue)
            {
                int x = xStart;
                bool beamIsActive = beamWasActive;
                while (beamIsActive == beamWasActive && x < xLimit)
                {
                    beamIsActive = this[x, y];
                    x++;
                }
                if (beamIsActive != beamWasActive) return x - 1; //succeeded within limits
                return 0;
            }

            public bool SendDroid(int x, int y)
            {
                droidsUsed++;
                var droid = new IntCode(droidProgram);
                var output = droid.Run(new BigInteger[] { x, y }).First();
                return output == 1;
            }

            public void PrintUsageSummary()
            {
                Console.WriteLine($"This solver used: {droidsUsed} new droids and reused: {previousResultsUsed} results from earlier droids");
            }
        }
    }
}
