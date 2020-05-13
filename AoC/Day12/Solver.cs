using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;
using AoC.Common;
using AoC.Utils;

namespace AoC.Day12
{
    class Solver : PuzzleSolver
    {
        JupiterMoon[] moons;

        public Solver() : this(Input.InputMode.Embedded, "input")
        {
        }

        public Solver(Input.InputMode mode, string input) : base(mode, input)
        {
        }

        protected override void ParseInput(string input)
        {
            var moonStates = InputParser<(int x, int y, int z)>.SplitAndParse(input, ParseMoonState);
            moons = moonStates.Select(coords => new JupiterMoon(coords.x, coords.y, coords.z)).ToArray();

            (int, int, int) ParseMoonState(string moonStateString)
            {
                var removedBrackets = moonStateString.Substring(1, moonStateString.Length - 2);
                var values = InputParser<int>.ParseCSVLine(removedBrackets, valDec => int.Parse(valDec.Split('=')[1])).ToArray();
                return (values[0], values[1], values[2]);
            }
        }

        protected override void PrepareSolution()
        {
            //no real common prep here (technically the first 1000 steps are done by part1 and could be skipped for part2)
        }

        protected override void SolvePartOne()
        {
            SimulateSteps(1000);
            resultPartOne = EnergyInSystem().ToString();
        }

        protected override void SolvePartTwo()
        {
            resultPartTwo = FindCycle().ToString();
        }

        public void SimulateSteps(int n)
        {
            for (int i = 0; i < n; i++)
            {
                SimulateStep();
            }
        }

        public void SimulateStep()
        {
            foreach (var moon in moons)
            {
                moon.UpdateVelocity(moons);
            }
            foreach (var moon in moons)
            {
                moon.UpdatePosition();
            }
        }

        public int EnergyInSystem()
        {
            return moons.Sum(x => x.TotalEnergy());
        }

        public BigInteger FindCycle()
        {
            ResetMoons();

            BigInteger xCycle = 0;
            BigInteger yCycle = 0;
            BigInteger zCycle = 0;
            BigInteger step = 0;
            while (xCycle == 0 || yCycle == 0 || zCycle == 0)
            {
                SimulateStep();
                step++;
                if (xCycle == 0 && moons.All(moon => moon.initialX == moon.X && moon.velocity.x == 0)) xCycle = step;
                if (yCycle == 0 && moons.All(moon => moon.initialY == moon.Y && moon.velocity.y == 0)) yCycle = step;
                if (zCycle == 0 && moons.All(moon => moon.initialZ == moon.Z && moon.velocity.z == 0)) zCycle = step;
            }
            return MathHelper.LCM(new List<BigInteger> { xCycle, yCycle, zCycle });
        }

        public void ResetMoons()
        {
            foreach (var moon in moons) moon.Reset();
        }
    }

    public class JupiterMoon
    {
        public readonly int initialX;
        public readonly int initialY;
        public readonly int initialZ;

        public (int x, int y, int z) velocity;
        public string initialState;

        public JupiterMoon(int x, int y, int z)
        {
            initialX = x;
            initialY = y;
            initialZ = z;
            X = x;
            Y = y;
            Z = z;
            velocity = (0, 0, 0);
            initialState = this.PosToString();
        }

        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }

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

        public void Reset()
        {
            X = initialX;
            Y = initialY;
            Z = initialZ;
            velocity = (0, 0, 0);
        }
    }
}
