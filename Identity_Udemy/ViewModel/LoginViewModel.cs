
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Identity_Udemy.ViewModel
{
    public class LoginViewModel
    {

        [Display(Name ="Email Adresiniz")]
        [Required(ErrorMessage ="Email Alanı Gereklidir")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name="Şifreniz")]
        [Required(ErrorMessage ="Şifre Alanı Gereklidir")]
        [DataType(DataType.Password)]
        [MinLength(4,ErrorMessage ="Şifreniz En Az 4 Karakterli Olmalıdır.")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
