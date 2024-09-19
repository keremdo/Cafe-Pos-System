using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DmlCafePos.Entities;

namespace DmlCafePos.Models
{
    public class OrderClosingViewModel
    {
        public Table Table{ get; set; } =null!;
        public Order Order{ get; set; } = null!;
        public List<string> OrderItems { get; set; } = null!;
        public List<Customer> Customers { get; set; } = new List<Customer>();
        public double SplitPrice { get; set; } = 0;

    }
}