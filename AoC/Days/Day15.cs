using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

namespace AoC
{
    static class Day15
    {
        public static int SolvePartOne()
        {
            string input = InputReader.StringsFromTxt("d15input.txt")[0];
            List<BigInteger> program = input.Split(',').Select(x => BigInteger.Parse(x)).ToList();
            RepairBot rb = new RepairBot(program);

            rb.ExploreMaze();

            var oxygenSearch = rb.mazeMap.FloodFillDistanceFinder(0, 0, new List<string> { "O" });
            var stepsToOxygen = oxygenSearch.poi.Where(x => x.tile == "O").First().distance;
            
            return stepsToOxygen;
        }

        public static int SolvePartTwo()
        {
            string input = InputReader.StringsFromTxt("d15input.txt")[0];
            List<BigInteger> program = input.Split(',').Select(x => BigInteger.Parse(x)).ToList();
            RepairBot rb = new RepairBot(program);

            rb.ExploreMaze();

            var oxy = rb.mazeMap.FindFirstMatchingTile("O");
            var oxygenFlooding = rb.mazeMap.FloodFillDistanceFinder(oxy.x, oxy.y, new List<string>());

            return oxygenFlooding.totalSteps;
        }
    }

    class RepairBot
    {
        BigIntCode brain;
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool Finished { get; private set; }
        public Grid<string> mentalMap;
        public Maze<string> mazeMap;

        public RepairBot(List<BigInteger> programming)
        {
            brain = new BigIntCode(programming);
            X = 0;
            Y = 0;
            mentalMap = new Grid<string>(" ");
            mazeMap = new Maze<string>("\u2588", new List<string> { "#", "O", "D" });
            Finished = false;
        }

        public void ExploreMaze(bool draw = false)
        {
            List<Direction> validDirections = new List<Direction> { Direction.North, Direction.South, Direction.West, Direction.East };
            List<Direction> pathTaken = new List<Direction>();

            if(draw) Console.Clear();
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
                    var lines = mentalMap.RowsAsStrings(true);
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
                if (mentalMap.GetTile(X, Y) == "D")
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
            mentalMap.SetTile(x, y, tile);
            mazeMap.SetTile(x, y, tile);
        }

        public void ExploreNeighbours(Direction dir)
        {
            if (Move(dir)) Move(dir.Opposite());
        }

        public List<bool> LookAround()
        {
            string north = mentalMap.GetTile(X, Y + 1);
            string south = mentalMap.GetTile(X, Y - 1);
            string west = mentalMap.GetTile(X - 1, Y);
            string east = mentalMap.GetTile(X + 1, Y);
            List<string> knownTiles = new List<string> { "\u2588", "#", "O" };
            return new List<bool> { knownTiles.Contains(north),
                                    knownTiles.Contains(south),
                                    knownTiles.Contains(west),
                                    knownTiles.Contains(east) };
        }
    }
}

