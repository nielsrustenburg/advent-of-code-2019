using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AoC.Common;
using AoC.Utils;

namespace AoC.Day8
{
    class Solver : PuzzleSolver
    {
        string input;
        int width;
        int height;
        int pixelsPerLayer;
        List<string> layers;

        public Solver() : this(Input.InputMode.Embedded, "input")
        {
        }

        public Solver(Input.InputMode mode, string input) : base(mode, input)
        {
        }

        protected override void ParseInput(string input)
        {
            this.input = input;
        }

        protected override void PrepareSolution()
        {
            width = 25;
            height = 6;
            pixelsPerLayer = width * height;
            layers = Enumerable.Range(0, input.Length / pixelsPerLayer).Select(x => input.Substring(x * pixelsPerLayer, pixelsPerLayer)).ToList();
        }

        protected override void SolvePartOne()
        {
            int leastZeros = int.MaxValue;
            string leastZerosLayer = null;
            foreach (string layer in layers)
            {
                int zeros = layer.Count(f => f == '0');
                if (zeros < leastZeros)
                {
                    leastZerosLayer = layer;
                    leastZeros = zeros;
                }
            }
            int ones = leastZerosLayer.Count(f => f == '1');
            int twos = leastZerosLayer.Count(f => f == '2');
            resultPartOne = (ones * twos).ToString();
        }

        protected override void SolvePartTwo()
        {
            char[] solution = new char[pixelsPerLayer];
            List<string> layers = Enumerable.Range(0, input.Length / pixelsPerLayer).
                        Select(x => input.Substring(x * pixelsPerLayer, pixelsPerLayer)).ToList();

            for (int i = 0; i < pixelsPerLayer; i++)
            {
                for (int d = 0; d < layers.Count; d++)
                {
                    char pixel = layers[d][i];
                    if (pixel != '2')
                    {
                        solution[i] = pixel;
                        break;
                    }
                }
            }

            List<string> image = new List<string>();
            for (int i = 0; i < height; i++)
            {
                string row = solution.Skip(i * width).Take(width).Aggregate("", (a, x) => a + (x == '1' ? "1" : " "));
                image.Add(row);
            }
            string imgString = string.Join((char) 10, image);
            resultPartTwo = imgString;
        }
    }
}
