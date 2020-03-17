using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using AoC.Utils;
using System.Linq;

namespace AoC.Tests
{
    static class TestSuite
    {
        public static void RunTests()
        {
            DirectionTests.RunAll();
            GridTests.TestGrid();

            string d8p2expout = "111  1  1 1111 111  111  \n" +
                                "1  1 1  1    1 1  1 1  1 \n" +
                                "1  1 1  1   1  111  1  1 \n" +
                                "111  1  1  1   1  1 111  \n" +
                                "1 1  1  1 1    1  1 1    \n" +
                                "1  1  11  1111 111  1    ";

            string d11p2expout = "..##..###..#....###....##.####..##..#..#...\n" +
                                 ".#..#.#..#.#....#..#....#....#.#..#.#..#...\n" +
                                 ".#....###..#....#..#....#...#..#....#..#...\n" +
                                 ".#....#..#.#....###.....#..#...#....#..#...\n" +
                                 ".#..#.#..#.#....#....#..#.#....#..#.#..#...\n" +
                                 "..##..###..####.#.....##..####..##...##....";

            var d1 = new Day1.Tester(new Day1.Solver(), "3362507", "5040874");
            var d2 = new Day2.Tester(new Day2.Solver(), "3716293", "6429");
            var d3 = new Day3.Tester(new Day3.Solver(), "352", "43848");
            var d4 = new Day4.Tester(new Day4.Solver(), "544", "334");
            var d5 = new Day5.Tester(new Day5.Solver(), "13294380", "11460760");
            var d6 = new Day6.Tester(new Day6.Solver(), "158090", "241");
            var d7 = new Day7.Tester(new Day7.Solver(), "567045", "39016654");
            var d8 = new Day8.Tester(new Day8.Solver(), "1441", d8p2expout);
            var d9 = new Day9.Tester(new Day9.Solver(), "2453265701", "80805");
            var d10 = new Day10.Tester(new Day10.Solver(), "282", "1008");
            var d11 = new Day11.Tester(new Day11.Solver(), "2276", d11p2expout);
            var d12 = new Day12.Tester(new Day12.Solver(), "10635", "583523031727256");
            var d13 = new Day13.Tester(new Day13.Solver(), "173", "8942");
            var d14 = new Day14.Tester(new Day14.Solver(), "483766", "3061522");
            var d15 = new Day15.Tester(new Day15.Solver(), "266", "274");
            var d16 = new Day16.Tester(new Day16.Solver(), "34694616", "17069048");
            var d17 = new Day17.Tester(new Day17.Solver(), "13580", "1063081");
            var d18 = new Day18.Tester(new Day18.Solver(), "3146", "2194");
            var d19 = new Day19.Tester(new Day19.Solver(), "201", "6610984");
            var d20 = new Day20.Tester(new Day20.Solver(), "668", "7778");
            var d21 = new Day21.Tester(new Day21.Solver(), "19353565", "1140612950");
            var d22 = new Day22.Tester(new Day22.Solver(), "5755", "42152620178084");
            var d23 = new Day23.Tester(new Day23.Solver(), "24555", "19463");
            var d24 = new Day24.Tester(new Day24.Solver(), "32509983", "2012");
            var d25 = new Day25.Tester(new Day25.Solver(), "34095120", "no part two");

            d1.RunAll();
            d2.RunAll();
            d3.RunAll();
            d4.RunAll();
            d5.RunAll();
            d6.RunAll();
            d7.RunAll();
            d8.RunAll();
            d9.RunAll();
            d10.RunAll();
            d11.RunAll();
            d12.RunAll();
            d13.RunAll();
            d14.RunAll();
            d15.RunAll();
            d16.RunAll();
            d17.RunAll();
            d18.RunAll();
            d19.RunAll();
            d20.RunAll();
            d21.RunAll();
            d22.RunAll();
            d23.RunAll();
            d24.RunAll();
            d25.RunAll();
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
