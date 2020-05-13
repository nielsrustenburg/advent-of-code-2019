using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;
using AoC.Common;
using AoC.Utils;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Collections;

namespace AoC.Day24
{
    class Solver : PuzzleSolver
    {
        string sanitizedInput;
        public Solver() : this(Input.InputMode.Embedded, "input")
        {
        }

        public Solver(Input.InputMode mode, string input) : base(mode, input)
        {
        }

        protected override void ParseInput(string input)
        {
            sanitizedInput = new string(input.Where(c => c == '#' || c == '.').ToArray());
        }

        protected override void PrepareSolution()
        {
            //no common prep
        }

        protected override void SolvePartOne()
        {
            resultPartOne = GetFirstRepeatedBiodiversity().ToString();
        }

        protected override void SolvePartTwo()
        {
            var centerGrid = new ErisGrid(sanitizedInput, true);
            centerGrid.DoTimeSteps(200);
            resultPartTwo = centerGrid.CountAllBugs(true).ToString();
        }

        public int GetFirstRepeatedBiodiversity()
        {
            var grid = new ErisGrid(sanitizedInput, false);
            var observedBiodiversities = new HashSet<int>() { grid.GetBiodiversity() };
            while (true)
            {
                grid.Step();
                var biodiversity = grid.GetBiodiversity();
                if (observedBiodiversities.Contains(biodiversity)) return biodiversity;
                observedBiodiversities.Add(biodiversity);
            }
        }

        internal class ErisGrid
        {
            BitArray state;
            ErisGrid gridInsideThis;
            ErisGrid gridOutsideThis;
            bool isRecursiveGrid;

            public ErisGrid(bool recursive)
            {
                state = new BitArray(25);
                isRecursiveGrid = recursive;
            }

            public ErisGrid(string hashesAndDots, bool recursive)
            {
                if (hashesAndDots.Length != 25) throw new ArgumentException("string representation must contain exactly 25 characters!");
                state = new BitArray(hashesAndDots.Select(c => c == '#').ToArray());
                isRecursiveGrid = recursive;
                if (isRecursiveGrid) state[12] = false; //make sure center is empty (shouldn't be necessary if input is valid though)
            }

            public ErisGrid(ErisGrid other, bool thisInsideOther) : this(true)
            {
                if (thisInsideOther) gridOutsideThis = other;
                else gridInsideThis = other;
            }

            public int GetBiodiversity()
            {
                int biodiversity = 0;
                int bitvalue = 1;
                for (int bit = 0; bit < state.Length; bit++)
                {
                    if (state[bit]) biodiversity += bitvalue;
                    bitvalue = bitvalue << 1;
                }
                return biodiversity;
            }

            public int CountAllBugs(bool recursive)
            {
                if (recursive)
                {
                    int count = 0;
                    var currentGrid = FindOutermostGrid();
                    while(currentGrid != null)
                    {
                        count += currentGrid.CountAllBugs(false);
                        currentGrid = currentGrid.gridInsideThis;
                    }
                    return count;
                }
                else
                {
                    int count = 0;
                    for (int i = 0; i < state.Length; i++)
                    {
                        if (state[i]) count++;
                    }
                    return count;
                }
            }

            public IEnumerable<string> GetPrintableState()
            {
                for (int y = 0; y < 5; y++)
                {
                    var sb = new StringBuilder();
                    for (int x = 0; x < 5; x++)
                    {
                        int index = y * 5 + x;
                        if (!isRecursiveGrid || index != 12)
                        {
                            sb.Append(state[index] ? '#' : '.');
                        } else
                        {
                            sb.Append('?');
                        }
                    }
                    yield return sb.ToString();
                }
            }

            public void DoTimeSteps(int numMinutes)
            {
                for (int n = 0; n < numMinutes; n++)
                {
                    Step();
                }
            }

            public void Step()
            {
                if (isRecursiveGrid)
                {
                    //add grids to current outers
                    var currentOuter = FindOutermostGrid();
                    var currentInner = FindInnermostGrid();
                    var newOuter = new ErisGrid(currentOuter, false);
                    currentOuter.gridOutsideThis = newOuter;
                    var newInner = new ErisGrid(currentInner, true);
                    currentInner.gridInsideThis = newInner;

                    //determine which indices to update for every grid
                    var bitsToFlipPerGrid = new List<(ErisGrid, IEnumerable<int>)>();
                    var currentGrid = newOuter;
                    while (currentGrid != null)
                    {
                        bitsToFlipPerGrid.Add((currentGrid, currentGrid.DetermineWhichBitsToFlip()));
                        currentGrid = currentGrid.gridInsideThis;
                    }

                    //update indices where necessary
                    foreach (var (grid, indices) in bitsToFlipPerGrid)
                    {
                        foreach (var index in indices)
                        {
                            grid.state[index] = !grid.state[index];
                        }
                    }

                    //delete empty grids on the edges
                    currentGrid = newOuter;
                    while (currentGrid != null && currentGrid.GetBiodiversity() == 0)
                    {
                        currentGrid = currentGrid.gridInsideThis;
                        if (currentGrid != null)
                        {
                            currentGrid.gridOutsideThis.gridInsideThis = null;
                            currentGrid.gridOutsideThis = null;
                        }
                    }
                    currentGrid = newInner;
                    while (currentGrid != null && currentGrid.GetBiodiversity() == 0)
                    {
                        currentGrid = currentGrid.gridOutsideThis;
                        if (currentGrid != null)
                        {
                            currentGrid.gridInsideThis.gridOutsideThis = null;
                            currentGrid.gridInsideThis = null;
                        }
                    }
                }
                else
                {
                    foreach (var index in DetermineWhichBitsToFlip())
                    {
                        state[index] = !state[index];
                    }
                }
            }

