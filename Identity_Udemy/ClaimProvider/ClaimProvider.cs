using Identity_Udemy.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Identity_Udemy.ClaimProvider
{
    public class ClaimProvider:IClaimsTransformation
    {
        private readonly UserManager<AppUser> userManager;

        public ClaimProvider(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal!=null && principal.Identity.IsAuthenticated)
            {
                ClaimsIdentity identity = principal.Identity as ClaimsIdentity;
                AppUser user = await userManager.FindByNameAsync(identity.Name);
                if (user!=null)
                {
                    if (user.BirthDay!=null)
                    {

                        var today = DateTime.Today;
                        var age=today.Year-user.BirthDay?.Year;
                        if (age>15)
                        {
                            Claim birthdayClaim = new Claim("violance", true.ToString(), ClaimValueTypes.String, "Internal");
                            identity.AddClaim(birthdayClaim);
                        }
                    }



                    if (user.City!=null)
                    {
                        if (!principal.HasClaim(c=>c.Type=="city"))
                        {
                            Claim cityClaim = new Claim("city", user.City, ClaimValueTypes.String, "Internal");
                        identity.AddClaim(cityClaim);
                        }

                    }
                }
            }
            return principal;

        }
    }
}
