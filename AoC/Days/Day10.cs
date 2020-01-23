using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC
{
    static class Day10
    {
        public static int SolvePartOne()
        {
            string[] input = InputReader.StringsFromTxt("d10input.txt");
            char[,] asteroids = new char[input[0].Length, input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                char[] chars = input[i].ToCharArray();
                for (int j = 0; j < chars.Length; j++)
                {
                    asteroids[j, i] = chars[j];
                }
            }

            int width = asteroids.GetLength(0);
            int height = asteroids.GetLength(1);

            int[,] viewCounts = new int[width, height];

            int mostVisible = 0;
            int bestx = -1;
            int besty = -1;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (asteroids[x, y] == '#')
                    {
                        bool[,] checkedAsteroids = new bool[width, height];
                        checkedAsteroids[x, y] = true;
                        for (int otherx = 0; otherx < width; otherx++)
                        {
                            for (int othery = 0; othery < height; othery++)
                            {
                                if (!checkedAsteroids[otherx, othery] && asteroids[otherx, othery] == '#')
                                {
                                    //Pretend this one as visible, even if its not
                                    //Find all other positions on the same line of sight and mark them checked (these can no longer be counted visible, even if they are)
                                    viewCounts[x, y]++;
                                    List<(int, int)> losPoints = GetLineOfSight((x, y), (otherx, othery), (width, height));
                                    foreach ((int x, int y) lp in losPoints)
                                    {
                                        checkedAsteroids[lp.x, lp.y] = true;
                                    }
                                }
                            }
                        }
                        if (viewCounts[x, y] > mostVisible)
                        {
                            mostVisible = viewCounts[x, y];
                            bestx = x;
                            besty = y;
                        }
                    }
                }
            }

            PrintViewCounts(viewCounts);
            Console.WriteLine($"({bestx},{besty})");
            return mostVisible;
        }

        private static void PrintViewCounts(int[,] viewCounts)
        {
            for (int i = 0; i < viewCounts.GetLength(0); i++)
            {
                string bob = "";
                for (int j = 0; j < viewCounts.GetLength(1); j++)
                {
                    bob = bob + (viewCounts[i, j] > 0 ? viewCounts[i, j].ToString() : ".");
                }
                Console.WriteLine(bob);
            }
        }

        private static List<(int, int)> GetLineOfSight((int x, int y) p1, (int x, int y) p2, (int x, int y) limits)
        {
            List<(int, int)> losPoints = new List<(int, int)> { };

            (int x, int y) diff = (Math.Abs(p1.x - p2.x), Math.Abs(p1.y - p2.y));

            //Find smallest angle
            int divBy;
            int smallestDiff = diff.x < diff.y ? diff.x : diff.y;
            if (smallestDiff == 0)
            {
                divBy = Math.Max(diff.x, diff.y);
            }
            else
            {
                divBy = 1;
                for (int i = 2; i <= smallestDiff; i++)
                {
                    if (diff.x % i == 0 && diff.y % i == 0)
                    {
                        divBy = i;
                    }
                }
            }

            (int x, int y) angle = ((p1.x - p2.x) / divBy, (p1.y - p2.y) / divBy);

            //bool withinBoundsPlus = true;
            bool withinBoundsMinus = true;
            int c = 1;
            while (/*withinBoundsPlus ||*/ withinBoundsMinus)
            {
                //withinBoundsPlus = p1.x + angle.x * c >= 0 &&
                //                    p1.y + angle.y * c >= 0 &&
                //                    p1.x + angle.x * c < limits.x &&
                //                    p1.y + angle.y * c < limits.y;
                withinBoundsMinus = p1.x + angle.x * -c >= 0 &&
                                    p1.y + angle.y * -c >= 0 &&
                                    p1.x + angle.x * -c < limits.x &&
                                    p1.y + angle.y * -c < limits.y;
                //if (withinBoundsPlus)
                //{
                //    losPoints.Add((p1.x + angle.x * c, p1.y + angle.y * c));
                //}
                if (withinBoundsMinus)
                {
                    losPoints.Add((p1.x + angle.x * -c, p1.y + angle.y * -c));
                }
                c++;
            }
            return losPoints;
        }

        public static int SolvePartTwo()
        {
            string[] input = InputReader.StringsFromTxt("d10input.txt");
            char[,] asteroids = new char[input[0].Length, input.Length];
            (int x, int y) laser = (22,19);//22,19

            for (int i = 0; i < input.Length; i++)
            {
                char[] chars = input[i].ToCharArray();
                for (int j = 0; j < chars.Length; j++)
                {
                    asteroids[j, i] = chars[j];
                }
            }
            asteroids[laser.x, laser.y] = 'X';//Laser location

            int width = asteroids.GetLength(0);
            int height = asteroids.GetLength(1);

            double[,] angleDegs = new double[width, height];
            int[,] manhattans = new int[width, height];
            Dictionary<double, List<(int, int, int)>> asteroidsAtAnglesManhattan = new Dictionary<double, List<(int, int,int)>>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (asteroids[x, y] == '#')
                    {
                        (int x, int y) diff = ((x - laser.x), (y - laser.y));
                        (int x, int y) smallDiff = FindSmallestAngle((x, y), laser, diff);
                        angleDegs[x, y] = DifferenceToAngle(smallDiff);
                        manhattans[x, y] = Math.Abs(x - laser.x) + Math.Abs(y - laser.y);
                        if (asteroidsAtAnglesManhattan.ContainsKey(angleDegs[x, y]))
                        {
                            asteroidsAtAnglesManhattan[angleDegs[x, y]].Add((x, y, manhattans[x, y]));
                        } else
                        {
                            asteroidsAtAnglesManhattan.Add(angleDegs[x,y],new List<(int, int, int)> { (x, y, manhattans[x, y]) });
                        }
                    }
                }
            }

            List<double> sortedAngles = asteroidsAtAnglesManhattan.Keys.OrderBy(x => x).ToList();
            List<(int, int)> destroyed = new List<(int, int)>();
            string[,] destroyedCount = new string[width, height];
            for(int i = 0; i<height; i++)
            {
                for(int j = 0; j < width; j++)
                {
                    destroyedCount[j, i] = "0";
                }
            }

            while (destroyed.Count < 200)
            {
                foreach (double angle in sortedAngles)
                {
                    if (asteroidsAtAnglesManhattan[angle].Count > 0)
                    {
                        (int x, int y, int dist) todestroy = asteroidsAtAnglesManhattan[angle].Aggregate((a, b) => a.Item3 > b.Item3 ? b : a);
                        destroyed.Add((todestroy.x, todestroy.y));
                        destroyedCount[todestroy.x, todestroy.y] = destroyed.Count.ToString();
                        asteroidsAtAnglesManhattan[angle].Remove(todestroy);
                    }
                }
            }

            List<string> boblist = new List<string>();
            for (int y = 0; y < height; y++)
            {
                string bob = "";
                for (int x = 0; x < width; x++)
                {
                    if (x == laser.x && y == laser.y)
                    {
                        bob = bob + "|XXX|";
                    }
                    else
                    {
                        //bob = bob + (angleDegs[x, y] == 0 ? "|___|" : $"|{RoundToSignificantDigits(angleDegs[x, y], 3).ToString()}|");
                        bob = bob + (destroyedCount[x, y] == "0" ? "|___|" : $"|{destroyedCount[x, y].ToString().PadLeft(3,' ')}|");
                    }
                }
                boblist.Add(bob);
            }
            System.IO.File.WriteAllLines(@"../../../Resources/bob.txt", boblist.ToArray());
            return destroyed[199].Item1 * 100 + destroyed[199].Item2;
        }

        public static double DifferenceToAngle((int x, int y) p)
        {
            //pythagoras
            //double hypo = Math.Sqrt(Math.Pow(p.x, 2) + Math.Pow(p.y, 2));

            //double resultingAngle = Math.Asin(p.x / hypo) * 180 / Math.PI;
            double resultingAngle = Math.Atan2(p.y, p.x) * 180 / Math.PI;
            //if (p.y > 0)
            //{
            //    resultingAngle = 180 - resultingAngle;
            //}
            if (p.y < 0 && p.x < 0)
            {
                resultingAngle = 360 + resultingAngle;
            }
            return resultingAngle +90;
        }
        public static double RoundToSignificantDigits(this double d, int digits)
        {
            if (d == 0)
                return 0;

            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);
            return scale * Math.Round(d / scale, digits);
        }

        public static (int x,int y) FindSmallestAngle((int x, int y) p1, (int x, int y)p2, (int x, int y) diff)
        {
            //Find smallest angle
            int divBy;
            int smallestDiff = diff.x < diff.y ? diff.x : diff.y;
            if (smallestDiff == 0)
            {
                divBy = Math.Max(diff.x, diff.y);
            }
            else
            {
                divBy = 1;
                for (int i = 2; i <= smallestDiff; i++)
                {
                    if (diff.x % i == 0 && diff.y % i == 0)
                    {
                        divBy = i;
                    }
                }
            }

            return ((p1.x - p2.x) / divBy, (p1.y - p2.y) / divBy);
        }
    }



    public class Asteroid
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Asteroid(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}

