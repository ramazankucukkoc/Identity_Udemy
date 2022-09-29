using System.ComponentModel.DataAnnotations;

namespace Identity_Udemy.ViewModel
{
    public class RoleViewModel
    {
        [Display(Name = "Email Adresiniz")]
        [Required(ErrorMessage = "Email Alanı Gereklidir")]
        public string Name { get; set; }
        public string Id { get; set; }
    }
}
