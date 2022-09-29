using Identity_Udemy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity_Udemy.Controllers
{
    public class BaseController : Controller
    {
        protected UserManager<AppUser> userManager { get; }
        protected AppUser CurrentUser => userManager.FindByNameAsync(User.Identity.Name).Result;
        protected SignInManager<AppUser> signInManager { get; }
        protected RoleManager<AppRole> roleManager { get; }

        public BaseController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager=null)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public void AddModelError(IdentityResult result)
        {
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);

            }
        }
    }
}
