using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Identity_Udemy.ViewModel
{
    public class PasswordResetViewModel
    {
        [Display(Name = "Email Adresiniz")]
        [Required(ErrorMessage = "Email Alanı Gereklidir")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Yeni Şifreniz")]
        [Required(ErrorMessage = "Şifre Alanı Gereklidir")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Şifreniz En Az 4 Karakterli Olmalıdır.")]
        public string PasswordNew { get; set; }

    }
}
