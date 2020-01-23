using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AoC
{
    class IntCode
    {
        List<int> memory;
        int pointer;
        List<int> output;
        int relativeBase;
        public bool Halted { get; private set; }
        IEnumerator<int> inputEnum;

        public IntCode(List<int> vals)
        {
            this.memory = new List<int>(vals);
            pointer = 0;
            Halted = false;
            relativeBase = 0;
        }

        public List<int> Run(List<int> input = null)
        {
            input = input ?? new List<int>();
            output = new List<int>();
            inputEnum = input.GetEnumerator();
            while (DoStep())
            {
            }
            return output;
        }

        public bool DoStep()
        {
            int instruction = memory[pointer];
            int opcode = instruction % 100;
            int paramModes = instruction / 100;

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

        private bool Halt()
        {
            Halted = true;
            return false;
        }

        private bool Add(int paramCode)
        {
            List<string> paramModes = GetParameterModes(paramCode, 3);
            List<int> parameters = GetParameters(3);

            int result = Read(parameters[0], paramModes[0]) + Read(parameters[1], paramModes[1]);
            Write(parameters[2], result);
            MovePointer(parameters.Count + 1);
            return true;
        }

        private bool Multiply(int paramCode)
        {
            List<string> paramModes = GetParameterModes(paramCode, 3);
            List<int> parameters = GetParameters(3);

            int result = Read(parameters[0], paramModes[0]) * Read(parameters[1], paramModes[1]);
            Write(parameters[2], result);
            MovePointer(parameters.Count + 1);
            return true;
        }

        private bool Input(int paramCode)
        {
            List<string> paramModes = GetParameterModes(paramCode, 1);
            List<int> parameters = GetParameters(1);

            if (inputEnum.MoveNext())
            {
                int input = inputEnum.Current;
                Write(parameters[0], input);
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

        private bool Output(int paramCode)
        {
            List<string> paramModes = GetParameterModes(paramCode, 1);
            List<int> parameters = GetParameters(1);

            int addToOutput = Read(parameters[0], paramModes[0]);
            output.Add(addToOutput);
            MovePointer(parameters.Count + 1);
            return true;
        }

        private bool JumpIfTrue(int paramCode)
        {
            List<string> paramModes = GetParameterModes(paramCode, 2);
            List<int> parameters = GetParameters(2);

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

        private bool JumpIfFalse(int paramCode)
        {
            List<string> paramModes = GetParameterModes(paramCode, 2);
            List<int> parameters = GetParameters(2);

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

        private bool LessThan(int paramCode)
        {
            List<string> paramModes = GetParameterModes(paramCode, 3);
            List<int> parameters = GetParameters(3);

            if (Read(parameters[0], paramModes[0]) < Read(parameters[1], paramModes[1])) Write(parameters[2], 1);
            else Write(parameters[2], 0);

            MovePointer(parameters.Count + 1);
            return true;
        }

        private bool Equals(int paramCode)
        {
            List<string> paramModes = GetParameterModes(paramCode, 3);
            List<int> parameters = GetParameters(3);

            if (Read(parameters[0], paramModes[0]) == Read(parameters[1], paramModes[1])) Write(parameters[2], 1);
            else Write(parameters[2], 0);

            MovePointer(parameters.Count + 1);
            return true;
        }

        private bool AdjustRelativeBase(int paramCode)
        {
            List<string> paramModes = GetParameterModes(paramCode, 1);
            List<int> parameters = GetParameters(1);

            relativeBase += Read(parameters[0], paramModes[0]);
            MovePointer(2);
            return true;
        }

        internal void Write(int param, int value)
        {
            IncreaseMemoryIfNeeded(param);
            memory[param] = value;
        }

        private int Read(int param, string mode)
        {
            if (mode == "immediate") return param;
            if (mode == "positional")
            {
                IncreaseMemoryIfNeeded(param);
                return memory[param];
            }
            if (mode == "relative")
            {
                IncreaseMemoryIfNeeded(param + relativeBase);
                return memory[param + relativeBase];
            }
            throw new Exception($"Unexpected parameter mode {mode}");
        }

        void MovePointer(int stepSize = 4)
        {
            pointer += stepSize;
        }

        void JumpPointer(int address)
        {
            pointer = address;
        }

        public List<int> GetParameters(int n)
        {
            return memory.Skip(pointer + 1).Take(n).ToList();
        }

        public List<string> GetParameterModes(int paramCode, int nParams)
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

        public List<int> GetMemory()
        {
            return memory;
        }

        public void IncreaseMemoryIfNeeded(int upToIndex)
        {
            if (upToIndex < 0) throw new IndexOutOfRangeException("Attempting to read/write from/to negative index");
            if (upToIndex >= memory.Count)
            {
                int difference = upToIndex - memory.Count + 1;
                memory.AddRange(Enumerable.Repeat(0, difference));
            }
        }

        public int GetValAtMemIndex(int index)
        {
            return memory[index];
        }

        public override string ToString()
        {
            return $"IC[{String.Join(',', memory.Select(x => x.ToString()))}]";
        }
    }
}
