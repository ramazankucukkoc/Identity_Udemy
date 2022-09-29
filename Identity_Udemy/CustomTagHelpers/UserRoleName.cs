using Identity_Udemy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Identity_Udemy.CustomTagHelpers
{
    [HtmlTargetElement("td",Attributes="user-roles")]
    public class UserRoleName:TagHelper
    {
        public UserManager<AppUser> UserManager { get; set; }

        public UserRoleName(UserManager<AppUser> userManager)
        {
            UserManager = userManager;
        }
        [HtmlAttributeName("user-roles")]
        public string UserId { get; set; }


        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            AppUser user=await UserManager.FindByIdAsync(UserId);
            IList<string> roles = await UserManager.GetRolesAsync(user);
            string html = string.Empty;
            roles.ToList().ForEach(x => {
                html += $"<span class='text-info'>{x} </span>";
            });
            output.Content.SetHtmlContent(html);
           
        }
    }
}
