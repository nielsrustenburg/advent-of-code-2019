using System;
using System.Collections.Generic;
using System.Text;

namespace AoC.Common
{
    public interface IPuzzleSolver
    {
        string SolutionOne();

        string SolutionTwo();

        void WriteSolutions();
    }
}
