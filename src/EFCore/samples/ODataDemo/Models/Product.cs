using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODataDemo.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public readonly static List<Product> ProductsDemo = new List<Product>()
        {
            new Product(){ Id = 1, Name = "AA1", Price = 100 },
            new Product(){ Id = 2, Name = "AA2", Price = 200 },
            new Product(){ Id = 3, Name = "AB2", Price = 200 },
            new Product(){ Id = 4, Name = "BB4", Price = 10 },
            new Product(){ Id = 5, Name = "CCCC2", Price = 15 },
            new Product(){ Id = 6, Name = "ADse2", Price = 200 },
            new Product(){ Id = 7, Name = "BD1", Price = 22 },
            new Product(){ Id = 8, Name = "CC12d", Price = 67 },
            new Product(){ Id = 9, Name = "Basd12", Price = 100 },
            new Product(){ Id = 10, Name = "BadA01", Price = 89 },
        };
    }
}
