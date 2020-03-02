using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

namespace AoC
{
    static class Day22
    {
        public static int SolvePartOne()
        {
            string[] input = InputReader.StringsFromTxt("d22input.txt");
            List<string[]> split = input.Select(i => i.Split(' ')).ToList();

            BigInteger deckSize = 10007;
            BigInteger trackMe = 2019;

            foreach (string[] move in split)
            {
                if (move[0] == "cut")
                {
                    trackMe = Card.Cut(trackMe, deckSize, int.Parse(move[1]));
                }
                else
                {
                    if (move[2] == "increment")
                    {
                        trackMe = Card.WithIncrement(trackMe, deckSize, int.Parse(move[3]));
                    }
                    else
                    {
                        if (move[2] == "new")
                        {
                            trackMe = Card.IntoNewStack(trackMe, deckSize);
                        }
                        else
                        {
                            throw new Exception("Meep moop shouldn't be here");
                        }
                    }
                }
            }
            return (int)trackMe;
        }

        public static BigInteger SolvePartTwo()
        {
            string[] input = InputReader.StringsFromTxt("d22input.txt");
            BigInteger deckSize = BigInteger.Parse("119315717514047");
            BigInteger shuffleNtimes = BigInteger.Parse("101741582076661");
            BigInteger findCard = 2020;
            ShuffleInstructions si = new ShuffleInstructions(input, deckSize);
            si.Compress();

            List<ShuffleInstructions> shufflesPerBit = new List<ShuffleInstructions> { si };
            int bitsNeeded = 0;
            BigInteger bValue = 1;
            BigInteger bValueNext = 2;
            while (bValueNext < shuffleNtimes)
            {
                ShuffleInstructions next = new ShuffleInstructions(shufflesPerBit[bitsNeeded]);
                next.AddInstructions(shufflesPerBit[bitsNeeded]);//Apply previous set of instructions twice
                next.Compress(); //Keep it small
                shufflesPerBit.Add(next);

                bValue = bValueNext;
                bValueNext = bValue * 2;
                bitsNeeded++;
            }
            ShuffleInstructions finalInstructions = new ShuffleInstructions(shufflesPerBit.Last());
            BigInteger remaining = shuffleNtimes - bValue;
            bValue = bValue / 2;

            //Apply Instruction-sets for all bits necessary to describe shuffleNtimes
            for (int i = shufflesPerBit.Count - 2; i >= 0; i--)
            {
                if (bValue <= remaining)
                {
                    finalInstructions.AddInstructions(shufflesPerBit[i]);
                    finalInstructions.Compress();
                    remaining = remaining - bValue;
                }
                bValue = bValue / 2;
            }
            return finalInstructions.TrackCardBackwards(findCard);
        }
    }

    static class Card
    {
        public static BigInteger Cut(BigInteger cardId, BigInteger deckSize, BigInteger cut)
        {
            return MathHelper.Mod(cardId - cut, deckSize);
        }

        public static BigInteger IntoNewStack(BigInteger cardId, BigInteger deckSize)
        {
            return deckSize - cardId - 1;
        }

        public static BigInteger WithIncrement(BigInteger cardId, BigInteger deckSize, BigInteger increment)
        {
            return MathHelper.Mod(cardId * increment, deckSize);
        }

        public static BigInteger ReverseCut(BigInteger cardId, BigInteger deckSize, BigInteger cut)
        {
            return (cardId + (deckSize + cut)) % deckSize;
        }

        public static BigInteger ReverseIntoNewStack(BigInteger cardId, BigInteger deckSize)
        {
            return IntoNewStack(cardId, deckSize);
        }

        public static BigInteger ReverseWithIncrement(BigInteger cardId, BigInteger deckSize, BigInteger increment)
        {
            return MathHelper.Mod(cardId * MathHelper.ModInv(increment, deckSize), deckSize);
        }
    }

    class Deck
    {
        List<BigInteger> cards;
        public int Count { get; private set; }

        public Deck(int size)
        {
            Count = size;
            cards = Enumerable.Range(0, Count).Select(x => (BigInteger)x).ToList();
        }

        public Deck(List<BigInteger> cards)
        {
            this.cards = cards;
            Count = cards.Count;
        }

