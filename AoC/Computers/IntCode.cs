using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

namespace AoC.Computers
{
    class IntCode
    {
        List<BigInteger> memory;
        int pointer;
        List<BigInteger> output;
        int relativeBase;
        IEnumerator<BigInteger> inputEnum;

        public IntCode(IEnumerable<BigInteger> vals)
        {
            this.memory = new List<BigInteger>(vals);
            pointer = 0;
            Halted = false;
            relativeBase = 0;
        }

        public bool Halted { get; private set; }

        public List<BigInteger> Run(IEnumerable<BigInteger> input = null)
        {
            input = input ?? new List<BigInteger>();
            output = new List<BigInteger>();
            inputEnum = input.GetEnumerator();
            while (DoStep())
            {
            }
            return output;
        }

        public bool DoStep()
        {
            BigInteger instruction = memory[pointer];
            BigInteger opcode = instruction % 100;
            BigInteger paramModes = instruction / 100;

            if (opcode == 99) return Halt();
            if (opcode == 1) return Add(paramModes);
            if (opcode == 2) return Multiply(paramModes);
            if (opcode == 3) return Input(paramModes);
            if (opcode == 4) return Output(paramModes);
            if (opcode == 5) return JumpIfTrue(paramModes);
            if (opcode == 6) return JumpIfFalse(paramModes);
            if (opcode == 7) return LessThan(paramModes);
            if (opcode == 8) return Equals(paramModes);
            if (opcode == 9) return AdjustRelativeBase(paramModes);

            throw new Exception($"IntCode encountered {memory[pointer]} at the pointer, expected [1-8] or 99");
        }

        public BigInteger this[int index]
        {
            get
            {
                return memory[index];
            }
            set
            {
                memory[index] = value;
            }
        }

        public (BigInteger pointerIndex, List<BigInteger> memory) GetInternalState()
        {
            return (pointer, new List<BigInteger>(memory));
        }

        public override string ToString()
        {
            return $"IC[{String.Join(',', memory.Select(x => x.ToString()))}]";
        }

        internal void Write(BigInteger param, string mode, BigInteger value)
        {
            int index;
            if (mode == "relative")
            {
                index = (int)param + relativeBase;
            }
            else
            {
                index = (int)param;
            }
            IncreaseMemoryIfNeeded(index);
            memory[index] = value;
        }

        private bool Halt()
        {
            Halted = true;
            return false;
        }

        private bool Add(BigInteger paramCode)
        {
            List<string> paramModes = GetParameterModes(paramCode, 3);
            List<BigInteger> parameters = GetParameters(3);

            BigInteger result = Read(parameters[0], paramModes[0]) + Read(parameters[1], paramModes[1]);
            Write(parameters[2], paramModes[2], result);
            MovePointer(parameters.Count + 1);
            return true;
        }

        private bool Multiply(BigInteger paramCode)
        {
            List<string> paramModes = GetParameterModes(paramCode, 3);
            List<BigInteger> parameters = GetParameters(3);

            BigInteger result = Read(parameters[0], paramModes[0]) * Read(parameters[1], paramModes[1]);
            Write(parameters[2], paramModes[2], result);
            MovePointer(parameters.Count + 1);
            return true;
        }

        private bool Input(BigInteger paramCode)
        {
            List<string> paramModes = GetParameterModes(paramCode, 1);
            List<BigInteger> parameters = GetParameters(1);

            if (inputEnum.MoveNext())
            {
                BigInteger input = inputEnum.Current;
                Write(parameters[0], paramModes[0], input);
                MovePointer(parameters.Count + 1);
                return true;
            }
            else
            {
                //Console.WriteLine("Please provide an integer input:");
                //input = int.Parse(Console.ReadLine());
                //Halt the program
                return false;
            }
        }

