using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.AboutDtos;

namespace MyPortfolio.WebUI.Controllers
{
    public class AdminAboutController : Controller
    {
        private readonly IAboutService _aboutService;

        public AdminAboutController(IAboutService aboutService)
        {
            _aboutService = aboutService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Veritabanındaki tek olan 'About' verisini çekiyoruz
            var values = await _aboutService.TGetAboutAsync();
            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> Index(UpdateAboutDto updateAboutDto)
        {
            // Gelen güncellemeyi servise gönderiyoruz
            await _aboutService.TUpdateAboutAsync(updateAboutDto);
            return RedirectToAction("Index");
        }
    }
}