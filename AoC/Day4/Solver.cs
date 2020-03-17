﻿using System;
using System.Collections.Generic;
using System.Text;
using AoC.common;
using System.Linq;
using AoC.Utils;

namespace AoC.Day4
{
    class Solver : PuzzleSolver
    {
        int lower;
        int upper;
        List<int> passwordsWithIncreasingDigits;
        List<int> partOnePasswords;
        List<int> partTwoPasswords;

        public Solver() : base(4)
        {
        }

        public Solver(IEnumerable<string> input) : base(input, 4)
        {
        }

        protected override void ParseInput(IEnumerable<string> input)
        {
            var lowerUpper = InputParser.ParseInts(input.First().Split('-')).ToList();
            lower = lowerUpper[0];
            upper = lowerUpper[1];
        }

        protected override void PrepareSolution()
        {
            passwordsWithIncreasingDigits = Enumerable.Range(lower, upper - lower).Where(pw => IncreasingDigits(pw)).ToList();
        }

        protected override void SolvePartOne()
        {
            partOnePasswords = passwordsWithIncreasingDigits.Where(pw => MatchingDigits(pw)).ToList();
            resultPartOne = partOnePasswords.Count.ToString();
        }

        protected override void SolvePartTwo()
        {
            partTwoPasswords = partOnePasswords.Where(pw => ContainsDigitPair(pw)).ToList();
            resultPartTwo = partTwoPasswords.Count.ToString();
        }

        public static bool IncreasingDigits(int number)
        {
            int lowestAllowedDigit = 0;
            int nDigits = (int)Math.Floor(Math.Log10(number) + 1);
            int divisor = (int)Math.Pow(10, nDigits);

            while (divisor > 0)
            {
                int currentDigit = number / divisor;
                if (currentDigit < lowestAllowedDigit) return false;
                lowestAllowedDigit = currentDigit;
                number = number % divisor;
                divisor /= 10;
            }
            return true;
        }

        public static bool MatchingDigits(int number)
        {
            string numString = number.ToString();
            for (int i = 1; i < numString.Length; i++)
            {
                if (numString[i] == numString[i - 1]) return true;
            }
            return false;
        }

        public static bool ContainsDigitPair(int number)
        {
            string numString = number.ToString();
            int nSameDigits = 1;

            for (int i = 1; i < numString.Length; i++)
            {
                if (numString[i] == numString[i - 1])
                {
                    nSameDigits++;
                }
                else
                {
                    if (nSameDigits == 2) break;
                    nSameDigits = 1;
                }
            }

            return nSameDigits == 2;
        }
    }
}