        private bool Output(BigInteger paramCode)
        {
            List<string> paramModes = GetParameterModes(paramCode, 1);
            List<BigInteger> parameters = GetParameters(1);

            BigInteger addToOutput = Read(parameters[0], paramModes[0]);
            output.Add(addToOutput);
            MovePointer(parameters.Count + 1);
            return true;
        }

        private bool JumpIfTrue(BigInteger paramCode)
        {
            List<string> paramModes = GetParameterModes(paramCode, 2);
            List<BigInteger> parameters = GetParameters(2);

            if (Read(parameters[0], paramModes[0]) != 0)
            {
                JumpPointer(Read(parameters[1], paramModes[1]));
            }
            else
            {
                MovePointer(parameters.Count + 1);
            }
            return true;
        }

        private bool JumpIfFalse(BigInteger paramCode)
        {
            List<string> paramModes = GetParameterModes(paramCode, 2);
            List<BigInteger> parameters = GetParameters(2);

            if (Read(parameters[0], paramModes[0]) == 0)
            {
                JumpPointer(Read(parameters[1], paramModes[1]));
            }
            else
            {
                MovePointer(parameters.Count + 1);
            }
            return true;
        }

        private bool LessThan(BigInteger paramCode)
        {
            List<string> paramModes = GetParameterModes(paramCode, 3);
            List<BigInteger> parameters = GetParameters(3);

            if (Read(parameters[0], paramModes[0]) < Read(parameters[1], paramModes[1])) Write(parameters[2], paramModes[2], 1);
            else Write(parameters[2], paramModes[2], 0);

            MovePointer(parameters.Count + 1);
            return true;
        }

        private bool Equals(BigInteger paramCode)
        {
            List<string> paramModes = GetParameterModes(paramCode, 3);
            List<BigInteger> parameters = GetParameters(3);

            if (Read(parameters[0], paramModes[0]) == Read(parameters[1], paramModes[1])) Write(parameters[2], paramModes[2], 1);
            else Write(parameters[2], paramModes[2], 0);

            MovePointer(parameters.Count + 1);
            return true;
        }

        private bool AdjustRelativeBase(BigInteger paramCode)
        {
            List<string> paramModes = GetParameterModes(paramCode, 1);
            List<BigInteger> parameters = GetParameters(1);

            relativeBase += (int) Read(parameters[0], paramModes[0]);
            MovePointer(2);
            return true;
        }

        private BigInteger Read(BigInteger param, string mode)
        {
            if (mode == "immediate") return param;
            if (mode == "positional")
            {
                IncreaseMemoryIfNeeded((int)param);
                return memory[(int)param];
            }
            if (mode == "relative")
            {
                IncreaseMemoryIfNeeded((int) param + relativeBase);
                return memory[(int) param + relativeBase];
            }
            throw new Exception($"Unexpected parameter mode {mode}");
        }

        private void MovePointer(int stepSize = 4)
        {
            pointer += stepSize;
        }

        private void JumpPointer(BigInteger address)
        {
            pointer = (int) address;
        }

        private List<BigInteger> GetParameters(int n)
        {
            return memory.Skip(pointer + 1).Take(n).ToList();
        }

        private List<string> GetParameterModes(BigInteger paramCode, int nParams)
        {
            string CharToParamMode(Char c)
            {
                if (c == '2') return "relative";
                if (c == '1') return "immediate";
                if (c == '0') return "positional";
                throw new Exception($"unexpected parameter mode code '{c}'");
            }
            return paramCode.ToString().PadLeft(nParams, '0').
                                            Select(x => CharToParamMode(x)).
                                            Reverse().
                                            ToList();
        }

        private void IncreaseMemoryIfNeeded(int upToIndex)
        {
            if (upToIndex < 0) throw new IndexOutOfRangeException("Attempting to read/write from/to negative index");
            if (upToIndex >= memory.Count)
            {
                int difference = upToIndex - memory.Count + 1;
                memory.AddRange(Enumerable.Repeat((BigInteger) 0, difference));
            }
        }
    }
}
