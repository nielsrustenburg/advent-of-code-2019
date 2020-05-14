using System;
using System.Linq;
using System.Collections.Generic;
using AoC.Common;
using System.IO;
using AoC.Day1;
using System.Diagnostics;

namespace AoC
{
    class Program
    {
        enum Mode
        {
            EvaluateRuntime,
            SolveOne,
            SolveAll,
        }

        static void Main(string[] args)
        {
            var mode = Mode.EvaluateRuntime;
            var inputMode = Input.InputMode.File;

            int numEvaluationRuns = 10;
            int dayNumIndividualSolve = 18;

            switch (mode)
            {
                case Mode.EvaluateRuntime:
                    EvaluateRuntime(inputMode, numEvaluationRuns);
                    break;
                case Mode.SolveOne:
                    SolveIndividualDay(inputMode, dayNumIndividualSolve);
                    break;
                case Mode.SolveAll:
                    SolveAllDays(inputMode);
                    break;
                default:
                    throw new Exception("use the Mode enum");
            }
        }

        private static void EvaluateRuntime(Input.InputMode inputMode, int numRuns)
        {
            var runtimes = new long[25, numRuns];
            var stopwatch = new Stopwatch();
            for (int runCount = 0; runCount < numRuns; runCount++)
            {
                for (int dayNum = 1; dayNum <= 25; dayNum++)
                {
                    var solver = GetSolver(inputMode, dayNum, stopwatch);
                    runtimes[dayNum - 1, runCount] = stopwatch.ElapsedMilliseconds;
                }
            }

            var averageRuntimes = new long[25];
            for (int dayNum = 1; dayNum <= 25; dayNum++)
            {
                long totalTime = 0;
                for(int runNum = 0; runNum < numRuns; runNum++)
                {
                    totalTime += runtimes[dayNum-1, runNum];
                }
                averageRuntimes[dayNum-1] = totalTime / numRuns;
            }

            for (int dayCount = 1; dayCount <= 25; dayCount++)
            {
                Console.WriteLine($"Day{dayCount} average runtime: {averageRuntimes[dayCount - 1]}ms");
            }
            Console.WriteLine($"Average total runtime: {runtimes.Cast<long>().Sum() / numRuns}ms");
        }

        private static void SolveIndividualDay(Input.InputMode inputMode, int dayNum)
        {
            var solver = GetSolver(inputMode, dayNum);
            Console.WriteLine($"Day{dayNum}:");
            solver.WriteSolutions();
        }

        private static void SolveAllDays(Input.InputMode inputMode)
        {
            for(int dayNum = 1; dayNum <= 25; dayNum++)
            {
                var solver = GetSolver(inputMode, dayNum);
                Console.WriteLine($"Day{dayNum}:");
                solver.WriteSolutions();
            }
        }

        private static PuzzleSolver GetSolver(Input.InputMode inputMode, int dayNum, Stopwatch stopwatch = null)
        {
            var fileName = GetFileName(inputMode, dayNum);
            bool stopwatchProvided = stopwatch != null;

            var solverType = Type.GetType($"AoC.Day{dayNum}.Solver");
            if (stopwatchProvided) stopwatch.Restart();
            var solver = (PuzzleSolver)Activator.CreateInstance(solverType, new Object[] { inputMode, fileName });
            if (stopwatchProvided) stopwatch.Stop();

            return solver;
        }

        private static string GetFileName(Input.InputMode inputMode, int dayNum)
        {
            switch (inputMode)
            {
                case Input.InputMode.Embedded:
                    return "input.txt";
                case Input.InputMode.File:
                    return $"{dayNum}.txt";
                default:
                    throw new Exception("set InputMode to either Embedded or File");
            }
        }
    }
}
