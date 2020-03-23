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
        List<IEnumerable<string>> wireDirs;
        Wire wire1;
        Wire wire2;

        public Solver() : base(3)
        {
        }

        public Solver(IEnumerable<string> input) : base(input, 3)
        {
        }

        protected override void ParseInput(IEnumerable<string> input)
        {
            wireDirs = InputParser.ParseCSVStrings(input).ToList();
        }

        protected override void PrepareSolution()
        {
            wire1 = new Wire(wireDirs[0]);
            wire2 = new Wire(wireDirs[1]);
        }

        protected override void SolvePartOne()
        {
            resultPartOne = IntersectionDistClosestToOrigin(wire1, wire2).ToString();
        }

        protected override void SolvePartTwo()
        {
            resultPartTwo = ShortestIntersectionDist(wire1, wire2).ToString();
        }

        public static int IntersectionDistClosestToOrigin(Wire w1, Wire w2)
        {
            List<(int x, int y)> intersections = w1.Intersections(w2);

            //Filter out the intersection at the origin
            var valid_intersections = intersections.Where(interx => !(interx.x == 0 && interx.y == 0));
            return valid_intersections.Select(interx => MathHelper.ManhattanDistance((0, 0), interx)).Min();
        }

        public static int ShortestIntersectionDist(Wire wire_a, Wire wire_b)
        {
            UnrollableWire uwireA = new UnrollableWire(wire_a);
            UnrollableWire uwireB = new UnrollableWire(wire_b);
            int shortestFound = int.MaxValue;

            while (uwireA.HasPotential(shortestFound) || uwireB.HasPotential(shortestFound))
            {
                ((GridLine gl, int distFromOG) segment, List<(GridLine, int)> other) = UnrollShortestWire(uwireA, uwireB);
                (bool found, int dist) = CheckForShorterIntersection(segment, other, shortestFound);
                if (found) shortestFound = dist;
            }
            return shortestFound;
        }

        public static ((GridLine gl, int distFromOG) segment, List<(GridLine, int)> other) UnrollShortestWire(UnrollableWire w1, UnrollableWire w2)
        {
            if (w1.fullyUnrolled && w2.fullyUnrolled) throw new Exception("WHY ARE YOU UNROLLING TWO FULLY UNROLLED WIRES????");
            if (w2.fullyUnrolled || (w1.unrolledLength <= w2.unrolledLength && !w1.fullyUnrolled))
            {
                return (w1.UnrollNext(), w2.unrolledSegmentsAndDists);
            }
            else
            {
                return (w2.UnrollNext(), w1.unrolledSegmentsAndDists);
            }
        }

        public static (bool, int) CheckForShorterIntersection((GridLine gline, int dist) line, List<(GridLine, int)> other_lines, int max_dist)
        {
            foreach ((GridLine gline, int dist) other in other_lines)
            {
                if (line.dist + other.dist > max_dist) return (false, int.MaxValue);
                var intersection = line.gline.GetIntersection(other.gline);
                if (intersection.intersects)
                {
                    int total_dist = line.dist + other.dist +
                                     MathHelper.ManhattanDistance(line.gline.from, intersection.coords) +
                                     MathHelper.ManhattanDistance(other.gline.from, intersection.coords);

                    if (total_dist > max_dist || total_dist == 0) //Wires intersecting at the origin are not interesting to us
                    {
                        return (false, int.MaxValue);
                    }
                    else
                    {
                        return (true, total_dist);
                    }
                }
            }
            return (false, int.MaxValue);
        }
    }

    class UnrollableWire
    {
        public Wire wire;
        public bool fullyUnrolled;
        public int unrolledUntilId;
        public int unrolledLength;
        public List<(GridLine gl, int glDistFromOrigin)> unrolledSegmentsAndDists;

        public UnrollableWire(Wire w)
        {
            wire = w;
            unrolledUntilId = 0;
            unrolledLength = 0;
            unrolledSegmentsAndDists = new List<(GridLine gl, int glDistFromOrigin)>();
        }

        public (GridLine gl, int glDistFromOrigin) UnrollNext()
        {
            unrolledSegmentsAndDists.Add((wire.GetWireSegmentAt(unrolledUntilId), unrolledLength));
            unrolledUntilId++;
            unrolledLength += unrolledSegmentsAndDists.Last().gl.length;
            UpdateFullyUnrolledStatus();
            return unrolledSegmentsAndDists.Last();
        }

        public void UpdateFullyUnrolledStatus()
        {
            fullyUnrolled = wire.Count <= unrolledUntilId;
        }

        public bool HasPotential(int shortestFound)
        {
            return (!fullyUnrolled && unrolledLength < shortestFound);
        }
    }

    class Wire
    {
        List<GridLine> lines = new List<GridLine>();
        public int Count { get { return lines.Count; } }

        public Wire(IEnumerable<string> path)
        {
            int x = 0;
            int y = 0;
            foreach (string instruction in path)
            {
                char direction = instruction[0];
                int distance = Int32.Parse(instruction.Substring(1));
                GridLine new_line = new GridLine((x, y), direction, distance);
                lines.Add(new_line);
                x = new_line.to.x;
                y = new_line.to.y;
            }
        }

        public GridLine GetWireSegmentAt(int id)
        {
            return lines[id];
        }

        public List<(int x, int y)> Intersections(Wire other)
        {
            List<(int x, int y)> intersections = new List<(int x, int y)>();
            foreach (GridLine line in lines)
            {
                foreach (GridLine other_line in other.lines)
                {
                    var intersection = line.GetIntersection(other_line);
                    if (intersection.intersects) intersections.Add(intersection.coords);
                }
            }
            return intersections;
        }

        internal IEnumerator<GridLine> GetLineEnumerator()
        {
            return lines.GetEnumerator();
        }
    }

    class GridLine
    {
        public (int x, int y) from;
        public (int x, int y) to;
        char direction;
        public int length;

        public GridLine((int, int) start, char direction, int distance)
        {
            from = start;
            this.direction = direction;
            length = distance;

            if (direction == 'R') to = (from.x + distance, from.y);
            if (direction == 'L') to = (from.x - distance, from.y);
            if (direction == 'U') to = (from.x, from.y + distance);
            if (direction == 'D') to = (from.x, from.y - distance);
        }

        public bool IsHorizontal()
        {
            return direction == 'L' || direction == 'R';
        }

        public bool Intersects(GridLine other)
        {
            if (IsHorizontal() == other.IsHorizontal()) return false;

            GridLine hline = IsHorizontal() ? this : other;
            GridLine vline = IsHorizontal() ? other : this;

            int left = hline.direction == 'R' ? hline.from.x : hline.to.x;
            int right = hline.direction == 'R' ? hline.to.x : hline.from.x;

            int down = vline.direction == 'U' ? vline.from.y : vline.to.y;
            int up = vline.direction == 'U' ? vline.to.y : vline.from.y;

            bool hmatch = left <= vline.from.x && vline.from.x <= right;
            bool vmatch = down <= hline.from.y && hline.from.y <= up;

            return hmatch && vmatch;
        }

        public (bool intersects, (int x, int y) coords) GetIntersection(GridLine other)
        {
            if (this.Intersects(other))
            {
                if (from.x == to.x)
                {
                    return (true, (from.x, other.from.y));
                }
                else
                {
                    return (true, (other.from.x, from.y));
                }
            }
            return (false, (int.MaxValue, int.MaxValue));
        }
    }
}
