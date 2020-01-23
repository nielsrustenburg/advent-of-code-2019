using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;

namespace AoC.Tests
{
    static class Day9Tests
    {
        public static void RunAll()
        {
            TestQuine();
            TestBigIntCode();
            TestOldIntCode();
            TestOld5through8();
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day9Tests Completed!");
        }

        public static void TestSolvePartOne()
        {
            void Test(BigInteger expectedOutput)
            {
                BigInteger output = Day9.SolvePartOne();
                if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
            }
            Test(2453265701);
        }

        public static void TestSolvePartTwo()
        {
            void Test(BigInteger expectedOutput)
            {
                BigInteger output = Day9.SolvePartTwo();
                if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
            }
            Test(80805);
        }

        public static void TestQuine()
        {
            List<int> program = new List<int> { 109, 1, 204, -1, 1001, 100, 1, 100, 1008, 100, 16, 101, 1006, 101, 0, 99 };
            IntCode quine = new IntCode(program);
            var output = quine.Run();
            if (!output.SequenceEqual(program)) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {program}");
        }

        public static void TestBigIntCode()
        {
            List<BigInteger> program = new List<int> { 1102, 34915192, 34915192, 7, 4, 7, 99, 0 }.Select(x => (BigInteger)x).ToList();
            BigIntCode quine = new BigIntCode(program);
            var output = quine.Run();
            if (output.Last().ToString().Length != 16) throw new Exception($"{TestSuite.GetCurrentMethod()}(): String Length was {output.Last().ToString().Length} , expected {16}");


            List<BigInteger> program2 = new List<string> { "104", "1125899906842624", "99" }.Select(x => BigInteger.Parse(x)).ToList();
            BigIntCode bob = new BigIntCode(program2);
            var output2 = bob.Run();
            if (output2.Last().ToString() == "") throw new Exception($"{TestSuite.GetCurrentMethod()}(): String was {output2[0].ToString()} , expected {1125899906842624}");
        }

        public static void TestOldIntCode()
        {
            void TestOutcome(BigIntCode ic, BigInteger expectedOutput)
            {
                string input = ic.ToString();
                ic.Run();
                BigInteger output = ic.GetValAtMemIndex(0);
                if (output != expectedOutput) throw new Exception($"{input}: {output}, expected {expectedOutput}");
            }
            void Test(BigIntCode ic, List<BigInteger> expectedOutput)
            {
                string input = ic.ToString();
                ic.Run();
                string output = ic.ToString();
                string expOut = new BigIntCode(expectedOutput).ToString();
                if (output != expOut) throw new Exception($"{input}: {output}, expected {expOut}");
            }
            BigIntCode ic1 = new BigIntCode(new List<int> { 1, 9, 10, 3, 2, 3, 11, 0, 99, 30, 40, 50 }.Select(x => (BigInteger)x).ToList()); //Add->Multiply
            BigIntCode ic2 = new BigIntCode(new List<int> { 1, 0, 0, 0, 99 }.Select(x => (BigInteger)x).ToList()); //Add
            BigIntCode ic3 = new BigIntCode(new List<int> { 2, 3, 0, 3, 99 }.Select(x => (BigInteger)x).ToList()); //Multiply
            BigIntCode ic4 = new BigIntCode(new List<int> { 2, 4, 4, 5, 99, 0 }.Select(x => (BigInteger)x).ToList()); //Multiply
            BigIntCode ic5 = new BigIntCode(new List<int> { 1, 1, 1, 4, 99, 5, 6, 0, 99 }.Select(x => (BigInteger)x).ToList()); //Add->Multiply
            TestOutcome(ic1, 3500);
            Test(ic2, new List<int> { 2, 0, 0, 0, 99 }.Select(x => (BigInteger)x).ToList());
            Test(ic3, new List<int> { 2, 3, 0, 6, 99 }.Select(x => (BigInteger)x).ToList());
            Test(ic4, new List<int> { 2, 4, 4, 5, 99, 9801 }.Select(x => (BigInteger)x).ToList());
            Test(ic5, new List<int> { 30, 1, 1, 4, 2, 5, 6, 0, 99 }.Select(x => (BigInteger)x).ToList());
        }

        public static void TestOld5through8()
        {

            List<int> program = new List<int> { 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,
                                                1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,
                                                999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99};


            //List<int> program = new List<int> { 3, 3, 1108, -1, 8, 3, 4, 3, 99 };
            BigIntCode ic = new BigIntCode(program.Select(x => (BigInteger)x).ToList());
            ic.Run(new List<BigInteger> { (BigInteger) 8 });

        }
    }
}


