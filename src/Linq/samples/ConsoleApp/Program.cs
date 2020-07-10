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

        public static IQueryable<UserInfo> GetSource()
        {
            List<UserInfo> datas = new List<UserInfo>();
            int count = 10;
            for (int i = 0; i < count; i++)
            {
                datas.Add(new UserInfo() { Id = i + 1, Name = "Asdfwef", Content = "sfsdfsdddssd", Value = i * 100.1 });
            }
            return datas.AsQueryable();
        }
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
            Ordering ordering0 = new Ordering { Name = "Id", Dir = Direction.Desc };
            var descDatas = datas.OrderBy(ordering0);
            Console.WriteLine("倒序后的值");
            Print(descDatas);

            List<Ordering> orderings = new List<Ordering>() { ordering0 };
            PageParameter pageParameter = new PageParameter() { PageIndex = -1, PageSize = 3, Orderings = orderings };
            var pageDatas = datas.Page(pageParameter);
            Console.WriteLine($"分页后的值 PageIndex:{pageParameter.PageIndex}   PageSize:{pageParameter.PageSize}");
            Print(pageDatas);

            var pagedResult = datas.ToPagedResult(pageParameter);
            Console.WriteLine($"分页结果 Toltal:{pagedResult.Toltal}");
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
