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
            string input = InputReader.StringFromLine("d15input.txt");
            List<BigInteger> program = input.Split(',').Select(x => BigInteger.Parse(x)).ToList();
            RepairBot rb = new RepairBot(program);

            rb.ExploreMaze();

            var oxygenSearch = rb.mazeMap.FloodFillDistanceFinder(0, 0, new List<string> { "O" });
            var stepsToOxygen = oxygenSearch.poi.Where(x => x.tile == "O").First().distance;
            
            return stepsToOxygen;
        }

        public static int SolvePartTwo()
        {
            string input = InputReader.StringFromLine("d15input.txt");
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
        public Doolhof<string> mazeMap;

        public RepairBot(List<BigInteger> programming)
        {
            brain = new BigIntCode(programming);
            X = 0;
            Y = 0;
            mentalMap = new Grid<string>(" ");
            mazeMap = new Doolhof<string>("\u2588", new List<string> { "#", "O", "D" });
            Finished = false;
        }

        public void ExploreMaze(bool draw = false)
        {
            List<string> dirs = new List<string> { "north", "south", "west", "east" };
            List<string> path = new List<string>();
            if(draw) Console.Clear();
            do
            {
                //Explore all unknown surrounding tiles
                var tileKnowledge = LookAround();
                for (int i = 0; i < tileKnowledge.Count; i++)
                {
                    if (!tileKnowledge[i])
                    {
                        ExploreNeighbours(dirs[i]);
                    }
                }
                //Pick the first unexplored non-wall neighbour if any exist and continue exploring
                //If none exist backtrack your path until one does exist or until we are back at our starting location in which case we are done
                tileKnowledge = LookAround();
                if (tileKnowledge.Where(x => !x).Any())
                {
                    string movedir = dirs[tileKnowledge.FindIndex(x => !x)];
                    Move(movedir, true);
                    path.Add(movedir);
                }
                else
                {
                    Move(OppositeDir(path.Last()), true);
                    path.RemoveAt(path.Count - 1);
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

            } while (path.Any());
        }

        public bool Move(string direction, bool closePrev = false)
        {
            int dircode = 1;
            if (direction == "south") dircode = 2;
            if (direction == "west") dircode = 3;
            if (direction == "east") dircode = 4;

            int response = (int)brain.Run(new List<BigInteger> { dircode })[0];
            if (response > 0)
            {
                if (mentalMap.GetTile(X, Y) == "D")
                {
                    SetMapTiles(X, Y, closePrev ? "#" : " ");
                }
                if (direction == "north") Y++;
                if (direction == "south") Y--;
                if (direction == "west") X--;
                if (direction == "east") X++;

                Finished = response == 2;
                SetMapTiles(X, Y, Finished ? "O" : "D");
                return true;
            }
            else
            {
                int mentalx = X;
                int mentaly = Y;
                if (direction == "north") mentaly++;
                if (direction == "south") mentaly--;
                if (direction == "west") mentalx--;
                if (direction == "east") mentalx++;
                SetMapTiles(mentalx, mentaly, "\u2588");
                return false;
            }
        }

        public void SetMapTiles(int x, int y, string tile)
        {
            mentalMap.SetTile(x, y, tile);
            mazeMap.SetTile(x, y, tile);
        }

        public void ExploreNeighbours(string direction)
        {
            if (direction == "north")
            {
                if (Move("north")) Move("south");
            }
            if (direction == "south")
            {
                if (Move("south")) Move("north");
            }
            if (direction == "west")
            {
                if (Move("west")) Move("east");
            }
            if (direction == "east")
            {
                if (Move("east")) Move("west");
            }
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

        public static string OppositeDir(string dir)
        {
            if (dir == "north") return "south";
            if (dir == "south") return "north";
            if (dir == "west") return "east";
            return "west";
        }
    }
}

