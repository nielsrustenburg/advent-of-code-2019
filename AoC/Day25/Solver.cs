using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;
using AoC.Common;
using AoC.Computers;
using AoC.Utils;
using AoC.Utils.AStar;

namespace AoC.Day25
{
    class Solver : PuzzleSolver
    {
        List<BigInteger> program;
        public static readonly string[] blacklistItems = { "giant electromagnet",
                                                           "escape pod",
                                                            "infinite loop",
                                                            "photons",
                                                            "molten lava"};

        public Solver() : this(Input.InputMode.Embedded, "input")
        {
        }

        public Solver(Input.InputMode mode, string input) : base(mode, input)
        {
        }

        protected override void ParseInput(string input)
        {
            program = InputParser<BigInteger>.ParseCSVLine(input, BigInteger.Parse).ToList();
        }

        protected override void PrepareSolution()
        {
            //not even a part 2 this time :D
        }

        protected override void SolvePartOne()
        {
            var droid = new Droid(program);
            var shipGraph = ExploreAndTakeItems(droid);
            MoveDroidToSecurityCheckpoint(droid, shipGraph);
            resultPartOne = BreachSecurity(droid,shipGraph);
        }

        protected override void SolvePartTwo()
        {
            resultPartTwo = "no part two";
        }

        enum RequiredWeightAdjustment
        {
            Heavier,
            Lighter,
            None
        }

        private string BreachSecurity(Droid droid, ShipGraph shipGraph)
        {
            var pressurePlate = shipGraph.GetUnexploredDoorsOf(droid.Room).First();
            string password = "this should be filled in by the time we return";
            var allItems = droid.Inventory;

            droid.DropItems(allItems);

            //Try each item individually
            //Remember items that are too heavy these are useless
            var uselessItems = new HashSet<string>();
            foreach (var item in allItems)
            {
                droid.TakeItem(item);
                var weightAdjustment = TryBreach();
                if (weightAdjustment == RequiredWeightAdjustment.None) return password;
                else if (weightAdjustment == RequiredWeightAdjustment.Lighter)
                {
                    uselessItems.Add(item);
                }
                droid.DropItem(item);
            }

            var remainingItems = allItems.Except(uselessItems);
            droid.TakeItems(remainingItems);

            //Try removing each item individually
            //Remember which items make the droid too light when dropped (these are essential)
            var essentialItems = new HashSet<string>();
            foreach (var item in remainingItems)
            {
                droid.DropItem(item);
                var weightAdjustment = TryBreach();
                if (weightAdjustment == RequiredWeightAdjustment.None) return password;
                else if (weightAdjustment != RequiredWeightAdjustment.Lighter)
                {
                    essentialItems.Add(item);
                }
                droid.TakeItem(item);
            }

            var undecidedItems = remainingItems.Except(essentialItems);
            droid.DropItems(undecidedItems);

            //Try all combinations of remaining items 
            //(if this is an extremely large number of combinations I suggest using a priori algorithm instead, but in practice this is not needed)
            var itemCombinations = SetHelper.Subsets(undecidedItems);
            foreach(var combination in itemCombinations)
            {
                droid.TakeItems(combination);
                var weightAdjustment = TryBreach();
                if (weightAdjustment == RequiredWeightAdjustment.None) return password;
                droid.DropItems(combination);
            }

            RequiredWeightAdjustment TryBreach()
            {
                droid.TakeDoor(pressurePlate);
                var responseLines = droid.LastResponse.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                var weightMessage = responseLines[4];
                if (weightMessage == "A loud, robotic voice says \"Alert! Droids on this ship are heavier than the detected value!\" and you are ejected back to the checkpoint.") return RequiredWeightAdjustment.Heavier;
                if (weightMessage == "A loud, robotic voice says \"Alert! Droids on this ship are lighter than the detected value!\" and you are ejected back to the checkpoint.") return RequiredWeightAdjustment.Lighter;
                password = responseLines[6].Split(' ')[11];
                return RequiredWeightAdjustment.None;
            }
            throw new Exception("breach failed");
        }

