

using Identity_Udemy.Enums;
using System.ComponentModel.DataAnnotations;

namespace Identity_Udemy.ViewModel
{
	public class UserViewModel
	{
		[Required(ErrorMessage ="Kullanıcı isim gereklidir.")]
		[Display(Name ="Kulanıcı Adı")]
		public string UserName { get; set; }


        [Display(Name = "Tel No")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email adresi gereklidir.")]
        [Display(Name = "Email adresiniz")]
		[EmailAddress(ErrorMessage ="Email adresi dogru formatta değil")]
        public string Email { get; set; }



        [Required(ErrorMessage = "Şifreniz gereklidir.")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name ="Doğum Tarihiniz")]
        [DataType(DataType.Date)]
        public DateTime? BirthDay { get; set; }
       
        public string Picture { get; set; }

        [Display(Name = "Şehir")]
        public string City { get; set; }

        [Display(Name = "Cinsiyet")]
        public Gender Gender { get; set; }


    }
}
