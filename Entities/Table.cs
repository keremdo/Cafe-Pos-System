using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DmlCafePos.Entities
{
    public class Table
    {
        public int TableId { get; set; }
        public string? TableName { get; set; }
        public Area Area {get; set;} = null!;
        public string MandantID {get;set;} = null!;
        public List<Order> Orders {get; set; } = new List<Order>();

        public bool isEmpty { get; set; } = false;
    }
}