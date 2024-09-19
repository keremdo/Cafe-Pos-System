using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Dmlcafepos.Entities;

namespace Dmlcafepos.ViewModels
{
    public class EditViewModel
    {
        [Required]
        public string? Id { get; set; }
        // [Required(ErrorMessage ="İsim Soyisim alanı boş bırakılamaz")]
        public string? FullName { get; set; }
        // [Required(ErrorMessage ="E Posta alanı boş bırakılamaz")]
        [EmailAddress]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage = "Şifreler Birbiri ile Eşleşmiyor")]
        public string? ConfirmPassword { get; set; }
        public IList<string>? SelectedRoles {get;set;} 
        public AppUser? AppUser { get; set; }
    }
}