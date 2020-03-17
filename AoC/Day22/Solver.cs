using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;
using AoC.common;
using AoC.Utils;

namespace AoC.Day22
{
    class Solver : PuzzleSolver
    {
        List<string> shuffleMoves;
        public Solver() : base(22)
        {
        }

        public Solver(IEnumerable<string> input) : base(input, 22)
        {
        }

        protected override void ParseInput(IEnumerable<string> input)
        {
            shuffleMoves = input.ToList();
        }

        protected override void PrepareSolution()
        {
            //no common prep
        }

        protected override void SolvePartOne()
        {
            ShuffleSequence shufSeq = new ShuffleSequence(shuffleMoves, 10007);
            resultPartOne = shufSeq.CardPositionAfterShuffle(2019).ToString();
        }

        protected override void SolvePartTwo()
        {
            BigInteger deckSize = BigInteger.Parse("119315717514047");
            BigInteger shuffleNtimes = BigInteger.Parse("101741582076661");
            BigInteger findCard = 2020;

            ShuffleSequence shufSeq = new ShuffleSequence(shuffleMoves, deckSize);
            shufSeq.CompressSequence();

            ShuffleSequence repeatedShuffles = SequenceAfterNRepetitions(shufSeq, shuffleNtimes);

            resultPartTwo = repeatedShuffles.CardPositionBeforeShuffle(findCard).ToString();
        }

        public static ShuffleSequence SequenceAfterNRepetitions(ShuffleSequence shufSeq, BigInteger reps)
        {
            List<ShuffleSequence> shufflesPerBit = new List<ShuffleSequence> { new ShuffleSequence(shufSeq) };
            int bitsNeeded = 0;
            BigInteger previousBitValue = 1;
            BigInteger bitValue = 2;

            //Create shufflesequences for each bit value up to and including the largest bit value smaller than "reps"
            while (bitValue < reps)
            {
                ShuffleSequence next = new ShuffleSequence(shufflesPerBit[bitsNeeded]);
                next.AddToSequence(next);
                next.CompressSequence();
                shufflesPerBit.Add(next);
                previousBitValue = bitValue;
                bitValue = previousBitValue * 2;
                bitsNeeded++;
            }

            //Combine shufflesequences for each activated bit in the bit-representation of "reps"
            ShuffleSequence repeatedShuffleSequence = new ShuffleSequence(shufSeq.DeckSize);
            BigInteger remaining = reps;

            for (int i = shufflesPerBit.Count - 1; i >= 0; i--)
            {
                if (previousBitValue <= remaining)
                {
                    repeatedShuffleSequence.AddToSequence(shufflesPerBit[i]);
                    repeatedShuffleSequence.CompressSequence();
                    remaining = remaining - previousBitValue;
                }
                previousBitValue = previousBitValue / 2;
            }

            return repeatedShuffleSequence;
        }
    }

    class ShuffleSequence
    {
        List<IShuffleAction> shuffleActions;
        public BigInteger DeckSize { get; set; }

        public ShuffleSequence(BigInteger deckSize)
        {
            this.DeckSize = deckSize;
            shuffleActions = new List<IShuffleAction>();
        }

        public ShuffleSequence(IEnumerable<string> instructions, BigInteger deckSize) : this(deckSize)
        {
            shuffleActions.AddRange(ShuffleActionStore.CreateShuffleActions(instructions));
        }

        public ShuffleSequence(ShuffleSequence copyMe) : this(copyMe.DeckSize)
        {
            foreach (IShuffleAction action in copyMe.shuffleActions)
            {
                shuffleActions.Add(action.Copy());
            }
        }

        public BigInteger CardPositionAfterShuffle(BigInteger currentPos)
        {
            foreach (IShuffleAction action in shuffleActions)
            {
                currentPos = action.NextCardPosition(currentPos, DeckSize);
            }
            return currentPos;
        }

