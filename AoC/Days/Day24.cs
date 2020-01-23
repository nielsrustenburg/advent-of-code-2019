using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

namespace AoC
{
    static class Day24
    {
        public static BigInteger SolvePartOne()
        {
            string[] input = InputReader.StringsFromTxt("d24input.txt");
            GameOfEris goe = new GameOfEris(input);
            return goe.Run();
        }

        public static int SolvePartTwo()
        {
            string[] input = InputReader.StringsFromTxt("d24input.txt");
            RecursiveGameOfEris rgoe = new RecursiveGameOfEris(input);
            return rgoe.Run(200);
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
            Width = grid.Width();
            Height = grid.Height();
            prevStates = new HashSet<string>();
        }

        public BigInteger Run()
        {
            while (true)
            {
                Console.SetCursorPosition(0, 0);
                List<string> bob = grid.GetImageStrings();
                foreach (var b in bob)
                {
                    Console.WriteLine(b);
                }
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
            newGrid.GetColorAt(x, y); //might be unnecessary but recall my Grid implementation being sucky and needing this
            bool bug = grid.GetColorAt(x, y) == "#";
            List<string> nb = grid.Neighbours(x, y);
            int neighbouringBugs = nb.Where(t => t == "#").Count();
            string tile;
            if (bug)
            {
                tile = neighbouringBugs == 1 ? "#" : ".";
            }
            else
            {
                tile = neighbouringBugs == 1 || neighbouringBugs == 2 ? "#" : ".";
            }
            newGrid.SetColorAt(x, y, tile);
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
            return (BigInteger)Enumerable.Range(0, Width * Height).Where(i => grid.GetColorAt(i % 5, i / 5) == "#").Select(t => Math.Pow(2, t)).Aggregate((double)0, (a, b) => a + b);
        }

        public override string ToString()
        {
            return string.Join("", Enumerable.Range(0, Width * Height).Select(i => grid.GetColorAt(i % 5, i / 5)));
        }
    }

    class RecursiveGameOfEris
    {
        //DONT FORGET TO MAKE THE CENTER A ?????
        List<ErisGrid> grids;
        public int Width { get; private set; }
        public int Height { get; private set; }

        public RecursiveGameOfEris(IEnumerable<string> input)
        {
            ErisGrid middleGrid = new ErisGrid(input);
            Width = middleGrid.Width();
            Height = middleGrid.Height();
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
                count += g.FindAll("#").Count;
            }
            return count;
        }

        public void GoNextState()
        {
            //Larger first, smaller last
            List<ErisGrid> newGrids = new List<ErisGrid>();
            for (int d = -1; d <= grids.Count; d++)
            {
                ErisGrid ng = new ErisGrid();
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        if (x != 2 || y != 2)//Never change the center
                        {
                            ng.GetColorAt(x, y);
                            ng.SetColorAt(x, y, GetUpdatedTileStatus(x, y, d));
                        }
                    }
                }
                newGrids.Add(ng);
            }
            grids = newGrids;
        }

        private string GetUpdatedTileStatus(int x, int y, int depth)
        {
            bool prevBug = depth != -1 && depth != grids.Count && grids[depth].GetColorAt(x, y) == "#";
            List<string> nbours = GetNeighbours(x, y, depth);
            int neighbugs = nbours.Where(nb => nb == "#").Count();
            //If in a new grid OR previously not a bug: 1-2 neighbours --> bug
            if (prevBug)
            {
                return neighbugs == 1 ? "#" : ".";
            }
            else
            {
                return neighbugs == 1 || neighbugs == 2 ? "#" : ".";
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
                    return new List<string> { grids[depth - 1].GetColorAt(2, 1) };
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
                    return Enumerable.Range(0, Width).Select(xcor => grids[depth + 1].GetColorAt(xcor, Height - 1)).ToList();
                }
                else
                {
                    return new List<string>();
                }
            }

            if (depth == -1 || depth == grids.Count) return new List<string>();
            //Return the one directly north of here
            return new List<string> { grids[depth].GetColorAt(x, y - 1) };
        }

        public List<string> GetSouthNeighbours(int x, int y, int depth)
        {
            //If on upper border return 2,3 (18) at surrounding level (if that level existed previous step)
            if (y == 4)
            {
                if (depth > 0)
                {
                    return new List<string> { grids[depth - 1].GetColorAt(2, 3) };
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
                    return Enumerable.Range(0, Width).Select(xcor => grids[depth + 1].GetColorAt(xcor, 0)).ToList();
                }
                else
                {
                    return new List<string>();
                }
            }

            if (depth == -1 || depth == grids.Count) return new List<string>();
            //Return the one directly south of here
            return new List<string> { grids[depth].GetColorAt(x, y + 1) };
        }

        public List<string> GetWestNeighbours(int x, int y, int depth)
        {
            //If on left border return 1,2 (12) at surrounding level (if that level existed previous step)
            if (x == 0)
            {
                if (depth > 0)
                {
                    return new List<string> { grids[depth - 1].GetColorAt(1, 2) };
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
                    return Enumerable.Range(0, Width).Select(ycor => grids[depth + 1].GetColorAt(Width - 1, ycor)).ToList();
                }
                else
                {
                    return new List<string>();
                }
            }

            if (depth == -1 || depth == grids.Count) return new List<string>();
            //Return the one directly west of here
            return new List<string> { grids[depth].GetColorAt(x - 1, y) };
        }

        public List<string> GetEastNeighbours(int x, int y, int depth)
        {
            //If on left border return 3,2 (14) at surrounding level (if that level existed previous step)
            if (x == 4)
            {
                if (depth > 0)
                {
                    return new List<string> { grids[depth - 1].GetColorAt(3, 2) };
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
                    return Enumerable.Range(0, Width).Select(ycor => grids[depth + 1].GetColorAt(0, ycor)).ToList();
                }
                else
                {
                    return new List<string>();
                }
            }

            if (depth == -1 || depth == grids.Count) return new List<string>();
            //Return the one directly east of here
            return new List<string> { grids[depth].GetColorAt(x + 1, y) };
        }
    }

    class ErisGrid : ColorGrid
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

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
                    GetColorAt(ncol, nrow);
                    SetColorAt(ncol, nrow, tile.ToString());
                    ncol++;
                }
                nrow++;
            }
            Width = Width();
            Height = Height();
        }

        public virtual List<string> Neighbours(int id)
        {
            int x = id % Width;
            int y = id / Width;
            return Neighbours(x, y);
        }

        public virtual List<string> Neighbours(int x, int y)
        {
            return base.Neighbours(x, y).Values.ToList();
        }
    }
}

