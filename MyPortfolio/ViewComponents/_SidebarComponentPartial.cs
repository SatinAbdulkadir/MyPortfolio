using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.EntityLayer.Concrete;
using MyPortfolio.WebUI.Models;

namespace MyPortfolio.WebUI.ViewComponents
{
    public class _SidebarComponentPartial : ViewComponent
    {
        private readonly IFeatureService _featureService;
        private readonly ISocialMediaService _socialMediaService;
        private readonly ITestimonialService _testimonialService;



        public _SidebarComponentPartial(IFeatureService featureService, ISocialMediaService socialMediaService,ITestimonialService testimonialService)
        {
            _featureService = featureService;
            _socialMediaService = socialMediaService;
            _testimonialService = testimonialService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var featureData = await _featureService.GetFeatureForBannerAsync();
            var socialMediaData = await _socialMediaService.TGetSocialMediaListAsync();
            var testimonials = await _testimonialService.TGetTestimonialListAsync();

            var model = new SidebarViewModel
            {
                Feature = featureData,
                SocialMedias = socialMediaData,
                Testimonials = testimonials 
            };

            return View(model);
        }
    }
}