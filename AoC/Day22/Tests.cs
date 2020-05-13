using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AoC.Day22
{
    class Tests
    {
        [TestCase("5755", "42152620178084")]
        public void TestSolver(string expOut1, string expOut2)
        {
            var solver = new Solver();
            Assert.AreEqual(expOut1, solver.SolutionOne());
            Assert.AreEqual(expOut2, solver.SolutionTwo());
        }

        //public static void TestShuffleActionMerges()
        //{
        //    {
        //        /*
        //        NewStack
        //        NewStack
        //        -------------------
        //        Nothing

        //        Cut i
        //        Cut j
        //        -------------------
        //        Cut i+j

        //        Increment i
        //        Increment j
        //        -------------------
        //        Increment i*j

        //        NewStack
        //        Cut i
        //        -------------------
        //        Cut -i
        //        NewStack

        //        Cut i
        //        NewStack
        //        -------------------
        //        NewStack
        //        Cut -i

        //        Cut i
        //        Inc j
        //        -------------------
        //        Inc j
        //        Cut i * j

        //        NewStack
        //        Increment i
        //        -------------------
        //        Increment -i
        //        Cut i

        //        Increment i
        //        NewStack
        //        -------------------
        //        Increment -i
        //        Cut 1
        //        */
        //    }
        //    BigInteger deckSize = 10;
        //    Cut cut = new Cut(6);
        //    DealIntoNewStack dins = new DealIntoNewStack();
        //    DealWithIncrement dwi = new DealWithIncrement(7);

        //    List<IShuffleAction> simplecutcut = new List<IShuffleAction> { cut, cut };
        //    List<IShuffleAction> simplecutdins = new List<IShuffleAction> { cut, dins };
        //    List<IShuffleAction> simplecutdwi = new List<IShuffleAction> { cut, dwi };

        //    List<IShuffleAction> simpledinsdins = new List<IShuffleAction> { dins, dins };
        //    List<IShuffleAction> simpledinscut = new List<IShuffleAction> { dins, cut };
        //    List<IShuffleAction> simpledinsdwi = new List<IShuffleAction> { dins, dwi };

        //    List<IShuffleAction> simpledwidwi = new List<IShuffleAction> { dwi, dwi };
        //    List<IShuffleAction> simpledwidins = new List<IShuffleAction> { dwi, dins };
        //    //List<IShuffleAction> simpledwicut = new List<IShuffleAction> { dwi, cut };

        //    List<List<IShuffleAction>> simpleCombinations = new List<List<IShuffleAction>> { simplecutcut,
        //                                                                                     simplecutdins,
        //                                                                                     simplecutdwi,
        //                                                                                     simpledinsdins,
        //                                                                                     simpledinscut,
        //                                                                                     simpledinsdwi,
        //                                                                                     simpledwidwi,
        //                                                                                     simpledwidins};

        //    List<IShuffleAction> cutcut = cut.MergeWithBelow(cut, deckSize).ToList();
        //    List<IShuffleAction> cutdins = cut.MergeWithBelow(dins, deckSize).ToList();
        //    List<IShuffleAction> cutdwi = cut.MergeWithBelow(dwi, deckSize).ToList();

        //    List<IShuffleAction> dinsdins = dins.MergeWithBelow(dins, deckSize).ToList();
        //    List<IShuffleAction> dinscut = dins.MergeWithBelow(cut, deckSize).ToList();
        //    List<IShuffleAction> dinsdwi = dins.MergeWithBelow(dwi, deckSize).ToList();

        //    List<IShuffleAction> dwidwi = dwi.MergeWithBelow(dwi, deckSize).ToList();
        //    List<IShuffleAction> dwidins = dwi.MergeWithBelow(dins, deckSize).ToList();
        //    //List<IShuffleAction> dwicut = dwi.MergeWithBelow(cut).ToList(); //NOT IMPLEMENTED

        //    List<List<IShuffleAction>> mergedCombinations = new List<List<IShuffleAction>> { cutcut,
        //                                                                                     cutdins,
        //                                                                                     cutdwi,
        //                                                                                     dinsdins,
        //                                                                                     dinscut,
        //                                                                                     dinsdwi,
        //                                                                                     dwidwi,
        //                                                                                     dwidins};
        //    for (int m = 0; m < mergedCombinations.Count; m++)
        //    {
        //        for (int card = 0; card < deckSize; card++)
        //        {
        //            BigInteger simpleCard = card;
        //            BigInteger mergeCard = card;
        //            foreach (IShuffleAction simpleAction in simpleCombinations[m])
        //            {
        //                simpleCard = simpleAction.NextCardPosition(simpleCard, deckSize);
        //            }
        //            foreach (IShuffleAction mergeAction in mergedCombinations[m])
        //            {
        //                mergeCard = mergeAction.NextCardPosition(mergeCard, deckSize);
        //            }
        //            TestSuite.Test(simpleCard, mergeCard, $"TestShuffleActionMerges: {m}");
        //        }
        //    }
        //}
    }
}
