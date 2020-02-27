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

            //Find the cycle for X,Y,Z on velocity+position using the initial state as our start of the cycle

            List<int> initialXpositions = jmoons.Select(j => j.X).ToList();
            List<int> initialYpositions = jmoons.Select(j => j.Y).ToList();
            List<int> initialZpositions = jmoons.Select(j => j.Z).ToList();
            List<int> initialVelocities = new List<int> { 0, 0, 0, 0 }; //Same for each axis

            BigInteger xCycle = 0;
            BigInteger yCycle = 0;
            BigInteger zCycle = 0;
            BigInteger step = 0;
            while (xCycle == 0 || yCycle == 0 || zCycle == 0)
            {
                SimulateStep(jmoons);
                step++;
                if(xCycle == 0)
                {
                    IEnumerable<int> xVels = jmoons.Select(j => j.velocity.x);
                    IEnumerable<int> xPos = jmoons.Select(j => j.X);
                    if (xVels.SequenceEqual(initialVelocities) && xPos.SequenceEqual(initialXpositions)) xCycle = step;
                }
                if (yCycle == 0)
                {
                    IEnumerable<int> yVels = jmoons.Select(j => j.velocity.y);
                    IEnumerable<int> yPos = jmoons.Select(j => j.Y);
                    if (yVels.SequenceEqual(initialVelocities) && yPos.SequenceEqual(initialYpositions)) yCycle = step;
                }
                if (zCycle == 0)
                {
                    IEnumerable<int> zVels = jmoons.Select(j => j.velocity.z);
                    IEnumerable<int> zPos = jmoons.Select(j => j.Z);
                    if (zVels.SequenceEqual(initialVelocities) && zPos.SequenceEqual(initialZpositions)) zCycle = step;
                }
            }
            return MathHelper.LCM(new List<BigInteger> { xCycle, yCycle, zCycle });
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

