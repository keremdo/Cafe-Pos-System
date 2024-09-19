using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DmlCafePos.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? ImageUrl { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public string MandantID { get; set; } = null!;

    }
}