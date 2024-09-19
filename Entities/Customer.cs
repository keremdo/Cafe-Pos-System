using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DmlCafePos.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }
        
        public string CustomerName { get; set;} = null!;
        
        public string CustomerSurname { get; set; } = null!;
        public string CustomerFullName {get {
            return CustomerName + " " + CustomerSurname;
        }}
        public string CustomerPhone { get; set; } = null!;
        public double CustomerBalance { get; set; } = 0;
        public string MandantID { get; set; } = null!;

    }
}