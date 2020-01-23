﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                if (DoubleDigits(pw) && IncreasingDigits(pw))
                {
                    pw_count++;
                }
            }
            return pw_count;
        }

        public static bool IncreasingDigits(int number)
        {
            int lowest_allowed_digit = 0;
            int divisor = 100000;

            int num = number;
            int next_dig;
            //Assuming 6digit numbers
            while(divisor > 0)
            {
                next_dig = num / divisor;
                if (next_dig < lowest_allowed_digit) return false;
                lowest_allowed_digit = next_dig;
                num = num % divisor;
                divisor /= 10;
            }
            return true;
        }

        public static bool MatchingDigits(int number)
        {
            string stringed_num = number.ToString();
            for(int i = 1; i < stringed_num.Length; i++)
            {
                if (stringed_num[i] == stringed_num[i - 1]) return true;
            }
            return false;
        }

        public static bool DoubleDigits(int number)
        {
            List<string> doubles = new List<string>();
            string stringed_num = number.ToString();
            for (int i = 1; i < stringed_num.Length; i++)
            {
                if (stringed_num[i] == stringed_num[i - 1])
                {
                    doubles.Add(stringed_num.Substring(i - 1, 2));
                }
            }

            //This is probably not as efficient as possible
            List<string> disqualified = new List<string>();
            for(int i = 0; i < doubles.Count-1; i++)
            {
                for(int j = i+1; j < doubles.Count; j++)
                {
                    if (doubles[i] == doubles[j]) disqualified.Add(doubles[i]);
                }
            }

            int n_true_doubles = doubles.Where(dbl => !disqualified.Contains(dbl)).Count();
            return n_true_doubles > 0;
        }
    }
}
