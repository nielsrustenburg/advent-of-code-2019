using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Numerics;
using System.Linq;

namespace AoC.Utils
{
    static class InputReader
    {
        public static List<int> IntsFromTxt(string fileName, string fileDir = @"../../../Resources/")
        {
            string[] lines = StringsFromTxt(fileName, fileDir);
            return lines.Select(line => Int32.Parse(line)).ToList();
        }

        public static string[] StringsFromTxt(string fileName, string fileDir = @"../../../Resources/")
        {
            string path = fileDir + fileName;
            string[] lines = File.ReadAllLines(path);
            return lines;
        }

        public static string FirstLineFromTxt(string fileName, string fileDir = @"../../../Resources/")
        {
            string path = fileDir + fileName;
            StreamReader file = new StreamReader(path);
            return file.ReadLine();
        }

        public static List<int> IntsFromCSVLine(string fileName, string fileDir = @"../../../Resources/")
        {
            string line = FirstLineFromTxt(fileName, fileDir);
            return line.Split(',').Select(strint => int.Parse(strint)).ToList();
        }

        public static List<BigInteger> BigIntegersFromCSVLine(string fileName, string fileDir = @"../../../Resources/")
        {
            string line = FirstLineFromTxt(fileName, fileDir);
            return line.Split(',').Select(strint => BigInteger.Parse(strint)).ToList();
        }

        public static List<List<string>> MultipleStringListsFromCSV(string fileName, string fileDir = @"../../../Resources/")
        {
            var lines = StringsFromTxt(fileName, fileDir);
            return lines.Select(s => s.Split(',').ToList()).ToList();
        }
    }
}
