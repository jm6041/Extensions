using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using ODataDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODataDemo.Controllers
{
    public class ProductsController:ODataController
    {
        [EnableQuery]
        [HttpGet]
        public List<Product> Get()
        {
            return Product.ProductsDemo;
        }
    }
}
