using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DmlCafePos.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public Table Table { get; set; } = null!;

        public string MandantID { get; set; } = null!;
        public bool isActive { get; set; } = false;
        public List<string> OrderItems { get; set; } = null!;
        public double Spread { get; set; } = 0;
        public double Price { get; set; } = 0;
        public DateTime? orderDate { get; set; }
        
    }
}