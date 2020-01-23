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
            ColorGrid cgrid = new ColorGrid(".");
            EmergencyHullPaintingRobot bot = new EmergencyHullPaintingRobot(program, cgrid);

            while (bot.PerformStep())
            {

            }
            List<string> image = cgrid.GetImageStrings(true);
            foreach (string s in image)
            {
                Console.WriteLine(s);
            }
            return cgrid.PanelsPainted();
        }

        public static int SolvePartTwo()
        {
            List<BigInteger> program = ReadInput();
            ColorGrid cgrid = new ColorGrid(".");
            cgrid.SetColorAt(0, 0, "#");
            EmergencyHullPaintingRobot bot = new EmergencyHullPaintingRobot(program, cgrid);
            while (bot.PerformStep())
            {

            }
            List<string> image = cgrid.GetImageStrings(true);
            foreach (string s in image)
            {
                Console.WriteLine(s);
            }
            return 0;
        }

        public static List<BigInteger> ReadInput(string fileName = "d11input.txt")
        {
            string strInput = InputReader.StringFromLine(fileName);
            string[] splInput = strInput.Split(',');
            List<BigInteger> program = splInput.Select(x => BigInteger.Parse(x)).ToList();
            return program;
        }
    }

    public class EmergencyHullPaintingRobot
    {
        int xPos;
        int yPos;
        string direction;
        BigIntCode brain;
        public ColorGrid ColorGrid { get; private set; }

        public EmergencyHullPaintingRobot(List<BigInteger> programming, ColorGrid cgrid, int xPos = 0, int yPos = 0)
        {
            this.xPos = xPos;
            this.yPos = yPos;
            direction = "north";
            ColorGrid = cgrid;
            brain = new BigIntCode(programming);
        }

        public bool PerformStep()
        {
            List<BigInteger> input = new List<BigInteger> { InspectPanel() };
            List<BigInteger> orders = brain.Run(input);
            if (!brain.Halted)
            {
                ChangePaint(orders[0] == 1 ? "#" : ".");
                TurnAndMove(orders[1] == 1);
                return true;
            }
            return false;
        }

        private int InspectPanel()
        {
            return ColorGrid.GetColorAt(xPos, yPos) == "#" ? 1 : 0;
        }

        private void ChangePaint(string color)
        {
            ColorGrid.SetColorAt(xPos, yPos, color);
        }

        private void TurnAndMove(bool turnRight)
        {
            if (turnRight) TurnRight(); else TurnLeft();
            MoveForward();
        }

        private void TurnRight()
        {
            if (direction == "north")
            {
                direction = "east";
                return;
            }
            if (direction == "east")
            {
                direction = "south";
                return;
            }
            if (direction == "south")
            {
                direction = "west";
                return;
            }
            direction = "north";
        }

        private void TurnLeft()
        {
            if (direction == "south")
            {
                direction = "east";
                return;
            }
            if (direction == "east")
            {
                direction = "north";
                return;
            }
            if (direction == "north")
            {
                direction = "west";
                return;
            }
            direction = "south";
        }

        private void MoveForward()
        {
            if (direction == "south")
            {
                yPos--;
                return;
            }
            if (direction == "east")
            {
                xPos++;
                return;
            }

            if (direction == "north")
            {
                yPos++;
                return;
            }
            xPos--;
        }

    }
}

