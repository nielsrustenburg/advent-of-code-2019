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
        NorthWest
    }

    public static class DirectionHelper
    {
        public static Direction[] GetAll(this Direction dir)
        {
            var result = new Direction[8];
            result[0] = dir;
            for(int i = 1; i < 8; i++)
            {
                result[i] = dir.ShiftBy(i);
            }
            return result;
        }
        public static Direction ClockWiseByQuarter(this Direction dir)
        {
            return dir.ShiftBy(2);
        }

        public static Direction CounterClockWiseByQuarter(this Direction dir)
        {
            return dir.ShiftBy(-2);
        }

        public static Direction ClockWiseByEighth(this Direction dir)
        {
            return dir.ShiftBy(1);
        }

        public static Direction CounterClockWiseByEighth(this Direction dir)
        {
            return dir.ShiftBy(-1);
        }

        public static Direction Opposite(this Direction dir)
        {
            return dir.ShiftBy(4);
        }

        public static Direction ShiftBy(this Direction dir, int increment)
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
