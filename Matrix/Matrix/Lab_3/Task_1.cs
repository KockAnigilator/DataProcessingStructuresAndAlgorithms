using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix.Lab_3
{
    public class Task_1
    {
        public int Throws(int n)
        {
            if (n == 0)
                return 0;

            int m = 1;
            while (m * (m + 1) / 2 < n)
            {
                m++;
            }
            return m;
        }
    }
}
