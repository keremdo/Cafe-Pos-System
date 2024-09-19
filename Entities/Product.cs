using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DmlCafePos.Entities
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public double ProductPurchasePrice { get; set; } = 0;
        public double ProductPrice  { get; set; }

        public double Spread { get{
            return ProductPrice - ProductPurchasePrice;
        } } 
        public string? ImageUrl { get; set; }
        [Required(ErrorMessage ="Kategori Seçimi Zorunludur")]
        public Category Category { get; set; } = null!;
        public string MandantID {get;set;} = null!;

    }
}