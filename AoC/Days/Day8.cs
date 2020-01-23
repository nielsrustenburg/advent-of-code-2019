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
            int width = 25;
            int height = 6;
            int layerSize = width * height;
            string input = InputReader.StringFromLine("d8input.txt");
            List<string> layers = Enumerable.Range(0, input.Length / layerSize).
                                    Select(x => input.Substring(x * layerSize, layerSize)).ToList();
            int leastZeros = int.MaxValue;
            string lzLayer = null;
            foreach(string layer in layers)
            {
                int zeros = layer.Count(f => f == '0');
                if (zeros < leastZeros)
                {
                    lzLayer = layer;
                    leastZeros = zeros;
                }
            }
            int ones = lzLayer.Count(f => f == '1');
            int twos = lzLayer.Count(f => f == '2');
            return ones*twos;
        }

        public static int SolvePartTwo()
        {
            int width = 25;
            int height = 6;
            int layerSize = width * height;
            string input = InputReader.StringFromLine("d8input.txt");
            int[] solution = Enumerable.Repeat(2, layerSize).ToArray();
            List<string> layers = Enumerable.Range(0, input.Length / layerSize).
                        Select(x => input.Substring(x * layerSize, layerSize)).ToList();
            foreach(string layer in layers)
            {
                for(int i = 0; i < layerSize; i++)
                {
                    if(solution[i] == 2)
                    {
                        solution[i] = (int)Char.GetNumericValue(layer[i]);
                    }
                }
            }

            void PrintImage(int[] img)
            {
                for(int i = 0; i < height; i++)
                {
                    string row = img.Skip(i * width).Take(width).Aggregate("", (a, x) => a + (x == 1 ? "1" : " "));
                    Console.WriteLine(row);
                }
            }

            PrintImage(solution);
            return 0;
        }
    }
}

