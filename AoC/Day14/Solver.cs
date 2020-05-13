using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;
using AoC.Common;
using AoC.Utils;

namespace AoC.Day14
{
    class Solver : PuzzleSolver
    {
        List<ChemicalProductionRecipe> recipes;
        ChemicalProductionGraph cpgraph;
        public Solver() : this(Input.InputMode.Embedded, "input")
        {
        }

        public Solver(Input.InputMode mode, string input) : base(mode, input)
        {

        }

        protected override void ParseInput(string input)
        {
            recipes = InputParser<ChemicalProductionRecipe>.SplitAndParse(input, ParseRecipe).ToList();

            ChemicalProductionRecipe ParseRecipe(string recipe)
            {
                (string, int) SplitAmountIngredient(string amountIngredient)
                {
                    var split = amountIngredient.Split(' ');
                    return (split[1], int.Parse(split[0]));
                }

                var ingrProdSplit = recipe.Split("=> ");
                var (product, batchSize) = SplitAmountIngredient(ingrProdSplit[1]);

                var antecedent = ingrProdSplit[0].Split(", ");
                var ingredientsAndAmounts = antecedent.Select(ing => SplitAmountIngredient(ing)).ToList();

                return new ChemicalProductionRecipe(ingredientsAndAmounts, product, batchSize);
            }
        }

        protected override void PrepareSolution()
        {
            cpgraph = new ChemicalProductionGraph(recipes);
        }

        protected override void SolvePartOne()
        {
            resultPartOne = OreRequiredToProduceFuel(1, cpgraph).ToString();
        }

        protected override void SolvePartTwo()
        {
            BigInteger oreLimit = BigInteger.Parse("1000000000000");
            BigInteger minimumAchievableProduction = 1;
            BigInteger previousProduction = 0;

            while (previousProduction != minimumAchievableProduction)
            {
                BigInteger oreRequired = OreRequiredToProduceFuel(minimumAchievableProduction, cpgraph);
                previousProduction = minimumAchievableProduction;
                minimumAchievableProduction = minimumAchievableProduction * oreLimit / oreRequired;
            }
            resultPartTwo = minimumAchievableProduction.ToString();
        }

        public static BigInteger OreRequiredToProduceFuel(BigInteger amount, ChemicalProductionGraph graph)
        {
            var amountRequired = graph.Nodes().ToDictionary(x => x, x => x == "FUEL" ? amount : 0);
            var unfinishedNodes = graph.Nodes().ToHashSet();

            while (unfinishedNodes.Any())
            {
                var nextUnfinishedNodes = new HashSet<string>(unfinishedNodes);
                foreach (var node in unfinishedNodes)
                {
                    var parents = graph.InNeighbours(node).Select(x => x.nb);
                    if (!unfinishedNodes.Intersect(parents).Any())
                    {
                        var children = graph.OutNeighbours(node);
                        BigInteger amountOfBatches = (amountRequired[node] - 1) / graph.BatchSize(node) + 1; //find smallest x s.t. x*batchsize > amountrequired
                        foreach ((string child, int perbatch) in children)
                        {
                            amountRequired[child] += perbatch * amountOfBatches;
                        }
                        nextUnfinishedNodes.Remove(node);
                    }
                }
                unfinishedNodes = nextUnfinishedNodes;
            }
            return amountRequired["ORE"];
        }
    }

    class ChemicalProductionRecipe
    {
        List<(string, int)> ingredients;
        string product;
        int batchSize;

        public ChemicalProductionRecipe(IEnumerable<(string, int)> ingredients, string product, int batchSize)
        {
            this.ingredients = new List<(string, int)>(ingredients);
            this.product = product;
            this.batchSize = batchSize;
        }

        public (List<(string, int)>, string, int) GetRecipeTuple()
        {
            return (ingredients, product, batchSize);
        }

    }

    class ChemicalProductionGraph : AdjacencyDiGraph<ChemicalProductionNode>
    {
        public ChemicalProductionGraph() : base()
        {

        }

        public ChemicalProductionGraph(IEnumerable<ChemicalProductionRecipe> recipes) : this()
        {
            var recipeTuples = recipes.Select(x => x.GetRecipeTuple()).ToList();
            AddNode("ORE", 1);
            foreach ((List<(string, int)> _, string name, int batchSize) in recipeTuples)
            {
                AddNode(name, batchSize);
            }

            foreach ((List<(string, int)> ingredientList, string product, int _) in recipeTuples)
            {
                foreach ((string ingredient, int amount) in ingredientList)
                {
                    AddEdge(product, ingredient, amount);
                }
            }
        }

        public void AddNode(string name, int batchsize)
        {
            nodes.Add(name, new ChemicalProductionNode(name, batchsize));
        }

        public int BatchSize(string name)
        {
            return nodes[name].BatchSize;
        }
    }

    class ChemicalProductionNode : AdjacencyDiGraphNode
    {
        protected ChemicalProductionNode(string name) : base(name) { }

        public ChemicalProductionNode(string name, int batchSize) : this(name)
        {
            BatchSize = batchSize;
        }

        public int BatchSize { get; }
    }
}
