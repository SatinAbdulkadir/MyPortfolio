using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;

namespace MyPortfolio.WebUI.ViewComponents
{
    public class _ContactComponentPartial: ViewComponent
    {
        private readonly IContactService _contactService;
        public _ContactComponentPartial(IContactService contactService) { _contactService = contactService; }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var value = await _contactService.TGetContactAsync();
            return View(value);
        }
    }
}
