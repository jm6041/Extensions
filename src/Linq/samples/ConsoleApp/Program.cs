using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public double Value { get; set; }

        public OrderInfo Order { get; set; }

        public static IQueryable<UserInfo> GetSource()
        {
            List<UserInfo> datas = new List<UserInfo>();
            int count = 10;
            for (int i = 0; i < count; i++)
            {
                datas.Add(new UserInfo() { Id = i + 1, Name = "Asdfwef", Content = "sfsdfsdddssd", Value = i * 100.1, Order = new OrderInfo() { Id = i + 2, Name = "Order" + (i + 1) } });
            }
            return datas.AsQueryable();
        }
    }

    public class OrderInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class MyODataParameter : ODataParameter
    {
        
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("安属性名分页查询");
            IQueryable<UserInfo> datas = UserInfo.GetSource();
            Console.WriteLine("原始值");
            Print(datas);
            var list = datas.ToList();
            list.OrderBy(x => x.Name);
            Ordering ordering0 = new Ordering { Name = "id", Dir = Direction.Desc };
            var descDatas = datas.OrderBy(ordering0);
            Console.WriteLine("倒序后的值");
            Print(descDatas);

            MyODataParameter odataPara = new MyODataParameter() { Top = 20, OrderBy = "Order.Name desc, id asc" };
            odataPara.AddOrder("Id", Direction.Asc);
            var descDatas1 = datas.ToODataResult(odataPara);
            Console.WriteLine("订单名字倒序后的值");
            Print(descDatas1.Result);

            List<Ordering> orderings = new List<Ordering>() { ordering0 };
            PageParameter pageParameter = new PageParameter() { PageIndex = 0, PageSize = 3, Orderings = orderings };
            var pageDatas = datas.Page(pageParameter);
            Console.WriteLine($"分页后的值 PageIndex:{pageParameter.PageIndex}   PageSize:{pageParameter.PageSize}");
            Print(pageDatas);

            var pagedResult = datas.ToPagedResult(pageParameter);
            Console.WriteLine($"分页结果 Toltal:{pagedResult.Count}");
            Print(pagedResult.Result);

            Console.ReadLine();
        }

        static void Print(IEnumerable<UserInfo> datas)
        {
            foreach (UserInfo d in datas)
            {
                Console.WriteLine($"Id:{d.Id}   Name:{d.Name}   Content:{d.Content}   Value:{d.Value}");
            }
        }
    }
}
