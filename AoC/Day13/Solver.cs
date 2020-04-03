using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;
using AoC.Common;
using AoC.Computers;
using AoC.Utils;

namespace AoC.Day13
{
    class Solver : PuzzleSolver
    {
        ArcadeMachine game;

        public Solver() : this(Input.InputMode.Embedded, "input")
        {
        }

        public Solver(Input.InputMode mode, string input) : base(mode, input)
        {
        }

        protected override void ParseInput(string input)
        {
            var program = InputParser<BigInteger>.ParseCSVLine(input, BigInteger.Parse).ToList();
            game = new ArcadeMachine(program);
        }

        protected override void PrepareSolution()
        {
            //No common prep
        }

        protected override void SolvePartOne()
        {
            resultPartOne = game.Blocks.ToString();
        }

        protected override void SolvePartTwo()
        {
            game.Play();
            resultPartTwo = game.Score.ToString();
        }
    }

    class ArcadeMachine
    {
        static readonly string[] tiles = new string[] { " ", "\u2588", "#", "=", "o" };
        BigInteger[] factorySettings;
        bool drawGame;

        IntCode backend;
        Grid<string> gameGrid;
        List<BigInteger> gameState;

        public int Blocks { get; private set; }
        public int Score { get; private set; }
        int paddleX;
        int ballX;


        enum PaddleMove
        {
            Left = -1,
            Stay = 0,
            Right = 1,
        }

        public ArcadeMachine(IEnumerable<BigInteger> program, bool draw = false)
        {
            factorySettings = program.ToArray();
            drawGame = draw;
            Restart();
            gameState = backend.Run();
            UpdateGameGrid();
        }

        public void Restart()
        {
            backend = new IntCode(factorySettings);
            gameGrid = new Grid<string>(" ");
        }

        public void Play()
        {
            Restart();
            backend.SetValAtMemIndex(0, 2);
            backend.Run();
            UpdateGameGrid();

            if (drawGame)
            {
                Console.Clear();
                DrawGameState();
            }

            do
            {
                MovePaddle(GetNextMove());
                UpdateGameGrid();
                if (drawGame) DrawGameState();
            } while (!backend.Halted);
        }

        private PaddleMove GetNextMove()
        {
            return paddleX == ballX ? PaddleMove.Stay : (paddleX > ballX ? PaddleMove.Left : PaddleMove.Right);
        }

        private void MovePaddle(PaddleMove move)
        {
            gameState = backend.Run(new BigInteger[] {(int) move });
        }

        private void DrawGameState()
        {
            Console.SetCursorPosition(0, 0);
            IEnumerable<string> gameScreen = gameGrid.RowsAsStrings();
            Console.WriteLine($"Score: {Score}");
            foreach (string row in gameScreen.Reverse())
            {
                Console.WriteLine(row);
            }
            System.Threading.Thread.Sleep(50);
        }

        private void UpdateGameGrid()
        {
            Blocks = 0;
            for (int i = 0; i < gameState.Count - 2; i += 3)
            {
                int x = (int)gameState[i];
                int y = (int)gameState[i + 1];
                if (!(x == -1 && y == 0))
                {
                    int val = (int)gameState[i + 2];
                    gameGrid.GetTile(x, y);
                    gameGrid.SetTile(x, y, tiles[val]);
                    if (val == 4) ballX = x;
                    if (val == 3) paddleX = x;
                    if (val == 2) Blocks++;
                }
                else
                {
                    Score = (int)gameState[i + 2];
                }
            }
        }
    }
}
