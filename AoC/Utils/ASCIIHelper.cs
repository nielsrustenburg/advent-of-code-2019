using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AoC.Utils
{
    static class ASCIIHelper
    {
        public static List<int> StringToASCII(string s)
        {
            return s.Select(c => (int)c).ToList();
        }

        public static List<BigInteger> StringToASCIIBI(string s, bool withNewLine = false)
        {
            List<BigInteger> output = s.Select(c => (BigInteger)c).ToList();
            if (withNewLine)
            {
                output.Add(10);
            }
            return output;
        }

        public static string ASCIIToString(IEnumerable<int> ascii)
        {
            return string.Concat(ascii.Select(x => (char)x));
        }

        public static string ASCIIToString(IEnumerable<BigInteger> ascii)
        {
            return string.Concat(ascii.Select(x => (char)x));
        }
    }
}

