using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;

namespace MyPortfolio.WebUI.ViewComponents
{
    public class _FeatureComponentPartial: ViewComponent
    {
        private readonly IFeatureService _featureService;

        // Constructor üzerinden servisimizi istiyoruz. 'new'lemek YASAK!
        public _FeatureComponentPartial(IFeatureService featureService)
        {
            _featureService = featureService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Business katmanına gidip DTO'muzu alıyoruz
            var value = await _featureService.GetFeatureForBannerAsync();

            // Veriyi View'e gönderiyoruz
            return View(value);
        }
    }
}
