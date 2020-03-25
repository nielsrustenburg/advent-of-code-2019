using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AoC.Utils
{
    static class InputParser
    {
        public static IEnumerable<string> Split(string input, char[] separators = null)
        {
            separators = separators ?? new char[] { '\r', '\n' };
            return input.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }

        public static IEnumerable<string> SplitCSV(string input, char separator = ',')
        {
            return input.Split(new char[] { separator });
        }
    }

    static class InputParser<T>
    {
        public static IEnumerable<T> ParseLines(IEnumerable<string> lines, Func<string, T> Parse)
        {
            return lines.Select(l => Parse(l));
        }

        public static IEnumerable<T> SplitAndParse(string input ,Func<string, T> Parse, char[] separators = null)
        {
            var splitInput = InputParser.Split(input, separators);
            return splitInput.Select(element => Parse(element));
        }

        public static IEnumerable<T> ParseCSVLine(string line, Func<string,T> Parse)
        {
            var separators = new char[] { ',' };
            return SplitAndParse(line, Parse, separators);
        }
    }
}
