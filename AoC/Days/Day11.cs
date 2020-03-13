using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

namespace AoC
{
    static class Day11
    {
        public static int SolvePartOne()
        {
            List<BigInteger> program = ReadInput();
            Grid<char> cgrid = new Grid<char>(' '); //Using different defaultTile so we can count all tiles that have been changed
            EmergencyHullPaintingRobot bot = new EmergencyHullPaintingRobot(program, cgrid);

            while (bot.PerformStep())
            {

            }

            return cgrid.CountNonDefault();
        }

        public static List<string> SolvePartTwo()
        {
            List<BigInteger> program = ReadInput();
            Grid<char> cgrid = new Grid<char>('.');
            cgrid.SetTile(0, 0, '#');
            EmergencyHullPaintingRobot bot = new EmergencyHullPaintingRobot(program, cgrid);
            while (bot.PerformStep())
            {

            }
            List<string> image = cgrid.RowsAsStrings();
            return image;
        }

        public static List<BigInteger> ReadInput(string fileName = "d11input.txt")
        {
            string strInput = InputReader.FirstLineFromTxt(fileName);
            string[] splInput = strInput.Split(',');
            List<BigInteger> program = splInput.Select(x => BigInteger.Parse(x)).ToList();
            return program;
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
            (xPos, yPos) = DirectionHelper.NeighbourInDirection(dir,xPos, yPos);
        }

    }
}

