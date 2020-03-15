using System;
using System.Collections.Generic;
using System.Text;
using AoC.Utils;

namespace AoC.Tests
{
    class DirectionTests
    {
        public static void RunAll()
        {
            TestTurnQuarter();
            TestNeighbourInDirection();
        }

        public static void TestTurnQuarter()
        {
            TestSuite.Test(Direction.North, Direction.West.ClockWiseByQuarter(), "TestTurnQuarter");
            TestSuite.Test(Direction.NorthEast, Direction.NorthWest.ClockWiseByQuarter(), "TestTurnQuarter");
            TestSuite.Test(Direction.East, Direction.North.ClockWiseByQuarter(), "TestTurnQuarter");
            TestSuite.Test(Direction.SouthEast, Direction.NorthEast.ClockWiseByQuarter(), "TestTurnQuarter");
            TestSuite.Test(Direction.South, Direction.East.ClockWiseByQuarter(), "TestTurnQuarter");
            TestSuite.Test(Direction.SouthWest, Direction.SouthEast.ClockWiseByQuarter(), "TestTurnQuarter");
            TestSuite.Test(Direction.West, Direction.South.ClockWiseByQuarter(), "TestTurnQuarter");
            TestSuite.Test(Direction.NorthWest, Direction.SouthWest.ClockWiseByQuarter(), "TestTurnQuarter");

            TestSuite.Test(Direction.South, Direction.West.CounterClockWiseByQuarter(), "TestTurnQuarter");
            TestSuite.Test(Direction.SouthWest, Direction.NorthWest.CounterClockWiseByQuarter(), "TestTurnQuarter");
            TestSuite.Test(Direction.West, Direction.North.CounterClockWiseByQuarter(), "TestTurnQuarter");
            TestSuite.Test(Direction.NorthWest, Direction.NorthEast.CounterClockWiseByQuarter(), "TestTurnQuarter");
            TestSuite.Test(Direction.North, Direction.East.CounterClockWiseByQuarter(), "TestTurnQuarter");
            TestSuite.Test(Direction.NorthEast, Direction.SouthEast.CounterClockWiseByQuarter(), "TestTurnQuarter");
            TestSuite.Test(Direction.East, Direction.South.CounterClockWiseByQuarter(), "TestTurnQuarter");
            TestSuite.Test(Direction.SouthEast, Direction.SouthWest.CounterClockWiseByQuarter(), "TestTurnQuarter");
        }

        public static void TestNeighbourInDirection()
        {
            (int x,int y) = (0, 0);
            TestSuite.Test((0, 1), DirectionHelper.NeighbourInDirection(Direction.North, x, y), "TestNeighbourInDirection: North");
            TestSuite.Test((1, 1), DirectionHelper.NeighbourInDirection(Direction.NorthEast, x, y), "TestNeighbourInDirection: NorthEast");
            TestSuite.Test((1, 0), DirectionHelper.NeighbourInDirection(Direction.East, x, y), "TestNeighbourInDirection: East");
            TestSuite.Test((1, -1), DirectionHelper.NeighbourInDirection(Direction.SouthEast, x, y), "TestNeighbourInDirection: SouthEast");
            TestSuite.Test((0, -1), DirectionHelper.NeighbourInDirection(Direction.South, x, y), "TestNeighbourInDirection: South");
            TestSuite.Test((-1, -1), DirectionHelper.NeighbourInDirection(Direction.SouthWest, x, y), "TestNeighbourInDirection: SouthWest");
            TestSuite.Test((-1, 0), DirectionHelper.NeighbourInDirection(Direction.West, x, y), "TestNeighbourInDirection: West");
            TestSuite.Test((-1, 1), DirectionHelper.NeighbourInDirection(Direction.NorthWest, x, y), "TestNeighbourInDirection: NorthWest");
        }
    }
}
