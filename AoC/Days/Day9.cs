using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;

namespace AoC
{
    static class Day9
    {
        public static BigInteger SolvePartOne()
        {
            List<BigInteger> program = InputReader.BigIntegersFromCSVLine("d9input.txt");
            BigIntCode bic = new BigIntCode(program);
            var output = bic.Run(new List<BigInteger> { (BigInteger)1 });
            return output.Last();
        }

        public static BigInteger SolvePartTwo()
        {
            List<BigInteger> program = InputReader.BigIntegersFromCSVLine("d9input.txt");
            BigIntCode bic = new BigIntCode(program);
            var output = bic.Run(new List<BigInteger> { (BigInteger)2 });
            return output.Last();
        }
    }
}

