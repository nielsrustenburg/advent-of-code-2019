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
            var example = new List<string> { "+++++",
                                             "+*+++",
                                             "++++*",
                                             "+#+++",
                                             "+***+",};
            var defaultChar = '+';

            var charGrid = new Grid<char>(example, defaultChar, true);
            var arrayCharGrid = new ArrayGrid<char>(example,'+',true);


            int expectedWidth = example[0].Length;
            int expectedHeight = example.Count;
            Assert.AreEqual(expectedWidth, charGrid.Width);
            Assert.AreEqual(expectedHeight, charGrid.Height);
            Assert.AreEqual(expectedWidth, arrayCharGrid.Width);
            Assert.AreEqual(expectedHeight, arrayCharGrid.Height);

            var expectedCounts = string.Join("", example).GroupBy(c => c).ToDictionary(c => c.Key, c => c.Count());
            var gridTileCounts = charGrid.GetTileCounts();
            var arrayGridTileCounts = arrayCharGrid.GetTileCounts();

            Assert.AreEqual(expectedCounts['*'], gridTileCounts['*']);
            Assert.AreEqual(expectedCounts['#'], gridTileCounts['#']);
            //Assert.AreEqual(expectedCounts['+'], gridTileCounts['+']); //doesn't keep track of defaultTile count
            Assert.AreEqual(expectedCounts['*'], arrayGridTileCounts['*']);
            Assert.AreEqual(expectedCounts['#'], arrayGridTileCounts['#']);
            Assert.AreEqual(expectedCounts['+'], arrayGridTileCounts['+']);


            var gridRepresentations = charGrid.RowsAsStrings();
            var arrayGridRepresentations = arrayCharGrid.RowsAsStrings();
            for (int i = 0; i < example.Count; i++)
            {
                Assert.AreEqual(example[i], gridRepresentations[i]);
                Assert.AreEqual(example[i], arrayGridRepresentations[i]);
            }

            var hashtagGridLocation = ((int x,int y))charGrid.FindFirstMatchingTile('#');
            var hashtagArrayGridLocation = ((int x, int y))arrayCharGrid.FindFirstMatchingTile('#');
            Assert.AreEqual((1, 1), (hashtagGridLocation.x, hashtagGridLocation.y));
            Assert.AreEqual((1, 1), (hashtagArrayGridLocation.x, hashtagArrayGridLocation.y));
        }
    }
}
