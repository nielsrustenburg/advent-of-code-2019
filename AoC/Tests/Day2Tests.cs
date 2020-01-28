using System;
using System.Collections.Generic;
using System.Text;

namespace AoC.Tests
{
    static class Day2Tests
    {
        public static void RunAll()
        {
            TestIntCode();
            TestSolvePartOne();
            TestSolvePartTwo();
            Console.WriteLine("Day2Tests Completed!");
        }

        static void TestIntCode()
        {
            void TestOutcome(IntCode ic, int expectedOutput)
            {
                string input = ic.ToString();
                 ic.Run();
                int output = ic.GetValAtMemIndex(0);
                if (output != expectedOutput) throw new Exception($"{input}: {output}, expected {expectedOutput}");
            }
            void Test(IntCode ic, List<int> expectedOutput)
            {
                string input = ic.ToString();
                ic.Run();
                string output = ic.ToString();
                string expOut = new IntCode(expectedOutput).ToString();
                if (output != expOut) throw new Exception($"{input}: {output}, expected {expOut}");
            }
            IntCode ic1 = new IntCode(new List<int> { 1, 9, 10, 3, 2, 3, 11, 0, 99, 30, 40, 50 }); //Add->Multiply
            IntCode ic2 = new IntCode(new List<int> { 1, 0, 0, 0, 99 }); //Add
            IntCode ic3 = new IntCode(new List<int> { 2, 3, 0, 3, 99 }); //Multiply
            IntCode ic4 = new IntCode(new List<int> { 2, 4, 4, 5, 99, 0 }); //Multiply
            IntCode ic5 = new IntCode(new List<int> { 1, 1, 1, 4, 99, 5, 6, 0, 99 }); //Add->Multiply
            TestOutcome(ic1, 3500);
            Test(ic2, new List<int> { 2, 0, 0, 0, 99 });
            Test(ic3, new List<int> { 2, 3, 0, 6, 99 });
            Test(ic4, new List<int> { 2, 4, 4, 5, 99, 9801 });
            Test(ic5, new List<int> { 30, 1, 1, 4, 2, 5, 6, 0, 99 });
        }

        static void TestSolvePartOne()
        {
            TestSuite.Test(3716293, Day2.SolvePartOne);
        }

        static void TestSolvePartTwo()
        {
            TestSuite.Test(6429, Day2.SolvePartTwo);
        }
    }
}
