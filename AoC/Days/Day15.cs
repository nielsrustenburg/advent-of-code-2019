using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

namespace AoC
{
    static class Day15
    {
        public static int WalkPartOne()
        {
            Console.Clear();
            string input = InputReader.StringFromLine("d15input.txt");
            List<BigInteger> program = input.Split(',').Select(x => BigInteger.Parse(x)).ToList();
            RepairBot rb = new RepairBot(program);

            while (!rb.Finished)
            {
                char dirkey = Console.ReadKey().KeyChar;
                if (dirkey == 'w') rb.Move("north");
                if (dirkey == 'a') rb.Move("west");
                if (dirkey == 's') rb.Move("south");
                if (dirkey == 'd') rb.Move("east");

                Console.SetCursorPosition(0, 0);
                var lines = rb.mentalMap.RowsAsStrings(true);
                foreach (string line in lines)
                {
                    Console.WriteLine(line);
                }
            }
            return 0;
        }

        public static int SolvePartOne()
        {
            Console.Clear();
            string input = InputReader.StringFromLine("d15input.txt");
            List<BigInteger> program = input.Split(',').Select(x => BigInteger.Parse(x)).ToList();
            RepairBot rb = new RepairBot(program);

            List<string> dirs = new List<string> { "north", "south", "west", "east" };
            int steps = 0;
            List<string> path = new List<string>();
            int oxyX = 0;
            int oxyY = 0;
            do//!rb.Finished
            {
                var wallknowledge = rb.LookAround();
                for (int i = 0; i < wallknowledge.Count; i++)
                {
                    if (!wallknowledge[i])
                    {
                        rb.Explore(dirs[i]);
                    }
                }
                wallknowledge = rb.LookAround();
                if (wallknowledge.Where(x => !x).Any())
                {
                    string movedir = dirs[wallknowledge.FindIndex(x => !x)];
                    rb.Move(movedir, true);
                    path.Add(movedir);
                    steps++;
                }
                else
                {
                    rb.Move(OppositeDir(path.Last()), true);
                    path.RemoveAt(path.Count - 1);
                    steps--;
                }

                if(oxyX == 0 && oxyY == 0 && rb.Finished)
                {
                    oxyX = rb.X;
                    oxyY = rb.Y;
                }
                Console.SetCursorPosition(0, 0);
                var lines = rb.mentalMap.RowsAsStrings(true);
                foreach (string line in lines)
                {
                    Console.WriteLine(line);
                }
            } while (path.Any());

            //Gas the grid!
            Grid<string> maze = rb.mentalMap;

            maze.SetTile(oxyX, oxyY, "O");
            maze.SetTile(rb.X, rb.Y, "#");
            //Find all unoxygen'd tiles
            List<(int x,int y)> vacuum = maze.FindAllMatchingTiles("#");
            int minutes = 0;
            while (vacuum.Any())
            {
                List<(int x, int y)> oxygenize = new List<(int x, int y)>();
                foreach(var point in vacuum)
                {
                    if (maze.GetNeighbours(point.x, point.y).Values.Contains("O"))
                    {
                        oxygenize.Add(point);
                    }
                }

                foreach(var point in oxygenize)
                {
                    maze.SetTile(point.x, point.y, "O");
                    vacuum.RemoveAt(vacuum.FindIndex(p => p.x == point.x && p.y == point.y));
                }
                minutes++;
                Console.SetCursorPosition(0, 0);
                var lines = maze.RowsAsStrings(true);
                foreach (string line in lines)
                {
                    Console.WriteLine(line);
                }
            }

            return steps;
        }

        public static int SolvePartTwo()
        {
            return 0;
        }

        public static string OppositeDir(string dir)
        {
            if (dir == "north") return "south";
            if (dir == "south") return "north";
            if (dir == "west") return "east";
            return "west";
        }
    }

    class RepairBot
    {
        BigIntCode brain;
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool Finished { get; private set; }
        public Grid<string> mentalMap;

        public RepairBot(List<BigInteger> programming)
        {
            brain = new BigIntCode(programming);
            X = 0;
            Y = 0;
            mentalMap = new Grid<string>(" ");
            Finished = false;
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
                mentalMap.GetTile(X, Y);
                mentalMap.SetTile(X, Y, closePrev ? "#" : " ");
                if (direction == "north") Y++;
                if (direction == "south") Y--;
                if (direction == "west") X--;
                if (direction == "east") X++;

                Finished = response == 2;
                mentalMap.GetTile(X, Y);
                mentalMap.SetTile(X, Y, Finished ? "X" : "D");
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
                mentalMap.GetTile(mentalx, mentaly);
                mentalMap.SetTile(mentalx, mentaly, "\u2588");
                return false;
            }
        }

        public void Explore(string direction)
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
            List<string> walls = new List<string> { "\u2588", "#"};
            return new List<bool> { walls.Contains(north),
                                    walls.Contains(south),
                                    walls.Contains(west),
                                    walls.Contains(east) };
        }
    }
}

