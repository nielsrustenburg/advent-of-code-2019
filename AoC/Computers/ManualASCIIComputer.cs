using System;
using System.Collections.Generic;
using System.Numerics;

namespace AoC.Computers
{
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
