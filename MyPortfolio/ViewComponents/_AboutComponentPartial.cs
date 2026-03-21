using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyPortfolio.BusinessLayer.Abstract;

namespace MyPortfolio.WebUI.ViewComponents
{
    public class _AboutComponentPartial : ViewComponent
    {
        private readonly IAboutService _aboutService;

        public _AboutComponentPartial(IAboutService aboutService)
        {
            _aboutService = aboutService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var value = await _aboutService.TGetAboutAsync();
            return View(value);
        }
    }
}

