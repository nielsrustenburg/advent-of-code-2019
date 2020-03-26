using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using AoC.Common;
using AoC.Utils;

namespace AoC.Day6
{
    class Tests
    {
        const string testCase1 = @"COM)B
B)C
C)D
D)E
E)F
B)G
G)H
D)I
E)J
J)K
K)L";

        const string testCase2 = @"COM)B
B)C
C)D
D)E
E)F
B)G
G)H
D)I
E)J
J)K
K)L
K)YOU
I)SAN";

        [TestCase(testCase1,42)]
        public void TestIndirectOrbits(string input,int expectedOutput)
        {
            var orbitRelations = InputParser<(string, string)>.SplitAndParse(input, Solver.ParseOrbitRelation);
            var orbitTree = OrbitTreeBuilder.MakeTrees(orbitRelations)[0];
            Assert.AreEqual(expectedOutput, orbitTree.CountAllOrbits());
        }

        [TestCase(testCase2,"YOU","SAN", 4)]
        public void TestDistanceBetweenNodes(string input, string from, string to, int expectedOutput)
        {
            var orbitRelations = InputParser<(string, string)>.SplitAndParse(input, Solver.ParseOrbitRelation);
            var orbitTree = OrbitTreeBuilder.MakeTrees(orbitRelations)[0];
            Assert.AreEqual(expectedOutput, orbitTree.DistanceBetweenNodes(from, to));
        }

        [TestCase("158090", "241")]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }
    }
}
