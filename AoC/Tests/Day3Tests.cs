using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC.Tests
{
    static class Day3Tests
    {
        public static void RunAll()
        {
            TestSmallerInstances();
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day3Tests Completed!");
        }

        static void TestSmallerInstances()
        {
            Wire w1 = new Wire("R8,U5,L5,D3".Split(','));
            Wire w2 = new Wire("U7,R6,D4,L4".Split(','));

            TestSuite.Test(6, () => Day3.IntersectionDistClosestToOrigin(w1,w2));
            TestSuite.Test(30, () => Day3.ShortestIntersectionDist(w1,w2));

            Wire w3 = new Wire("R75,D30,R83,U83,L12,D49,R71,U7,L72".Split(','));
            Wire w4 = new Wire("U62,R66,U55,R34,D71,R55,D58,R83".Split(','));

            TestSuite.Test(159, () => Day3.IntersectionDistClosestToOrigin(w3, w4));
            TestSuite.Test(610, () => Day3.ShortestIntersectionDist(w3, w4));

            Wire w5 = new Wire("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51".Split(','));
            Wire w6 = new Wire("U98,R91,D20,R16,D67,R40,U7,R15,U6,R7".Split(','));

            TestSuite.Test(135, () => Day3.IntersectionDistClosestToOrigin(w5, w6));
            TestSuite.Test(410, () => Day3.ShortestIntersectionDist(w5, w6));
        }

        static void TestSolvePartOne()
        {
            TestSuite.Test(352, Day3.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            TestSuite.Test(43848, Day3.SolvePartTwo);
        }
    }
}