        public void DealIntoNewStack()
        {
            BigInteger[] newCards = new BigInteger[Count];
            for (int id = 0; id < Count; id++)
            {
                int newId = (int)Card.IntoNewStack(id, Count);
                newCards[newId] = cards[id];
            }
            cards = newCards.ToList();
        }

        public void Cut(int cut)
        {
            BigInteger[] newCards = new BigInteger[Count];
            for (int id = 0; id < Count; id++)
            {
                int newId = (int)Card.Cut(id, Count, cut);
                newCards[newId] = cards[id];
            }
            cards = newCards.ToList();
        }

        public void DealWithIncrement(int increment)
        {
            BigInteger[] newCards = new BigInteger[Count];
            for (int id = 0; id < Count; id++)
            {
                int newId = (int)Card.WithIncrement(id, Count, increment);
                newCards[newId] = cards[id];
            }
            cards = newCards.ToList();
        }

        public void PerformShuffle(string technique, int by)
        {
            if (technique == "cut") Cut(by);
            if (technique == "increment") DealWithIncrement(by);
            if (technique == "newstack") DealIntoNewStack();
        }

        public void PerformShuffle(string instruction)
        {
            (string technique, int by) = ShuffleHelper.PrepareInstructions(instruction);
            PerformShuffle(technique, by);
        }

        public void PerformShuffle(IEnumerable<string> instructions)
        {
            foreach (string instruction in instructions)
            {
                PerformShuffle(instruction);
            }
        }

        public override string ToString()
        {
            return string.Join(',', cards);
        }
    }

    class ShuffleInstructions
    {
        public List<(string technique, BigInteger by)> Instructions { get; private set; }
        public BigInteger DeckCount { get; set; }
        public Dictionary<string, int> instCounts;

        public ShuffleInstructions(IEnumerable<string> instructions, BigInteger deckCount)
        {
            Instructions = instructions.Select(x => ShuffleHelper.PrepareInstructions(x)).Select(x => (x.technique, (BigInteger)x.by)).ToList();
            DeckCount = deckCount;
            InitializeInstructionCounts();
        }

        public ShuffleInstructions(ShuffleInstructions toBeCopied)
        {
            DeckCount = toBeCopied.DeckCount;
            Instructions = toBeCopied.Instructions.Select(ins => (ins.technique, ins.by)).ToList();
            InitializeInstructionCounts();
        }

        private void InitializeInstructionCounts()
        {
            instCounts = new Dictionary<string, int>
            {
                { "newstack", Instructions.Where(x => x.technique == "newstack").Count() },
                { "increment", Instructions.Where(x => x.technique == "increment").Count() },
                { "cut", Instructions.Where(x => x.technique == "cut").Count() }
            };
        }

        public void AddInstructions(IEnumerable<string> instructions)
        {
            var newInstructions = instructions.Select(x => ShuffleHelper.PrepareInstructions(x)).Select(x => (x.technique, (BigInteger)x.by));
            AddInstructions(newInstructions);
        }

        public void AddInstructions(IEnumerable<(string technique, BigInteger by)> instructions)
        {
            foreach (var inst in instructions)
            {
                AddInstruction(inst);
            }
        }

        public void AddInstructions(ShuffleInstructions otherInstructions)
        {
            AddInstructions(otherInstructions.Instructions.Select(x => (x.technique, x.by)));
        }

        public BigInteger TrackCard(BigInteger cardId)
        {
            foreach (var (technique, by) in Instructions)
            {
                if (technique == "increment")
                {
                    cardId = Card.WithIncrement(cardId, DeckCount, by);
                }
                if (technique == "cut")
                {
                    cardId = Card.Cut(cardId, DeckCount, by);
                }
                if (technique == "newstack")
                {
                    cardId = Card.IntoNewStack(cardId, DeckCount);
                }
            }
            return cardId;
        }

