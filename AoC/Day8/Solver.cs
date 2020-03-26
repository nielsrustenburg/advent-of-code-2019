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
        Image image;

        public Solver() : this(Input.InputMode.Embedded, "input")
        {
        }

        public Solver(Input.InputMode mode, string input) : base(mode, input)
        {
        }

        protected override void ParseInput(string input)
        {
            image = new Image(25, 6, input);
        }

        protected override void PrepareSolution()
        {
        }

        protected override void SolvePartOne()
        {
            var pixelCounts = image.PixelCountsPerLayer();
            var fewestZeros = pixelCounts.OrderBy(pc => pc['0']).First();
            resultPartOne = (fewestZeros['1'] * fewestZeros['2']).ToString();
        }

        protected override void SolvePartTwo()
        {
            resultPartTwo = image.DecodeImage(new char[] { '2' }).Replace('0', ' ');
        }
    }

    class Image
    {
        int height;
        int width;
        int depth;
        char[,,] image;

        //hehe public image
        public Image(int width, int height, string data)
        {
            this.width = width;
            this.height = height;
            var pixelsPerLayer = width * height;
            depth = data.Length / pixelsPerLayer;

            image = new char[depth, width, height];
            int contentCounter = 0;
            for (int layerCounter = 0; layerCounter < depth; layerCounter++)
            {
                for (int heightCounter = 0; heightCounter < height; heightCounter++)
                {
                    for (int widthCounter = 0; widthCounter < width; widthCounter++)
                    {
                        image[layerCounter, widthCounter, heightCounter] = data[contentCounter];
                        contentCounter++;
                    }
                }
            }
            var content = Enumerable.Range(0, depth).Select(x => data.Substring(x * pixelsPerLayer, pixelsPerLayer)).ToArray();
        }

        public string ShowLayers()
        {
            var layers = new string[depth];
            for (int d = 0; d < depth; d++)
            {
                var rows = new string[height];
                for (int h = 0; h < height; h++)
                {
                    var rowbuilder = new StringBuilder();
                    for (int w = 0; w < width; w++)
                    {
                        rowbuilder.Append(image[d, w, h]);
                    }
                    rows[h] = rowbuilder.ToString();
                }
                layers[d] = string.Join("\r\n", rows);
            }
            return string.Join("\r\n\r\n", layers);
        }

        public Dictionary<char, int>[] PixelCountsPerLayer()
        {
            var result = new Dictionary<char, int>[depth];
            for (int d = 0; d < depth; d++)
            {
                var pixelCounts = new Dictionary<char, int>();
                for(int h = 0; h < height; h++)
                {
                    for(int w = 0; w < width; w++)
                    {
                        var pixel = image[d, w, h];
                        if (!pixelCounts.ContainsKey(pixel))
                        {
                            pixelCounts.Add(pixel, 0);
                        }
                        pixelCounts[pixel]++;
                    }
                }
                result[d] = pixelCounts;
            }
            return result;
        }

        public string DecodeImage(IEnumerable<char> transparantPixels)
        {
            var transparant = transparantPixels.ToHashSet();
            var rows = new string[image.GetLength(2)];
            for (int h = 0; h < height; h++)
            {
                var rowbuilder = new StringBuilder();
                for (int w = 0; w < width; w++)
                {
                    for (int d = 0; d < depth; d++)
                    {
                        if (!transparant.Contains(image[d, w, h]))
                        {
                            rowbuilder.Append(image[d, w, h]);
                            break;
                        }
                    }
                }
                rows[h] = rowbuilder.ToString();
            }
            return string.Join("\r\n", rows);
        }
    }
}
