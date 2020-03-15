using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;
using AoC.Utils;

namespace AoC
{
    static class Day17
    {
        public static int SolvePartOne()
        {
            string input = InputReader.FirstLineFromTxt("d17input.txt");
            Grid<char> map = CreateScaffoldMap(input);

            List<(int x, int y)> scaffolds = map.FindAllMatchingTiles('#');
            List<(int x, int y)> intersections = scaffolds.Where(s => map.GetNeighbours(s.x, s.y).Values.Count(n => n == '#') == 4).ToList();
            List<int> alignmentParameters = intersections.Select(i => i.x * i.y).ToList();

            return alignmentParameters.Sum();
        }

        public static Grid<char> CreateScaffoldMap(string input)
        {
            List<BigInteger> program = input.Split(',').Select(a => BigInteger.Parse(a)).ToList();
            ASCIIComputer cameras = new ASCIIComputer(program);
            string imageString = cameras.RunString();
            List<string> image = imageString.Split("\n").ToList();
            Grid<char> map = new Grid<char>(image, '.', false);
            return map;
        }

        public static int SolvePartTwo()
        {
            string input = InputReader.FirstLineFromTxt("d17input.txt");
            List<BigInteger> program = input.Split(',').Select(g => BigInteger.Parse(g)).ToList();
            program[0] = 2; //Override movement logic

            ASCIIComputer vacuumBot = new ASCIIComputer(program);

            string routine = string.Join<char>(',', "AABCBCBCBA");
            string a = string.Join<char>(',', "R6L84R6");
            string b = string.Join<char>(',', "L66R6L8L66");
            string c = string.Join<char>(',', "R66L82L82");
            string camera = "n";

            vacuumBot.Run(routine);
            vacuumBot.Run(a);
            vacuumBot.Run(b);
            vacuumBot.Run(c);
            List<BigInteger> result = vacuumBot.Run(camera);

            return (int)result.Last();
        }
    }
}

