using System;
using System.Collections.Generic;
using System.Text;

namespace AoC.common
{
    public interface IPuzzleSolver
    {
        string SolutionOne();

        string SolutionTwo();

        void WriteSolutions();
    }
}
