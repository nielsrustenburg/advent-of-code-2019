using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;
using AoC.Common;
using AoC.Computers;
using AoC.Utils;

namespace AoC.Day15
{
    class Solver : PuzzleSolver
    {
        List<BigInteger> program;
        RepairBot robot;

        public Solver() : this(Input.InputMode.Embedded, "input")
        {
        }

        public Solver(Input.InputMode mode, string input) : base(mode, input)
        {
        }

        protected override void ParseInput(string input)
        {
            program = InputParser<BigInteger>.ParseCSVLine(input, BigInteger.Parse).ToList();
        }

        protected override void PrepareSolution()
        {
            robot = new RepairBot(program);
            robot.ExploreMaze();
        }

        protected override void SolvePartOne()
        {
            var (totalSteps, poi) = robot.mazeMap.FloodFillDistanceFinder(0, 0, new List<string> { "O" });
            resultPartOne = poi.Where(x => x.tile == "O").First().distance.ToString();
        }

        protected override void SolvePartTwo()
        {
            var (oxyX,oxyY) = ((int,int)) robot.mazeMap.FindFirstMatchingTile("O");
            var (totalSteps, _) = robot.mazeMap.FloodFillDistanceFinder(oxyX,oxyY, new List<string>());
            resultPartTwo = totalSteps.ToString();
        }
    }

    class RepairBot
    {
        IntCode brain;
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool Finished { get; private set; }
        public Grid<string> mentalMap;
        public Maze<string> mazeMap;

        public RepairBot(List<BigInteger> programming)
        {
            brain = new IntCode(programming);
            X = 0;
            Y = 0;
            mentalMap = new Grid<string>(" ", true);
            mazeMap = new Maze<string>("\u2588", new List<string> { "#", "O", "D" }, true);
            Finished = false;
        }

        public void ExploreMaze(bool draw = false)
        {
            List<Direction> validDirections = new List<Direction> { Direction.North, Direction.South, Direction.West, Direction.East };
            List<Direction> pathTaken = new List<Direction>();

            if (draw) Console.Clear();
            do
            {
                //Explore all unknown surrounding tiles
                var tileKnowledge = LookAround();
                for (int i = 0; i < tileKnowledge.Count; i++)
                {
                    if (!tileKnowledge[i])
                    {
                        ExploreNeighbours(validDirections[i]);
                    }
                }
                //Pick the first unexplored non-wall neighbour if any exist and continue exploring
                //If none exist backtrack your path until one does exist or until we are back at our starting location in which case we are done
                tileKnowledge = LookAround();
                if (tileKnowledge.Where(x => !x).Any())
                {
                    Direction moveDirection = validDirections[tileKnowledge.FindIndex(x => !x)];
                    Move(moveDirection, true);
                    pathTaken.Add(moveDirection);
                }
                else
                {
                    Move(pathTaken.Last().Opposite(), true);
                    pathTaken.RemoveAt(pathTaken.Count - 1);
                }

                if (draw)
                {
                    Console.SetCursorPosition(0, 0);
                    var lines = mentalMap.RowsAsStrings();
                    foreach (string line in lines)
                    {
                        Console.WriteLine(line);
                    }
                }

            } while (pathTaken.Any());
        }

        public bool Move(Direction direction, bool closePrev = false)
        {
            //Sadly AoC uses different int values than I do
            int dirCode = 4;
            if (direction == Direction.North) dirCode = 1;
            if (direction == Direction.South) dirCode = 2;
            if (direction == Direction.West) dirCode = 3;

            int response = (int)brain.Run(new List<BigInteger> { dirCode })[0];
            if (response > 0)
            {
                if (mentalMap[X, Y] == "D")
                {
                    SetMapTiles(X, Y, closePrev ? "#" : " ");
                }
                (X, Y) = DirectionHelper.NeighbourInDirection(direction, X, Y);

                Finished = response == 2;
                SetMapTiles(X, Y, Finished ? "O" : "D");
                return true;
            }
            else
            {
                (int mentalX, int mentalY) = DirectionHelper.NeighbourInDirection(direction, X, Y);
                SetMapTiles(mentalX, mentalY, "\u2588");
                return false;
            }
        }

        public void SetMapTiles(int x, int y, string tile)
        {
            mentalMap[x, y] = tile;
            mazeMap[x, y] = tile;
        }

        public void ExploreNeighbours(Direction dir)
        {
            if (Move(dir)) Move(dir.Opposite());
        }

        public List<bool> LookAround()
        {
            string north = mentalMap[X, Y + 1];
            string south = mentalMap[X, Y - 1];
            string west = mentalMap[X - 1, Y];
            string east = mentalMap[X + 1, Y];
            List<string> knownTiles = new List<string> { "\u2588", "#", "O" };
            return new List<bool> { knownTiles.Contains(north),
                                    knownTiles.Contains(south),
                                    knownTiles.Contains(west),
                                    knownTiles.Contains(east) };
        }
    }
}
