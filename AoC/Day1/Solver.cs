using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AoC.Common;
using AoC.Utils;
using System.Diagnostics;

namespace AoC.Day1
{
    class Solver : PuzzleSolver
    {
        List<int> masses;
        List<int> fuelPerModule;

        public Solver() : this(Input.InputMode.Embedded, "input")
        {
        }

        public Solver(Input.InputMode mode, string input) : base(mode, input)
        {
        }

        protected override void ParseInput(string input)
        {
            masses = InputParser<int>.SplitAndParse(input, int.Parse).ToList();
        }

        protected override void PrepareSolution()
        {
            fuelPerModule = masses.Select(mass => FuelForModule(mass)).ToList();
        }

        protected override void SolvePartOne()
        {
            resultPartOne = fuelPerModule.Sum().ToString();
        }

        protected override void SolvePartTwo()
        {
            var totalFuelRequired = fuelPerModule.Select(fuel => RecursiveFuelForModule(fuel, fuel)).Sum();
            resultPartTwo = totalFuelRequired.ToString();
        }

        public static int FuelForModule(int mass)
        {
            if (mass < 6) return 0;

            return (mass / 3) - 2; 
        }

        public static int RecursiveFuelForModule(int mass, int fuelCount)
        {
            if (mass < 1) return fuelCount;

            var fuelMass = FuelForModule(mass);

            return RecursiveFuelForModule(fuelMass, fuelCount + fuelMass);
        }
    }
}
