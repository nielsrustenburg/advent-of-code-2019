using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;

namespace AoC.Tests
{
    static class TestSuite
    {
        public static void RunTests()
        {
            DirectionTests.RunAll();
            GridTests.TestGrid();

            Day1Tests.RunAll();
            Day2Tests.RunAll();
            Day3Tests.RunAll();
            Day4Tests.RunAll();
            Day5Tests.RunAll();
            Day6Tests.RunAll();
            Day7Tests.RunAll();
            Day8Tests.RunAll();
            Day9Tests.RunAll();
            Day10Tests.RunAll();
            Day11Tests.RunAll();
            Day12Tests.RunAll();
            Day13Tests.RunAll();
            Day14Tests.RunAll();
            Day15Tests.RunAll();
            Day16Tests.RunAll();
            Day17Tests.RunAll();
            Day18Tests.RunAll();
            Day19Tests.RunAll();
            Day20Tests.RunAll();
            Day21Tests.RunAll();
            Day22Tests.RunAll();
            Day23Tests.RunAll();
            Day24Tests.RunAll();
            Day25Tests.RunAll();
        }

        public static void Test<T,V>(T expected, V received, string testName)
        {
            if (expected.ToString() != received.ToString()) throw new Exception($"{testName} failed: \n expected value: \n {expected.ToString()} \n received value \n {received.ToString()}");
        }

        public static void Test<T, G>(T expected, Func<G> function)
        {
            string output = function().ToString();
            if (expected.ToString() != output) throw new Exception($"{function.Method.Name} failed: \n expected value: \n {expected.ToString()} \n received value \n {output}");
        }

        public static void Test<T,V,G>(T expected, V input, Func<V,G> function) 
        {
            string output = function(input).ToString();
            if (expected.ToString() != output) throw new Exception($"{function.Method.Name} failed: \n expected value: \n {expected.ToString()} \n received value \n {output}");
        }

        public static void TestCount<V,G>(int expectedCount, V input, Func<V,IEnumerable<G>> function)
        {
            IEnumerable<G> output = function(input);
            int outputCount = output.Count();
            if (expectedCount != outputCount) throw new Exception($"{function.Method.Name} failed: \n expected element Count: \n {expectedCount} \n actual output Count: \n {outputCount}");
        }

        public static void TestSequence<T,V>(IEnumerable<T> expected, V input, Func<V,IEnumerable<T>> function)
        {
            List<T> output = function(input).ToList();
            List<T> expList = expected.ToList();
            string errorMessage = $"{function.Method.Name} failed: \n ";
            bool equalLength = output.Count == expList.Count;

            IEnumerable<int> missingIndices = null;
            if (!equalLength)
            {
                errorMessage = errorMessage + ($"expected output length: \n {expList.Count} \n actual output length \n {output.Count} \n");
                missingIndices = Enumerable.Range(Math.Min(output.Count, expList.Count), Math.Max(output.Count, expList.Count) - Math.Min(output.Count, expList.Count));
            }

            string indexErrorMessage = "Mismatch at indices: ";
            bool equalContent = equalLength;
            for (int i = 0; i < Math.Min(output.Count, expList.Count); i++)
            {
                bool equalAtIndex_i = output[i].Equals(expList[i]);
                if (!equalAtIndex_i)
                {
                    indexErrorMessage = indexErrorMessage + $"{i}, ";
                }
                equalContent = equalContent && equalAtIndex_i;
            }

            if (!equalContent)
            {
                errorMessage = errorMessage + indexErrorMessage;
                if (!equalLength)
                {
                    errorMessage = errorMessage + string.Join(',', missingIndices.Select(x => x.ToString()));
                }
                throw new Exception(errorMessage);
            }
        }

        public static string GetCurrentMethod()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }
    }
}
