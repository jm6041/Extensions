using System;
using System.Collections.Generic;
using Wiry.Base32;

namespace BCLConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string guidb32 = Guid.NewGuid().ToBase32();
            Console.WriteLine(guidb32);
            Console.WriteLine(guidb32.Length);

            int maxymd = 9999 * 31 * 31 + 12 * 31 + 31;
            byte[] maxymdbs = BitConverter.GetBytes(maxymd);
            foreach (var ymdbt in maxymdbs)
            {
                Console.Write($"{ymdbt}\t");
            }

            Console.WriteLine(maxymd);
            string maxb32 = Base32.ToBase32NP(BitConverter.GetBytes(maxymd));
            Console.WriteLine(maxb32);
            Console.WriteLine(maxb32.Length);

            Console.WriteLine("3字节");
            byte[] max3bytes = { 255, 255, 255 };
            string max3bytesb32 = Base32.ToBase32NP(max3bytes);
            Console.WriteLine(max3bytesb32);
            Console.WriteLine(Base32.ToBase32(max3bytes));
            Console.WriteLine(Base32Encoding.Standard.GetString(max3bytes));
            Console.WriteLine(max3bytesb32.Length);
            byte[] max4bytes = { 255, 255, 255, 0 };
            Console.WriteLine(BitConverter.ToInt32(max4bytes));

            Console.WriteLine("2字节");
            ushort maxUInt16 = ushort.MaxValue;
            Console.WriteLine(maxUInt16);
            byte[] max2bytes = BitConverter.GetBytes(maxUInt16);
            string maxUInt16B32 = Base32.ToBase32NP(max2bytes);
            Console.WriteLine(maxUInt16B32);
            Console.WriteLine(Base32.ToBase32(max2bytes));
            Console.WriteLine(Base32Encoding.Standard.GetString(max2bytes));
            Console.WriteLine(maxUInt16B32.Length);

            Console.WriteLine("1字节");
            byte maxByte = byte.MaxValue;
            Console.WriteLine(maxByte);
            string maxByte32 = Base32.ToBase32NP(BitConverter.GetBytes(maxByte));
            Console.WriteLine(maxByte32);
            Console.WriteLine(maxByte32.Length);

            Console.WriteLine("5字节40位");
            byte[] byte40 = { 255, 255, 255, 255, 255 };
            string byte40B32 = Base32.ToBase32NP(byte40);
            Console.WriteLine(byte40B32);
            Console.WriteLine(Base32.ToBase32(byte40));
            Console.WriteLine(Base32Encoding.Standard.GetString(byte40));
            Console.WriteLine(byte40B32.Length);

            string b32 = "777777==";
            byte[] b32bs = Base32Encoding.Standard.ToBytes(b32);
            foreach (var bi in b32bs)
            {
                Console.Write($"{bi}\t");
            }
            Console.WriteLine();

            Console.WriteLine("9999-12");
            uint y9999 = 9999 * 12 + 12;
            byte[] y9999bs = BitConverter.GetBytes(y9999);
            foreach (var bi in y9999bs)
            {
                Console.Write($"{bi}\t");
            }
            Console.WriteLine();

            Console.WriteLine("Guid测试：");
            for (int i = 0; i <= 10; i++)
            {
                Guid nid = Guid.NewGuid();
                Console.WriteLine(nid.ToString("N"));
                byte[] gbs = nid.ToByteArray();
                string gidb32A = Base32.ToBase32(gbs);
                string gidb32B = Base32Encoding.Standard.GetString(gbs);
                string gidb32C = Base32.ToBase32NP(gbs);

                Console.WriteLine(gidb32A);
                Console.WriteLine(gidb32B);
                Console.WriteLine(gidb32C);

                byte[] tbs = Base32.FromBase32(gidb32C);
                Guid tid = new Guid(tbs);

                Console.WriteLine(tid.ToString("N"));

                Console.WriteLine("-------------------------------");
            }

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

            Console.WriteLine("---------- Id32 ----------");
            for (int i = 0; i <= 10; i++)
            {
                Console.WriteLine(Id32.NewId());
            }

            for (int i = 0; i <= 10; i++)
            {
                Console.WriteLine(Id32.NewId2());
            }

            Console.WriteLine("Hello World!");
        }
    }
}
