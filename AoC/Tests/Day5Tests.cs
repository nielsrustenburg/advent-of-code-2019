using System;
using System.Collections.Generic;
using System.Text;

namespace AoC.Tests
{
    static class Day5Tests
    {

        public static void RunAll()
        {
            TestSolvePartOne();
            TestSolvePartTwo();
            TestOpcodes5Through8();
            Console.WriteLine("Day5Tests Completed!");
        }
        public static void TestOpcodes5Through8()
        {
            for (int i = 0; i < 16; i++)
            {
                int expOut = 1000;
                if (i > 8) expOut++;
                if (i < 8) expOut--;
                TestSuite.TestSequence(new List<int> { expOut }, i, TestLessEqualGreaterEight);
            }

            List<int> TestLessEqualGreaterEight(int input)
            {
                List<int> program = new List<int> { 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,
                                                1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,
                                                999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99};
                IntCode ic = new IntCode(program);
                return ic.Run(new List<int> { input });
            }
        }

        static void TestSolvePartOne()
        {
            TestSuite.Test(13294380, Day5.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            TestSuite.Test(11460760, Day5.SolvePartTwo);
        }
    }
}
