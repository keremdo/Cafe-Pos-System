using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;

namespace DmlCafePos.Entities
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public Order Order { get; set; } = null!;
        public Product Product {get;set;} = null!;
        public int Quantity { get; set; }

        public double totalOrdedItemPrice {get{
            return Product.ProductPrice * Quantity;
        }}


        public string MandantID { get; set; } = null!;
    }
}