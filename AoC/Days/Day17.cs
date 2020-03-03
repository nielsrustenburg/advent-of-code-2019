using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;

namespace AoC
{
    static class Day17
    {
        public static int SolvePartOne()
        {
            string input = InputReader.StringFromLine("d17input.txt");
            List<BigInteger> program = input.Split(',').Select(a => BigInteger.Parse(a)).ToList();
            BigIntCode bic = new BigIntCode(program);
            Grid<string> map = new Grid<string>(" ");
            List<BigInteger> output = bic.Run();
            DrawMap(output, map);

            List<(int x, int y)> scaffolds = map.FindAllMatchingTiles("#");
            List<(int x, int y)> intersections = new List<(int x, int y)>();
            foreach (var scaf in scaffolds)
            {
                if (map.GetNeighbours(scaf.x, scaf.y).Values.Count(s => s == "#") == 4) intersections.Add(scaf);
            }

            return intersections.Sum(inx => inx.x * inx.y);
        }

        public static void DrawMap(List<BigInteger> output, Grid<string> map)
        {
            int x = 0;
            int y = 0;
            int id = 0;
            while (id < output.Count)
            {
                if (output[id] != 10)
                {
                    map.GetTile(x, y);
                    string icon;
                    if (output[id] > 300)
                    {
                        icon = "&";
                    } else
                    {
                        icon = ((char)output[id]).ToString();
                    }
                    map.SetTile(x,y,icon);
                    x++;
                }
                else
                {
                    x = 0;
                    y++;
                    if (x == -1 || y == -1) System.Diagnostics.Debugger.Break();
                    map.GetTile(x, y);
                }
                id++;
            }
            Console.SetCursorPosition(0, 0);
            var lines = map.RowsAsStrings();
            foreach (string line in lines)
            {
                Console.WriteLine(line);
            }
        }

        public static List<BigInteger> StoAsciiInput(string s)
        {
            string withCommas = String.Join<char>(',', s);
            List<BigInteger> bilist = withCommas.Select(x => (BigInteger) x).ToList();
            bilist.Add(10);
            return bilist;
        }

        public static int SolvePartTwo()
        {
            string input = InputReader.StringFromLine("d17input.txt");
            List<BigInteger> program = input.Split(',').Select(g => BigInteger.Parse(g)).ToList();
            if (program[0] != 1) throw new Exception();
            program[0] = 2;
            BigIntCode bic = new BigIntCode(program);
            Grid<string> map = new Grid<string>(" ");
            List<BigInteger> output = bic.Run();

            DrawMap(output, map);

            var botspot = map.FindFirstMatchingTile("^");
            DirectionRobot dbot = new DirectionRobot(x:botspot.x, y:botspot.y, map: map, yflip: true);
            dbot.AddWalkableTiles(new List<string> { "#" });
            List<string> botPath = new List<string>();
            bool validStep = true;
            int straight = 0;
            while (validStep)
            {
                if (dbot.MoveForward())
                {
                    straight++;
                } else
                {
                    botPath.Add(straight.ToString());
                    straight = 1;

                    dbot.TurnRight();
                    if (dbot.MoveForward())
                    {
                        botPath.Add("R");
                    } else
                    {
                        dbot.TurnLeft();//undo right step
                        dbot.TurnLeft();
                        if (dbot.MoveForward())
                        {
                            botPath.Add("L");
                        }
                        else
                        {
                            validStep = false;
                        }
                    }
                }
            }
            botPath.RemoveAt(0);

            Console.WriteLine(String.Join(',', botPath));

            List<BigInteger> instructions = new List<BigInteger>();
            string routine = "AABCBCBCBA";
            string a = "R6L84R6";
            string b = "L66R6L8L66";
            string c = "R66L82L82";
            string camera = "n";
            instructions.AddRange(StoAsciiInput(routine));
            instructions.AddRange(StoAsciiInput(a));
            instructions.AddRange(StoAsciiInput(b));
            instructions.AddRange(StoAsciiInput(c));
            instructions.AddRange(StoAsciiInput(camera));
            
            Console.WriteLine(String.Join(',', instructions));
            
            output = bic.Run(instructions);
            DrawMap(output, map);

            return (int) output.Last();
        }
    }

    public class DirectionRobot
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public (bool onX, bool posi) Facing { get; private set; }
        public Grid<string> Map { get; private set; }
        public bool FlippedYAxis { get; private set; }
        public List<string> WalkableTiles { get; private set; }

        public DirectionRobot(int x = 0, int y = 0, string facing = "north", Grid<string> map = null, bool yflip = false)
        {
            X = x;
            Y = y;
            Map = map;
            FlippedYAxis = yflip;
            Facing = ((facing == "west" || facing == "east"), facing == "north" || facing == "east");
            WalkableTiles = new List<string>();
        }

        public void TurnRight()
        {
            //North = False True    --> True True       East
            //South = False False   --> True False      West
            //West =  True False    --> False True      North
            //East =  True True     --> False False     South
            Facing = (!Facing.onX, (!Facing.onX && Facing.posi || Facing.onX && !Facing.posi));  
        }

        public void TurnLeft()
        {
            //North = False True    --> True False      West
            //South = False False   --> True True       East
            //West =  True False    --> False False     South
            //East =  True True     --> False True      North
            Facing = (!Facing.onX, (Facing.onX && Facing.posi || !Facing.onX && !Facing.posi));
        }

        public void AddWalkableTiles(IEnumerable<string> tiles)
        {
            WalkableTiles.AddRange(tiles);
        }

        public bool MoveForward()
        {
            int toX = X;
            int toY = Y;
            if (Facing.onX)
            {
                toX = Facing.posi ? X + 1 : X - 1;
            } else
            {
                toY = (Facing.posi && !FlippedYAxis) || (!Facing.posi && FlippedYAxis)  ? Y + 1 : Y - 1;
            }
            //If legal move
            if (WalkableTiles.Contains(Map.GetTile(toX, toY)))
            {
                X = toX;
                Y = toY;
                return true;
            }
            return false;
        }
    }
}

