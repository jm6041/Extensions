using Jimlicat.OpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace OpenXmlConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTimeOffset? ndto = null;
            int? nint = null;
            if (ndto is null)
            {
                Console.WriteLine("ndto is null");
            }
            if (nint is null)
            {
                Console.WriteLine("nint is null");
            }
            object tintobj = 6;
            if (tintobj is null)
            {
                Console.WriteLine("tintobj is null");
            }
            if (tintobj is int)
            {
                Console.WriteLine("tintobj is int");
            }

            Order nullOrder = new Order();
            if (nullOrder is null)
            {
                Console.WriteLine("nullOrder is null Order");
            }

            IList<Order> source = Order.Source;
            List<ColumnItem> list = new List<ColumnItem>()
            {
                new ColumnItem(){ PropertyName = nameof(Order.Id) },
                new ColumnItem(){ PropertyName = nameof(Order.Name), Show="名字"},
                new ColumnItem(){ PropertyName = nameof(Order.Enabled) },
                new ColumnItem(){ PropertyName = nameof(Order.Created), Show="创建时间" },
                new ColumnItem(){ PropertyName = nameof(Order.Price), Show="价格" },
                new ColumnItem(){ PropertyName = nameof(Order.Quantity), Show="数量" },
                new ColumnItem(){ PropertyName = nameof(Order.Season), Show="季节" },
                new ColumnItem(){ PropertyName = nameof(Order.Updated), Show="更新时间" },
            };
            var cc = new ColumnCollection();
            cc.Items.AddRange(list);
            var exporter = SpreadsheetExporterFactory.Create(source, cc);
            string file = Path.Combine(AppContext.BaseDirectory, Guid.NewGuid().ToString("N") + ".xlsx");
            MemoryStream ms = null;
            FileStream fs = null;
            try
            {
                ms = exporter.Export();                
                fs = new FileStream(file, FileMode.Create);
                ms.Position = 0;
                ms.CopyTo(fs);
                ms.Flush();
                fs.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (ms != null)
                {
                    ms.Close();
                    ms.Dispose();
                }
                fs.Close();
                fs.Dispose();
            }
            Console.WriteLine("Hello World!");
        }
    }

    public class Order
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public Season Season { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public bool Enabled { get; set; }

        public static IList<Order> Source = Init();

        public static List<Order> Init()
        {
            Random r = new Random();
            // 添加测试数据
            List<Order> list = new List<Order>();
            for (int i = 0; i < 3; i++)
            {
                DateTimeOffset? up = null;
                Season sn = Season.Autumn;
                if (i % 4 == 1)
                {
                    sn = Season.Summer;
                }
                if (i % 2 == 0)
                {
                    up = DateTimeOffset.Now;
                }
                Order f = new Order()
                {
                    Id = Guid.NewGuid(),
                    Name = "Order" + (i + 1).ToString(),
                    Price = r.NextDouble() * 10000,
                    Quantity = i + 1,
                    Created = DateTimeOffset.Now,
                    Season = sn,
                    Enabled = i % 2 == 0,
                    Updated = up,
                };
                list.Add(f);
            }
            return list;
        }        
    }

    public enum Season
    {
        Spring = 0,
        Summer = 1,
        Autumn = 2,
        Winter = 3
    }
}
