﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace AoC
{
    static class MathHelper
    {
        public static int Mod(int a, int b)
        {
            int res = a % b;
            return res + (res < 0 ? b : 0);
        }

        public static BigInteger Mod(BigInteger a, BigInteger b)
        {
            BigInteger res = a % b;
            return res + (res < 0 ? b : 0);
        }

        public static BigInteger GCD(BigInteger a, BigInteger b)
        {
            if (a == 0) return b;
            if (b == 0) return a;

            BigInteger greater, smaller;
            if(a >= b)
            {
                greater = a;
                smaller = b;
            } else
            {
                greater = b;
                smaller = a;
            }
            return GCD(smaller, Mod(greater, smaller));
        }

        public static BigInteger ModInv(BigInteger a, BigInteger b)
        {
            (BigInteger aInv, BigInteger _) = ModInvHelper(a, b, (1, 0), (0, 1));
            if(a >= b)
            {
                ModInvHelper(a, b, (1, 0), (0, 1));
            } else
            {
                ModInvHelper(b, a, (0, 1), (1, 0));
            }
            return Mod(aInv, b);
        }

        private static (BigInteger,BigInteger) ModInvHelper(BigInteger greater, BigInteger smaller, (BigInteger a, BigInteger b) g, (BigInteger a, BigInteger b) s)
        {
            //Find the modular inverse A^-1 for A mod B
            if (smaller == 0) throw new Exception("No Modular Inverse Possible");

            BigInteger x = greater / smaller;
            BigInteger remainder = Mod(greater, smaller);
            if (smaller == 1)
            {
                //return mod inv
                return s;
            }
            else
            {
                //ga dieper
                return ModInvHelper(smaller, remainder, s, (g.a - x * s.a, g.b - x * s.b));
            }            
        }
    }
}
