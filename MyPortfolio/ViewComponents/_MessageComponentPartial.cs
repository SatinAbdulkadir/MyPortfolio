using Microsoft.AspNetCore.Mvc;

namespace MyPortfolio.WebUI.ViewComponents
{
    public class _MessageComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Veritabanından bir şey çekmeyeceğiz, sadece formu ekrana basacağız.
            return View();
        }
    }
}