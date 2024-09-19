using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DmlCafePos.Entities;

namespace DmlCafePos.Models
{
    public class CustomerListViewModel
    {
        public List<Customer> Customers{ get; set; } = new List<Customer>();
        public Customer? Customer {get; set;}  
        public string? CustomerName {get; set;} 
        public string? CustomerSurname {get; set;} 
        public string? CustomerPhone {get; set;} 
    }
}