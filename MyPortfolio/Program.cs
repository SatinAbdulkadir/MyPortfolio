using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Concrete;
using MyPortfolio.BusinessLayer.Models;
using MyPortfolio.BusinessLayer.ValidationRules;
using MyPortfolio.DataAccessLayer.Abstract;
using MyPortfolio.DataAccessLayer.Context;
using MyPortfolio.DataAccessLayer.Repositories;
using MyPortfolio.EntityLayer.Concrete;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();

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
builder.Services.AddScoped<IMessageService, MessageManager>();
builder.Services.AddScoped<ITurnstileService,TurnstileService>();
///  Ayarlarý JSON'dan Model ile eþleþtir (Options Pattern)
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

//  Servis kaydýný gerçekleþtir
builder.Services.AddScoped<IMailService, MailManager>();

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
    options.Password.RequiredLength = 8;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 5;
});

// --- 5. Cookie ve Oturum Politikasý ---
builder.Services.ConfigureApplicationCookie(options =>
{
    // Rotalar
    options.LoginPath = "/Login/Index/";
    options.LogoutPath = "/Login/Logout/";
    options.AccessDeniedPath = "/ErrorPage/Index/";

    // Güvenlik ve Kimlik Özellikleri
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict; 
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; 
    options.Cookie.Name = "MyPortfolio.Identity.Cookie";

    // Oturum Süresi
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
});

builder.Services.AddControllersWithViews();

builder.Services.AddValidatorsFromAssemblyContaining<AboutValidator>();
builder.Services.AddScoped<MyPortfolio.BusinessLayer.Helpers.FileImageHelper>();



//UI Düzenleme 



builder.Services.AddWebOptimizer(pipeline =>
{
    pipeline.AddCssBundle("/css/site.css",
        "/css/_variables.css",
        "/css/_base.css",
        "/css/_layout.css",
        "/css/_components.css",
        "/css/_animations.css",
        "/css/_responsive.css"
    );
});
var app = builder.Build();






// --- 6. Middleware (Pipeline) Sýralamasý ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseWebOptimizer();
app.UseStaticFiles();

app.UseRouting();

// KRÝTÝK BÖLGE: Sýralama asla deðiþmemeli!
app.UseAuthentication(); // 1. Kimsin?
app.UseAuthorization();  // 2. Girebilir misin?

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// --- 7. Seed Data (Kurumsal Standart) ---
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

    // Eðer veritabanýnda hiç kullanýcý yoksa (Ýlk kurulum)
    if (!userManager.Users.Any())
    {
        var adminSettings = configuration.GetSection("AdminUser");

        var user = new AppUser
        {
            UserName = adminSettings["UserName"] ?? "admin", // Null ise varsayýlan deðer
            Email = adminSettings["Email"] ?? "admin@site.com",
            Name = "Abdulkadir",
            Surname = "Admin",
            EmailConfirmed = true
        };

        // appsettings'den oku, yoksa varsayýlan güvenli þifreyi dene
        string password = adminSettings["Password"] ?? "AS_Portfolio_2026_V1!";

        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            // Eðer Identity kurallarýna (8 karakter vb.) takýlýrsa hata burada yakalanýr
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"Seed Hata: {error.Description}");
            }
        }
    }
}

app.Run();
