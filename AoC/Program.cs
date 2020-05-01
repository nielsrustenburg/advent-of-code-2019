using System;
using System.Linq;
using System.Collections.Generic;
using AoC.Common;

namespace AoC
{
    class Program
    {
        static void Main(string[] args)
        {
            int numRuns = 100;
            long[,] runtimes = new long[25, 100];
            var stopwatch = new System.Diagnostics.Stopwatch();
            for (int runCount = 0; runCount < numRuns; runCount++)
            {
                for (int dayCount = 1; dayCount <= 25; dayCount++)
                {
                    stopwatch.Restart();
                    var solverType = Type.GetType($"AoC.Day{dayCount}.Solver");
                    var solver = (PuzzleSolver)Activator.CreateInstance(solverType);
                    stopwatch.Stop();
                    runtimes[dayCount-1, runCount] = stopwatch.ElapsedMilliseconds;
                    //Console.WriteLine($"Day{dayCounter}: \n{solver.SolutionOne()}\n{solver.SolutionTwo()}\n");
                }
            }
            var averageRuntimes = Enumerable.Range(0, 25).Select(dayNum => Enumerable.Range(0, numRuns).Aggregate((long)0, (a, runNum) => a + runtimes[dayNum, runNum])/numRuns).ToList();
            for(int dayCount = 1; dayCount <= 25; dayCount++)
            {
                Console.WriteLine($"Day{dayCount} average runtime: {averageRuntimes[dayCount-1]}ms");
            }
            Console.WriteLine($"Average total runtime: {runtimes.Cast<long>().Sum()/numRuns}ms");
        }
    }
}
