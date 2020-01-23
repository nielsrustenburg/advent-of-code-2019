using System;
using System.Collections.Generic;
using System.Text;

namespace AoC.Tests
{
    static class Day7Tests
    {

        public static void RunAll()
        {
            TestSolvePartOne();
            TestSolvePartTwo();
            TestPermutations();

            Console.WriteLine("Day7Tests Completed!");
        }

        private static void TestPermutations()
        {
            void Test(List<int> input, int expectedOutput)
            {
                int output = Day7.Permutations(input).Count;
                if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}({input}): {output}, expected {expectedOutput}");
            }
            Test(new List<int> { 1, 2, 3 }, 6);
            Test(new List<int> { 1, 2, 3, 4}, 24);
        }

        public static void TestSolvePartOne()
        {
            void Test(int expectedOutput)
            {
                int output = Day7.SolvePartOne();
                if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
            }
            Test(567045);
        }

        public static void TestSolvePartTwo()
        {
            void Test(int expectedOutput)
            {
                int output = Day7.SolvePartTwo();
                if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
            }
            Test(39016654);
        }

        public static void TestEnumerator()
        {
            List<int> oneTwoThree = new List<int> { 1, 2, 3 };
            IEnumerator<int> enumer = oneTwoThree.GetEnumerator();

            for(int i =1; i < 5; i++)
            {
                enumer.MoveNext();
                if (enumer.Current == i) Console.WriteLine($"Yay {enumer.Current} is {i}");
                if(i == 3)
                {
                    oneTwoThree.Add(4);
                    enumer = oneTwoThree.GetEnumerator();
                }
            }
        }
    }
}
