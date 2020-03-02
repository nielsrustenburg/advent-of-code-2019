using System;
using System.Collections.Generic;
using System.Text;

namespace AoC
{
    interface IGrid<T>
    {
        void SetTile(int x, int y, T tile);

        T GetTile(int x, int y);

        int Count(T tile);
    }
}
