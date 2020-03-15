using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AoC.Utils;

namespace AoC
{
    public static class Day5
    {
        public static int SolvePartOne()
        {
            List<int> program = InputReader.IntsFromCSVLine("d5input.txt");
            IntCode ic = new IntCode(program);
            List<int> output = ic.Run(new List<int> { 1 });
            return output.Last();
        }

        public static int SolvePartTwo()
        {
            List<int> program = InputReader.IntsFromCSVLine("d5input.txt");
            IntCode ic = new IntCode(program);
            List<int> output = ic.Run(new List<int> { 5 });
            return output.Last();
        }
    }
}
