using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dmlcafepos.ViewModels
{
    public class CreateViewModel
    {
        [Required(ErrorMessage ="İsim Soyisim alanı boş bırakılamaz")]
        public string? FullName { get; set; } = string.Empty;
        [Required(ErrorMessage ="E Posta alanı boş bırakılamaz")]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage = "Şifreler Birbiri ile Eşleşmiyor")]
        public string? ConfirmPassword { get; set; }
    }
}