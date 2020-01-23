using System;
using AoC.Tests;

namespace AoC
{
    class Program
    {
        static void Main(string[] args)
        {
            TestSuite.RunTests();

            //Console.WriteLine(...SolvePartOne());
            //Console.WriteLine(...SolvePartTwo());

            Console.WriteLine("Finished! Press any key to close");
            Console.ReadKey();
        }
    }
}
