using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AoC.Utils;
using System.Diagnostics;

namespace AoC
{
    static class Day1
    {
        public static int FuelForModule(int mass)
        {
            return (mass / 3) - 2; //ints get rounded down by default
        }

        public static int SolvePartOne()
        {
            List<int> masses = InputReader.IntsFromTxt("d1input.txt");
            int fuel_required = masses.Select(mass => FuelForModule(mass)).Sum();
            return fuel_required;
        }

        public static int TotalFuelForModule(int module_fuel)
        {
            int total_fuel = 0;
            int add_fuel = module_fuel;
            do
            {
                total_fuel += add_fuel;
                add_fuel = FuelForModule(add_fuel);
            } while (add_fuel > 0);
            return total_fuel;
        }

        public static int SolvePartTwo()
        {
            List<int> masses = InputReader.IntsFromTxt("d1input.txt");
            List<int> fuel_per_module = masses.Select(mass => FuelForModule(mass)).ToList();
            Debug.Assert(fuel_per_module.Sum() == 3362507);
            int total_fuel_req = fuel_per_module.Select(fuel => TotalFuelForModule(fuel)).Sum();
            return total_fuel_req;
        }
    }
}