        public BigInteger CardPositionBeforeShuffle(BigInteger currentPos)
        {
            IEnumerable<IShuffleAction> sa = shuffleActions as IEnumerable<IShuffleAction>;
            foreach (IShuffleAction action in sa.Reverse())
            {
                currentPos = action.PreviousCardPosition(currentPos, DeckSize);
            }
            return currentPos;
        }

        public void AddToSequence(ShuffleSequence other)
        {
            List<IShuffleAction> newActions = new List<IShuffleAction>();
            foreach (IShuffleAction action in other.shuffleActions)
            {
                newActions.Add(action.Copy());
            }
            shuffleActions.AddRange(newActions);
        }

        public void CompressSequence()
        {
            CompressIncrementInstructions();
            CompressCutInstructions();
        }

        private void CompressIncrementInstructions()
        {
            int bottomIncrementInstruction = shuffleActions.FindLastIndex(a => a is DealWithIncrement);
            while (bottomIncrementInstruction > 0)
            {
                bottomIncrementInstruction = MergeOrMoveUpIncrementInstruction(bottomIncrementInstruction);
            }
        }

        private int MergeOrMoveUpIncrementInstruction(int bottomIncrementInstruction)
        {
            IEnumerable<IShuffleAction> newActions = shuffleActions[bottomIncrementInstruction - 1].MergeWithBelow(shuffleActions[bottomIncrementInstruction], DeckSize);
            shuffleActions.RemoveRange(bottomIncrementInstruction - 1, 2);
            int newIndex = bottomIncrementInstruction - 2;
            foreach (IShuffleAction newAction in newActions)
            {
                newIndex++;
                shuffleActions.Insert(newIndex, newAction);
            }
            return shuffleActions.FindLastIndex(newIndex, a => a is DealWithIncrement);
        }

        private void CompressCutInstructions()
        {
            int topCutInstruction = shuffleActions.FindIndex(a => a is Cut);
            while (topCutInstruction > -1 && topCutInstruction < shuffleActions.Count - 1)
            {
                topCutInstruction = MergeOrMoveDownCutInstruction(topCutInstruction);
            }
        }

        private int MergeOrMoveDownCutInstruction(int topCutInstruction)
        {
            IEnumerable<IShuffleAction> newActions = shuffleActions[topCutInstruction].MergeWithBelow(shuffleActions[topCutInstruction + 1], DeckSize);
            shuffleActions.RemoveRange(topCutInstruction, 2);
            int newIndex = topCutInstruction - 1;
            foreach (IShuffleAction newAction in newActions)
            {
                newIndex++;
                shuffleActions.Insert(newIndex, newAction);
            }
            return shuffleActions.FindIndex(topCutInstruction, a => a is Cut);
        }

    }

    static class ShuffleActionStore
    {
        public static IShuffleAction CreateShuffleAction(string instruction)
        {
            string[] split = instruction.Split(' ');
            if (split[0] == "cut")
            {
                return new Cut(BigInteger.Parse(split[1]));
            }
            else if (split[2] == "increment")
            {
                return new DealWithIncrement(BigInteger.Parse(split[3]));
            }
            else if (split[2] == "new")
            {
                return new DealIntoNewStack();
            }
            else
            {
                throw new Exception("Meep moop shouldn't be here");
            }
        }

        public static IEnumerable<IShuffleAction> CreateShuffleActions(IEnumerable<string> instructions)
        {
            foreach (string instruction in instructions)
            {
                yield return CreateShuffleAction(instruction);
            }
        }
    }

    interface IShuffleAction
    {
        BigInteger NextCardPosition(BigInteger currentPos, BigInteger deckSize);

        BigInteger PreviousCardPosition(BigInteger currentPos, BigInteger deckSize);

        IEnumerable<IShuffleAction> MergeWithBelow(IShuffleAction other, BigInteger deckSize);

        IShuffleAction Copy();
    }

    class DealWithIncrement : IShuffleAction
    {
        public BigInteger Increment { get; }

