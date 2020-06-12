using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace NumberConvertTool
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
               
            }
            string r0 = XMath.XConvert(long.MaxValue, 2);
            string r1 = XMath.XConvert(int.MaxValue, 36);
            string r2 = XMath.XConvert(int.MinValue, 36);
            Console.WriteLine(r0);
            Console.WriteLine(r1);
            Console.WriteLine(r2);
            Console.WriteLine("Hello World!");
        }
    }
}
