using Identity_Udemy.ClaimProvider;
using Identity_Udemy.CustomValidation;
using Identity_Udemy.Models;
using Identity_Udemy.Requirement;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppIdentityDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
});
builder.Services.AddTransient<IAuthorizationHandler, ExpireDateExchangeHandler>();
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("AnkaraPolicy", policy =>
    {
        policy.RequireClaim("city", "ankara");
    });
    opt.AddPolicy("ViolencePolicy", policy =>
    {
        policy.RequireClaim("violance");
    });
    opt.AddPolicy("ExchangePolicy", policy =>
    {
        policy.AddRequirements(new ExpireDateExchangeRequirement());
    });
});

builder.Services.AddAuthentication().AddFacebook(opts =>
{
    opts.AppId = builder.Configuration["Authentication:Facebook:AppId"];
    opts.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];

});

//VERÝLERÝN DATABASE EKLEME YAPABÝLÝRÝYORUZ AddEntityFrameworkStores metoduyla.
builder.Services.AddIdentity<AppUser, AppRole>(opt =>
{
    opt.User.RequireUniqueEmail = true;
    opt.User.AllowedUserNameCharacters = "abcçdefgðhýijklmnoöpqrsþtuüvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";

    opt.Password.RequiredLength = 4;
    opt.Password.RequireNonAlphanumeric = false;//* ? vs gibi karakter þifreye girmesini istemiyoruz
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireDigit = false;//0-9 arasý sayý iþlemleri
}).AddPasswordValidator<CustomPasswordValidator>().AddUserValidator<CustomUserValidator>().AddErrorDescriber<CustomIdentityErrorDescriber>()
    .AddEntityFrameworkStores<AppIdentityDbContext>()
    .AddDefaultTokenProviders();

CookieBuilder cookieBuilder = new CookieBuilder();
cookieBuilder.Name = "MyBlog";
cookieBuilder.HttpOnly = false;
cookieBuilder.SameSite = SameSiteMode.Lax;
cookieBuilder.SecurePolicy = CookieSecurePolicy.SameAsRequest;

builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.LoginPath = new PathString("/Home/Login");
    opts.LogoutPath = new PathString("/Member/LogOut");
    opts.Cookie = cookieBuilder;
    opts.SlidingExpiration = true;
    opts.ExpireTimeSpan = System.TimeSpan.FromDays(60);
    opts.AccessDeniedPath = new PathString("/Member/AccessDenied");//Yetkisiz Kulanýcý sayfa eriþemediði ile ilgili bilgi verecek sayfa bu . 
});

builder.Services.AddScoped<IClaimsTransformation, ClaimProvider>();






var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStatusCodePages();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
