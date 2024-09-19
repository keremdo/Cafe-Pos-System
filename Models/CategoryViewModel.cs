using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dmlcafepos.Data.Concrete.EfCoreIdentity;
using Dmlcafepos.Entities;
using DmlCafePos.Entities;

namespace DmlCafePos.Models
{
    public class CategoryViewModel
    {
        public List<Category> Categories {get;set;} = new List<Category>(); 
        public string CategoryName { get; set; } = null!;
        public int CategoryId { get; set; }
        public Category Category {get;set; }= null!;
        public Product Product {get;set;} = null!;
        public string? ProductName {get;set;}   
        public double ProductPurchasePrice {get;set; } = 0;
        public double ProductPrice {get;set;}

    }
}