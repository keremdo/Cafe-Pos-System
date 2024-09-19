using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DmlCafePos.Entities
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public string? PaymentMethod { get; set; }
        public double PaymentPrice { get; set; }
        public string MandantID { get; set; } = null!;
        public DateTime PaymentDate { get; set; }
    }
}