        private void MoveDroidToSecurityCheckpoint(Droid droid, ShipGraph shipGraph)
        {
            var (path, _) = GraphSearch.Dijkstra(shipGraph, droid.Room, "== Security Checkpoint ==");
            foreach(var nextRoom in path)
            {
                droid.TakeDoor((Direction)shipGraph.GetDoorDirection(droid.Room, nextRoom));
            }
        }

        private ShipGraph ExploreAndTakeItems(Droid droid)
        {
            var shipGraph = new ShipGraph();
            shipGraph.AddRoom(droid);

            var unexploredRooms = new HashSet<string> { droid.Room };
            var path = new List<Direction>();

            while (unexploredRooms.Any())
            {
                var lastRoom = droid.Room;

                if (unexploredRooms.Contains(droid.Room))
                {
                    droid.TakeItems(droid.FloorItems.Except(blacklistItems));
                    foreach (var door in shipGraph.GetUnexploredDoorsOf(droid.Room))
                    {
                        droid.TakeDoor(door);
                        if (droid.Room != lastRoom)
                        {
                            if (!shipGraph.ContainsNode(droid.Room))
                            {
                                shipGraph.AddRoom(droid);
                                unexploredRooms.Add(droid.Room);
                            }
                            shipGraph.AddDoorKnowledge(lastRoom, droid.Room, door);
                            droid.TakeDoor(door.Opposite());
                        }
                    }
                    unexploredRooms.Remove(lastRoom);
                }
                else
                {
                    var neighbours = shipGraph.Neighbours(droid.Room).Select(node => node.nb);
                    var unexploredNextToCurrent = unexploredRooms.Intersect(neighbours);

                    Direction door;
                    if (unexploredNextToCurrent.Any())
                    {
                        var nextRoom = unexploredNextToCurrent.First();
                        var direction = shipGraph.GetDoorDirection(droid.Room, nextRoom);
                        door = (Direction)direction;
                        path.Add(door);
                    }
                    else
                    {
                        door = path.Last().Opposite();
                        path.RemoveAt(path.Count - 1);
                    }
                    droid.TakeDoor(door);
                }
            }
            return shipGraph;
        }

        public void PlayTheGame()
        {
            Console.Clear();
            ManualASCIIComputer rd22 = new ManualASCIIComputer(program);
            rd22.RunManual();
        }

        class Droid
        {
            ASCIIComputer computer;

            private HashSet<Direction> _doors;
            private HashSet<string> _floorItems;
            private HashSet<string> _inventory;

            public string Room { get; private set; }

            public string LastResponse { get; private set; }
            public HashSet<Direction> Doors { get => _doors.ToHashSet(); private set { _doors = value; } }
            public HashSet<string> FloorItems { get => _floorItems.ToHashSet(); private set { _floorItems = value; } }
            public HashSet<string> Inventory { get => _inventory.ToHashSet(); private set { _inventory = value; } }

            public Droid(IEnumerable<BigInteger> program)
            {
                computer = new ASCIIComputer(program);
                var description = PerformAction("");
                UpdateRoomStatus(description);
                UpdateInventory();
            }

            public void TakeItems(IEnumerable<string> items)
            {
                foreach(var item in items)
                {
                    TakeItem(item);
                }
            }

            public void TakeItem(string item)
            {
                if (!_floorItems.Contains(item)) throw new Exception("trying to pick up unavailable item");
                var description = PerformAction($"take {item}");
                UpdateStatus();

                void UpdateStatus()
                {
                    if (description[0] == $"You take the {item}.")
                    {
                        _inventory.Add(item);
                        _floorItems.Remove(item);
                    } else throw new Exception("Something went wrong while picking up an item");
                }
            }

            public void DropItems(IEnumerable<string> items)
            {
                foreach (var item in items)
                {
                    DropItem(item);
                }
            }

            public void DropItem(string item)
            {
                if (!_inventory.Contains(item)) throw new Exception("trying to drop an item you don't have");
                var description = PerformAction($"drop {item}");
                UpdateStatus();

                void UpdateStatus()
                {
                    if (description[0] == $"You drop the {item}.")
                    {
                        _inventory.Remove(item);
                        _floorItems.Add(item);
                    }else throw new Exception("Something went wrong while dropping an item");
                }
            }

