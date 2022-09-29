using Identity_Udemy.Enums;
using Identity_Udemy.Models;
using Identity_Udemy.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace Identity_Udemy.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : base(userManager, signInManager)
        {
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Member");
            }
            return View();
        }
        public IActionResult LogIn(string ReturnUrl)
        {
            TempData["ReturnUrl"]=ReturnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel userLogin)
        {
            if (ModelState.IsValid)
            {
                AppUser user=await userManager.FindByEmailAsync(userLogin.Email);
                if (user!=null)
                {

                    if (await userManager.IsLockedOutAsync(user))
                    {
                        ModelState.AddModelError("", "Hesabınız bir süreliğine kilitlenmiştir.Lütfen daha sonrs tekrar deneyiniz");

                    }


                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, userLogin.Password, userLogin.RememberMe, false);
                    if (result.Succeeded)
                    {
                        await userManager.ResetAccessFailedCountAsync(user);

                        if (TempData["ReturnUrl"]!=null)
                        {
                            return Redirect(TempData["ReturnUrl"].ToString());
                        }
                        return RedirectToAction("Index", "Member");
                    }
                    else
                    {
                        await userManager.AccessFailedAsync(user);
                       
                        int fail = await userManager.GetAccessFailedCountAsync(user);
                        ModelState.AddModelError("", $"{fail}. kez başarısız giriş");
                        if (fail==3)
                        {
                            await userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.Now.AddMinutes(20)));
                            ModelState.AddModelError("", "Hesabınıza 3 başarısız girişten dolayı 20 dakika süreyle kilitlenmiştir.Lütfen daha sonra tekrar deneyiniz");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Geçersiz email adresi veya şifresi ");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Bu email adresine kayıtlı kullanıcı bulunmamıştır.");
                }
            }
            return View(userLogin);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModel userViewModel)
        {
            //ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));
            ModelState.Remove("City");
            ModelState.Remove("Gender");
            ModelState.Remove("Picture");
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser();
                appUser.UserName = userViewModel.UserName;
                appUser.Email=userViewModel.Email;
                appUser.PhoneNumber = userViewModel.PhoneNumber;

            IdentityResult result=await userManager.CreateAsync(appUser, userViewModel.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("LogIn");

                }
                else
                {
                    AddModelError(result);
                }
            }


            return View(userViewModel);
        }
    
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public  IActionResult ResetPassword(PasswordResetViewModel passwordResetViewModel)
        {
            AppUser user = userManager.FindByEmailAsync(passwordResetViewModel.Email).Result;
            if (user != null)
            {
                string passwordResetToken=userManager.GeneratePasswordResetTokenAsync(user).Result;
                string passwrodResetLink = Url.Action("ResetPasswordConfirm", "Home", new
                {
                    userId = user.Id,
                    token = passwordResetToken
                }, HttpContext.Request.Scheme);
                Helper.PasswordReset.PasswordResetSendEmail(passwrodResetLink);
                ViewBag.status = "success";
            }
            else
            {
                ModelState.AddModelError("", "Sistemde kayıtlı email adresi bulunmamıştır");
            }
                return View(passwordResetViewModel);
        }
        public IActionResult ResetPasswordConfirm(string userId,string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;
            return View();
        }
        public async Task<IActionResult> ResetPasswordConfirm([Bind("PasswordNew")] PasswordResetViewModel passwordResetViewModel)
        {
            string token = TempData["token"].ToString();
            string userId = TempData["userId"].ToString();

            AppUser user = await userManager.FindByIdAsync(userId);

            if (user!=null)
            {
                IdentityResult result = await userManager.ResetPasswordAsync(user, token, passwordResetViewModel.PasswordNew);

                if (result.Succeeded)
                {

                    await userManager.UpdateSecurityStampAsync(user);

                    ViewBag.status = "success";
                }
                else
                {
                    AddModelError(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "Böyle kullanıcı bulunamamıştır");
            }

            return View(passwordResetViewModel);
        }
    }
}