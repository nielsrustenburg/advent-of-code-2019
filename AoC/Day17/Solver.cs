using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;
using AoC.Common;
using AoC.Computers;
using AoC.Utils;

namespace AoC.Day17
{
    class Solver : PuzzleSolver
    {
        List<BigInteger> program;
        Grid<char> map;
        List<(int x, int y)> scaffolds;
        List<(int x, int y)> intersections;

        public Solver() : this(Input.InputMode.Embedded, "input")
        {
        }

        public Solver(Input.InputMode mode, string input) : base(mode, input)
        {
        }

        protected override void ParseInput(string input)
        {
            program = InputParser<BigInteger>.ParseCSVLine(input, BigInteger.Parse).ToList();
            map = CreateScaffoldMap();
        }

        protected override void PrepareSolution()
        {
            scaffolds = map.FindAllMatchingTiles('#');
            intersections = scaffolds.Where(s => map.GetNeighbours(s.x, s.y).Values.Count(n => n == '#') == 4).ToList();
        }

        protected override void SolvePartOne()
        {
            var alignmentParameters = intersections.Select(i => i.x * i.y);
            resultPartOne = alignmentParameters.Sum().ToString();
        }

        //Hardcoded with manually found pattern
        protected override void SolvePartTwo()
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

            resultPartTwo = result.Last().ToString();
        }

        private Grid<char> CreateScaffoldMap()
        {
            ASCIIComputer cameras = new ASCIIComputer(program);
            string imageString = cameras.RunString();
            List<string> image = imageString.Split("\n").ToList();
            Grid<char> map = new Grid<char>(image, '.', false);
            return map;
        }
    }
}
