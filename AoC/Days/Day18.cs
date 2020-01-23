using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC
{
    static class Day18
    {
        public static ColorGrid KillDeadEnds(ColorGrid ogcg)
        {
            ColorGrid cg = ogcg.CopyMe();
            var dots = cg.FindAll(".");
            List<(int x, int y)> killMe = new List<(int x, int y)>();
            do
            {
                killMe = new List<(int x, int y)>();
                foreach (var dot in dots)
                {
                    if (cg.HasNeighbour4(dot.x, dot.y, "#", 3))
                    {
                        killMe.Add(dot);
                    }
                }
                foreach (var dot in killMe)
                {
                    cg.SetColorAt(dot.x, dot.y, "#");
                    dots.RemoveAt(dots.FindIndex(p => p.x == dot.x && p.y == dot.y));
                }
            } while (killMe.Any());

            List<string> output = cg.GetImageStrings(true);
            System.IO.StreamWriter sw = new System.IO.StreamWriter("../../../Resources/nodead183.txt");
            foreach(var line in output)
            {
                sw.WriteLine(line);
            }
            sw.Close();
            return cg;
        }

        public static ColorGrid RemoveTrapDoors(ColorGrid ogcg)
        {
            ColorGrid cg = ogcg.CopyMe();
            List<string> keys = "abcdefghijklmnopqrstuvwxyz".ToCharArray().Select(x => x.ToString()).ToList();
            List<string> doorColors = keys.Select(k => k.ToUpper()).ToList();
            var doors = cg.FindAll(doorColors);
            List<(int x, int y)> removeDoors = new List<(int x, int y)>();
            do
            {
                removeDoors = new List<(int x, int y)>();
                foreach (var door in doors)
                {
                    if (cg.HasNeighbour4(door.x, door.y, "#", 3))
                    {
                        removeDoors.Add(door);
                    }
                }
                foreach (var door in removeDoors)
                {
                    cg.SetColorAt(door.x, door.y, "#");
                    doors.RemoveAt(doors.FindIndex(p => p.x == door.x && p.y == door.y));
                }
            } while (removeDoors.Any());

            List<string> output = cg.GetImageStrings(true);
            System.IO.StreamWriter sw = new System.IO.StreamWriter("../../../Resources/nodeadnotrap182.txt");
            foreach (var line in output)
            {
                sw.WriteLine(line);
            }
            sw.Close();
            return cg;
        }

        public static List<string> ReachableBy(ColorGrid cg, string fromTile, List<string> lookingFor, List<string> walkable)
        {
            ColorGrid mapCopy = cg.CopyMe();
            List<(int, string, int, int)> keysFound = new List<(int, string, int, int)>();
            List<string> legalTypes = new List<string>(walkable);
            legalTypes.AddRange(lookingFor);
            legalTypes.RemoveAt(legalTypes.FindIndex(x => x == fromTile));
            List<(int x, int y)> legalTiles = mapCopy.FindAll(legalTypes);
            bool hasMoved = true;
            int steps = 1;
            if (fromTile == "e") System.Diagnostics.Debugger.Break();
            while (hasMoved)
            {
                List<(int x, int y)> reached = new List<(int x, int y)>();
                List<(int x, int y)> keysAdded = new List<(int x, int y)>();
                foreach ((int x, int y) point in legalTiles)
                {
                    if (mapCopy.HasNeighbour4(point.x, point.y, fromTile))
                    {
                        string tileColor = mapCopy.GetColorAt(point.x, point.y);
                        if (lookingFor.Contains(tileColor))
                        {
                            //WE FOUND A KEY HERE
                            keysFound.Add((steps, tileColor, point.x, point.y));
                            keysAdded.Add(point);
                        } else
                        {
                            reached.Add(point);
                        }
                    }
                }

                foreach ((int x, int y) point in reached)
                {
                    mapCopy.SetColorAt(point.x, point.y, fromTile);
                    legalTiles.RemoveAt(legalTiles.FindIndex(p => p.x == point.x && p.y == point.y));
                }

                //if (fromTile == "e")
                //{
                //    Console.Clear();
                //    foreach (var line in mapCopy.GetImageStrings())
                //    {
                //        Console.WriteLine(line);
                //    }
                //}
                foreach ((int x, int y) point in keysAdded)
                {
                    legalTiles.RemoveAt(legalTiles.FindIndex(p => p.x == point.x && p.y == point.y));
                }

                steps++;
                hasMoved = reached.Any();
            }
            if (keysFound.Select(x => x.Item2).Distinct().ToList().Contains("@")) return new List<string> { "@" };
            return keysFound.Select(x => x.Item2).Distinct().ToList();
        }

        public static void FindDoorsBlockingKeys(ColorGrid og)
        {
            List<string> keys = "abcdefghijklmnopqrstuvwxyz".ToCharArray().Select(x => x.ToString()).ToList();
            List<string> doors = keys.Select(k => k.ToUpper()).ToList();

            List<string> walkable = new List<string> { ".", "@" };
            walkable.AddRange(keys);

            List<string> searchingFor = new List<string> { "@" };
            searchingFor.AddRange(doors);

            List<string> output = new List<string>();
            foreach (var key in keys)
            {
                List<string> myDoors = ReachableBy(og, key, searchingFor, walkable);
                output.Add($"{key}: {String.Join('|', myDoors)}");
            }
            System.IO.StreamWriter sw = new System.IO.StreamWriter("../../../Resources/blockedby18.txt");
            foreach (var line in output)
            {
                sw.WriteLine(line);
            }
            sw.Close();
        }

        public static List<(string branch,string label,int steps)> FindGraphStructure(ColorGrid cg, string from)
        {
            ColorGrid grid = cg.CopyMe();
            List<string> blockers = new List<string> { "@", "#", from };
            List<string> keys = "abcdefghijklmnopqrstuvwxyz".ToCharArray().Select(x => x.ToString()).ToList();
            List<string> doorColors = keys.Select(k => k.ToUpper()).ToList();
            List<string> allDoorsAndKeys = new List<string>(keys);
            allDoorsAndKeys.AddRange(doorColors);
            List<string> walkable = new List<string> { "." };
            walkable.AddRange(allDoorsAndKeys);

            var potentials = grid.FindAll(walkable);
            int steps = 1;
            bool hasMoved = true;

            List<(string branch, string label, int steps)> nodes = new List<(string branch, string label, int steps)>();
            List<string> branches = new List<string> { from };
            while (hasMoved)
            {
                hasMoved = false;
                List<string> newBranches = new List<string>();
                foreach (var branch in branches)
                {
                    List<string> myNewBranches = new List<string>();
                    List<(int x, int y)> reached = new List<(int, int)>();
                    foreach (var point in potentials)
                    {
                        if (grid.HasNeighbour4(point.x, point.y, branch))
                        {
                            string label = grid.GetColorAt(point.x, point.y);
                            if ( label != ".")
                            {
                                nodes.Add((branch, label, steps));
                            }
                            reached.Add(point);
                        }
                    }
                    if (reached.Count == 1)
                    {
                        myNewBranches.Add(branch);
                        grid.SetColorAt(reached[0].x, reached[0].y, branch);
                        potentials.RemoveAt(potentials.FindIndex(p => p.x == reached[0].x && p.y == reached[0].y));
                    }
                    if (reached.Count > 1)
                    {
                        nodes.Add((branch, "intx", steps-1));
                        for(int i = 1; i <= reached.Count; i++)
                        {
                            myNewBranches.Add($"{branch}:{i}");
                        }
                        for (int i = 0; i < reached.Count; i++)
                        {
                            grid.SetColorAt(reached[i].x, reached[i].y, myNewBranches[i]);
                            potentials.RemoveAt(potentials.FindIndex(p => p.x == reached[i].x && p.y == reached[i].y));
                        }
                    }

                    newBranches.AddRange(myNewBranches);
                    hasMoved = hasMoved || reached.Any();
                }
                steps++;
                //Console.Clear();
                //foreach (var line in grid.GetImageStrings())
                //{
                //    Console.WriteLine(line);
                //}
                branches = newBranches;

            }
            return nodes;
        }

        public static int HomeMade(ColorGrid cg, string mySolution)
        {
            //string mySolution = "@qaiwesctuxrvzbmohjlfkdypgn"; //3166
            //string mySolution = "@qcxaiwesturvzbmohjlfkdypgn"; //3146
            //string mySolution = "@qcxaiwesturvzbmohjlfkdypgn"; //3146
            //string mySolution = "@qcaiwesxtuvrzbmohjlfkdypgn"; //3146 best?
            //string mySolution = "@qcxaiwestuvrzbmohjlfkdypgn"; //3146 best?
            //string mySolution = "@qcaiwestuzmohjlfkdypgnxvrb"; //

            //string mySolution = "@cqxrvsaiwetuzbmohjlfkdypgn"; //3170
            List<string> myPath = mySolution.ToCharArray().Select(nd => nd.ToString()).ToList();
            List<(string, int)> myPathWithLengths = new List<(string, int)>();
            List<string> passableTerrain = new List<string> { "." };
            passableTerrain.AddRange(myPath);
            int distance = 0;
            for(int i = 1; i<myPath.Count; i++)
            {
                (bool found, int dist) = cg.DistanceBetween(myPath[i - 1], myPath[i], passableTerrain);
                if (found)
                {
                    distance += dist;
                    myPathWithLengths.Add((myPath[i], dist));
                    passableTerrain.Add(myPath[i].ToUpper());
                } else
                {
                    throw new Exception("Trying to go through unreachable key");
                }
            }
            return distance;
        }

        public static int SolvePartOne()
        {
            List<string> mazeInput = InputReader.StringsFromTxt("jdog18.txt").ToList();
            ColorGrid og = new ColorGrid("#");
            for(int y = 0; y < mazeInput.Count; y++)
            {
                string row = mazeInput[y];
                for (int x = 0; x < row.Length; x++)
                {
                    og.GetColorAt(x, y);
                    if (row[x].ToString() != "#")
                    {
                        og.SetColorAt(x, y, row[x].ToString());
                    }
                }
            }

            //var structures = new List<int> { 1,2,3,4}.Select( n => FindGraphStructure(og, n.ToString())).ToList();
            //var cg = RemoveTrapDoors(og);
            //KillDeadEnds(cg);
            string mypath = "@crwsbtjyniofqapgxkdhlzmveu";
            HomeMade(og, mypath);
            //string mypath2 = "@zqsaiwecxvrtubmohjlfkdypgn";

            //string mypath3 = "@qcaiwesxtuvrzbmohjlfkdypgn";
            //HomeMade(og, mypath3);
            //FindDoorsBlockingKeys(og);
            //KillDeadEnds(og);

            (bool _, int x, int y) myLoc = og.Find("@");
            KeyNode root = new KeyNode(og, myLoc.x, myLoc.y);
            return root.bestSubSol.dist;
        }

        public static int SolvePartTwo()
        {
            return 0;
        }
    }

    public class KeyNode{
        internal int xPos;
        internal int yPos;
        internal List<string> path;
        internal int pathLength; 
        internal int bestFound;
        internal ColorGrid map;
        internal KeyNode parent; 
        internal (int dist, List<string> path) bestSubSol;

        public KeyNode(ColorGrid initialMap, int x, int y){
            xPos = x;
            yPos = y;
            pathLength = 0;
            parent = null; //ONLY USE THIS CONSTRUCTOR FOR ROOT NODES
            path = new List<string>();
            bestFound = int.MaxValue;
            map = initialMap.CopyMe();
            bestSubSol = TryAllReachableKeys();
        }

        public KeyNode(KeyNode parent, (int dist, string letter, int x, int y) key){
            this.parent = parent;
            pathLength = parent.pathLength + key.dist;
            xPos = key.x;
            yPos = key.y;
            path = new List<string>(parent.path);
            path.Add(key.letter);
            bestFound = parent.bestFound;
            map = UnlockDoor(key);
            bestSubSol = TryAllReachableKeys();
        }

        public (int, List<string>) TryAllReachableKeys(){
            List<(int, string, int, int)> reachableKeys = FindAccessibleKeys().OrderBy(a => a.Item1).ToList();//Slightly faster if we always try shortest first
            List<string> bestPath = path;
            if (reachableKeys.Any())
            {
                foreach (var reachable in reachableKeys)
                {
                    IEnumerable<string> allKeys = "nmse".ToCharArray().Select(nd => nd.ToString());
                    IEnumerable<string> openKeys = allKeys.Where(x => !path.Contains(x));
                    int minDist = reachable.Item1 + pathLength + FurthestUnreachedKey(reachable.Item2, openKeys);
                    //int minDist = reachable.Item1 + pathLength;
                    if (minDist < bestFound)
                    {
                        KeyNode child = new KeyNode(this, reachable);
                        (int dis, List<string> p) result = child.ShowMeWhatYouGot();
                        if (result.dis < bestFound)
                        {
                            bestFound = result.dis;
                            bestPath = result.p;
                        }
                    }
                }
            }
            else
            {
                bestFound = pathLength;
            }
            return (bestFound,bestPath);
        }

        public (int, List<string>) ShowMeWhatYouGot(){
            return bestSubSol;
        }

        public List<(int, string, int, int)> FindAccessibleKeys(){
            ColorGrid mapCopy = map.CopyMe();
            List<(int, string, int, int)> keysFound = new List<(int, string, int, int)>();
            List<string> legalTypes = ".abcdefghijklmnopqrstuvwxyz".ToCharArray().Select(x => x.ToString()).ToList();
            List<(int x,int y)> legalTiles = mapCopy.FindAll(legalTypes);
            bool hasMoved = true;
            int steps = 1;
            while(hasMoved){
                List<(int x, int y)> reached = new List<(int x, int y)>();
                foreach((int x, int y) point in legalTiles)
                {
                    if (mapCopy.HasNeighbour4(point.x, point.y, "@"))
                    {
                        reached.Add(point);
                        string tileColor = mapCopy.GetColorAt(point.x,point.y);
                        if(tileColor != "."){
                            //WE FOUND A KEY HERE
                            keysFound.Add((steps, tileColor, point.x, point.y ));
                        }
                    }
                }

                foreach((int x, int y) point in reached)
                {
                    mapCopy.SetColorAt(point.x, point.y, "@");
                    legalTiles.RemoveAt(legalTiles.FindIndex(p => p.x == point.x && p.y == point.y));
                }
                steps++;
                hasMoved = reached.Any();
            }
            return keysFound;
        }

        public int FurthestUnreachedKey(string from, IEnumerable<string> unreachedKeys)
        {
            List<string> allowed = map.allColors.Where(x => x != "#").ToList();
            return unreachedKeys.Select(x => map.DistanceBetween(from, x, allowed)).Select(x => x.Item1 ? x.Item2 : 0).Aggregate(0, (a,b) => a < b ? b : a);
        }
        
        private ColorGrid UnlockDoor((int dist, string letter, int x, int y) key){
            ColorGrid mapCopy = parent.map.CopyMe();
            mapCopy.SetColorAt(parent.xPos,parent.yPos, "."); //Leave a dot at my old position
            mapCopy.SetColorAt(key.x,key.y, "@"); //Place my character on the new position
            (bool _, int x, int y) door = mapCopy.Find(key.letter.ToUpper());
            mapCopy.SetColorAt(door.x,door.y, ".");//Unlock the door;
            return mapCopy;
        }

        public override string ToString(){
            return path.Last();
        }
    }
}

