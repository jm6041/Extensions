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

            for (int i = 0; i <= 10; i++)
            {
                Guid nid = Guid.NewGuid();
                Console.WriteLine(nid);
                string guidb64 = nid.ToBase64Url();
                Console.WriteLine(guidb64);
                Console.WriteLine(guidb64.Length);

                GuidHelper.TryFromBase64(guidb64, out Guid gid);
                Console.WriteLine(gid);
                Console.WriteLine("-------------------------------");
            }
            Console.WriteLine("Hello World!");
        }
    }
}
