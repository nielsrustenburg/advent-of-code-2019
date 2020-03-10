using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;

namespace AoC
{
    static class Day14
    {
        public static int SolvePartOne()
        {
            string[] input = InputReader.StringsFromTxt("d14input.txt");
            ChemicalProductionGraph cpgraph = new ChemicalProductionGraph(input);
            return (int)OreRequiredToProduceFuel(1, cpgraph);
        }

        public static int SolvePartTwo()
        {
            string[] input = InputReader.StringsFromTxt("d14input.txt");
            ChemicalProductionGraph cpgraph = new ChemicalProductionGraph(input);

            BigInteger oreLimit = BigInteger.Parse("1000000000000");
            BigInteger minimumAchievableProduction = 1;
            BigInteger previousProduction = 0;

            while (previousProduction != minimumAchievableProduction)
            {
                BigInteger oreRequired = OreRequiredToProduceFuel(minimumAchievableProduction, cpgraph);
                previousProduction = minimumAchievableProduction;
                minimumAchievableProduction = minimumAchievableProduction * oreLimit / oreRequired;
            }

            return (int)minimumAchievableProduction;
        }

        public static BigInteger OreRequiredToProduceFuel(BigInteger amount, ChemicalProductionGraph graph)
        {
            Dictionary<string, BigInteger> amountRequired = graph.Nodes().ToDictionary(x => x, x => x == "FUEL" ? amount : 0);
            HashSet<string> unfinishedNodes = graph.Nodes().ToHashSet();

            while (unfinishedNodes.Any())
            {
                HashSet<string> nextUnfinishedNodes = new HashSet<string>(unfinishedNodes);
                foreach (string node in unfinishedNodes)
                {
                    var parents = graph.InNeighbours(node).Select(x => x.nb);
                    if (!unfinishedNodes.Intersect(parents).Any())
                    {
                        var children = graph.OutNeighbours(node);
                        BigInteger batches = (amountRequired[node] - 1) / graph.BatchSize(node) + 1; //find smallest x s.t. x*batchsize > amountrequired
                        foreach ((string child, int perbatch) in children)
                        {
                            amountRequired[child] += perbatch * batches;
                        }
                        nextUnfinishedNodes.Remove(node);
                    }
                }
                unfinishedNodes = nextUnfinishedNodes;
            }
            return amountRequired["ORE"];
        }

    }

    class ChemicalProductionGraph : AdjacencyDiGraph<ChemicalProductionNode>
    {
        public ChemicalProductionGraph() : base()
        {

        }

        public ChemicalProductionGraph(IEnumerable<string> recipes) : this()
        {
            List<(List<(string, int)>, string, int)> interpretedRecipes = recipes.Select(x => InterpretRecipe(x)).ToList();

            AddNode("ORE", 1);
            foreach((List<(string, int)> _,string name, int batchSize) in interpretedRecipes)
            {
                AddNode(name, batchSize);
            }

            foreach ((List<(string, int)> ingredientList, string product, int _) in interpretedRecipes)
            {
                foreach((string ingredient, int amount) in ingredientList)
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

        public static (List<(string ingredient, int amount)> ingredients, string product, int batchSize) InterpretRecipe(string recipe)
        {
            string[] ingrProdSplit = recipe.Split("=> ");
            (string product, int batchSize) = SplitNameAmount(ingrProdSplit[1]);

            string[] antecedent = ingrProdSplit[0].Split(", ");
            List<(string,int)> ingredients = antecedent.Select(ing => SplitNameAmount(ing)).ToList();

            return (ingredients, product, batchSize);
        }

        private static (string, int) SplitNameAmount(string input)
        {
            string[] spl = input.Split(' ');
            return (spl[1], int.Parse(spl[0]));
        }
    }

    class ChemicalProductionNode : AdjacencyDiGraphNode
    {
        public int BatchSize { get; }

        protected ChemicalProductionNode(string name) : base(name) { }

        public ChemicalProductionNode(string name, int batchSize) : this(name)
        {
            BatchSize = batchSize;
        }
    }

}

