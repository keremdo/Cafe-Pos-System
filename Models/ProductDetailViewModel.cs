using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DmlCafePos.Entities;

namespace DmlCafePos.Models
{
    public class ProductDetailViewModel
    {
        public Product Product{ get; set; } = null!;
        public List<Category> Categories{ get; set; } = new List<Category>();
    }
}