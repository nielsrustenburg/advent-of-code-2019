﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;
using AoC.Common;
using AoC.Utils;

namespace AoC.Day24
{
    class Solver : PuzzleSolver
    {
        string[] rows;
        public Solver() : this(Input.InputMode.Embedded, "input")
        {
        }

        public Solver(Input.InputMode mode, string input) : base(mode, input)
        {
        }

        protected override void ParseInput(string input)
        {
            rows = InputParser.Split(input).ToArray();
        }

        protected override void PrepareSolution()
        {
            //no common prep
        }

        protected override void SolvePartOne()
        {
            GameOfEris goe = new GameOfEris(rows);
            resultPartOne = goe.Run().ToString();
        }

        protected override void SolvePartTwo()
        {
            RecursiveGameOfEris rgoe = new RecursiveGameOfEris(rows);
            resultPartTwo = rgoe.Run(200).ToString();
        }
    }

    class GameOfEris
    {
        ErisGrid grid;
        public int Width { get; private set; }
        public int Height { get; private set; }
        HashSet<string> prevStates;
        public GameOfEris(IEnumerable<string> input)
        {
            grid = new ErisGrid(input);
            Width = grid.Width;
            Height = grid.Height;
            prevStates = new HashSet<string>();
        }

        public BigInteger Run()
        {
            while (true)
            {
                bool done = UpdatePrevStates();
                if (done)
                {
                    return BioDiversityScore();
                }
                else
                {
                    GoNextState();
                }
            }
        }