            public void UpdateInventory()
            {
                var description = PerformAction("inv");
                _inventory = GetBulletPointsBelow(description, 0).ToHashSet();
            }

            public void TakeDoor(Direction doorDirection)
            {
                if (!_doors.Contains(doorDirection)) throw new Exception($"room does not have door in {doorDirection.ToString()}");

                var description = PerformAction(doorDirection.ToString().ToLower());
                UpdateRoomStatus(description);
            }

            private void UpdateRoomStatus(string[] description)
            {
                Room = description[0];

                int doorsIndex = Array.IndexOf(description, "Doors here lead:");
                int itemsIndex = Array.IndexOf(description, "Items here:");

                var doorStrings = GetBulletPointsBelow(description, doorsIndex);
                _doors = doorStrings.Select(s => Enum.Parse<Direction>(s.Substring(0, 1).ToUpper() + s.Substring(1))).ToHashSet();

                if (itemsIndex > -1)
                {
                    _floorItems = GetBulletPointsBelow(description, itemsIndex).ToHashSet();
                }
                else
                {
                    _floorItems = new HashSet<string>();
                }
            }

            private string[] PerformAction(string actionString)
            {
                LastResponse = computer.RunString(actionString);
                var lastDescription = LastResponse.Split("\n\n\n", StringSplitOptions.RemoveEmptyEntries).Last();
                return lastDescription.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            }


            private static IEnumerable<string> GetBulletPointsBelow(string[] lines, int index)
            {
                for (int i = index + 1; i < lines.Length; i++)
                {
                    if (lines[i][0] == '-') yield return lines[i].Substring(2);
                    else yield break;
                }

            }
        }

        class ShipGraph : AdjacencyGraph<RoomNode>
        {
            Dictionary<string, string> itemsToRooms;

            public ShipGraph() : base()
            {
                itemsToRooms = new Dictionary<string, string>();
            }

            public Direction? GetDoorDirection(string from, string to)
            {
                return nodes[from].GetDoorDirection(to);
            }

            internal void AddDoorKnowledge(string roomFrom, string roomTo, Direction dir)
            {
                AddEdge(roomFrom, roomTo, 1);
                nodes[roomFrom].AddDoorKnowledge(dir, roomTo);
                nodes[roomTo].AddDoorKnowledge(dir.Opposite(), roomFrom);
            }

            internal void AddRoom(string name, IEnumerable<Direction> doors, IEnumerable<string> items)
            {
                nodes.Add(name, new RoomNode(name, doors, items));

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        itemsToRooms.Add(item, name);
                    }
                }
            }

            internal void AddRoom(Droid droid)
            {
                AddRoom(droid.Room, droid.Doors, droid.FloorItems);
            }

            internal IEnumerable<string> GetRoomItems(string room)
            {
                return nodes[room].GetItems();
            }

            internal IEnumerable<Direction> GetUnexploredDoorsOf(string room)
            {
                return nodes[room].GetUnexploredDoors();
            }
        }

        class RoomNode : AdjacencyGraphNode
        {
            Dictionary<string, Direction> neighbourDirections;
            HashSet<Direction> doors;
            HashSet<string> items;

            public RoomNode(string name, IEnumerable<Direction> doors, IEnumerable<string> items) : base(name)
            {
                neighbourDirections = new Dictionary<string, Direction>();
                this.doors = doors.ToHashSet();
                this.items = new HashSet<string>(items ?? new string[0]);
            }

            public Direction? GetDoorDirection(string to)
            {
                if (neighbourDirections.TryGetValue(to, out Direction value))
                {
                    return value;
                }
                else
                {
                    return null;
                }
            }

            internal void AddDoorKnowledge(Direction dir, string otherRoom)
            {
                neighbourDirections.Add(otherRoom, dir);
            }

            internal IEnumerable<string> GetItems()
            {
                return items;
            }

            internal IEnumerable<Direction> GetUnexploredDoors()
            {
                return doors.Except(neighbourDirections.Values);
            }
        }
    }
}
