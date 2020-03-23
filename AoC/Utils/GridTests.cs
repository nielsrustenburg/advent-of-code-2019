using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;

namespace AoC.Utils
{
    class GridTests
    {
        [Test]
        public void TestGrid()
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
            Assert.AreEqual(expectedWidth, charGrid.Width);
            Assert.AreEqual(expectedHeight, charGrid.Height);

            int expectedNonDefaultCount = example.Aggregate(0, (a, b) => a + b.Where(x => x != defaultChar).Count());
            Assert.AreEqual(expectedNonDefaultCount, charGrid.CountNonDefault());

            List<string> representation = charGrid.RowsAsStrings();
            for (int i = 0; i < example.Count; i++)
            {
                Assert.AreEqual(example[i], representation[i]);
            }

            var hashtag = charGrid.FindFirstMatchingTile('#');
            Assert.AreEqual((1, 1), (hashtag.x, hashtag.y));
        }
    }
}
