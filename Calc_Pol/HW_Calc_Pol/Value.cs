using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW_Calc_Pol
{
    struct Value
    {
        public bool isNumber;
        public double number;

        public Value(bool isNumber = false, double number = 0)
        {
            this.isNumber = isNumber;
            this.number = number;
        }
    }
}
