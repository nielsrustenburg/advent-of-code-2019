using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AoC.Utils
{
    static class InputParser
    {
        public static IEnumerable<int> ParseInts(IEnumerable<string> lines)
        {
            return InputParser<int>.ParseLines(lines, int.Parse);
        }

        public static IEnumerable<BigInteger> ParseBigIntegers(IEnumerable<string> lines)
        {
            return InputParser<BigInteger>.ParseLines(lines, BigInteger.Parse);
        }

        public static IEnumerable<IEnumerable<string>> ParseCSVStrings(IEnumerable<string> lines)
        {
            return InputParser<IEnumerable<string>>.ParseLines(lines, ParseCSVLine);
        }

        internal static IEnumerable<IEnumerable<int>> ParseCSVInts(IEnumerable<string> input)
        {
            return ParseCSVStrings(input).Select(l => ParseInts(l));
        }

        internal static IEnumerable<IEnumerable<BigInteger>> ParseCSVBigIntegers(IEnumerable<string> input)
        {
            return ParseCSVStrings(input).Select(l => ParseBigIntegers(l));
        }


        public static IEnumerable<string> ParseCSVLine(string line)
        {
            return line.Split(',');
        }
    }

    static class InputParser<T>
    {
        public static IEnumerable<T> ParseLines(IEnumerable<string> lines, Func<string, T> parse)
        {
            return lines.Select(l => parse(l));
        }
    }
}
