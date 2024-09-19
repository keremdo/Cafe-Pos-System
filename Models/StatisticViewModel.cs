using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DmlCafePos.Entities;

namespace DmlCafePos.Models
{
    public class StatisticViewModel
    {
        public List<Order> Orders{ get; set; } = new List<Order>();
        public List<Table> Tables { get; set; } = new List<Table>();
        
        public List<Payment> Payments { get; set; } = new List<Payment>();
    }
}