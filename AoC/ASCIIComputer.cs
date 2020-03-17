using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using AoC.Utils;

namespace AoC
{
    class ASCIIComputer : BigIntCode
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

    class ManualASCIIComputer : ASCIIComputer
    {
        public ManualASCIIComputer(IEnumerable<BigInteger> program) : base(program) { }

        public void RunManual()
        {
            Console.WriteLine(RunString());
            while (true)
            {
                string nextCommand = Console.ReadLine();
                Console.WriteLine(RunString(nextCommand));
            }
        }
    }
}
