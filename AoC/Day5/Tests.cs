﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;
using NUnit.Framework;
using AoC.Computers;

namespace AoC.Day5
{
    class Tests
    {
        [Test]
        public static void TestOpcodes5Through8()
        {
            for (int i = 0; i < 16; i++)
            {
                int expOut = 1000;
                if (i > 8) expOut++;
                if (i < 8) expOut--;
                var output = TestLessEqualGreaterEight(i);
                Assert.AreEqual(expOut, (int) output);
            }

            BigInteger TestLessEqualGreaterEight(int input)
            {
                List<BigInteger> program = new List<BigInteger> { 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,
                                                                  1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,
                                                                  999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99};
                var intCode = new IntCode(program);
                return intCode.Run(new List<BigInteger> { input }).First();
            }
        }

        [TestCase("13294380", "11460760")]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }
    }
}
