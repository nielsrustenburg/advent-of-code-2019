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
            string input = InputReader.StringFromLine("d9input.txt");
            List<BigInteger> program = input.Split(',').Select(x => BigInteger.Parse(x)).ToList();
            BigIntCode bic = new BigIntCode(program);
            var output = bic.Run(new List<BigInteger> { (BigInteger)1 });
            return output.Last();
        }

        public static BigInteger SolvePartTwo()
        {
            string input = InputReader.StringFromLine("d9input.txt");
            List<BigInteger> program = input.Split(',').Select(x => BigInteger.Parse(x)).ToList();
            BigIntCode bic = new BigIntCode(program);
            var output = bic.Run(new List<BigInteger> { (BigInteger)2 });
            return output.Last();
        }
    }
}

