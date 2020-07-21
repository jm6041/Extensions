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
            IList<Order> source = Order.Source;
            List<ColumnItem> list = new List<ColumnItem>()
            {
                new ColumnItem(){ PropertyName = nameof(Order.Id) },
                new ColumnItem(){ PropertyName = nameof(Order.Name), Show="名字"},
                new ColumnItem(){ PropertyName = nameof(Order.Enabled) },
                new ColumnItem(){ PropertyName = nameof(Order.Created), Show="创建时间" },
                new ColumnItem(){ PropertyName = nameof(Order.Price), Show="价格" },
                new ColumnItem(){ PropertyName = nameof(Order.Quantity), Show="数量" },
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
        public DateTimeOffset Created { get; set; }
        public bool Enabled { get; set; }

        public static IList<Order> Source = Init();

        public static List<Order> Init()
        {
            Random r = new Random();
            // 添加测试数据
            List<Order> list = new List<Order>();
            for (int i = 0; i < 3; i++)
            {
                string n = "f" + (i + 1).ToString();
                Order f = new Order()
                {
                    Id = Guid.NewGuid(),
                    Name = "Order" + (i + 1).ToString(),
                    Price = r.NextDouble() * 10000,
                    Quantity = i + 1,
                    Created = DateTimeOffset.Now,
                    Enabled = i % 2 == 0,
                };
                list.Add(f);
            }
            return list;
        }
    }
}
