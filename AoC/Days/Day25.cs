using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

namespace AoC
{
    static class Day25
    {
        public static int SolvePartOne()
        {
            string input = InputReader.StringFromLine("d25input.txt");
            List<BigInteger> program = input.Split(',').Select(x => BigInteger.Parse(x)).ToList();
            Console.Clear();
            //ScoutDroid r2d2 = new ScoutDroid(program);
            ManualASCIIComputer rd22 = new ManualASCIIComputer(program);
            //Did this manually
            //Strategy find all items that are safe to pick up
            //Bring all items to the checkpoint
            //Try each individual item to single out items which are too heavy to be in a good solution
            //Put all remaining items together, remove individual items to find which items are essential to a solution (too light when removed)
            //Do an Apriori like algorithm on the remaining items
            rd22.RunManual();
            return 0;
        }

        public static int SolvePartTwo()
        {
            return 0;
        }
    }

    class ASCIIComputer
    {
        BigIntCode bic;

        public ASCIIComputer(IEnumerable<BigInteger> input)
        {
            List<BigInteger> program = new List<BigInteger>(input);
            bic = new BigIntCode(program);
        }

        public string Run()
        {
            return Run(new List<BigInteger>());
        }

        public string Run(string command)
        {
            return Run(ASCIIHelper.StringToASCIIBI(command, true));
        }

        public string Run(IEnumerable<BigInteger> input)
        {
            return Run(new List<BigInteger>(input));
        }

        public string Run(List<BigInteger> input)
        {
            List<BigInteger> output = bic.Run(input);
            return string.Concat(output.Select(x => (char)x));
        }
    }

    class ManualASCIIComputer : ASCIIComputer
    {
        public ManualASCIIComputer(IEnumerable<BigInteger> program) : base(program) { }

        public void RunManual()
        {
            Console.WriteLine(Run());
            while (true)
            {
                string nextCommand = Console.ReadLine();
                Console.WriteLine(Run(nextCommand));
            }
        }
    }

    class DroidSquad
    {
        Dictionary<string, string> itemsToRooms;
        Dictionary<string, Node> roomNameToNode;
    }

    class ScoutDroid : ASCIIComputer
    {
        Room currentRoom;
        FloorPlan knownRooms;

        public ScoutDroid(IEnumerable<BigInteger> programming) : base(programming)
        {
            knownRooms = new FloorPlan();
            string output = Run();
            knownRooms.Update(AnalyzeSurroundings(output));
        }

        public void Explore()
        {
            while (knownRooms.HasUnexploredDoors.Any())
            {
                FloorPlan.GetPathTo(currentRoom.Name, knownRooms.HasUnexploredDoors.Last());
            }
        }

        public List<(string content, string tag)> AnalyzeSurroundings(string surroundings)
        {
            string[] lines = surroundings.Split("\n");
            List<(string content, string tag)> analysis = new List<(string content, string tag)>();
            int i = 0;
            while (i < 3)
            {
                if (lines[i] != "") throw new Exception("Unexpected line");
                i++;
            }
            if (lines[i].First() == '=')
            {
                analysis.Add((lines[i], "Name"));
                analysis.Add((lines[i + 1], "Flavor"));
                i += 4;
            }
            else
            {
                throw new Exception("Not a room");
            }
            if (lines[i - 1] != "Doors here lead:") throw new Exception("Unexpected line");
            while (lines[i] != "")
            {
                string door = lines[i].Substring(2);
                analysis.Add((door, "Door"));
                i++;
            }

            if (lines[i + 1] == "Items here:")
            {
                i += 2;
                while (lines[i] != "")
                {
                    string item = lines[i].Substring(2);
                    analysis.Add((item, "Item"));
                    i++;
                }
            }

            if (lines[i + 1] == "Command?") return analysis;
            throw new Exception("Unexpected surroundings");
        }
    }

    class FloorPlan
    {
        Dictionary<string, Room> rooms;
        public List<string> HasUnexploredDoors { get; private set; }

        public FloorPlan()
        {
            rooms = new Dictionary<string, Room>();
            HasUnexploredDoors = new List<string>();
        }

        internal Room Update(List<(string content, string tag)> roomInfo)
        {
            string name = roomInfo.Find(x => x.tag == "Name").content;
            if (rooms.ContainsKey(name))
            {
                //WE ALREADY KNOW THIS ROOM??
                return rooms[name];
            }
            else
            {
                Room newRoom = new Room(roomInfo);
                rooms.Add(name, newRoom);
                HasUnexploredDoors.Add(name);
                return newRoom;
            }
        }

        internal void Update(List<(string content, string tag)> roomInfo, string previousRoom, string moveOrder)
        {
            Room updateRoom = Update(roomInfo);
            //Update previous rooms knowledge
            bool prevRoomExploredCompletely = rooms[previousRoom].UpdateDoorKnowledge(moveOrder, updateRoom.Name);
            if (prevRoomExploredCompletely) HasUnexploredDoors.Remove(previousRoom);

            //Update new rooms knowledge
            bool exploredFully = updateRoom.UpdateDoorKnowledge(OppositeDirection(moveOrder), previousRoom);
            if (exploredFully) HasUnexploredDoors.Remove(updateRoom.Name);
        }

        public string OppositeDirection(string direction)
        {
            if (direction == "east") return "west";
            if (direction == "west") return "east";
            if (direction == "north") return "south";
            if (direction == "south") return "north";
            throw new Exception("not a valid direction");
        }

        internal static void GetPathTo(string from, string to)
        {
            //BFS 
            throw new NotImplementedException();
        }
    }

    class Room
    {
        public string Name { get; private set; }
        string flavorText;
        List<string> items;
        Dictionary<string, string> doors;

        public Room(List<(string content, string tag)> roomInfo)
        {
            items = new List<string>();
            doors = new Dictionary<string, string>();
            foreach (var info in roomInfo)
            {
                if (info.tag == "Name") Name = info.content;
                if (info.tag == "Flavor") flavorText = info.content;
                if (info.tag == "Item") items.Add(info.content);
                if (info.tag == "Door") doors.Add(info.content, "unexplored");
            }
        }

        public bool UpdateDoorKnowledge(string direction, string roomName)
        {
            if (doors[direction] != "unexplored") throw new Exception("Already knew this one");
            doors[direction] = roomName;
            return !doors.ContainsValue("unexplored");
        }
    }
}

