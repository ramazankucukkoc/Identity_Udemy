using Identity_Udemy.Enums;
using Identity_Udemy.Models;
using Identity_Udemy.ViewModel;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Identity_Udemy.Controllers
{   
    // Bir controller uzerine  [Authorize] attiriubute eklersen sadece üyeler girer.

    [Authorize] 
    public class MemberController : BaseController
    {
        public MemberController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : base(userManager, signInManager)
        {
        }

        public IActionResult Index()
        {
            AppUser user = CurrentUser;
            UserViewModel userViewModel = user.Adapt<UserViewModel>();
            return View(userViewModel);

        }
        public IActionResult UserEdit()
        {
            AppUser user =CurrentUser;


            UserViewModel userViewModel =user.Adapt<UserViewModel>();

            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));

            return View(userViewModel);
        }
        [HttpPost]
        public async Task<IActionResult>UserEdit(UserViewModel userViewModel,IFormFile userPicture)
        {
            ModelState.Remove("Password");
            ModelState.Remove("Picture");

            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));

            if (ModelState.IsValid)
            {
                AppUser user = CurrentUser;
                if (userPicture != null && userPicture.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(userPicture.FileName);

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserPicture", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await userPicture.CopyToAsync(stream);
                        user.Picture = "/UserPicture/" + fileName;
                    }
                }
                user.UserName=userViewModel.UserName;
                user.Email = userViewModel.Email;
                user.PhoneNumber=userViewModel.PhoneNumber;
                user.City = userViewModel.City;
                user.BirthDay = userViewModel.BirthDay;
                user.Gender = (int)userViewModel.Gender;

                IdentityResult result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    await userManager.UpdateSecurityStampAsync(user);
                    await signInManager.SignOutAsync();
                    await signInManager.SignInAsync(user, true);

                    ViewBag.success = "true";
                }
                else
                {
                  AddModelError(result);
                }
            }
            return View(userViewModel);//Kullanıcı girmiş oldugu degerler yanlış ise textboxların içindeki degerler boş olmasın diye.
        }
        public IActionResult PasswordChange()
        {
            return View();
        }       
        [HttpPost]
        public IActionResult PasswordChange(PasswordChangeViewModel passwordChangeViewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser user = CurrentUser;

                    bool exist = userManager.CheckPasswordAsync(user, passwordChangeViewModel.PaaswordOld).Result;
                    if (exist)
                    {
                        IdentityResult result = userManager.ChangePasswordAsync(user, passwordChangeViewModel.PaaswordOld,
                            passwordChangeViewModel.PasswordNew).Result;
                        if (result.Succeeded)
                        {
                        userManager.UpdateSecurityStampAsync(user);//Bunu yazmak zorundayız çünkü eski şifreyle 
                        //sayfalarda dolaşmamısı için yazdık.Şifre değiştirdikden sonra yazıyoruz.
                        signInManager.SignOutAsync();
                        signInManager.SignOutAsync();
                        signInManager.PasswordSignInAsync(user,passwordChangeViewModel.PasswordNew,true,false);

                            ViewBag.success = "true";
                        }
                        else
                        {
                        AddModelError(result);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Eski şifreniz yanlış");
                    }                
            }
            return View(passwordChangeViewModel);
        }
        public void LogOut()
        {
            signInManager.SignOutAsync();
        }
        public IActionResult AccessDenied(string ReturnUrl)
        {
            if (ReturnUrl.ToLower().Contains("violencePage"))
            {
            ViewBag.message = "Erişmeye çalıştığınız sayfa şiddet videoları içerdiğinden dolayı 15 yaşında büyük olmanız gerekmektedir.";
            }
            else if (ReturnUrl.ToLower().Contains("ankaraPage"))
            {
                ViewBag.message = "Erişmeye çalıştığınız sayfaya sadece şehri ankara olan kullanıcılar erişebilir.";
            }
            else if (ReturnUrl.ToLower().Contains("exchange"))
            {
                ViewBag.message = "30 günlük ücrretsiz deneme hakkınız sona ermiştir.";
            }
            else
            {
                ViewBag.message = "Bu sayfaya erişim izniniz yoktur.Erişim almak için site yöneticisiyle görüşürüz.";
            }

            return View();
        }


        [Authorize(Roles = "manager,admin")]
        public IActionResult Manager()
        {
            return View();
        }

        [Authorize(Roles ="editor,admin")]
        public IActionResult Editor()
        {
            return View();
        }

        [Authorize(Policy = "AnkaraPolicy")]
        public IActionResult AnkaraPage()
        {
            return View();
        }

        [Authorize(Policy = "ViolencePolicy")]
        public IActionResult ViolencePage()
        {
            return View();
        }
   
        public async Task<IActionResult> ExchangeRedirect()
        {
            bool result = User.HasClaim(x => x.Type == "ExpireDateExchange");
            if (!result)
            {
                Claim ExpireDateExchange = new Claim("ExpireDateExchange",
                    DateTime.Now.AddDays(30).Date.ToShortDateString(),ClaimValueTypes.String,"Internal");

                await userManager.AddClaimAsync(CurrentUser, ExpireDateExchange);
                await signInManager.SignOutAsync();
                await signInManager.SignInAsync(CurrentUser, true);

            }
            return RedirectToAction("Exchange");
        }

        [Authorize(Policy = "ExchangePolicy")]
        public IActionResult Exchange()
        {
            return View();
        }
    


    }
}