        public BigInteger TrackCardBackwards(BigInteger cardId)
        {
            var reversed = Instructions.Reverse<(string technique, BigInteger by)>();
            foreach (var (technique, by) in reversed)
            {
                if (technique == "increment")
                {
                    cardId = Card.ReverseWithIncrement(cardId, DeckCount, by);
                }
                if (technique == "cut")
                {
                    cardId = Card.ReverseCut(cardId, DeckCount, by);
                }
                if (technique == "newstack")
                {
                    cardId = Card.ReverseIntoNewStack(cardId, DeckCount);
                }
            }
            return cardId;
        }

        public void Compress()
        {
            //EliminateNewStackInstructions();
            CompressIncrements();
            CompressCuts();
        }

        public void CompressCuts()
        {
            int topCut = 0;
            while (instCounts["cut"] > 1)
            {
                topCut = MoveDownOrMergeTopCut(topCut);
            }
        }

        int MoveDownOrMergeTopCut(int topCut)
        {
            topCut = Instructions.FindIndex(topCut, x => x.technique == "cut");
            if (topCut == Instructions.Count) return topCut; //CompressCuts might end up in an endless loop if I messed up here, returning topCut would fix that but hide the problem
            var rewritten = ShuffleHelper.Rewrite(Instructions[topCut], Instructions[topCut + 1], DeckCount);
            ReplaceInstruction(topCut, rewritten[0]);
            if (rewritten.Count == 2)
            {
                ReplaceInstruction(topCut + 1, rewritten[1]);
            }
            else
            {
                if (rewritten.Count == 0) throw new Exception("HUHHH shouldn't happen");
                RemoveInstruction(topCut + 1);
            }
            return topCut; //Could update the topCut here before returning, would be slightly more efficient, 
                           //but requires the assumption that all rules have a Cut as the last result
                           //However we can guarantee that no Cut appeared above our topCut so this suffices for me
        }

        public void CompressIncrements()
        {
            int bottomInc = Instructions.Count - 1;
            while (instCounts["increment"] > 1 || instCounts["increment"] == 1 && Instructions[0].technique != "increment")
            {//Second half of the guard is to make it so this action also removes all NewStack Instructions
                bottomInc = MoveUpOrMergeBottomIncrement(bottomInc);
            }
        }

        private int MoveUpOrMergeBottomIncrement(int bottomInc)
        {
            bottomInc = Instructions.FindLastIndex(bottomInc, x => x.technique == "increment"); //StartIndex of FindLastIndex is the highest index (but first it visits)
            if (bottomInc == 0) return bottomInc;
            var rewritten = ShuffleHelper.Rewrite(Instructions[bottomInc - 1], Instructions[bottomInc], DeckCount);
            ReplaceInstruction(bottomInc - 1, rewritten[0]);
            if (rewritten.Count == 2)
            {
                ReplaceInstruction(bottomInc, rewritten[1]);
            }
            else
            {
                if (rewritten.Count == 0) throw new Exception("HUHHH shouldn't happen");
                RemoveInstruction(bottomInc);
            }
            return bottomInc; //Could update the topCut here before returning, would be slightly more efficient, 
                              //but requires the assumption that all rules have a Cut as the last result
                              //However we can guarantee that no Cut appeared above our topCut so this suffices for me
        }

        private void EliminateNewStackInstructions()
        {
            //If there are 2+ newstack instructions, 1+ and 1+ increment instructions we can eliminate some instructions
            while (instCounts["newstack"] > 1 || (instCounts["newstack"] > 0 && instCounts["increment"] > 0))
            {
                EliminateTopNewStack();
            }
        }

        private void EliminateTopNewStack()
        {
            int nsIndex = Instructions.FindIndex(x => x.technique == "newstack");
            //Look on either side for a Increment or NewStack Instruction
            (bool down, int steps) = FindNearest(nsIndex, new List<string> { "increment", "newstack" });
            while (steps > 0)
            {
                int top, bottom;
                if (down)
                {
                    top = nsIndex;
                    bottom = nsIndex + 1;
                }
                else
                {
                    top = nsIndex - 1;
                    bottom = nsIndex;
                }
                var rewritten = ShuffleHelper.Rewrite(Instructions[top], Instructions[bottom], DeckCount);
                if (rewritten.Count == 2)
                {
                    ReplaceInstruction(top, rewritten[0]);
                    ReplaceInstruction(bottom, rewritten[1]);
                }
                else
                {
                    if (rewritten.Count == 0)
                    {
                        //This should only happen for newstack newstack
                        RemoveInstruction(bottom);
                        RemoveInstruction(top);
                    }
                    else
                    {
                        throw new Exception("Excuse me what are you doing here???");
                    }
                }
                steps--;
                nsIndex = down ? nsIndex + 1 : nsIndex - 1;
            }
        }