        private void GoNextState()
        {
            ErisGrid newGrid = new ErisGrid();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    UpdateTile(x, y, newGrid);
                }
            }
            grid = newGrid;
        }

        private void UpdateTile(int x, int y, ErisGrid newGrid)
        {
            bool bug = grid.GetTile(x, y) == "#";
            int amountOfNeighbourBugs = grid.GetNeighbours(x, y).Values.Count(t => t == "#");
            string tile = amountOfNeighbourBugs == 1 ||
                          (amountOfNeighbourBugs == 2 && !bug) ? "#" : ".";
            newGrid.SetTile(x, y, tile);
        }

        private bool UpdatePrevStates()
        {
            string current = ToString();
            if (prevStates.Contains(current)) return true;
            prevStates.Add(current);
            return false;
        }

        private BigInteger BioDiversityScore()
        {
            return (BigInteger)Enumerable.Range(0, Width * Height).Where(i => grid.GetTile(i % 5, i / 5) == "#").Select(t => Math.Pow(2, t)).Aggregate((double)0, (a, b) => a + b);
        }

        public override string ToString()
        {
            return string.Join("", Enumerable.Range(0, Width * Height).Select(i => grid.GetTile(i % 5, i / 5)));
        }
    }

    class RecursiveGameOfEris
    {
        List<ErisGrid> grids;
        public int Width { get; private set; }
        public int Height { get; private set; }

        public RecursiveGameOfEris(IEnumerable<string> input)
        {
            ErisGrid middleGrid = new ErisGrid(input);
            Width = middleGrid.Width;
            Height = middleGrid.Height;
            grids = new List<ErisGrid> { middleGrid };
        }

        public int Run(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                GoNextState();
            }
            return CountBugs();
        }

        public int CountBugs()
        {
            int count = 0;
            foreach (ErisGrid g in grids)
            {
                count += g.Count("#");
            }
            return count;
        }

        public void GoNextState()
        {
            //Add the outer layer first, inner layer last (d == -1 / d == grids.Count)
            List<ErisGrid> newGrids = new List<ErisGrid>();
            for (int d = -1; d <= grids.Count; d++)
            {
                ErisGrid ng = new ErisGrid();
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        if (x != 2 || y != 2)//Never change the center tile
                        {
                            ng.GetTile(x, y);
                            ng.SetTile(x, y, GetUpdatedTileStatus(x, y, d));
                        }
                    }
                }
                newGrids.Add(ng);
            }
            grids = newGrids;
        }

        private string GetUpdatedTileStatus(int x, int y, int depth)
        {
            //If in a new grid OR previously not a bug: 1-2 neighbours --> bug
            bool prevBug = depth != -1 && depth != grids.Count && grids[depth].GetTile(x, y) == "#";
            int amountOfNeighbourBugs = GetNeighbours(x, y, depth).Count(nb => nb == "#");
            if (prevBug)
            {
                return amountOfNeighbourBugs == 1 ? "#" : ".";
            }
            else
            {
                return amountOfNeighbourBugs == 1 || amountOfNeighbourBugs == 2 ? "#" : ".";
            }
        }

        public List<string> GetNeighbours(int x, int y, int depth)
        {
            List<string> nbors = new List<string>();
            nbors.AddRange(GetNorthNeighbours(x, y, depth));
            nbors.AddRange(GetEastNeighbours(x, y, depth));
            nbors.AddRange(GetSouthNeighbours(x, y, depth));
            nbors.AddRange(GetWestNeighbours(x, y, depth));
            return nbors;
        }

        public List<string> GetNorthNeighbours(int x, int y, int depth)
        {
            //If on upper border return 2,1 (8) at surrounding level (if that level existed previous step)
            if (y == 0)
            {
                if (depth > 0)
                {
                    return new List<string> { grids[depth - 1].GetTile(2, 1) };
                }
                else
                {
                    return new List<string>();
                }
            }

            //If on bottom center return bottom border at encased level (if that level existed previous step)
            if (x == 2 && y == 3)
            {
                if (depth < grids.Count - 1)
                {
                    return Enumerable.Range(0, Width).Select(xcor => grids[depth + 1].GetTile(xcor, Height - 1)).ToList();
                }
                else
                {
                    return new List<string>();
                }
            }

            if (depth == -1 || depth == grids.Count) return new List<string>();
            //Return the one directly north of here
            return new List<string> { grids[depth].GetTile(x, y - 1) };
        }

        public List<string> GetSouthNeighbours(int x, int y, int depth)
        {
            //If on upper border return 2,3 (18) at surrounding level (if that level existed previous step)
            if (y == 4)
            {
                if (depth > 0)
                {
                    return new List<string> { grids[depth - 1].GetTile(2, 3) };
                }
                else
                {
                    return new List<string>();
                }
            }

            //If on top center return top border at encased level (if that level existed previous step)
            if (x == 2 && y == 1)
            {
                if (depth < grids.Count - 1)
                {
                    return Enumerable.Range(0, Width).Select(xcor => grids[depth + 1].GetTile(xcor, 0)).ToList();
                }
                else
                {
                    return new List<string>();
                }
            }

            if (depth == -1 || depth == grids.Count) return new List<string>();
            //Return the one directly south of here
            return new List<string> { grids[depth].GetTile(x, y + 1) };
        }

        public List<string> GetWestNeighbours(int x, int y, int depth)
        {
            //If on left border return 1,2 (12) at surrounding level (if that level existed previous step)
            if (x == 0)
            {
                if (depth > 0)
                {
                    return new List<string> { grids[depth - 1].GetTile(1, 2) };
                }
                else
                {
                    return new List<string>();
                }
            }

            //If on right center return right border at encased level (if that level existed previous step)
            if (x == 3 && y == 2)
            {
                if (depth < grids.Count - 1)
                {
                    return Enumerable.Range(0, Width).Select(ycor => grids[depth + 1].GetTile(Width - 1, ycor)).ToList();
                }
                else
                {
                    return new List<string>();
                }
            }

            if (depth == -1 || depth == grids.Count) return new List<string>();
            //Return the one directly west of here
            return new List<string> { grids[depth].GetTile(x - 1, y) };
        }

        public List<string> GetEastNeighbours(int x, int y, int depth)
        {
            //If on left border return 3,2 (14) at surrounding level (if that level existed previous step)
            if (x == 4)
            {
                if (depth > 0)
                {
                    return new List<string> { grids[depth - 1].GetTile(3, 2) };
                }
                else
                {
                    return new List<string>();
                }
            }

            //If on left center return left border at encased level (if that level existed previous step)
            if (x == 1 && y == 2)
            {
                if (depth < grids.Count - 1)
                {
                    return Enumerable.Range(0, Width).Select(ycor => grids[depth + 1].GetTile(0, ycor)).ToList();
                }
                else
                {
                    return new List<string>();
                }
            }

            if (depth == -1 || depth == grids.Count) return new List<string>();
            //Return the one directly east of here
            return new List<string> { grids[depth].GetTile(x + 1, y) };
        }
    }

    class ErisGrid : Grid<string>
    {

        public ErisGrid() : base(".")
        {

        }

        public ErisGrid(IEnumerable<string> input) : this()
        {
            int nrow = 0;
            foreach (string row in input)
            {
                int ncol = 0;
                foreach (char tile in row)
                {
                    GetTile(ncol, nrow);
                    SetTile(ncol, nrow, tile.ToString());
                    ncol++;
                }
                nrow++;
            }
        }

        public virtual List<string> Neighbours(int id)
        {
            int x = id % Width;
            int y = id / Width;
            return Neighbours(x, y);
        }

        public virtual List<string> Neighbours(int x, int y)
        {
            return GetNeighbours(x, y).Values.ToList();
        }
    }
}