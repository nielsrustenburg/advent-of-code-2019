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

        public Solver() : base(1)
        {
        }

        public Solver(IEnumerable<string> input) : base(input, 1)
        {
        }

        protected override void ParseInput(IEnumerable<string> input)
        {
            masses = InputParser.ParseInts(input).ToList();
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
            int totalFuelRequired = fuelPerModule.Select(fuel => TotalFuelForModule(fuel)).Sum();
            resultPartTwo = totalFuelRequired.ToString();
        }

        public static int FuelForModule(int mass)
        {
            return (mass / 3) - 2; 
        }

        public static int TotalFuelForModule(int moduleFuel)
        {
            int totalFuel = 0;
            int addFuel = moduleFuel;
            do
            {
                totalFuel += addFuel;
                addFuel = FuelForModule(addFuel);
            } while (addFuel > 0);
            return totalFuel;
        }


    }
}
