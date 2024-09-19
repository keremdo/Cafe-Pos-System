using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DmlCafePos.ViewModels
{
    public class SignUpViewModel
    {
        [EmailAddress]
        public string Email { get; set; } = null!;
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage = "Şifreler Birbiri ile Eşleşmiyor")]
        public string ConfirmPassword { get; set; } = null!;
        public string FullName { get; set; } = null!;

        public string Address { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;


    }
}