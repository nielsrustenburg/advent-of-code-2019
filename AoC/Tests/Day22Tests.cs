using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;

namespace AoC.Tests
{
    static class Day22Tests
    {
        public static void RunAll()
        {
            TestCardCut();
            TestCardIntoNewStack();
            TestCardWithIncrement();
            TestCombinedShuffles();
            TestSolvePartOne();
            TestReverseCardIntoNewStack();
            TestReverseCardCut();
            TestReverseCardWithIncrement();

            TestNSNSRule();
            TestCutCutRule();
            TestIncIncRule();

            TestNSCutRule();
            TestNSIncRule();

            TestCutNSRule();
            TestCutIncRule();

            TestIncNSRule();

            TestSolvePartTwo();
            Console.WriteLine("Day22Tests Completed!");
        }

        public static void TestSolvePartOne()
        {
            void Test(int expectedOutput)
            {
                int output = Day22.SolvePartOne();
                if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
            }
            Test(5755);
        }

        public static void TestCardCut()
        {
            List<string> instructions = new List<string> { "cut 3" };
            string expectedResult = "3,4,5,6,7,8,9,0,1,2";
            DeckTest(instructions, expectedResult);

            instructions = new List<string> { "cut -4" };
            expectedResult = "6,7,8,9,0,1,2,3,4,5";
            DeckTest(instructions, expectedResult);
        }

        public static void TestCardWithIncrement()
        {
            List<string> instructions = new List<string> { "deal with increment 3" };
            string expectedResult = "0,7,4,1,8,5,2,9,6,3";
            DeckTest(instructions, expectedResult);
        }

        public static void TestCardIntoNewStack()
        {
            List<string> instructions = new List<string> { "deal into new stack" };
            string expectedResult = "9,8,7,6,5,4,3,2,1,0";
            DeckTest(instructions, expectedResult);
        }

        public static void TestCombinedShuffles()
        {
            List<string> instructions = new List<string> { "deal with increment 7", "deal into new stack", "deal into new stack" };
            string expectedResult = "0,3,6,9,2,5,8,1,4,7";
            DeckTest(instructions, expectedResult);

            instructions = new List<string> { "cut 6", "deal with increment 7", "deal into new stack" };
            expectedResult = "3,0,7,4,1,8,5,2,9,6";
            DeckTest(instructions, expectedResult);

            instructions = new List<string> { "deal with increment 7", "deal with increment 9", "cut -2" };
            expectedResult = "6,3,0,7,4,1,8,5,2,9";
            DeckTest(instructions, expectedResult);

            instructions = new List<string> {   "deal into new stack",
                                                "cut -2",
                                                "deal with increment 7",
                                                "cut 8",
                                                "cut -4",
                                                "deal with increment 7",
                                                "cut 3",
                                                "deal with increment 9",
                                                "deal with increment 3",
                                                "cut -1" };
            expectedResult = "9,2,5,8,1,4,7,0,3,6";
            DeckTest(instructions, expectedResult);
        }

        public static void TestNSNSRule()
        {
            //NewStack
            //NewStack
            //-------------------
            //Nothing
            List<string> top = new List<string> { "deal into new stack", "deal into new stack" };
            List<string> bottom = new List<string>();
            TestInstructionEquivalence(top, bottom);
        }

        public static void TestCutCutRule()
        {
            //Cut i
            //Cut j
            //-------------------
            //Cut i+j
            for (int i = -9; i < 10; i++)
            {
                for (int j = -9; j < 10; j++)
                {
                    List<string> top = new List<string> { $"cut {i}", $"cut {j}" };
                    List<string> bottom = new List<string> { $"cut {i + j}" };
                    TestInstructionEquivalence(top, bottom);
                }
            }
        }

        public static void TestIncIncRule()
        {
            //Increment i
            //Increment j
            //-------------------
            //Increment i*j
            List<int> incBy = new List<int> { 1, 3, 7, 9 };
            foreach (int i in incBy)
            {
                foreach (int j in incBy)
                {
                    List<string> top = new List<string> { $"deal with increment {i}", $"deal with increment {j}" };
                    List<string> bottom = new List<string> { $"deal with increment {i * j}" };
                    TestInstructionEquivalence(top, bottom);
                }
            }
        }

        public static void TestNSCutRule()
        {
            //NewStack
            //Cut i
            //-------------------
            //Cut -i
            //NewStack
            for (int i = -9; i < 10; i++)
            {
                List<string> top = new List<string> { $"deal into new stack", $"cut {i}" };
                List<string> bottom = new List<string> { $"cut {-i}", "deal into new stack" };
                TestInstructionEquivalence(top, bottom);
            }
        }

        public static void TestCutNSRule()
        {
            //Cut i
            //NewStack
            //-------------------
            //NewStack
            //Cut -i
            for (int i = -9; i < 10; i++)
            {
                List<string> top = new List<string> { $"cut {i}", $"deal into new stack" };
                List<string> bottom = new List<string> { "deal into new stack", $"cut {-i}" };
                TestInstructionEquivalence(top, bottom);
            }
        }

