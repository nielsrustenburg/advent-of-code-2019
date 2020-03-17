using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AoC.common;
using AoC.Utils;
using AoC.Tests;
namespace AoC.Day12
{
    class Tester : PuzzleSolverTester
    {
        public Tester(IPuzzleSolver solver, string expectedOne, string expectedTwo) : base(solver, expectedOne, expectedTwo)
        {
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

            Solver.SimulateStep(jmoons);

            if (!(jmoons[0].X == 2 && jmoons[0].Y == -1 && jmoons[0].Z == 1 &&
                  jmoons[1].X == 3 && jmoons[1].Y == -7 && jmoons[1].Z == -4 &&
                  jmoons[2].X == 1 && jmoons[2].Y == -7 && jmoons[2].Z == 5 &&
                  jmoons[3].X == 2 && jmoons[3].Y == 2 && jmoons[3].Z == 0)) throw new Exception("Moons are not updated to correct coordinates");
        }

        public override void RunAll()
        {
            TestSolver();
        }
    }
}
