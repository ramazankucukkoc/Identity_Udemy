using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity_Udemy.Models
{
    //string yazmamızın sebebi Pirmary keyleri string olarak eşleştirme
    public class AppIdentityDbContext:IdentityDbContext<AppUser,AppRole,string>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options):base(options)
        {

        }

    }
}
