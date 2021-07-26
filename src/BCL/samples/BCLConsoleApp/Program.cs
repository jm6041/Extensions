using System;
using System.Collections.Generic;
using Wiry.Base32;

namespace BCLConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Guid gid = Guid.NewGuid();
            Console.WriteLine(gid.ToString("N").ToUpper());
            byte[] gidbs = gid.ToByteArray();
            string guidb32 = Base32.ToBase32NP(gidbs);
            Console.WriteLine(guidb32);
            Console.WriteLine(guidb32.Length);

            byte[] tnumbs = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            PrintBytes(tnumbs);
            PrintBytes(tnumbs[0..4]);
            PrintBytes(tnumbs[2..^3]);
            PrintBytes(tnumbs[0..5]);
            PrintBytes(tnumbs[5..10]);

            ulong gidlp1 = BitConverter.ToUInt64(gidbs[0..8]);
            ulong gidlp2 = BitConverter.ToUInt64(gidbs[8..]);
            string c36g1 = XMath.XConvert36(gidlp1);
            string c36g2 = XMath.XConvert36(gidlp2);
            string c36max = XMath.XConvert36(ulong.MaxValue);
            Console.WriteLine(c36g1);
            Console.WriteLine(c36g1.Length);
            Console.WriteLine(c36g2);
            Console.WriteLine(c36g2.Length);
            Console.WriteLine(c36max);
            Console.WriteLine(c36max.Length);

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

            Console.WriteLine("---------- Id32 ----------");
            for (int i = 0; i <= 10; i++)
            {
                Console.WriteLine(Id32.NewId());
            }

            Console.WriteLine("Hello World!");
        }

        public static void PrintBytes(byte[] bs)
        {
            foreach (var b in bs)
            {
                Console.Write($"{b} ");
            }
            Console.WriteLine();
        }
    }
}
