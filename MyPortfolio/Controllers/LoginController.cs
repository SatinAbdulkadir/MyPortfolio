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

    // EKSİK OLAN VE HATAYA SEBEP OLAN KISIM BURASIYDI REİS:
    [HttpGet]
    public IActionResult Index(string key)
    {
        var secretKey = _configuration.GetValue<string>("AdminSettings:LoginKey");

        if (key==secretKey)
        {
            return View();
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginDto model)
    {
        var validationResult = await _validator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors) { ModelState.AddModelError(error.PropertyName, error.ErrorMessage); }
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

        if (result.Succeeded)
        {
            return RedirectToAction("Index", "AdminExperience");
        }

        ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı reis!");
        return View(model);
    }

    // Çıkış yapma metodunu da güncel yapıya ekleyelim dursun:
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}