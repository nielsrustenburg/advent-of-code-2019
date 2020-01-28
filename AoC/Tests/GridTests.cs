using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC.Tests
{
    static class GridTests
    {
        public static void TestGrid()
        {
            List<string> example = new List<string> { "+++++",
                                                      "+*+++",
                                                      "++++*",
                                                      "+#+++",
                                                      "+***+",};
            char defaultChar = '+';
            Grid<char> charGrid = new Grid<char>(example, defaultChar);
            int expectedWidth = example[0].Length;
            int expectedHeight = example.Count;
            if (charGrid.Width != expectedWidth) throw new Exception($"Expected width of: {expectedWidth}, found: {charGrid.Width}");
            if (charGrid.Height != expectedHeight) throw new Exception($"Expected height of: {expectedHeight}, found: {charGrid.Height}");

            int expectedNonDefault = example.Aggregate(0, (a, b) => a + b.Where(x => x != defaultChar).Count());
            if (charGrid.Count() != expectedNonDefault) throw new Exception($"Expected: {expectedNonDefault} occurrences of non '{defaultChar}', found:{charGrid.Count()}");

            List<string> representation = charGrid.RowsAsStrings();
            for(int i =0; i < example.Count; i++)
            {
                if (representation[i] != example[i]) throw new Exception($"Unmatching grid representation: line{i} \n expected: \n line{example[i]} \n got \n {representation[i]}");
            }

            var hashtag = charGrid.FindLocationOf('#');
            if (hashtag.x != 1 || hashtag.y != -3) throw new Exception($"Hashtag not in expected location");
        }
    }
}
