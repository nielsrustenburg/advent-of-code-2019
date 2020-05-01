using System;
using AoC.Common;

namespace AoC
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int dayCounter = 1; dayCounter <= 25; dayCounter++)
            {
                var solverType = Type.GetType($"AoC.Day{dayCounter}.Solver");
                var solver = (PuzzleSolver)Activator.CreateInstance(solverType);
                //Console.WriteLine($"Day{dayCounter}: \n{solver.SolutionOne()}\n{solver.SolutionTwo()}\n");
            }
            Console.WriteLine("Finished! Press any key to close");
            //Console.ReadKey();
        }
    }

    class Dog
    {

    }
}
