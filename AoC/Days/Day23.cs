using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;

namespace AoC
{
    static class Day23
    {
        public static BigInteger SolvePartOne()
        {
            string input = InputReader.StringFromLine("d23input.txt");
            List<BigInteger> program = input.Split(',').Select(x => BigInteger.Parse(x)).ToList();
            Network network = new Network(50, program);
            return network.Run().y;
        }

        public static BigInteger SolvePartTwo()
        {
            string input = InputReader.StringFromLine("d23input.txt");
            List<BigInteger> program = input.Split(',').Select(x => BigInteger.Parse(x)).ToList();
            NatNetwork network = new NatNetwork(50, program);
            return network.Run().y;
        }
    }

    class Network
    {
        NetworkInterfaceController[] nics;
        List<BigInteger>[] queues;
        public (BigInteger x, BigInteger y) address255;
        public int idleCount;
        public bool finished;

        public Network(int computerCount, List<BigInteger> program)
        {
            nics = new NetworkInterfaceController[computerCount];
            queues = new List<BigInteger>[computerCount];
            idleCount = 0;
            finished = false;
            for(int i = 0; i < computerCount; i++)
            {
                nics[i] = new NetworkInterfaceController(i, program);
                queues[i] = new List<BigInteger> { i };
            }
        }

        public (BigInteger x, BigInteger y) Run()
        {
            while (!finished)
            {
                DoIteration();
            }
            return address255;
        }

        public void DoIteration()
        {
            List<BigInteger> newPackages = new List<BigInteger>();
            for(int i = 0; i < nics.Length; i++)
            {
                newPackages.AddRange(nics[i].DoStep(queues[i]));
            }
            CheckForIdleSystem(newPackages);
            NewPackagesToQueues(newPackages);
            FillEmptyQueues();
        }

        private void CheckForIdleSystem(List<BigInteger> newPackages)
        {
            if (newPackages.Count > 0)
            {
                idleCount = 0;
            }
            else
            {
                if (idleCount > 0)
                {
                    HandleIdleSystem(newPackages);
                    Console.WriteLine($"system idle{idleCount}");
                }
                idleCount++;
            }
        }

        public virtual void HandleIdleSystem(List<BigInteger> newPackages)
        {
            
        }

        private void AddToQueue(int address, BigInteger x, BigInteger y)
        {
            if (address == 255)
            {
                HandleTwoFiftyFive(x, y);
            }
            else
            {
                if (queues[address] == null)
                {
                    queues[address] = new List<BigInteger> { x, y };
                }
                else
                {
                    queues[address].Add(x);
                    queues[address].Add(y);
                }
            }
        }

        public virtual void HandleTwoFiftyFive(BigInteger x, BigInteger y)
        {
            if (!finished)
            {
                finished = true;
                address255 = (x, y);
            }
        }

        private void NewPackagesToQueues(List<BigInteger> newPackages)
        {
            queues = new List<BigInteger>[nics.Length];
            for (int i = 0; i < newPackages.Count; i = i + 3)
            {
                AddToQueue((int)newPackages[i], newPackages[i + 1], newPackages[i + 2]);
            }
        }

        private void FillEmptyQueues()
        {
            for (int i = 0; i < queues.Length; i++)
            {
                if(queues[i] == null)
                {
                    queues[i] = new List<BigInteger> { -1 };
                }
            }
        }
    }

    class NatNetwork : Network
    {
        bool firstReset;
        (BigInteger x, BigInteger y) lastReset;
        public NatNetwork(int computerCount, List<BigInteger> program) : base(computerCount, program)
        {
            firstReset = true;
        }

        public override void HandleTwoFiftyFive(BigInteger x, BigInteger y)
        {
            address255 = (x, y);
        }

        public override void HandleIdleSystem(List<BigInteger> newPackages)
        {
            newPackages.Add(0);
            newPackages.Add(address255.x);
            newPackages.Add(address255.y);
            if(!firstReset && lastReset.y == address255.y)
            {
                finished = true;
            }
            if (firstReset) firstReset = false;
            lastReset = address255;
            idleCount = 0;
        }
    }

    class NetworkInterfaceController
    {
        BigIntCode bic;
        BigInteger id;

        public NetworkInterfaceController(BigInteger id, IEnumerable<BigInteger> programming)
        {
            this.id = id;
            bic = new BigIntCode(programming.ToList());
        }

        public List<BigInteger> StartUp()
        {
            return DoStep(new List<BigInteger> { id });
        }

        public List<BigInteger> DoStep(List<BigInteger> input)
        {
            return bic.Run(input);
        }
    }
}

