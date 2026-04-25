using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;

namespace MyPortfolio.WebUI.ViewComponents
{
    public class _FeatureComponentPartial: ViewComponent
    {
        private readonly IFeatureService _featureService;

       
        public _FeatureComponentPartial(IFeatureService featureService)
        {
            _featureService = featureService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            var value = await _featureService.GetFeatureForBannerAsync();

            
            return View(value);
        }
    }
}
