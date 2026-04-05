




using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Concrete;
using MyPortfolio.BusinessLayer.ValidationRules;
using MyPortfolio.DataAccessLayer.Abstract;
using MyPortfolio.DataAccessLayer.Context;
using MyPortfolio.DataAccessLayer.Repositories;
using MyPortfolio.EntityLayer.Concrete;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Katman Kayýtlarý (Dependency Injection) ---
builder.Services.AddScoped(typeof(IGenericDal<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IFeatureService, FeatureManager>();
builder.Services.AddScoped<IAboutService, AboutManager>();
builder.Services.AddScoped<IContactService, ContactManager>();
builder.Services.AddScoped<IExperienceService, ExperienceManager>();
builder.Services.AddScoped<IPortfolioService, PortfolioManager>();
builder.Services.AddScoped<ISkillService, SkillManager>();
builder.Services.AddScoped<ISocialMediaService, SocialMediaManager>();
builder.Services.AddScoped<ITestimonialService, TestimonialManager>();
builder.Services.AddScoped<IAppUserService, AppUserManager>();

// --- 2. Veritabaný Baðlantýsý ---
builder.Services.AddDbContext<MyPortfolioContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- 3. Araçlar (AutoMapper) ---
builder.Services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());

// --- 4. Identity ve Güvenlik Ayarlarý ---
builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<MyPortfolioContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
});

// --- 5. Cookie ve Oturum Politikasý ---
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login/Index/";
    options.LogoutPath = "/Login/Logout/";
    options.AccessDeniedPath = "/ErrorPage/Index/";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true; // Her iþlemde süre yenilenir
});

builder.Services.AddControllersWithViews();

builder.Services.AddValidatorsFromAssemblyContaining<AboutValidator>();
builder.Services.AddScoped<MyPortfolio.BusinessLayer.Helpers.FileImageHelper>();



var app = builder.Build();

// --- 6. Middleware (Pipeline) Sýralamasý ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// KRÝTÝK BÖLGE: Sýralama asla deðiþmemeli!
app.UseAuthentication(); // 1. Kimsin?
app.UseAuthorization();  // 2. Girebilir misin?

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// --- 7. Seed Data (Ýlk Çalýþtýrma Verisi) ---
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    if (!userManager.Users.Any())
    {
        var user = new AppUser
        {
            UserName = "admin",
            Email = "admin@site.com",
            Name = "Abdulkadir",
            Surname = "Admin",
            EmailConfirmed = true
        };
        await userManager.CreateAsync(user, "123456aA!");
    }
}

app.Run();
