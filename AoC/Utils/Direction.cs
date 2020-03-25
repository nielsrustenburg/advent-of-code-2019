using System;
using System.Collections.Generic;
using System.Text;

namespace AoC.Utils
{
    //NW  N  NE    7  0  1
    //W       E    6     2
    //SW  S  SE    5  4  3

    public enum Direction : int
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest,
    }

    public static class DirectionHelper
    {
        public static Direction ClockWiseByQuarter(this Direction dir)
        {
            return ShiftBy(dir, 2);
        }

        public static Direction CounterClockWiseByQuarter(this Direction dir)
        {
            return ShiftBy(dir, -2);
        }

        public static Direction ClockWiseByEighth(this Direction dir)
        {
            return ShiftBy(dir, 1);
        }

        public static Direction CounterClockWiseByEighth(this Direction dir)
        {
            return ShiftBy(dir, -1);
        }

        public static Direction Opposite(this Direction dir)
        {
            return ShiftBy(dir, 4);
        }

        public static Direction ShiftBy(Direction dir, int increment)
        {
            return (Direction)MathHelper.Mod((int)dir + increment, 8);
        }

        public static (int x, int y) StepInDirection(Direction dir, int x, int y, int distance )
        {
            int newY = y;
            if (dir < Direction.East || dir > Direction.West)
            {
                newY += distance;
            }
            else if (dir > Direction.East && dir < Direction.West)
            {
                newY -= distance;
            }

            int newX = x;
            if (dir > Direction.North && dir < Direction.South)
            {
                newX += distance;
            }
            else if (dir > Direction.South)
            {
                newX -= distance;
            }
            return (newX, newY);
        }

        public static (int x, int y) NeighbourInDirection(Direction dir, int x, int y)
        {
            return StepInDirection(dir, x, y, 1);
        }
    }
}
