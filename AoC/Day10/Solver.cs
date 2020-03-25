using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AoC.Common;
using AoC.Utils;

namespace AoC.Day10
{
    class Solver : PuzzleSolver
    {
        AsteroidField afield;
        List<AsteroidVisionSet> avsets;
        AsteroidVisionSet bestAVSet;

        public Solver() : this(Input.InputMode.Embedded, "input")
        {
        }

        public Solver(Input.InputMode mode, string input) : base(mode, input)
        {
        }

        protected override void ParseInput(string input)
        {
            //plzfix
            var lines = InputParser.Split(input);
            afield = new AsteroidField(lines);
        }

        protected override void PrepareSolution()
        {
            avsets = afield.GetAsteroidVisionSets();
            bestAVSet = avsets.Select(set => (set, set.VisibleAsteroidsAmount())).Aggregate((a, b) => a.Item2 > b.Item2 ? a : b).Item1;
        }

        protected override void SolvePartOne()
        {
            resultPartOne = bestAVSet.VisibleAsteroidsAmount().ToString();
        }

        protected override void SolvePartTwo()
        {
            Asteroid twohundredthAsteroid = bestAVSet.NthAsteroidHitByLaser(199);
            resultPartTwo = (twohundredthAsteroid.X * 100 + twohundredthAsteroid.InvertedY).ToString();
        }
    }

    class AsteroidField
    {
        List<Asteroid> asteroids;

        public AsteroidField(IEnumerable<string> input)
        {
            List<string> asteroidRows = input.Reverse().ToList(); //Y axis starts at bottom of input

            asteroids = new List<Asteroid>();
            for (int y = 0; y < asteroidRows.Count; y++)
            {
                for (int x = 0; x < asteroidRows[0].Length; x++)
                {
                    if (asteroidRows[y][x] == '#') asteroids.Add(new Asteroid(x, y, asteroidRows.Count));
                }
            }
        }

        public List<AsteroidVisionSet> GetAsteroidVisionSets()
        {
            List<AsteroidVisionSet> avsets = new List<AsteroidVisionSet>();
            for (int i = 0; i < asteroids.Count; i++)
            {
                AsteroidVisionSet avset = new AsteroidVisionSet(asteroids[i], asteroids.Take(i).Concat(asteroids.Skip(i + 1)));
                avsets.Add(avset);
            }
            return avsets;
        }
    }

    class AsteroidVisionSet
    {
        public Asteroid Origin;
        List<Asteroid> otherRoids;
        List<(int x, int y)> distFromOrigin;
        List<(int x, int y)> smallestSteps;
        List<double> angles;
        List<List<int>> sortedIds;
        List<int> bucketSizes;

        public AsteroidVisionSet(Asteroid origin, IEnumerable<Asteroid> others)
        {
            this.Origin = origin;
            otherRoids = new List<Asteroid>(others);

            smallestSteps = otherRoids.Select(a => SmallestStepBetweenAsteroids(origin, a)).ToList();
            angles = smallestSteps.Select(x => StepTo360Angle(x)).ToList();
            Dictionary<double, List<int>> buckets = new Dictionary<double, List<int>>();
            for (int i = 0; i < otherRoids.Count; i++)
            {
                if (!buckets.ContainsKey(angles[i]))
                {
                    buckets.Add(angles[i], new List<int>());
                }
                buckets[angles[i]].Add(i);
            }

            //Sort buckets by distance from origin
            distFromOrigin = otherRoids.Select(a => (origin.X - a.X, origin.Y - a.Y)).ToList();
            foreach (List<int> bucket in buckets.Values)
            {
                bucket.Sort((a, b) => Math.Abs(distFromOrigin[a].x).CompareTo(Math.Abs(distFromOrigin[b].x)));
            }

            sortedIds = buckets.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToList();
            bucketSizes = sortedIds.Select(x => x.Count).ToList();
        }

        public int VisibleAsteroidsAmount()
        {
            return sortedIds.Count;
        }

        public Asteroid NthAsteroidHitByLaser(int n)
        {
            int bucketsRemaining = bucketSizes.Count;
            HashSet<int> breakPoints = bucketSizes.Distinct().ToHashSet();

            int i = 0;
            while (n > bucketsRemaining)
            {
                n = n - bucketsRemaining;
                i++;
                if (breakPoints.Contains(i))
                {
                    bucketsRemaining = breakPoints.Count(b => b > i);
                }
            }

            for (int j = 0; j < sortedIds.Count; j++)
            {
                if (n == 0) return otherRoids[sortedIds[j][i]];
                if (sortedIds[j].Count >= i)
                {
                    n--;
                }
            }

            throw new Exception("Bad Implementation, shouldn't be possible to reach this point");
        }

        public static (int x, int y) SmallestStepBetweenAsteroids(Asteroid a1, Asteroid a2)
        {
            int diffX = a2.X - a1.X;
            int diffY = a2.Y - a1.Y;

            int divideBy = (int)MathHelper.GCD(diffX, diffY);

            return (diffX / divideBy, diffY / divideBy);
        }

        public static double StepTo360Angle((int x, int y) step)
        {
            //y-axis important!! up or down increase value????
            double resultingAngle = Math.Atan2(step.y, step.x) * 180 / Math.PI;
            resultingAngle = resultingAngle * -1;
            resultingAngle = (resultingAngle + 450) % 360;
            return resultingAngle;
        }
    }

    public struct Asteroid
    {
        public Asteroid(int x, int y, int yLim)
        {
            X = x;
            Y = y;
            InvertedY = yLim - 1 - y;
        }

        public int InvertedY { get; }
        public int X { get; }
        public int Y { get; }


        public override string ToString() => $"({X}, {Y})";
    }
}
