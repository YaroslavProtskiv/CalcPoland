using System;
using System.Collections.Generic;

namespace HW_Calc_Pol
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter equation!");
            string equation = Console.ReadLine();
            Result result = new Result(equation);

            if (result.result.isNumber == true)
            {
                Console.WriteLine("Result\t=\t" + result.result.number);
            }            
            Console.ReadLine();
        }
    }
}
