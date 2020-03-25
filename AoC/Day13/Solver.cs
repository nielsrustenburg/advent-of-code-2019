using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;
using AoC.Common;
using AoC.Computers;
using AoC.Utils;

namespace AoC.Day13
{
    class Solver : PuzzleSolver
    {
        List<BigInteger> program;
        List<BigInteger> output1;

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
            //No common prep
        }

        protected override void SolvePartOne()
        {
            BigIntCode bic = new BigIntCode(program);
            output1 = bic.Run();
            int c = 0;
            for (int i = 2; i < output1.Count; i += 3)
            {
                if (output1[i] == 2) c++;
            }
            resultPartOne = c.ToString();
        }

        //Move large portion of code to some ArcadeGame class? 
        protected override void SolvePartTwo()
        {
            bool draw = false;
            var program2 = new List<BigInteger>(program);
            program2[0] = 2;//Activate free play mode
            BigIntCode bic = new BigIntCode(program2);
            List<string> tiles = new List<string> { " ", "\u2588", "#", "=", "o" };
            Grid<string> game = new Grid<string>(" ");
            int score = 0;
            int padx = 0;
            int ballx = 0;
            bool stopNext = false;
            List<BigInteger> output = bic.Run();
            if (draw) Console.Clear();
            while (!stopNext)
            {
                if (draw) Console.SetCursorPosition(0, 0);
                stopNext = bic.Halted;
                //Draw current state to grid
                for (int i = 0; i < output.Count - 2; i += 3)
                {
                    int x = (int)output[i];
                    int y = (int)output[i + 1];
                    if (!(x == -1 && y == 0))
                    {
                        int val = (int)output[i + 2];
                        game.GetTile(x, y);
                        game.SetTile(x, y, tiles[val]);
                        if (val == 4) ballx = x;
                        if (val == 3) padx = x;
                    }
                    else
                    {
                        score = (int)output[i + 2];
                    }
                }
                if (draw)
                {
                    IEnumerable<string> gameState = game.RowsAsStrings();
                    Console.WriteLine($"Score: {score}");
                    foreach (string row in gameState.Reverse())
                    {
                        Console.WriteLine(row);
                    }
                    System.Threading.Thread.Sleep(50);
                }
                BigInteger command = 0;
                if (padx > ballx) command = -1;
                if (padx < ballx) command = 1;
                output = bic.Run(new List<BigInteger> { command });

            }
            resultPartTwo = score.ToString();
        }
    }
}
