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
            List<int> program = new List<int> { 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,
                                                1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,
                                                999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99};
                                                

            //List<int> program = new List<int> { 3, 3, 1108, -1, 8, 3, 4, 3, 99 };
            IntCode ic = new IntCode(program);
            ic.Run(new List<int> { 8 });
        }

        //public static void TestSolvePartOne()
        //{
        //    void Test(int expectedOutput)
        //    {
        //        int output = Day5.SolvePartOne();
        //        if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
        //    }
        //    Test(13294380);
        //}

        //public static void TestSolvePartTwo()
        //{
        //    void Test(int expectedOutput)
        //    {
        //        int output = Day5.SolvePartTwo();
        //        if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
        //    }
        //    Test(11460760);
        //}

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
