using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace AoC
{
    static class InputReader
    {
        // My apologies for this terrible class

        public static List<int> IntsFromTxt(string fileName, string fileDir = @"../../../Resources/")
        {
            string path = fileDir + fileName;
            string[] lines = File.ReadAllLines(path);
            return lines.Select(line => Int32.Parse(line)).ToList();
        }

        public static string[] StringsFromTxt(string fileName, string fileDir = @"../../../Resources/")
        {
            string path = fileDir + fileName;
            string[] lines = File.ReadAllLines(path);
            return lines;
        }

        public static string StringFromLine(string fileName, string fileDir = @"../../../Resources/")
        {
            string path = fileDir + fileName;
            return File.ReadAllLines(path)[0];
        }

        public static List<int> IntsFromCSVLine(string fileName, string fileDir = @"../../../Resources/")
        {
            string path = fileDir + fileName;
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            string line = file.ReadLine();
            return line.Split(',').Select(strint => Int32.Parse(strint)).ToList();
        }

        public static List<List<string>> MultipleStringListsFromCSV(string fileName, string fileDir = @"../../../Resources/")
        {
            string path = fileDir + fileName;

            List<List<string>> output = new List<List<string>>();
            
            string line;
            using (StreamReader file = new StreamReader(path))
            {
                while((line = file.ReadLine()) != null)
                {
                    output.Add(line.Split(',').ToList());
                }
            }
            return output;
        }

        public static List<(string orbitee,string orbiter)> ReadOrbitRelationsFromTxt(string fileName, string fileDir = @"../../../Resources/")
        {
            string path = fileDir + fileName;

            List<(string, string)> output = new List<(string, string)>();

            string line;
            using (StreamReader file = new StreamReader(path))
            {
                while ((line = file.ReadLine()) != null)
                {
                    string[] splitLine = line.Split(')');
                    output.Add((splitLine[0], splitLine[1]));
                }
            }
            return output;
        }
    }
}
