using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AoC
{
    static class Day21
    {
        public static int SolvePartOne()
        {
            string input = InputReader.StringFromLine("d21input.txt");
            List<BigInteger> program = input.Split(',').Select(x => BigInteger.Parse(x)).ToList();
            SpringDroid sd = new SpringDroid(program);
            sd.Not("A", "T");
            sd.Not("B", "J");
            sd.Or("T", "J");
            sd.Not("C", "T");
            sd.Or("T", "J");
            sd.And("D", "J");
            //if any of ABC has a hole and D is clear, jump to D

            return (int) sd.Walk();
        }

        public static int SolvePartTwo()
        {
            string input = InputReader.StringFromLine("d21input.txt");
            List<BigInteger> program = input.Split(',').Select(x => BigInteger.Parse(x)).ToList();
            SpringDroid sd = new SpringDroid(program);
            sd.Not("A", "T");
            sd.Not("B", "J");
            sd.Or("T", "J");
            sd.Not("C", "T");
            sd.Or("T", "J");
            sd.And("D", "J");
            sd.Not("H", "T");
            sd.And("E", "T");
            sd.Or("H", "T");
            sd.And("T", "J");
            //if any of ABC has a hole and D is clear, jump to D

            return (int)sd.Run();
        }
    }

	class SpringDroid
    {
        BigIntCode brain;
        Dictionary<string, List<BigInteger>> asciiOptions;

        public SpringDroid(List<BigInteger> programming)
        {
            brain = new BigIntCode(programming);
            OutputToConsole(brain.Run());
            List<string> options = new List<string> { "WALK", "AND", "OR", "NOT", "A", "B", "C", "D", "T", "J" };
            asciiOptions = options.ToDictionary(p => p, p => ASCIIHelper.StringToASCIIBI(p));
            asciiOptions.Add("NEWLINE", new List<BigInteger> { 10 });
        }

        public BigInteger Walk()
        {
            List<BigInteger> input = ASCIIHelper.WithNewLine(ASCIIHelper.StringToASCIIBI($"WALK"));
            List<BigInteger> output = brain.Run(input);
            int takeN = output.Count;
            if(output.Last() > 255)
            {
                takeN--;    
            }
            OutputToConsole(output.Take(takeN));
            return output.Last();
        }

        public BigInteger Run()
        {
            List<BigInteger> input = ASCIIHelper.WithNewLine(ASCIIHelper.StringToASCIIBI($"RUN"));
            List<BigInteger> output = brain.Run(input);
            int takeN = output.Count;
            if (output.Last() > 255)
            {
                takeN--;
            }
            OutputToConsole(output.Take(takeN));
            return output.Last();
        }

        public void Not(string aregister, string wregister)
        {
            List<BigInteger> input = ASCIIHelper.WithNewLine(ASCIIHelper.StringToASCIIBI($"NOT {aregister} {wregister}"));
            OutputToConsole(brain.Run(input));
        }

        public void And(string aregister, string wregister)
        {
            List<BigInteger> input = ASCIIHelper.WithNewLine(ASCIIHelper.StringToASCIIBI($"AND {aregister} {wregister}"));
            OutputToConsole(brain.Run(input));
        }

        public void Or(string aregister, string wregister)
        {
            List<BigInteger> input = ASCIIHelper.WithNewLine(ASCIIHelper.StringToASCIIBI($"OR {aregister} {wregister}"));
            OutputToConsole(brain.Run(input));
        }

        public void OutputToConsole(IEnumerable<BigInteger> op)
        {
            Console.Write(string.Concat(op.Select(x => (char)x)));
        }
    }
}

