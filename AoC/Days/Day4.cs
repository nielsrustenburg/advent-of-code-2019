using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoC.Utils;

namespace AoC
{
    static class Day4
    {
        public static int SolvePartOne()
        {
            int pw_count = 0;
            for(int pw = 356261; pw < 846303; pw++)
            {
                if(MatchingDigits(pw) && IncreasingDigits(pw))
                {
                    pw_count++;
                }
            }
            return pw_count;
        }

        public static int SolvePartTwo()
        {
            int pw_count = 0;
            for (int pw = 356261; pw < 846303; pw++)
            {
                if (ContainsDigitPair(pw) && IncreasingDigits(pw))
                {
                    pw_count++;
                }
            }
            return pw_count;
        }

        public static bool IncreasingDigits(int number)
        {
            int lowestAllowedDigit = 0;
            int nDigits = (int) Math.Floor(Math.Log10(number) + 1);
            int divisor = (int) Math.Pow(10, nDigits); 

            while(divisor > 0)
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
            for(int i = 1; i < numString.Length; i++)
            {
                if (numString[i] == numString[i - 1]) return true;
            }
            return false;
        }

        public static bool ContainsDigitPair(int number)
        {
            string numString = number.ToString();
            int nSameDigits = 1;

            for(int i = 1; i < numString.Length; i++)
            {
                if (numString[i] == numString[i - 1])
                {
                    nSameDigits++;
                } else
                {
                    if (nSameDigits == 2) break;
                    nSameDigits = 1;
                }
            }

            return nSameDigits == 2;
        }
    }
}
