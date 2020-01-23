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

            Dictionary<string, GraphNode> nodes = CreateNecessities(input);

            List<GraphNode> unfinishedNodes = new List<GraphNode>(nodes.Values);
            while (unfinishedNodes.Any())
            {
                unfinishedNodes.RemoveAll(x => x.Done);
                foreach(GraphNode n in unfinishedNodes)
                {
                    n.TryUpdateChildReqs();
                }
            }

            int result = (int) nodes["ORE"].Required;
            return result;
        }

        public static Dictionary<string,GraphNode> CreateNecessities(string[] input)
        {
            Dictionary<string, TranslationRule> dict = new Dictionary<string, TranslationRule>();
            Dictionary<string, GraphNode> nodes = new Dictionary<string, GraphNode>();
            nodes.Add("ORE", new GraphNode("ORE"));
            foreach (string line in input)
            {
                List<(int, string)> ruleLine = new List<(int, string)>();
                string[] ruleSplit = line.Split("=> ");
                ruleLine.Add(StringToAmountType(ruleSplit[1]));
                string[] antecedent = ruleSplit[0].Split(", ");
                ruleLine.AddRange(antecedent.Select(x => StringToAmountType(x)));
                TranslationRule tr = new TranslationRule(ruleLine);
                dict.Add(ruleLine[0].Item2, tr);
                GraphNode newNode = new GraphNode(ruleLine[0].Item2);
                nodes.Add(ruleLine[0].Item2, newNode);
            }

            foreach (var kvp in dict)
            {
                var children = kvp.Value.Input.Select(x => nodes[x.type]).ToList();
                nodes[kvp.Key].AddChildren(children);
                nodes[kvp.Key].AddRule(kvp.Value);
                foreach (var child in children)
                {
                    child.AddParent(nodes[kvp.Key]);
                }
            }
            return nodes;
        }

        public static int SolvePartTwo()
        {
            string[] input = InputReader.StringsFromTxt("d14input.txt");
            Dictionary<string, GraphNode> nodes = CreateNecessities(input);

            BigInteger oreLimit = BigInteger.Parse("1000000000000");
            BigInteger oreRequired = 483766;
            BigInteger atLeastFuel = 1;
            BigInteger prevAtLF = 0;

            while (prevAtLF != atLeastFuel)
            {
                prevAtLF = atLeastFuel;
                atLeastFuel = atLeastFuel * oreLimit / oreRequired;
                foreach (var node in nodes.Values)
                {
                    node.Reset(atLeastFuel);
                }
                List<GraphNode> unfinishedNodes = new List<GraphNode>(nodes.Values);
                while (unfinishedNodes.Any())
                {
                    unfinishedNodes.RemoveAll(x => x.Done);
                    foreach (GraphNode n in unfinishedNodes)
                    {
                        n.TryUpdateChildReqs();
                    }
                }
                oreRequired = nodes["ORE"].Required;
            }

            return (int) atLeastFuel;
        }

        public static int SolvePartOneAttemptFail()
        {
            string[] input = InputReader.StringsFromTxt("d14input.txt");
            Dictionary<string, TranslationRule> dict = new Dictionary<string, TranslationRule>();
            foreach (string line in input)
            {
                List<(int, string)> ruleLine = new List<(int, string)>();
                string[] ruleSplit = line.Split("=> ");
                ruleLine.Add(StringToAmountType(ruleSplit[1]));
                string[] antecedent = ruleSplit[0].Split(", ");
                ruleLine.AddRange(antecedent.Select(x => StringToAmountType(x)));
                TranslationRule tr = new TranslationRule(ruleLine);
                dict.Add(ruleLine[0].Item2, tr);
            }

            List<(int amount, string type)> requirements = new List<(int amount, string type)> { (1, "FUEL") };
            Dictionary<string, int> leftovers = new Dictionary<string, int>();
            bool done = false;
            int oreNeeded = 0;
            while (requirements.Count > 0)
            {
                var tr = dict[requirements[0].type];
                List<(int, string)> newReqs = tr.Produce(requirements[0].amount, leftovers);
                requirements.RemoveAt(0);

                //Merge new requirements with existing
                foreach ((int amount, string type) newreq in newReqs)
                {
                    if (newreq.type != "ORE")
                    {
                        int match = requirements.FindIndex(x => x.type == newreq.type);
                        if (match > -1)
                        {
                            requirements[match] = (requirements[match].amount + newreq.amount, requirements[match].type);
                        }
                        else
                        {
                            requirements.Add(newreq);
                        }
                    }
                    else
                    {
                        oreNeeded += newreq.amount;
                    }
                }
            }
            return oreNeeded;
        }

        public static (int, string) StringToAmountType(string input)
        {
            string[] spl = input.Split(' ');
            return (int.Parse(spl[0]), spl[1]);
        }
    }

    class GraphNode
    {
        public string Name { get; private set; }
        public BigInteger Required { get; private set; }
        public bool Done { get; private set; }
        List<GraphNode> parents;
        List<GraphNode> children;
        TranslationRule rule;

        public GraphNode(string name)
        {
            this.Name = name;
            children = new List<GraphNode>();
            parents = new List<GraphNode>();
            Reset(1);
        }

        public void AddChildren(IEnumerable<GraphNode> children)
        {
            this.children.AddRange(children);
        }

        public void AddParent(GraphNode parent)
        {
            parents.Add(parent);
        }

        public void AddRule(TranslationRule tr)
        {
            rule = tr;
        }

        public void IncreaseRequired(BigInteger by)
        {
            Required += by;
        }

        public void Reset(BigInteger fuelReq)
        {
            Required = Name == "FUEL" ? fuelReq : 0;
            Done = false;
        }
        

        public bool TryUpdateChildReqs()
        {
            if (Done) throw new Exception("Should not call TryUpdateChildReqs on finished node");
            if (parents.All(x => x.Done))
            {
                if (children.Any())
                {
                    BigInteger c = Required / rule.Output.amount;
                    if (Required % rule.Output.amount != 0) c++;
                    foreach (var child in children)
                    {
                        child.IncreaseRequired(rule.Input.Find(x => x.type == child.Name).amount * c);
                    }
                }
                Done = true;
            }
            return Done;
        }
    }

    class TranslationRule
    {
        public List<(int amount, string type)> Input { get; private set; }
        public (int amount, string type) Output { get; private set; }

        public TranslationRule(List<(int, string)> ruleLine)
        {
            Output = ruleLine[0];
            Input = ruleLine.Skip(1).ToList();
        }

        internal List<(int, string)> Produce(int amount, Dictionary<string, int> extraReagents)
        {
            int c = 1;
            while (c * Output.amount < amount)
            {
                c++;
            }

            List<(int, string)> newReqs = Input.Select(x => (x.amount * c, x.type)).ToList();

            int lim = newReqs.Count;
            for (int i = 0; i < lim; i++)
            {
                (int amount, string type) req = newReqs[i];
                extraReagents.TryGetValue(req.type, out int exAmount);
                if (exAmount > 0)
                {
                    if (req.amount > exAmount)
                    {
                        req.amount = req.amount - exAmount;
                        extraReagents[req.type] = 0;
                    }
                    else
                    {
                        extraReagents[req.type] -= req.amount;
                        lim--;
                        newReqs.RemoveAt(i);
                        i--; // WEEEE WOOO DANGER UGLY CODING ALERT
                    }
                }
            }

            if (c * Output.amount > amount)
            {
                if (!extraReagents.TryGetValue(Output.type, out int val))
                {
                    extraReagents.Add(Output.type, val);
                }
                extraReagents[Output.type] += c * Output.amount - amount;
            }

            return newReqs;
        }
    }
}

