using System;

namespace BCLConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string guidb32 = Guid.NewGuid().ToBase32();
            Console.WriteLine(guidb32);
            Console.WriteLine(guidb32.Length);

            string guidb64 = Guid.NewGuid().ToBase64();
            Console.WriteLine(guidb64);
            Console.WriteLine(guidb64.Length);

            Console.WriteLine("Hello World!");
        }
    }
}
