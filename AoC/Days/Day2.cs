using System;
using System.Collections.Generic;
using System.Text;

namespace AoC
{
    public static class Day2
    {
        public static int SolvePartOne()
        {
            List<int> backup = InputReader.IntsFromCSVLine("d2input.txt");
            return (RunIntCodewithNV(backup, 12, 2));
        }

        public static int SolvePartTwo()
        {
            List<int> backup_code = InputReader.IntsFromCSVLine("d2input.txt");
            return FindNounVerb(backup_code, 19690720);

        }

        public static int FindNounVerb(List<int> code, int target, int lower = 0, int upper = 99)
        {
            for (int noun = lower; noun <= upper ; noun++)
            {
                for (int verb = lower; verb <= upper; verb++)
                {
                    try {
                        int output = RunIntCodewithNV(new List<int>(code), noun, verb);
                        if (output == target)
                        {
                            return 100 * noun + verb;
                        }
                    } catch
                    {
                    } 
                }
            }
            throw new Exception($"Can't find target: {target} in range:({lower},{upper})");
        }

        public static int RunIntCodewithNV(List<int> code, int noun, int verb)
        {
            IntCode program = new IntCode(code);
            program.Write(1, noun);
            program.Write(2, verb);
            program.Run();
            int output = program.GetValAtMemIndex(0);
            return output;
        }
    }
}
