using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;
using AoC.Common;
using AoC.Computers;
using AoC.Utils;

namespace AoC.Day17
{
    class Solver : PuzzleSolver
    {
        List<BigInteger> program;
        string[] scaffoldImageRows;

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
            var cameras = new ASCIIComputer(program);
            var imageString = cameras.RunString();
            scaffoldImageRows = InputParser.Split(imageString).ToArray();
        }

        protected override void SolvePartOne()
        {
            var map = new Grid<char>(scaffoldImageRows, '.', false);
            var scaffolds = map.FindAllMatchingTiles('#');
            var intersections = scaffolds.Where(s => map.GetNeighbours(s.x, s.y).Values.Count(n => n == '#') == 4).ToList();
            var alignmentParameters = intersections.Select(i => i.x * i.y);
            resultPartOne = alignmentParameters.Sum().ToString();
        }

        //Works if the routine doesn't require changing patterns on a straight section i.e. covering R12L8 with R6 + 6L8
        protected override void SolvePartTwo()
        {
            var testrobot = new SimulationRobot(scaffoldImageRows);
            var greedyPath = testrobot.FindGreedyPath();
            var routine = FindValidMovementRoutine(greedyPath);
            var robot = new ASCIIComputer(program);
            resultPartTwo = routine.UploadToASCIIComputer(robot);
        }

        public static MovementRoutine FindValidMovementRoutine(string[] fullPath)
        {
            var abc = new string[] { "A", "B", "C" };
            var fullPathString = string.Join(',', fullPath);
            var patternABuilder = new StringBuilder();
            //Pattern A
            for (int a = 0; a < fullPath.Length; a++)
            {
                patternABuilder.Append(fullPath[a]);
                var branchesAfterFittingA = TryFittingPathWithPatterns(fullPathString, new string[] { patternABuilder.ToString() });
                foreach (var (pathAfterA, patternsUsedA) in branchesAfterFittingA)
                {
                    //Pattern B
                    var patternBBuilder = new StringBuilder();
                    for (int b = patternsUsedA.Count * (a+1); b < fullPath.Length; b++)
                    {
                        patternBBuilder.Append(fullPath[b]);
                        var branchesAfterFittingAB = TryFittingPathWithPatterns(pathAfterA, new string[] { patternABuilder.ToString(), patternBBuilder.ToString() });
                        foreach (var (pathAfterAB, patternsUsedAB) in branchesAfterFittingAB)
                        {
                            //Pattern C
                            var patternCBuilder = new StringBuilder();
                            for(int c = b+1; c < fullPath.Length; c++)
                            {
                                patternCBuilder.Append(fullPath[c]);
                                var branchesAfterFittingABC = TryFittingPathWithPatterns(pathAfterAB, new string[] { patternABuilder.ToString(), patternBBuilder.ToString(), patternCBuilder.ToString() });
                                foreach(var(pathAfterABC, patternsUsedABC) in branchesAfterFittingABC)
                                {
                                    if(pathAfterABC == string.Empty)
                                    {
                                        var intRoutine = new List<int>(patternsUsedA);
                                        intRoutine.AddRange(patternsUsedAB);
                                        intRoutine.AddRange(patternsUsedABC);

                                        var main = string.Join(',',intRoutine.Select(x => abc[x]));
                                        var routine = new MovementRoutine(main, patternABuilder.ToString(), patternBBuilder.ToString(), patternCBuilder.ToString());
                                        if (routine.IsValid) return routine;
                                    }
                                }
                                patternCBuilder.Append(',');
                            }
                        }
                        patternBBuilder.Append(',');
                    }
                }
                patternABuilder.Append(',');
            }
            throw new Exception("No valid routine could be found");
        }