        public (bool downwards, int steps) FindNearest(int startIndex, List<string> searchFor)
        {
            int steps = 1;
            while (startIndex + steps < Instructions.Count || startIndex - steps >= 0)
            {
                if (startIndex + steps < Instructions.Count)
                {
                    if (searchFor.Contains(Instructions[startIndex + steps].technique)) return (true, steps);
                }

                if (startIndex - steps >= 0)
                {
                    if (searchFor.Contains(Instructions[startIndex - steps].technique)) return (false, steps);
                }
                steps++;
            }
            throw new Exception("Trying to find something that doesn't exist");
        }

        private void ReplaceInstruction(int index, (string technique, BigInteger by) newInstruction)
        {
            var (technique, _) = Instructions[index];
            instCounts[technique]--;
            Instructions[index] = newInstruction;
            instCounts[newInstruction.technique]++;
        }

        private void RemoveInstruction(int index)
        {
            var (technique, _) = Instructions[index];
            instCounts[technique]--;
            Instructions.RemoveAt(index);
        }

        private void AddInstruction((string technique, BigInteger by) instruction)
        {
            Instructions.Add(instruction);
            instCounts[instruction.technique]++;
        }
    }

    static class ShuffleHelper
    {
        public static (string technique, int by) PrepareInstructions(string instruction)
        {
            string[] split = instruction.Split(' ');
            if (split[0] == "cut")
            {
                return ("cut", int.Parse(split[1]));
            }
            else
            {
                if (split[2] == "increment")
                {
                    return ("increment", int.Parse(split[3]));
                }
                else
                {
                    if (split[2] == "new")
                    {
                        return ("newstack", -1);
                    }
                    else

                    {
                        throw new Exception("Meep moop shouldn't be here");
                    }
                }
            }
        }

        public static List<(string type, BigInteger by)> Rewrite((string type, BigInteger by) top, (string type, BigInteger by) bottom, BigInteger deckCount)
        {
            if (top.type == bottom.type)
            {
                if (top.type == "cut")
                {
                    return new List<(string type, BigInteger by)> { (top.type, MathHelper.Mod(top.by + bottom.by, deckCount)) };
                }
                if (top.type == "increment")
                {
                    return new List<(string type, BigInteger by)> { (top.type, MathHelper.Mod(top.by * bottom.by, deckCount)) };
                }
                if (top.type == "newstack")
                {
                    return new List<(string type, BigInteger by)>();
                }
            }

            if (top.type == "cut")
            {
                if (bottom.type == "newstack")
                {
                    return new List<(string type, BigInteger by)> { bottom, (top.type, MathHelper.Mod(-top.by, deckCount)) };
                }
                if (bottom.type == "increment")
                {
                    return new List<(string type, BigInteger by)> { bottom, (top.type, MathHelper.Mod(bottom.by * top.by, deckCount)) };
                }
            }

            if (top.type == "newstack")
            {
                if (bottom.type == "cut")
                {
                    return new List<(string type, BigInteger by)> { (bottom.type, MathHelper.Mod(-bottom.by, deckCount)), top };
                }
                if (bottom.type == "increment")
                {
                    return new List<(string type, BigInteger by)> { (bottom.type, MathHelper.Mod(-bottom.by, deckCount)), ("cut", bottom.by) };
                }
            }

            if (top.type == "increment")
            {
                if (bottom.type == "newstack")
                {
                    return new List<(string type, BigInteger by)> { (top.type, MathHelper.Mod(-top.by, deckCount)), ("cut", 1) };
                }
                //Not implementing it for Cut as second argument, as that one is mathematically problematic
            }

            throw new Exception($"Rewrite rule not implemented for: \n ({top.type},{top.by}) \n ({bottom.type},{bottom.by})");
        }
    }
}


