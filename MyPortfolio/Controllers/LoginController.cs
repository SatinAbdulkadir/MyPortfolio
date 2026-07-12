using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Dtos.AppUserDtos;
using MyPortfolio.EntityLayer.Concrete;
using Microsoft.Extensions.Configuration;
using MyPortfolio.WebUI.Models;

[AllowAnonymous]
public class LoginController : Controller
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IValidator<LoginDto> _validator;
    private readonly IConfiguration _configuration;

    public LoginController(SignInManager<AppUser> signInManager, IValidator<LoginDto> validator, IConfiguration configuration)
    {
        _signInManager = signInManager;
        _validator = validator;
        _configuration = configuration;
    }

    
    [HttpGet]
    public IActionResult Index(string key)
    {
        var secretKey = _configuration.GetValue<string>("AdminSettings:LoginKey");

        if (key==secretKey)
        {
            // Key, POST'ta tekrar doğrulanmak üzere forma hidden alan olarak gömülür
            ViewBag.LoginKey = key;
            return View();
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginDto model, string key)
    {
        // Güvenlik: gizli anahtar sadece GET'te değil POST'ta da doğrulanır;
        // yoksa form sayfasını hiç görmeden direkt POST atarak şifre denenebilir
        var secretKey = _configuration.GetValue<string>("AdminSettings:LoginKey");
        if (key != secretKey)
        {
            return NotFound();
        }

        ViewBag.LoginKey = key;

        var validationResult = await _validator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors) { ModelState.AddModelError(error.PropertyName, error.ErrorMessage); }
            return View(model);
        }

        // lockoutOnFailure: true → 5 hatalı denemede hesap 30 dk kilitlenir (brute-force koruması)
        var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, true);

        if (result.Succeeded)
        {
            return RedirectToAction("Index", "AdminExperience");
        }

        if (result.IsLockedOut)
        {
            ModelState.AddModelError("", "Çok fazla hatalı deneme yapıldı. Hesap 30 dakika kilitlendi.");
            return View(model);
        }

        ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı reis!");
        return View(model);
    }

    // POST: üçüncü taraf bir sayfanın GET isteğiyle admini sessizce oturumdan düşürmesini engeller
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}