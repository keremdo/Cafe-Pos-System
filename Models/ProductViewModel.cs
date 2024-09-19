using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DmlCafePos.Entities;

namespace DmlCafePos.Models
{
    public class ProductViewModel
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public Product Product { get; set; } = null!;
        public Category Category {get;set;}= null!;
        public string? ProductName { get; set; }
        public double ProductPrice { get; set; }
        public double ProductPurchasePrice { get; set; }
        public string? Description { get; set; }
        public string? CategoryName { get; set; }
        public int CategoryId { get; set; }


    }
}