using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;

namespace MyPortfolio.WebUI.ViewComponents
{
    public class _TestimonialComponentPartial : ViewComponent
    {
        private readonly ITestimonialService _testimonialService;

        public _TestimonialComponentPartial(ITestimonialService testimonialService)
        {
            _testimonialService = testimonialService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _testimonialService.TGetTestimonialListAsync();
            return View(values);
        }
    }
}