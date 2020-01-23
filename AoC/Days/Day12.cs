using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

namespace AoC
{
    static class Day12
    {
        public static int SolvePartOne()
        {
            List<JupiterMoon> jmoons = new List<JupiterMoon>
            {
                new JupiterMoon(1,4,4),
                new JupiterMoon(-4,-1,19),
                new JupiterMoon(-15,-14,12),
                new JupiterMoon(-17,1,10)
            };

            for (int i = 0; i < 1000; i++)
            {
                SimulateStep(jmoons);
            }

            return jmoons.Sum(x => x.TotalEnergy());
        }

        public static BigInteger SolvePartTwo()
        {
            List<JupiterMoon> jmoons = new List<JupiterMoon>
            {
                new JupiterMoon(1,4,4),
                new JupiterMoon(-4,-1,19),
                new JupiterMoon(-15,-14,12),
                new JupiterMoon(-17,1,10)
            };

            BigInteger x = 0;
            BigInteger y = 0;
            BigInteger z = 0;
            BigInteger c = 0;
            while (x == 0 || y == 0 || z == 0)
            {
                SimulateStep(jmoons);
                c++;
                int xVelsum = jmoons.Sum(m => Math.Abs(m.velocity.x));
                int yVelsum = jmoons.Sum(m => Math.Abs(m.velocity.y));
                int zVelsum = jmoons.Sum(m => Math.Abs(m.velocity.z));
                x = CheckAndUpdate(xVelsum, x, c);
                y = CheckAndUpdate(yVelsum, y, c);
                z = CheckAndUpdate(zVelsum, z, c);
            }
            BigInteger CheckAndUpdate(int sum, BigInteger n, BigInteger count)
            {
                if (sum == 0)
                {
                    if (n == 0)
                    {
                        return c;
                    }
                    else
                    {
                        if (c % n != 0) throw new Exception("Assumption is wrong!!!");
                    }
                }
                return n;
            }
            return x * y * z;
        }

        public static void SimulateStep(List<JupiterMoon> jmoons)
        {
            foreach (var moon in jmoons)
            {
                moon.UpdateVelocity(jmoons);
            }
            foreach (var moon in jmoons)
            {
                moon.UpdatePosition();
            }
        }
    }

    public class JupiterMoon
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }
        public (int x, int y, int z) velocity;
        public string initialState;

        public JupiterMoon(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
            velocity = (0, 0, 0);
            initialState = this.PosToString();
        }

        public void UpdateVelocity(IEnumerable<JupiterMoon> moons)
        {
            int xDif, yDif, zDif;
            xDif = yDif = zDif = 0;
            foreach (JupiterMoon moon in moons)
            {
                if (moon != this)
                {
                    if (moon.X > X) xDif++;
                    if (moon.X < X) xDif--;
                    if (moon.Y > Y) yDif++;
                    if (moon.Y < Y) yDif--;
                    if (moon.Z > Z) zDif++;
                    if (moon.Z < Z) zDif--;
                }
            }
            velocity = (velocity.x + xDif, velocity.y + yDif, velocity.z + zDif);
        }

        public void UpdatePosition()
        {
            X += velocity.x;
            Y += velocity.y;
            Z += velocity.z;
        }

        public int TotalEnergy()
        {
            int potential = Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
            int kinetic = Math.Abs(velocity.x) + Math.Abs(velocity.y) + Math.Abs(velocity.z);
            return potential * kinetic;
        }

        public override string ToString()
        {
            return $"p:{X},{Y},{Z} v:{velocity.x},{velocity.y},{velocity.z}";
        }

        public string PosToString()
        {
            return $"p:{X},{Y},{Z}";
        }
    }
}

