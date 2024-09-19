using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DmlCafePos.Entities;

namespace DmlCafePos.Models
{
    public class OrderViewModel
    {
        public List<Order> Orders { get; set; } = new List<Order>();

        public Order? Order{ get; set; }
        public List<Table> Tables { get; set; } = new List<Table>();
        public List<Product> Products{ get; set; } = new List<Product>(); 
        public List<Category> Categories  { get; set; } = new List<Category>();
        public Table Table { get; set; } = null!;
        public List<Area> Areas {get;set;} =  new List<Area>();
        public int AreaId {get;set;}
    }
}