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
            var routine = FindValidMovementRoutine(greedyPath, new List<string>[0], new int[0]);
            var robot = new ASCIIComputer(program);
            resultPartTwo = routine.UploadToASCIIComputer(robot);
        }

        public static MovementRoutine FindValidMovementRoutine(IEnumerable<string> path, List<string>[] previousPatterns, int[] patternsUsed)
        {
            const int maxCharacters = MovementRoutine.characterLimit;
            const int maxPatternsInRoutine = MovementRoutine.maxPatternsInRoutine;

            if (!path.Any())
            {
                var abc = new string[] { "A", "B", "C" };
                var mainRoutine = patternsUsed.Select(p => abc[p]);
                var moveRoutine = new MovementRoutine(mainRoutine, previousPatterns[0], previousPatterns[1], previousPatterns[2]);
                if (moveRoutine.IsWithinCharacterLimits) return moveRoutine; else return null;
            }
            if (previousPatterns.Length >= 3) return null;

            var patternsWithNew = new List<string>[previousPatterns.Length + 1];
            for (int patternId = 0; patternId < previousPatterns.Length; patternId++)
            {
                patternsWithNew[patternId] = previousPatterns[patternId];
            }
            var newPattern = new List<string>();
            patternsWithNew[previousPatterns.Length] = newPattern;

            var pathAfterNewPattern = path;
            int patternLengthSansCommas = 0;
            bool patternIsValid = true;
            var pathEnumerator = path.GetEnumerator();
            while (patternIsValid && pathEnumerator.MoveNext())
            {
                patternLengthSansCommas += pathEnumerator.Current.Length;
                if ((patternLengthSansCommas + newPattern.Count) <= maxCharacters)
                {
                    newPattern.Add(pathEnumerator.Current);
                    pathAfterNewPattern = pathAfterNewPattern.Skip(1);
                    var branches = TryFittingPathWithPatterns(pathAfterNewPattern, patternsWithNew, maxPatternsInRoutine-patternsUsed.Length);
                    foreach (var (remainingPath, patternsUsedInBranch) in branches)
                    {
                        var updatedPatternsUsed = patternsUsed.Concat(new int[] { previousPatterns.Length }).Concat(patternsUsedInBranch).ToArray();
                        var result = FindValidMovementRoutine(remainingPath, patternsWithNew, updatedPatternsUsed);
                        if (result != null) return result;
                    }
                }
                else
                {
                    patternIsValid = false;
                }
            }
            return null;
        }

        private static IEnumerable<(IEnumerable<string>, List<int>)> TryFittingPathWithPatterns(IEnumerable<string> path, List<string>[] patterns, int patternBudget)
        {
            var result = new List<(IEnumerable<string>, List<int>)>();
            if (patternBudget > 0)
            {
                for (int patternIndex = 0; patternIndex < patterns.Length; patternIndex++)
                {
                    var pattern = patterns[patternIndex];
                    var pathEnumerator = path.GetEnumerator();
                    var patternEnumerator = pattern.GetEnumerator();
                    var patternFits = true;
                    while (patternFits)
                    {
                        patternFits = pathEnumerator.MoveNext();
                        if (!patternEnumerator.MoveNext())
                        {
                            var partialResults = TryFittingPathWithPatterns(path.Skip(pattern.Count), patterns, patternBudget-1);
                            foreach (var (remainingPath, patternsUsed) in partialResults)
                            {
                                patternsUsed.Insert(0, patternIndex);
                                result.Add((remainingPath, patternsUsed));
                            }
                            break;
                        }
                        patternFits = patternFits && pathEnumerator.Current == patternEnumerator.Current;
                    }
                }
            }
            result.Add((path, new List<int>()));
            return result;
        }
    }

    class MovementRoutine
    {
        public const int characterLimit = 20;
        public const int maxPatternsInRoutine = (characterLimit + 1) / 2;

        MovementFunctionId[] main;
        string[] a;
        string[] b;
        string[] c;

        public MovementRoutine(string main, string a, string b, string c) : this(main.Split(','), a.Split(','), b.Split(','), c.Split(','))
        {
        }

        public MovementRoutine(IEnumerable<string> main, IEnumerable<string> a, IEnumerable<string> b, IEnumerable<string> c)
        {
            this.main = ParseMainRoutineInput(main);
            this.a = ParseMovementFunctionInput(a);
            this.b = ParseMovementFunctionInput(b);
            this.c = ParseMovementFunctionInput(c);
            CheckCharacterLimits();
        }

        enum MovementFunctionId
        {
            A,
            B,
            C
        }

        public bool IsWithinCharacterLimits { get; private set; }

        public bool TryMainRoutine(SimulationRobot robot)
        {
            var routineIsValid = true;
            var routineEnumerator = ((IEnumerable<MovementFunctionId>)main).GetEnumerator();
            while (routineEnumerator.MoveNext() && routineIsValid)
            {
                var currentRoutine = SelectRoutine(routineEnumerator.Current);
                routineIsValid = PerformRoutine(robot, currentRoutine);
            }
            return routineIsValid && robot.HasVisitedAllScaffolds();
        }

        public string UploadToASCIIComputer(ASCIIComputer computer)
        {
            computer[0] = 2; //Awaken robot
            computer.Run(string.Join(',', main.Select(mf => mf.ToString())));
            computer.Run(string.Join(',', a));
            computer.Run(string.Join(',', b));
            computer.Run(string.Join(',', c));
            var result = computer.Run("n");//turn camera off

            return result.Last().ToString();
        }

        private void CheckCharacterLimits()
        {
            IsWithinCharacterLimits = main.Length <= maxPatternsInRoutine &&
                                      MovementFunctionIsWithinCharacterLimits(a) &&
                                      MovementFunctionIsWithinCharacterLimits(b) &&
                                      MovementFunctionIsWithinCharacterLimits(c);
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

        private string[] SelectRoutine(MovementFunctionId mf)
        {
            switch (mf)
            {
                case MovementFunctionId.A:
                    return a;
                case MovementFunctionId.B:
                    return b;
                case MovementFunctionId.C:
                    return c;
                default:
                    throw new Exception("use the MovementFunction enum");
            }
        }

        private string[] ParseMovementFunctionInput(IEnumerable<string> input)
        {
            foreach (var instruction in input)
            {
                if (!(instruction == "R" || instruction == "L" || int.Parse(instruction) > -1)) throw new Exception($"instructions unclear, expected L,R or a positive integer, found: {instruction}");
            }
            return input.ToArray();
        }

        private MovementFunctionId[] ParseMainRoutineInput(IEnumerable<string> input)
        {
            var inputArray = input.ToArray();
            MovementFunctionId[] result = new MovementFunctionId[inputArray.Length];
            for (int i = 0; i < result.Length; i++)
            {
                var mfString = inputArray[i];
                if (Enum.TryParse(mfString, out MovementFunctionId mf))
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

        private bool MovementFunctionIsWithinCharacterLimits(string[] mf)
        {
            int characters = mf.Length - 1;
            foreach(var instruction in mf){
                characters += instruction.Length;
                if (characters > characterLimit) return false;
            }
            return true;
        }
    }

    class SimulationRobot
    {
        private IGrid<bool> visited;
        private readonly int _initialX, _initialY;
        private readonly Direction _initialFacing;
        private IGrid<char> layout;

        public SimulationRobot(IEnumerable<string> layoutRows)
        {
            layout = new ArrayGrid<char>(layoutRows, '.', true);

            var robotState = layout.GetAllTileTypes().Intersect(new List<char> { 'v', '^', '>', '<' }).First();
            (_initialX, _initialY) = ((int,int)) layout.FindFirstMatchingTile(robotState);
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

            layout[_initialX, _initialY] = '#';//separate the robot-tile from the scaffolds

            visited = new ArrayGrid<bool>(false,layout.Width, layout.Height, true);
            visited[_initialX, _initialY] = true;

            X = _initialX;
            Y = _initialY;
            Facing = _initialFacing;
            IsDead = false;
        }

        public int X { get; private set; }
        public int Y { get; private set; }
        public Direction Facing { get; private set; }
        public bool IsDead { get; private set; }

        public void StepForward()
        {
            (X, Y) = DirectionHelper.StepInDirection(Facing, X, Y, 1);
            visited[X, Y] = true;
            IsDead = IsDead || layout[X,Y] != '#';
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
                var neighbourTiles = layout.GetNeighbours(X, Y, false);
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
            return layout.FindAllMatchingTiles('#').All(scaffold => visited[scaffold.x, scaffold.y]);
        }

        public string[] GetImageStrings()
        {
            var image = layout.RowsAsStrings();
            var strBuilder = new StringBuilder(image[image.Count-1 - Y]);
            strBuilder[X] = IsDead ? 'X' : FacingAsChar();
            image[image.Count-1 - Y] = strBuilder.ToString();
            return image.ToArray();
        }

        public void Reset()
        {
            X = _initialX;
            Y = _initialY;
            Facing = _initialFacing;
            visited = new ArrayGrid<bool>(false,layout.Width,layout.Height,true);
            visited[X, Y] = true;
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