        public static void TestCutIncRule()
        {
            //Cut i
            //Inc j
            //-------------------
            //Inc j
            //Cut i * j
            List<int> incBy = new List<int> { 1, 3, 7, 9 };
            for (int i = -9; i < 10; i++)
            {
                foreach (int j in incBy)
                {
                    List<string> top = new List<string> { $"cut {i}", $"deal with increment {j}" };
                    List<string> bottom = new List<string> { $"deal with increment {j}", $"cut {i * j}" };
                    TestInstructionEquivalence(top, bottom);
                }
            }
        }

        public static void TestNSIncRule()
        {
            //NewStack
            //Increment i
            //-------------------
            //Increment -i
            //Cut i
            List<int> incBy = new List<int> { 1, 3, 7, 9 };
            foreach (int i in incBy)
            {
                List<string> top = new List<string> { $"deal into new stack", $"deal with increment {i}" };
                List<string> bottom = new List<string> { $"deal with increment {-i}", $"cut {i}" };
                TestInstructionEquivalence(top, bottom);
            }
        }

        public static void TestIncNSRule()
        {
            //Increment i
            //NewStack
            //-------------------
            //Increment -i
            //Cut 1
            List<int> incBy = new List<int> { 1, 3, 7, 9 };
            foreach (int i in incBy)
            {
                List<string> top = new List<string> { $"deal with increment {i}", $"deal into new stack" };
                List<string> bottom = new List<string> { $"deal with increment {-i}", $"cut {1}" };
                TestInstructionEquivalence(top, bottom);
            }
        }

        public static void TestInstructionEquivalence(List<string> instructionsA, List<string> instructionsB)
        {
            Deck deckA = new Deck(10);
            deckA.PerformShuffle(instructionsA);

            Deck deckB = new Deck(10);
            deckB.PerformShuffle(instructionsB);

            if (deckA.ToString() != deckB.ToString())
            {
                List<string> errorMessage = new List<string> { "Instructions not equivalent" };
                errorMessage.Add($"Deck A: {deckA}");
                errorMessage.Add($"Deck B: {deckB}");
                errorMessage.Add("");
                errorMessage.Add("Instructions Deck A:");
                errorMessage.AddRange(instructionsA);
                errorMessage.Add("");
                errorMessage.Add("Instructions Deck B:");
                errorMessage.AddRange(instructionsB);
                throw new Exception(string.Join(" \n ", errorMessage));
            }
        }

        public static void DeckTest(List<string> instructions, string expected)
        {
            Deck cards = new Deck(10);
            cards.PerformShuffle(instructions);
            if (cards.ToString() != expected)
            {
                List<string> errorMessage = new List<string> { "Unexpected result...", "Instructions:" };
                errorMessage.AddRange(instructions);
                errorMessage.Add($"received: {cards}");
                errorMessage.Add($"expected: {expected}");
                throw new Exception(string.Join(" \n ", errorMessage));
            }
        }

        public static void TestReverseCardIntoNewStack()
        {
            BigInteger deckSize = 10;
            List<BigInteger> expectedResult = Enumerable.Range(0, (int)deckSize).Select(x => (BigInteger)x).ToList();
            List<BigInteger> cards = expectedResult.OrderByDescending(x => x).ToList();
            List<BigInteger> result = cards.Select(c => Card.ReverseIntoNewStack(c, deckSize)).ToList();

            for (int i = 0; i < deckSize; i++)
            {
                if (expectedResult[i] != result[i]) throw new Exception($"Unexpected Result exp:{expectedResult[i]} got:{result[i]}");
            }
        }

        public static void TestReverseCardCut()
        {
            BigInteger deckSize = 10;
            List<BigInteger> cards = new List<BigInteger> { 7, 8, 9, 0, 1, 2, 3, 4, 5, 6 };
            List<BigInteger> expectedResult = Enumerable.Range(0, (int)deckSize).Select(x => (BigInteger)x).ToList();
            List<BigInteger> result = cards.Select(c => Card.ReverseCut(c, deckSize, 3)).ToList();

            for (int i = 0; i < deckSize; i++)
            {
                if (expectedResult[i] != result[i]) throw new Exception($"Unexpected Result exp:{expectedResult[i]} got:{result[i]}");
            }
        }

        public static void TestReverseCardWithIncrement()
        {
            BigInteger deckSize = 10;
            List<BigInteger> expectedResult = Enumerable.Range(0, (int)deckSize).Select(x => (BigInteger)x).ToList();
            List<BigInteger> cards = new List<BigInteger> { 0, 3, 6, 9, 2, 5, 8, 1, 4, 7 };
            List<BigInteger> result = cards.Select(c => Card.ReverseWithIncrement(c, deckSize, 3)).ToList();

            for (int i = 0; i < deckSize; i++)
            {
                if (expectedResult[i] != result[i]) throw new Exception($"Unexpected Result exp:{expectedResult[i]} got:{result[i]}");
            }
        }

        public static void TestSolvePartTwo()
        {
            void Test(string expectedString)
            {
                BigInteger expectedOutput = BigInteger.Parse(expectedString);
                BigInteger output = Day22.SolvePartTwo();
                if (output != expectedOutput) throw new Exception($"{TestSuite.GetCurrentMethod()}(): {output}, expected {expectedOutput}");
            }
            Test("42152620178084");
        }
    }
}
