using Microsoft.EntityFrameworkCore;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Concrete;
using MyPortfolio.DataAccessLayer.Abstract;
using MyPortfolio.DataAccessLayer.Context;
using MyPortfolio.DataAccessLayer.Repositories;

var builder = WebApplication.CreateBuilder(args);


// 1. DataAccess Katmaný Kaydý (Generic olduðu için typeof kullanýyoruz)
builder.Services.AddScoped(typeof(IGenericDal<>), typeof(GenericRepository<>));

// 2. Business Katmaný Kaydý
builder.Services.AddScoped<IFeatureService, FeatureManager>();
builder.Services.AddScoped<IAboutService, AboutManager>();
builder.Services.AddScoped<IContactService, ContactManager>();
builder.Services.AddScoped<IExperienceService, ExperienceManager>();
builder.Services.AddScoped<IPortfolioService, PortfolioManager>();
builder.Services.AddScoped<ISkillService, SkillManager>();
builder.Services.AddScoped<ISocialMediaService, SocialMediaManager>();
builder.Services.AddScoped<ITestimonialService, TestimonialManager>();







//////////////////////////////////////////////////////////////////////////////////


// DbContext'i sisteme ve appsettings'deki baðlantýya entegre ediyoruz:
builder.Services.AddDbContext<MyPortfolioContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
