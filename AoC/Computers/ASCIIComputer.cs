using System.Collections.Generic;
using System.Numerics;
using System.Text;
using AoC.Utils;

namespace AoC.Computers
{
    class ASCIIComputer : IntCode
    {
        public ASCIIComputer(IEnumerable<BigInteger> vals) : base(vals)
        {
        }

        public List<BigInteger> Run(string input, bool newLineAtEnd = true)
        {
            return Run(ASCIIHelper.StringToASCIIBI(input, newLineAtEnd));
        }

        public string RunString()
        {
            return ASCIIHelper.ASCIIToString(Run());
        }

        public string RunString(string input)
        {
            return ASCIIHelper.ASCIIToString(Run(input));
        }
    }
}
