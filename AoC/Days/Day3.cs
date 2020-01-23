using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC
{
    static class Day3
    {
        public static int SolvePartOne()
        {
            var wire_dirs = InputReader.MultipleStringListsFromCSV("d3input.txt");
            //Read input
            Wire wire1 = new Wire(wire_dirs[0]);
            Wire wire2 = new Wire(wire_dirs[1]);

            List<(int x,int y)> intersections = wire1.Intersections(wire2);
            var valid_intersections = intersections.Where(interx => !(interx.x == 0 && interx.y == 0));

            return valid_intersections.Select(interx => ManhattanFromOrigin(interx)).Min();
        }

        public static int SolvePartTwo()
        {
            var wire_dirs = InputReader.MultipleStringListsFromCSV("d3input.txt");
            //Read input
            Wire wire1 = new Wire(wire_dirs[0]);
            Wire wire2 = new Wire(wire_dirs[1]);

            return ShortestIntersectionDist(wire1, wire2);
        }

        public static int ManhattanFromOrigin((int x,int y) point)
        {
            return Math.Abs(point.x) + Math.Abs(point.y);
        }

        public static int ShortestIntersectionDist(Wire wire_a, Wire wire_b)
        { 
            List<(GridLine,int)> a_lines = new List<(GridLine,int)>();
            List<(GridLine,int)> b_lines = new List<(GridLine,int)>();

            int a_dist, b_dist;
            a_dist = b_dist = 0;
            int shortest_found = int.MaxValue;

            IEnumerator<GridLine> enum_a = wire_a.GetLineEnumerator();
            IEnumerator<GridLine> enum_b = wire_b.GetLineEnumerator();
            bool a_unfinished = enum_a.MoveNext();
            bool b_unfinished = enum_b.MoveNext();

            GridLine current_line;

            //Check one: did we run out of wire? Check two: is the closest summed distance shorter than the distance from either wire to new potential intersections?
            while ((a_unfinished && b_unfinished) && !(shortest_found <= a_dist && shortest_found <= b_dist))
            {
                if(a_dist <= b_dist)
                {
                    current_line = enum_a.Current;
                    a_lines.Add((current_line, a_dist));
                    (bool found, int dist) = CheckForShorterIntersection(a_lines.Last(), b_lines, shortest_found);
                    if (found) shortest_found = dist;
                    a_dist += current_line.length;

                    a_unfinished = enum_a.MoveNext();
                } else
                {
                    current_line = enum_b.Current;
                    b_lines.Add((current_line, b_dist));
                    (bool found, int dist) = CheckForShorterIntersection(b_lines.Last(), a_lines, shortest_found);
                    if (found) shortest_found = dist;
                    b_dist += current_line.length;

                    b_unfinished = enum_b.MoveNext();
                }
            }
            return shortest_found;
        }

        public static (bool, int) CheckForShorterIntersection((GridLine gline, int dist) line, List<(GridLine, int)> other_lines, int max_dist)
        {
            foreach((GridLine gline, int dist) other in other_lines)
            {
                if (line.dist + other.dist > max_dist) return (false, int.MaxValue);
                if (line.gline.Intersects(other.gline))
                {
                    (int x, int y) x_point = line.gline.GetIntersection(other.gline);
                    int total_dist = line.dist + other.dist + line.gline.PointFromDist(x_point.x, x_point.y) + other.gline.PointFromDist(x_point.x, x_point.y);
                    if(total_dist > max_dist)
                    {
                        return (false, int.MaxValue);
                    } else
                    {
                        return (true, total_dist);
                    }
                }
            }
            return (false, int.MaxValue);
        }
    }

    class GridLine
    {
        public (int x, int y) from;
        public (int x, int y) to;
        char direction;
        public int length;

        public GridLine((int,int) start, char direction, int distance)
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

            GridLine hline, vline;
            if (IsHorizontal())
            {
                hline = this;
                vline = other;
            } else
            {
                hline = other;
                vline = this;
            }

            int left, right;
            if(hline.direction == 'R')
            {
                left = hline.from.x;
                right = hline.to.x;
            } else
            {
                left = hline.to.x;
                right = hline.from.x;
            }

            int down,up;
            if(vline.direction == 'U')
            {
                down = vline.from.y;
                up = vline.to.y;
            } else
            {
                down = vline.to.y;
                up = vline.from.y;
            }

            bool hmatch = left <= vline.from.x && vline.from.x <= right;
            bool vmatch = down <= hline.from.y && hline.from.y <= up;

            return hmatch && vmatch;
        }

        public (int x,int y) GetIntersection(GridLine other)
        {
            //!!!!!THIS ASSUMES THESE TWO HAVE BEEN CHECKED FOR INTERSECTION EARLIER!!!!
            if(from.x == to.x)
            {
                return (from.x, other.from.y);
            } else
            {
                return (other.from.x, from.y);
            }
        } 

        public int PointFromDist(int x, int y)
        {
            //Assume this point is on either same x or y
            return Math.Abs(from.x - x) + Math.Abs(from.y - y);
        }
    }

    class Wire
    {
        List<GridLine> lines = new List<GridLine>();

        public Wire(List<string> path)
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

        public List<(int x,int y)> Intersections(Wire other)
        {
            List<(int x, int y)> intersections = new List<(int x, int y)>();
            foreach (GridLine line in lines)
            {
                foreach(GridLine other_line in other.lines)
                {
                    if (line.Intersects(other_line)) intersections.Add(line.GetIntersection(other_line));
                }
            }
            return intersections;
        }

        internal IEnumerator<GridLine> GetLineEnumerator()
        {
            return lines.GetEnumerator();
        }
    }
}
