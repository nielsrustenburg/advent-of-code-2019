using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

namespace AoC
{
    static class Day13
    {
        public static int SolvePartOne()
        {
            string line = InputReader.FirstLineFromTxt("d13input.txt");
            List<BigInteger> program = line.Split(',').Select(x => BigInteger.Parse(x)).ToList();
            BigIntCode bic = new BigIntCode(program);
            List<BigInteger> output = bic.Run();
            int c = 0;
            for(int i = 2; i < output.Count; i += 3)
            {
                if (output[i] == 2) c++;
            }
            return c;
        }

        public static int SolvePartTwo()
        {
            return SolvePartTwo(false);
        }

        public static int SolvePartTwo(bool draw)
        {
            string line = InputReader.FirstLineFromTxt("d13input.txt");
            List<BigInteger> program = line.Split(',').Select(x => BigInteger.Parse(x)).ToList();
            program[0] = 2; //Activate free play mode
            BigIntCode bic = new BigIntCode(program);
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
                if(draw) Console.SetCursorPosition(0, 0);
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
                        if(val == 4) ballx = x;
                        if(val == 3) padx = x;
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
            return score;
        }
    }
}

