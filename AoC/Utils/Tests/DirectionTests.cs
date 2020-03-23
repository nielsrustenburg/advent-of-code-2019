using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AoC.Utils.Tests
{
    class DirectionTests
    {
        [TestCase(Direction.North, Direction.NorthEast, Direction.East)]
        [TestCase(Direction.East, Direction.SouthEast, Direction.South)]
        [TestCase(Direction.South, Direction.SouthWest, Direction.West)]
        [TestCase(Direction.West, Direction.NorthWest, Direction.North)]
        [TestCase(Direction.NorthEast, Direction.East, Direction.SouthEast)]
        [TestCase(Direction.SouthEast, Direction.South, Direction.SouthWest)]
        [TestCase(Direction.SouthWest, Direction.West, Direction.NorthWest)]
        [TestCase(Direction.NorthWest, Direction.North, Direction.NorthEast)]
        public void TestTurnClockwise(Direction initial, Direction expEighth, Direction expQuarter)
        {
            var eighth = initial.ClockWiseByEighth();
            Assert.AreEqual(expEighth, eighth);

            var quarter = initial.ClockWiseByQuarter();
            Assert.AreEqual(expQuarter, quarter);
        }

        [TestCase(Direction.East, Direction.NorthEast, Direction.North)]
        [TestCase(Direction.South, Direction.SouthEast, Direction.East)]
        [TestCase(Direction.West, Direction.SouthWest, Direction.South)]
        [TestCase(Direction.North, Direction.NorthWest, Direction.West)]
        [TestCase(Direction.SouthEast, Direction.East, Direction.NorthEast)]
        [TestCase(Direction.SouthWest, Direction.South, Direction.SouthEast)]
        [TestCase(Direction.NorthWest, Direction.West, Direction.SouthWest)]
        [TestCase(Direction.NorthEast, Direction.North, Direction.NorthWest)]
        public void TestTurnCounterClockwise(Direction initial, Direction expEighth, Direction expQuarter)
        {
            var eighth = initial.CounterClockWiseByEighth();
            Assert.AreEqual(expEighth, eighth);

            var quarter = initial.CounterClockWiseByQuarter();
            Assert.AreEqual(expQuarter, quarter);
        }

        [Test]
        public void TestNeighbourInDirection()
        {
            var (x, y) = (0, 0);
            var north = DirectionHelper.NeighbourInDirection(Direction.North, x, y);
            var northEast = DirectionHelper.NeighbourInDirection(Direction.NorthEast, x, y);
            var east = DirectionHelper.NeighbourInDirection(Direction.East, x, y);
            var southEast = DirectionHelper.NeighbourInDirection(Direction.SouthEast, x, y);
            var south = DirectionHelper.NeighbourInDirection(Direction.South, x, y);
            var southWest = DirectionHelper.NeighbourInDirection(Direction.SouthWest, x, y);
            var west = DirectionHelper.NeighbourInDirection(Direction.West, x, y);
            var northWest = DirectionHelper.NeighbourInDirection(Direction.NorthWest, x, y);

            Assert.AreEqual((x, y + 1), north);
            Assert.AreEqual((x + 1, y + 1), northEast);
            Assert.AreEqual((x + 1, y), east);
            Assert.AreEqual((x + 1, y - 1), southEast);
            Assert.AreEqual((x, y - 1), south);
            Assert.AreEqual((x - 1, y - 1), southWest);
            Assert.AreEqual((x - 1, y), west);
            Assert.AreEqual((x - 1, y + 1), northWest);
        }
    }
}
