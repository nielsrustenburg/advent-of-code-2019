using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AoC.Common;
using AoC.Utils;

namespace AoC.Day3
{
    class Solver : PuzzleSolver
    {
        Wire[] wires;
        List<Wire.Intersection> intersections;

        public Solver() : this(Input.InputMode.Embedded, "input")
        {
        }

        public Solver(Input.InputMode mode, string input) : base(mode, input)
        {
        }

        protected override void ParseInput(string input)
        {
            var lines = InputParser.Split(input);
            var wirePaths = lines.Select(line => InputParser<(Direction, int)>.ParseCSVLine(line, ParseWireStep).ToList());
            wires = wirePaths.Select(wp => new Wire(wp)).ToArray();

            (Direction dir, int dist) ParseWireStep(string step)
            {
                Direction dir;
                switch (step[0])
                {
                    case 'U': dir = Direction.North; break;
                    case 'D': dir = Direction.South; break;
                    case 'L': dir = Direction.West; break;
                    case 'R': dir = Direction.East; break;
                    default: throw new ArgumentException("expecting first character to be U,D,L or R");
                }
                int dist = int.Parse(step.Substring(1));
                return (dir, dist);
            }
        }

        protected override void PrepareSolution()
        {
            intersections = wires[0].FindIntersectionsWith(wires[1]);
        }

        protected override void SolvePartOne()
        {
            var distanceOfClosestIntersection = intersections.Select(intersection => MathHelper.ManhattanDistance((0, 0), (intersection.x, intersection.y)))
                                                             .Aggregate(int.MaxValue, (a, b) => a <= b ? a : b);

            resultPartOne = distanceOfClosestIntersection.ToString();
        }

        protected override void SolvePartTwo()
        {
            var intersectionCombinedWireLengths = intersections.Select(intersection => intersection.combinedWireLength)
                                                               .Aggregate(int.MaxValue, (a, b) => a <= b ? a : b);

            resultPartTwo = intersectionCombinedWireLengths.ToString(); 
        }
    }
    public class Wire
    {
        List<WirePiece> path;
        public Wire(IEnumerable<(Direction, int)> inputPath)
        {
            path = new List<WirePiece>();
            int x = 0;
            int y = 0;
            int wireLength = 0;
            foreach ((Direction dir, int length) in inputPath)
            {
                path.Add(new WirePiece(x, y, wireLength, dir, length));

                wireLength += length;
                (x, y) = DirectionHelper.StepInDirection(dir, x, y, length);
            }
        }

        public List<Intersection> FindIntersectionsWith(Wire other)
        {
            var intersections = new List<Intersection>();
            foreach (var wirePiece in path)
            {
                foreach (var otherWirePiece in other.path)
                {
                    var intersection = wirePiece.FindIntersection(otherWirePiece);
                    if (intersection != null)
                    {
                        intersections.Add(intersection);
                    }
                }
            }
            return intersections;
        }

        struct WirePiece
        {
            public readonly int x;
            public readonly int y;
            public readonly int toX;
            public readonly int toY;
            public readonly int distanceFromOrigin;
            public readonly Direction direction;

            public WirePiece(int x, int y, int distFromOrigin, Direction direction, int length)
            {
                this.x = x;
                this.y = y;
                distanceFromOrigin = distFromOrigin;
                this.direction = direction;
                (toX, toY) = DirectionHelper.StepInDirection(direction, x, y, length);
            }

            public Intersection FindIntersection(WirePiece other)
            {
                if (direction == other.direction || direction == other.direction.Opposite()) return null;

                // I assume you only use N/E/S/W for wirepieces.. perhaps I need to make a separate enum?
                WirePiece horizontal, vertical;
                if (direction == Direction.North || direction == Direction.South)
                {
                    horizontal = other;
                    vertical = this;
                }
                else
                {
                    horizontal = this;
                    vertical = other;
                }

                int left, right;
                if (horizontal.direction == Direction.East)
                {
                    left = horizontal.x;
                    right = horizontal.toX;
                }
                else
                {
                    left = horizontal.toX;
                    right = horizontal.x;
                }

                int top, bottom;
                if (vertical.direction == Direction.North)
                {
                    bottom = vertical.y;
                    top = vertical.toY;
                }
                else
                {
                    bottom = vertical.toY;
                    top = vertical.y;
                }

                var horizontalMatch = left <= vertical.x && right >= vertical.x;
                var verticalMatch = bottom <= horizontal.y && top >= horizontal.y;

                if (horizontalMatch && verticalMatch && !(vertical.x == 0 && horizontal.y == 0))
                {
                    var combinedWireLength = horizontal.distanceFromOrigin + vertical.distanceFromOrigin + Math.Abs(horizontal.x - vertical.x) + Math.Abs(vertical.y - horizontal.y);
                    return new Intersection(vertical.x, horizontal.y, combinedWireLength);
                }
                else
                {
                    return null;
                }
            }
        }

        public class Intersection
        {
            public readonly int x;
            public readonly int y;
            public readonly int combinedWireLength;

            public Intersection(int x, int y, int combinedLength)
            {
                this.x = x;
                this.y = y;
                combinedWireLength = combinedLength;
            }
        }
    }
}
