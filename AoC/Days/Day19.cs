using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

namespace AoC
{
    static class Day19
    {
        public static int SolvePartOne()
        {
            string strInput = InputReader.StringFromLine("d19input.txt");
            List<BigInteger> program = strInput.Split(',').Select(x => BigInteger.Parse(x)).ToList();
            return TractorBeamSlice(program, 0,50,0,50);
        }

        public static int TractorBeamSlice(List<BigInteger> program, int xStart, int xCount, int yStart, int yCount, bool draw = false)
        {
            int pulls = 0;
            List<string> rows = new List<string>();
            for (int y = 0; y < yCount; y++)
            {
                string row = "";
                for (int x = 0; x < xCount; x++)
                {
                    BigIntCode bot = new BigIntCode(program);
                    List<BigInteger> output = bot.Run(new List<BigInteger> { x+xStart, y+yStart });
                    pulls += (int)output[0];
                    row = row + (output[0] == 0 ? "." : "#");
                }
                rows.Add(row);
            }
            if (draw)
            {
                Console.Clear();
                foreach (var row in rows)
                {
                    Console.WriteLine(row);
                }
            }
            return pulls;
        }

        public static int SolvePartTwo()
        {
            string strInput = InputReader.StringFromLine("d19input.txt");
            List<BigInteger> program = strInput.Split(',').Select(a => BigInteger.Parse(a)).ToList();
            int xAns = 0, yAns = 0;
            int targetWidth = 100;

            int[] lefts = new int[100], rights = new int[100];
            int pointer = 0;

            int c = 0;

            int xLeft = 0, xRight = 0, y = 50;
            while (true)
            {
                BigIntCode leftBot = new BigIntCode(program);
                BigInteger leftSide = leftBot.Run(new List<BigInteger> { xLeft, y }).First();
                while (leftSide == 0)
                {
                    xLeft++;
                    leftBot = new BigIntCode(program);
                    leftSide = leftBot.Run(new List<BigInteger> { xLeft, y }).First();
                }

                if (xRight < xLeft) xRight = xLeft;
                BigIntCode rightBot = new BigIntCode(program);
                BigInteger rightSide = rightBot.Run(new List<BigInteger> { xRight, y }).First();
                while (rightSide == 1)
                {
                    xRight++;
                    rightBot = new BigIntCode(program);
                    rightSide = rightBot.Run(new List<BigInteger> { xRight, y }).First();
                }
                xRight--;

                if (y == 1087) System.Diagnostics.Debugger.Break();
                int topPointer = (pointer + 1) % 100;
                if (xLeft + targetWidth - 1 <= rights[topPointer])
                {
                    return 10000 * xLeft + (y-targetWidth + 1);
                }

                lefts[pointer] = xLeft;
                rights[pointer] = xRight;
                //Console.WriteLine($"{c}: Left:{xLeft} Right:{xRight}");
                pointer = (pointer + 1) % targetWidth;
                y++;
                c = (c + 1) % 100;
            }
        }
    }
}

