using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using Jimlicat.OpenXml;

namespace OpenXmlConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IList<Order> source = Order.Source;
            List<ColumnInfo> list = new List<ColumnInfo>()
            {
                new ColumnInfo(){ PropertyName = nameof(Order.Id) },
                new ColumnInfo(){ PropertyName = nameof(Order.Name), Show="名字"},
                new ColumnInfo(){ PropertyName = nameof(Order.Enabled) },
                new ColumnInfo(){ PropertyName = nameof(Order.Created), Show="创建时间" },
                new ColumnInfo(){ PropertyName = nameof(Order.Price), Show="价格" },
                new ColumnInfo(){ PropertyName = nameof(Order.Quantity), Show="数量" },
                new ColumnInfo(){ PropertyName = nameof(Order.Season), Show="季节" },
                new ColumnInfo(){ PropertyName = nameof(Order.Updated), Show="更新时间" },
            };
            SpreadsheetInfo spreadsheetInfo = new SpreadsheetInfo(list);
            var exporter1 = SpreadsheetExporterFactory.Create(source, spreadsheetInfo);
            string file1 = Path.Combine(AppContext.BaseDirectory, Guid.NewGuid().ToString("N") + ".xlsx");
            exporter1.Export(file1);

            var exporter2 = SpreadsheetExporterFactory.Create(source);
            using (var ms2 = new MemoryStream())
            {
                exporter2.Export(ms2);
                ToFile(ms2);
            }

            var data = new List<ExpandoObject>();
            var dic1 = new ExpandoObject();
            dic1.TryAdd("Name", "T1");
            dic1.TryAdd("P0", 11);
            data.Add(dic1);
            var dic2 = new ExpandoObject();
            dic2.TryAdd("Name", "T2");
            dic2.TryAdd("P0", 22);
            data.Add(dic2);
            List<ColumnInfo> dataCInfos = new List<ColumnInfo>()
            {
                new ColumnInfo(){ PropertyName = "Name", Show="名字"},
                new ColumnInfo(){ PropertyName = "P0" },
            };

            var exporter3 = SpreadsheetExporterFactory.Create(data, new SpreadsheetInfo(dataCInfos));
            using (var ms3 = new MemoryStream())
            {
                exporter3.Export(ms3);
                ToFile(ms3);
            }

            Console.WriteLine("Hello World!");
        }

        public static void ToFile(MemoryStream ms)
        {
            string file = Path.Combine(AppContext.BaseDirectory, Guid.NewGuid().ToString("N") + ".xlsx");
            FileStream fs = null;
            try
            {
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
        }
    }

    public class Order
    {
        public Guid Id { get; set; }
        [DisplayName("名字")]
        public string Name { get; set; }
        [DisplayName("价格")]
        public double Price { get; set; }
        public int Quantity { get; set; }
        public Season Season { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        [DisplayName("是否启用")]
        public bool Enabled { get; set; }

        public static IList<Order> Source = Init();

        public static List<Order> Init()
        {
            Random r = new Random();
            // 添加测试数据
            List<Order> list = new List<Order>();
            for (int i = 0; i < 36; i++)
            {
                DateTimeOffset? up = null;
                int sr = r.Next(1, 4);
                Season sn;
                switch (sr) {
                    case 1:
                        sn = Season.Spring;
                        break;
                    case 2:
                        sn = Season.Summer;
                        break;
                    case 3:
                        sn = Season.Autumn;
                        break;
                    case 4:
                        sn = Season.Winter;
                        break;
                    default:
                        sn = Season.Spring;
                        break;
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
        [Description("春天")]
        Spring = 0,
        [Description("夏天")]
        Summer = 1,
        Autumn = 2,
        Winter = 3
    }
}
