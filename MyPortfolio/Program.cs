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

// --- 1. Katman Kay�tlar� (Dependency Injection) ---
builder.Services.AddScoped(typeof(IGenericDal<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IFeatureService, FeatureManager>();
builder.Services.AddScoped<IAboutService, AboutManager>();
builder.Services.AddScoped<IContactService, ContactManager>();
builder.Services.AddScoped<IExperienceService, ExperienceManager>();
builder.Services.AddScoped<IPortfolioService, PortfolioManager>();
builder.Services.AddScoped<IPortfolioDetailService, PortfolioDetailManager>();
builder.Services.AddScoped<IPortfolioImageService, PortfolioImageManager>();
builder.Services.AddScoped<ISkillService, SkillManager>();
builder.Services.AddScoped<ISocialMediaService, SocialMediaManager>();
builder.Services.AddScoped<ITestimonialService, TestimonialManager>();
builder.Services.AddScoped<IAppUserService, AppUserManager>();
builder.Services.AddScoped<IMessageService, MessageManager>();
builder.Services.AddScoped<ITurnstileService,TurnstileService>();
///  Ayarlar� JSON'dan Model ile e�le�tir (Options Pattern)
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

//  Servis kayd�n� ger�ekle�tir
builder.Services.AddScoped<IMailService, MailManager>();

// --- 2. Veritaban� Ba�lant�s� ---
builder.Services.AddDbContext<MyPortfolioContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- 3. Ara�lar (AutoMapper) ---
builder.Services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());

// --- 4. Identity ve G�venlik Ayarlar� ---
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

// --- 5. Cookie ve Oturum Politikas� ---
builder.Services.ConfigureApplicationCookie(options =>
{
    // Rotalar
    options.LoginPath = "/Login/Index/";
    options.LogoutPath = "/Login/Logout/";
    options.AccessDeniedPath = "/Error/403"; // Temalı "Erişim Engellendi" sayfası

    // G�venlik ve Kimlik �zellikleri
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict; 
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; 
    options.Cookie.Name = "MyPortfolio.Identity.Cookie";

    // Oturum S�resi
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
});

// CSRF koruması: tüm POST/PUT/DELETE isteklerinde antiforgery token otomatik doğrulanır.
// Form tag helper'ları token'ı formlara kendiliğinden gömer; AJAX istekleri ise
// token'ı aşağıda tanımlanan header ile gönderir (bkz. site.js iletişim formu).
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute());

    // Admin'de yapılan her başarılı POST, public sayfa önbelleğinin sürümünü yeniler
    options.Filters.Add(typeof(MyPortfolio.WebUI.Filters.AdminContentCacheBustFilter));
});

// --- Önbellekleme ---
// Ana sayfa bileşenleri cache tag helper ile önbelleğe alınır (bkz. Home/Index.cshtml);
// böylece her ziyarette 8+ DB sorgusu yerine önbellekten hazır HTML servis edilir.
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<MyPortfolio.WebUI.Services.ContentCacheVersion>();

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "RequestVerificationToken";
});

builder.Services.AddValidatorsFromAssemblyContaining<AboutValidator>();
builder.Services.AddScoped<MyPortfolio.BusinessLayer.Helpers.FileImageHelper>();




//UI D�zenleme 



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






// --- 6. Middleware (Pipeline) S�ralamas� ---
if (!app.Environment.IsDevelopment())
{
    // Yakalanmam�� exception'lar temal� 500 sayfas�na y�nlenir (ErrorController)
    app.UseExceptionHandler("/Error/500");
    app.UseHsts();
}

// 404, 403, 429 gibi bo� g�vdeli hata yan�tlar�n� temal� hata sayfas�yla de�i�tirir.
// URL de�i�mez (re-execute), JSON d�nen endpoint'ler etkilenmez.
app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseHttpsRedirection();

// --- G�venlik Ba�l�klar� (Security Headers) ---
app.Use(async (context, next) =>
{
    // Taray�c�, sunucunun bildirdi�i i�erik t�r�ne g�venir; MIME-sniffing yapamaz
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    // Site ba�ka sitelerin i�ine iframe olarak g�m�lemez (clickjacking engeli)
    context.Response.Headers["X-Frame-Options"] = "DENY";
    // D�� sitelere ge�erken tam URL yerine sadece origin bilgisi s�zar
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    // Kullan�lmayan taray�c� yetenekleri (kamera, mikrofon, konum) kapat�l�r
    context.Response.Headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=()";
    await next();
});

app.UseWebOptimizer();
app.UseStaticFiles();

app.UseRouting();

// --- Rate Limiting (istek sınırlama) ---
// Mesaj formu ve login POST'larını IP başına sınırlar; brute-force ve spam'i frenler.
// Kurallar middleware'in kendi içinde tanımlı: Middlewares/SimpleRateLimitMiddleware.cs
app.UseMiddleware<MyPortfolio.WebUI.Middlewares.SimpleRateLimitMiddleware>();

// KR�T�K B�LGE: S�ralama asla de�i�memeli!
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

    // E�er veritaban�nda hi� kullan�c� yoksa (�lk kurulum)
    if (!userManager.Users.Any())
    {
        var adminSettings = configuration.GetSection("AdminUser");

        var user = new AppUser
        {
            UserName = adminSettings["UserName"] ?? "admin", // Null ise varsay�lan de�er
            Email = adminSettings["Email"] ?? "admin@site.com",
            Name = "Abdulkadir",
            Surname = "Admin",
            EmailConfirmed = true
        };

        // appsettings'den oku, yoksa varsay�lan g�venli �ifreyi dene
        string password = adminSettings["Password"] ?? "AS_Portfolio_2026_V1!";

        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            // E�er Identity kurallar�na (8 karakter vb.) tak�l�rsa hata burada yakalan�r
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"Seed Hata: {error.Description}");
            }
        }
    }
}

app.Run();