        private static IEnumerable<(string, List<int>)> TryFittingPathWithPatterns(string path, string[] patterns)
        {
            var remainingPathsWithPatternSequence = new List<(string, List<int>)>();
            if (path.Length > 0 && path[0] == ',') path = path.Substring(1);
            for (int i = 0; i < patterns.Length; i++)
            {
                if (path.Length >= patterns[i].Length && path.Substring(0, patterns[i].Length) == patterns[i])
                {
                    var branch = TryFittingPathWithPatterns(path.Substring(patterns[i].Length), patterns);
                    foreach (var (remainingPath, patternsUsed) in branch)
                    {
                        patternsUsed.Insert(0, i);
                        remainingPathsWithPatternSequence.Add((remainingPath, patternsUsed));
                    }
                }
            }
            if (!remainingPathsWithPatternSequence.Any()) remainingPathsWithPatternSequence.Add((path, new List<int>()));
            return remainingPathsWithPatternSequence;
        }
    }

    class MovementRoutine
    {
        public bool IsValid { get; private set; }

        MovementFunction[] main;
        string[] a;
        string[] b;
        string[] c;

        public MovementRoutine(string main, string a, string b, string c)
        {
            IsValid = true;
            this.main = ParseMainRoutine(main);
            this.a = ParseMovementFunction(a);
            this.b = ParseMovementFunction(b);
            this.c = ParseMovementFunction(c);
        }

        public bool TryMainRoutine(SimulationRobot robot)
        {
            var routineIsValid = true;
            var routineEnumerator = ((IEnumerable<MovementFunction>)main).GetEnumerator();
            while (routineEnumerator.MoveNext() && routineIsValid)
            {
                var currentRoutine = SelectRoutine(routineEnumerator.Current);
                routineIsValid = PerformRoutine(robot, currentRoutine);
            }
            return routineIsValid && robot.HasVisitedAllScaffolds();
        }

        public string UploadToASCIIComputer(ASCIIComputer computer)
        {
            computer.SetValAtMemIndex(0, 2); //Awaken robot
            computer.Run(string.Join(',', main.Select(mf => mf.ToString())));
            computer.Run(string.Join(',', a));
            computer.Run(string.Join(',', b));
            computer.Run(string.Join(',', c));
            var result = computer.Run("n");//turn camera off

            return result.Last().ToString();
        }

        private bool PerformRoutine(SimulationRobot robot, string[] routine)
        {
            foreach (var instruction in routine)
            {
                if (instruction == "R")
                {
                    robot.TurnRight();
                }
                else if (instruction == "L")
                {
                    robot.TurnLeft();
                }
                else
                {
                    var distance = int.Parse(instruction);
                    while (distance > 0)
                    {
                        robot.StepForward();
                        if (robot.IsDead) return false;
                        distance--;
                    }
                }
            }
            return true;
        }

        private string[] SelectRoutine(MovementFunction mf)
        {
            switch (mf)
            {
                case MovementFunction.A:
                    return a;
                case MovementFunction.B:
                    return b;
                case MovementFunction.C:
                    return c;
                default:
                    throw new Exception("use the MovementFunction enum");
            }
        }

        private string[] ParseMovementFunction(string input)
        {
            if (input.Length > 20) IsValid = false;
            var result = input.Split(',');
            foreach (var instruction in result)
            {
                if (!(instruction == "R" || instruction == "L" || int.Parse(instruction) > -1)) throw new Exception($"instructions unclear, expected L,R or a positive integer, found: {instruction}");
            }
            return result;
        }

        private MovementFunction[] ParseMainRoutine(string input)
        {
            if (input.Length > 20) IsValid = false;
            var split = input.Split(',');
            MovementFunction[] result = new MovementFunction[split.Length];
            for (int i = 0; i < result.Length; i++)
            {
                var mfString = split[i];
                if (Enum.TryParse(mfString, out MovementFunction mf))
                {
                    result[i] = mf;
                }
                else
                {
                    throw new Exception($"Expected A,B or C found {mfString}");
                }
            }
            return result;
        }

        enum MovementFunction
        {
            A,
            B,
            C
        }
    }

    class SimulationRobot
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public Direction Facing { get; private set; }
        public bool IsDead { get; private set; }

        private Grid<bool> visited;

        private readonly int _initialX, _initialY;
        private readonly Direction _initialFacing;
        private Grid<char> layout;


        public SimulationRobot(IEnumerable<string> layoutRows)
        {
            layout = new Grid<char>(layoutRows, '.', true);

            var robotState = layout.GetAllTileTypes().Intersect(new List<char> { 'v', '^', '>', '<' }).First();
            (_, _initialX, _initialY) = layout.FindFirstMatchingTile(robotState);
            switch (robotState)
            {
                case 'v':
                    _initialFacing = Direction.South;
                    break;
                case '^':
                    _initialFacing = Direction.North;
                    break;
                case '>':
                    _initialFacing = Direction.East;
                    break;
                case '<':
                    _initialFacing = Direction.West;
                    break;
                default:
                    throw new Exception($"ruh roh, robotState is an unexpected charValue: {robotState}");
            }

            layout.SetTile(_initialX, _initialY, '#'); //separate the robot-tile from the scaffolds

            visited = new Grid<bool>(false);
            visited.SetTile(_initialX, _initialY, true);

            X = _initialX;
            Y = _initialY;
            Facing = _initialFacing;
            IsDead = false;
        }

        public void StepForward()
        {
            (X, Y) = DirectionHelper.StepInDirection(Facing, X, Y, 1);
            visited.SetTile(X, Y, true);
            IsDead = IsDead || layout.GetTile(X, Y) != '#';
        }

        public void TurnLeft()
        {
            Facing = Facing.CounterClockWiseByQuarter();
        }

        public void TurnRight()
        {
            Facing = Facing.ClockWiseByQuarter();
        }

        public string[] FindGreedyPath(bool resetAfter = true)
        {
            Reset();
            int steps = 0;
            var path = new List<string>();
            bool finished = false;

            while (!finished)
            {
                var neighbourTiles = layout.GetNeighbours(X, Y);
                if (neighbourTiles[Facing] == '#')
                {
                    StepForward();
                    steps++;
                }
                else
                {
                    if (steps > 0) path.Add(steps.ToString());
                    steps = 0;

                    var left = neighbourTiles[Facing.CounterClockWiseByQuarter()];
                    var right = neighbourTiles[Facing.ClockWiseByQuarter()];
                    if (right == '#')
                    {
                        path.Add("R");
                        TurnRight();
                    }
                    else if (left == '#')
                    {
                        path.Add("L");
                        TurnLeft();
                    }
                    else
                    {
                        finished = true;
                    }
                }
            }
            if (!HasVisitedAllScaffolds()) throw new Exception("Finished greedy path search without covering all scaffolds, make sure scaffold layout is valid!");
            if (resetAfter) Reset();
            return path.ToArray();
        }

        public bool HasVisitedAllScaffolds()
        {
            return layout.FindAllMatchingTiles('#').All(scaffold => visited.GetTile(scaffold.x, scaffold.y));
        }

        public string[] GetImageStrings()
        {
            var image = layout.RowsAsStrings();
            StringBuilder strBuilder = new StringBuilder(image[Math.Abs(Y)]);
            strBuilder[X] = IsDead ? 'X' : FacingAsChar();
            image[Math.Abs(Y)] = strBuilder.ToString();
            return image.ToArray();
        }

        public void Reset()
        {
            X = _initialX;
            Y = _initialY;
            Facing = _initialFacing;
            visited = new Grid<bool>(false);
            visited.SetTile(X, Y, true);
            IsDead = false;
        }

        private char FacingAsChar()
        {
            switch (Facing)
            {
                case Direction.North:
                    return '^';
                case Direction.East:
                    return '>';
                case Direction.South:
                    return 'v';
                case Direction.West:
                    return '<';
                default:
                    throw new Exception($"unexpected Direction, only N,E,S,W allowed, found {Facing}");
            }
        }
    }
}
