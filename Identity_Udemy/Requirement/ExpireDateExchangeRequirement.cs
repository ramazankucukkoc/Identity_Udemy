using Microsoft.AspNetCore.Authorization;

namespace Identity_Udemy.Requirement
{
    public class ExpireDateExchangeRequirement:IAuthorizationRequirement
    {

    }
    public class ExpireDateExchangeHandler : AuthorizationHandler<ExpireDateExchangeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ExpireDateExchangeRequirement requirement)
        {
            if (context.User!=null && context.User.Identity != null)
            {
                var claim = context.User.Claims.Where(x => x.Type == "ExpireDateExchange" && x.Value != null).FirstOrDefault();
                if (claim!=null)
                {
                    if (DateTime.Now<Convert.ToDateTime(claim.Value))
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                    }
                }
            }
            //Task dönmemiz gerekiyor onun  return Task.CompletedTask; dönüyorum.
            return Task.CompletedTask;

        }
    }
}
