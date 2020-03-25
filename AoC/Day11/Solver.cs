using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using AoC.Computers;
using System.Linq;
using AoC.Common;
using AoC.Utils;

namespace AoC.Day11
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
            //no common preparation needed/possible for the two parts (IntCode program runs differently based on starting tile)
        }

        protected override void SolvePartOne()
        {
            Grid<char> cgrid = new Grid<char>(' '); //Using different defaultTile so we can count all tiles that have been changed
            EmergencyHullPaintingRobot bot = new EmergencyHullPaintingRobot(program, cgrid);

            while (bot.PerformStep())
            {

            }

            resultPartOne = cgrid.CountNonDefault().ToString();
        }

        protected override void SolvePartTwo()
        {
            Grid<char> cgrid = new Grid<char>('.');
            cgrid.SetTile(0, 0, '#');
            EmergencyHullPaintingRobot bot = new EmergencyHullPaintingRobot(program, cgrid);
            while (bot.PerformStep())
            {

            }
            List<string> image = cgrid.RowsAsStrings();
            string imgString = string.Join((char) 10, image);
            resultPartTwo = imgString;
        }
    }

    public class EmergencyHullPaintingRobot
    {
        int xPos;
        int yPos;
        Direction dir;
        BigIntCode brain;
        public Grid<char> PaintGrid { get; private set; }

        public EmergencyHullPaintingRobot(List<BigInteger> programming, Grid<char> cgrid, int xPos = 0, int yPos = 0)
        {
            this.xPos = xPos;
            this.yPos = yPos;
            dir = Direction.North;
            PaintGrid = cgrid;
            brain = new BigIntCode(programming);
        }

        public bool PerformStep()
        {
            List<BigInteger> input = new List<BigInteger> { InspectPanel() };
            List<BigInteger> orders = brain.Run(input);
            if (!brain.Halted)
            {
                ChangePaint(orders[0] == 1 ? '#' : '.');
                TurnAndMove(orders[1] == 1);
                return true;
            }
            return false;
        }

        private int InspectPanel()
        {
            return PaintGrid.GetTile(xPos, yPos) == '#' ? 1 : 0;
        }

        private void ChangePaint(char color)
        {
            PaintGrid.SetTile(xPos, yPos, color);
        }

        private void TurnAndMove(bool turnRight)
        {
            if (turnRight) TurnRight(); else TurnLeft();
            MoveForward();
        }

        private void TurnRight()
        {
            dir = dir.ClockWiseByQuarter();
        }

        private void TurnLeft()
        {
            dir = dir.CounterClockWiseByQuarter();
        }

        private void MoveForward()
        {
            (xPos, yPos) = DirectionHelper.NeighbourInDirection(dir, xPos, yPos);
        }

    }
}