            private List<int> DetermineWhichBitsToFlip()
            {
                List<int> bitsToFlip = new List<int>();
                for (int i = 0; i < 25; i++)
                {
                    if (!isRecursiveGrid || i != 12) //don't update middle if recursive grid
                    {
                        int amountOfNeighbourBugs = CountNeighbourBugs(i);
                        if (state[i])
                        {
                            if (amountOfNeighbourBugs != 1) bitsToFlip.Add(i);
                        }
                        else
                        {
                            if (amountOfNeighbourBugs == 1 || amountOfNeighbourBugs == 2) bitsToFlip.Add(i);
                        }
                    }
                }
                return bitsToFlip;
            }

            private int CountNeighbourBugs(int index)
            {
                int bugCount = GetDirectNeighbourIndices(index).Count(i => state[i]);
                if (isRecursiveGrid)
                {
                    bool onWestOuterEdge = index % 5 == 0;
                    bool onEastOuterEdge = index % 5 == 4;
                    bool onNorthOuterEdge = index < 5;
                    bool onSouthOuterEdge = index > 19;
                    bool northInner = index == 7;
                    bool westInner = index == 11;
                    bool eastInner = index == 13;
                    bool southInner = index == 17;
                    //Count relevant bugs in neighbouring grids as well + directNeighbourBugCount
                    if (gridInsideThis != null)
                    {
                        if (northInner) bugCount += gridInsideThis.CountEdgeBugs(false, Direction.North);
                        else if (westInner) bugCount += gridInsideThis.CountEdgeBugs(false, Direction.West);
                        else if (eastInner) bugCount += gridInsideThis.CountEdgeBugs(false, Direction.East);
                        else if (southInner) bugCount += gridInsideThis.CountEdgeBugs(false, Direction.South);
                    }

                    if (gridOutsideThis != null)
                    {
                        if (onWestOuterEdge) bugCount += gridOutsideThis.CountEdgeBugs(true, Direction.West);
                        else if (onEastOuterEdge) bugCount += gridOutsideThis.CountEdgeBugs(true, Direction.East);
                        if (onNorthOuterEdge) bugCount += gridOutsideThis.CountEdgeBugs(true, Direction.North);
                        else if (onSouthOuterEdge) bugCount += gridOutsideThis.CountEdgeBugs(true, Direction.South);
                    }
                }
                return bugCount;
            }

            private int CountEdgeBugs(bool inner, Direction dir)
            {
                if (inner)
                {
                    if (dir == Direction.North) return state[7] ? 1 : 0;
                    if (dir == Direction.East) return state[13] ? 1 : 0;
                    if (dir == Direction.South) return state[17] ? 1 : 0;
                    if (dir == Direction.West) return state[11] ? 1 : 0;
                }
                else
                {
                    var zeroToFour = Enumerable.Range(0, 5);
                    if (dir == Direction.North) return zeroToFour.Count(n => state[n]);
                    if (dir == Direction.East) return zeroToFour.Count(n => state[(n * 5) + 4]);
                    if (dir == Direction.South) return zeroToFour.Count(n => state[n + 20]);
                    if (dir == Direction.West) return zeroToFour.Count(n => state[n * 5]);
                }
                throw new Exception("should never reach this");
            }

            private IEnumerable<int> GetDirectNeighbourIndices(int index)
            {
                var west = index - 1;
                var east = index + 1;
                var north = index - 5;
                var south = index + 5;
                if (index % 5 != 0) yield return west;
                if (index % 5 != 4) yield return east;
                if (north >= 0) yield return north;
                if (south < 25) yield return south;
            }

            private ErisGrid FindInnermostGrid()
            {
                if (gridInsideThis == null) return this;
                else return gridInsideThis.FindInnermostGrid();
            }

            private ErisGrid FindOutermostGrid()
            {
                if (gridOutsideThis == null) return this;
                else return gridOutsideThis.FindOutermostGrid();
            }
        }
    }
}
