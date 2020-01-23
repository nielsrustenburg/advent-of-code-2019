using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;

namespace AoC.Tests
{
    static class Day12Tests
    {
        public static void RunAll()
        {
            TestSimulateStep();
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day12Tests Completed!");
        }

        public static void TestSolvePartOne()
        {
            void Test(int expectedOutput)
            {
                int output = Day12.SolvePartOne();
                if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
            }
            Test(10635);
        }

        public static void TestSolvePartTwo()
        {
            void Test(BigInteger expectedOutput)
            {
                BigInteger output = Day12.SolvePartTwo();
                if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
            }
            Test(BigInteger.Parse("583523031727256"));
        }

        public static void TestSimulateStep()
        {
            List<JupiterMoon> jmoons = new List<JupiterMoon>
            {
                new JupiterMoon(-1,0,2),
                new JupiterMoon(2,-10,-7),
                new JupiterMoon(4,-8,8),
                new JupiterMoon(3,5,-1)
            };
            Day12.SimulateStep(jmoons);

            if (!(jmoons[0].X == 2 && jmoons[0].Y == -1 && jmoons[0].Z == 1 &&
                  jmoons[1].X == 3 && jmoons[1].Y == -7 && jmoons[1].Z == -4 &&
                  jmoons[2].X == 1 && jmoons[2].Y == -7 && jmoons[2].Z == 5 &&
                  jmoons[3].X == 2 && jmoons[3].Y == 2 && jmoons[3].Z == 0)) throw new Exception("Moons are not updated to correct coordinates");
        }
    }
}
