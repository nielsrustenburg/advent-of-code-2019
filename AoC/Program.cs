using System;
using AoC.Common;

namespace AoC
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(...SolvePartOne());
            //Console.WriteLine(...SolvePartTwo());

            var d25 = new Day25.Solver();

            for(int i = 1; i <= 25; i++)
            {
                var solverType = Type.GetType($"AoC.Day{i}.Solver");
                var solver = (PuzzleSolver) Activator.CreateInstance(solverType);
                Console.WriteLine($"Day{i}: \n{solver.SolutionOne()}\n{solver.SolutionTwo()}\n");
            }
            Console.WriteLine("Finished! Press any key to close");
            Console.ReadKey();
        }
    }

    class Dog
    {

    }
}
