using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AoC
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
                output = WithNewLine(output);
            }
            return output;
        }

        public static List<BigInteger> WithNewLine(List<BigInteger> l)
        {
            List<BigInteger> ln = new List<BigInteger>(l);
            ln.Add(10);
            return ln;
        }
    }
}

