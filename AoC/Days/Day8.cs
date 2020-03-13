using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC
{
    static class Day8
    {
        public static int SolvePartOne()
        {
            string input = InputReader.StringFromLine("d8input.txt");

            int width = 25;
            int height = 6;
            int pixelsPerLayer = width * height;

            List<string> layers = Enumerable.Range(0, input.Length / pixelsPerLayer).
                                    Select(x => input.Substring(x * pixelsPerLayer, pixelsPerLayer)).ToList();

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
            return ones * twos;
        }

        public static List<string> SolvePartTwo()
        {
            return SolvePartTwo(false);
        }

        public static List<string> SolvePartTwo(bool print)
        {
            int width = 25;
            int height = 6;
            int pixelsPerLayer = width * height;
            string input = InputReader.StringFromLine("d8input.txt");
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
                if(print) Console.WriteLine(row);
            }

            return image;
        }
    }
}

