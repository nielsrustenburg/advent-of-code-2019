using System;
using System.Collections.Generic;
using System.Text;
using AoC.common;
using AoC.Tests;

namespace AoC.Day2
{
    class Tester : PuzzleSolverTester
    {
        public Tester(IPuzzleSolver solver, string expectedOne, string expectedTwo) : base(solver, expectedOne, expectedTwo)
        {
        }

        public override void RunAll()
        {
            TestIntCode();
            TestSolver();
        }

        static string CreateAndRunIntCode(IEnumerable<int> inputCode)
        {
            IntCode ic = new IntCode(inputCode);
            ic.Run();
            return ic.ToString();
        }

        static void TestIntCode()
        {
            TestSuite.Test("IC[3500,9,10,70,2,3,11,0,99,30,40,50]", new List<int> { 1, 9, 10, 3, 2, 3, 11, 0, 99, 30, 40, 50 }, CreateAndRunIntCode); //Add->Multiply
            TestSuite.Test("IC[2,0,0,0,99]", new List<int> { 1, 0, 0, 0, 99 }, CreateAndRunIntCode); //Add
            TestSuite.Test("IC[2,3,0,6,99]", new List<int> { 2, 3, 0, 3, 99 }, CreateAndRunIntCode); //Multiply
            TestSuite.Test("IC[2,4,4,5,99,9801]", new List<int> { 2, 4, 4, 5, 99, 0 }, CreateAndRunIntCode); //Multiply
            TestSuite.Test("IC[30,1,1,4,2,5,6,0,99]", new List<int> { 1, 1, 1, 4, 99, 5, 6, 0, 99 }, CreateAndRunIntCode); //Add->Multiply
        }
    }
}