        public DealWithIncrement(BigInteger increment)
        {
            this.Increment = increment;
        }

        public IEnumerable<IShuffleAction> MergeWithBelow(IShuffleAction below, BigInteger deckSize)
        {
            if (below is DealIntoNewStack) return new List<IShuffleAction> { new DealWithIncrement(MathHelper.Mod(-Increment, deckSize)), new Cut(1) };
            if (below is DealWithIncrement dwi) return new List<IShuffleAction> { new DealWithIncrement(MathHelper.Mod(Increment * dwi.Increment, deckSize)) };
            if (below is Cut cut) throw new NotImplementedException("not implemented for Cut below, shouldn't be necessary though");
            throw new NotImplementedException();
        }

        public BigInteger NextCardPosition(BigInteger currentPos, BigInteger deckSize)
        {
            return MathHelper.Mod(currentPos * Increment, deckSize);
        }

        public BigInteger PreviousCardPosition(BigInteger currentPos, BigInteger deckSize)
        {
            return MathHelper.Mod(currentPos * MathHelper.ModInv(Increment, deckSize), deckSize);
        }

        public IShuffleAction Copy()
        {
            return new DealWithIncrement(Increment);
        }

        public override string ToString()
        {
            return $"Deal with increment: {Increment}";
        }
    }

    class Cut : IShuffleAction
    {
        public BigInteger CutFrom { get; }

        public Cut(BigInteger cutFrom)
        {
            this.CutFrom = cutFrom;
        }

        public IEnumerable<IShuffleAction> MergeWithBelow(IShuffleAction below, BigInteger deckSize)
        {
            if (below is DealIntoNewStack) return new List<IShuffleAction> { below.Copy(), new Cut(MathHelper.Mod(-CutFrom, deckSize)) };
            if (below is Cut cut) return new List<IShuffleAction> { new Cut(MathHelper.Mod(CutFrom + cut.CutFrom, deckSize)) };
            if (below is DealWithIncrement dwi) return new List<IShuffleAction> { below.Copy(), new Cut(MathHelper.Mod(dwi.Increment * CutFrom, deckSize)) };
            throw new NotImplementedException();
        }

        public BigInteger NextCardPosition(BigInteger currentPos, BigInteger deckSize)
        {
            return MathHelper.Mod(currentPos - CutFrom, deckSize);
        }

        public BigInteger PreviousCardPosition(BigInteger currentPos, BigInteger deckSize)
        {
            //return (currentPos + (deckSize + cutFrom)) % deckSize;
            return MathHelper.Mod(currentPos + CutFrom, deckSize);
        }

        public IShuffleAction Copy()
        {
            return new Cut(CutFrom);
        }

        public override string ToString()
        {
            return $"Cut: {CutFrom}";
        }
    }

    class DealIntoNewStack : IShuffleAction
    {
        public DealIntoNewStack()
        {
        }

        public IEnumerable<IShuffleAction> MergeWithBelow(IShuffleAction below, BigInteger deckSize)
        {
            if (below is DealIntoNewStack) return new List<IShuffleAction>();
            if (below is Cut cut) return new List<IShuffleAction> { new Cut(MathHelper.Mod(-cut.CutFrom, deckSize)), this.Copy() };
            if (below is DealWithIncrement dwi) return new List<IShuffleAction> { new DealWithIncrement(MathHelper.Mod(-dwi.Increment, deckSize)), new Cut(dwi.Increment) };
            throw new NotImplementedException();
        }

        public BigInteger NextCardPosition(BigInteger currentPos, BigInteger deckSize)
        {
            return deckSize - currentPos - 1;
        }

        public BigInteger PreviousCardPosition(BigInteger currentPos, BigInteger deckSize)
        {
            return NextCardPosition(currentPos, deckSize);
        }

        public IShuffleAction Copy()
        {
            return new DealIntoNewStack();
        }

        public override string ToString()
        {
            return "Deal into new stack";
        }
    }
}
