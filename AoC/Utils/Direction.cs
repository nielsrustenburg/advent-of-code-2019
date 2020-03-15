using System;
using System.Collections.Generic;
using System.Text;

namespace AoC.Utils
{
    //NW  N  NE    7  0  1
    //W       E    6     2
    //SW  S  SE    5  4  3

    enum Direction : int
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

    static class DirectionHelper
    {
        public static Direction ClockWiseByQuarter(this Direction dir)
        {
            return ShiftedBy(dir, 2);
        }

        public static Direction CounterClockWiseByQuarter(this Direction dir)
        {
            return ShiftedBy(dir, -2);
        }

        public static Direction ClockWiseByEighth(this Direction dir)
        {
            return ShiftedBy(dir, 1);
        }

        public static Direction CounterClockWiseByEighth(this Direction dir)
        {
            return ShiftedBy(dir, -1);
        }

        public static Direction Opposite(this Direction dir)
        {
            return ShiftedBy(dir, 4);
        }

        public static Direction ShiftedBy(Direction dir, int increment)
        {
            return (Direction)MathHelper.Mod((int)dir + increment, 8);
        }

        public static (int x, int y) NeighbourInDirection(Direction dir, int x, int y)
        {
            int newY = y;
            if (dir < Direction.East || dir > Direction.West)
            {
                newY++;
            }
            else if (dir > Direction.East && dir < Direction.West)
            {
                newY--;
            }

            int newX = x;
            if (dir > Direction.North && dir < Direction.South)
            {
                newX++;
            }
            else if (dir > Direction.South)
            {
                newX--;
            }
            return (newX, newY);
        }
    }
}
