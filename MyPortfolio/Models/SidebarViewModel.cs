using MyPortfolio.BusinessLayer.Dtos.FeatureDtos;
using MyPortfolio.BusinessLayer.Dtos.SocialMediaDtos;
using MyPortfolio.BusinessLayer.Dtos.TestimonialDtos;

namespace MyPortfolio.WebUI.Models
{
    public class SidebarViewModel
    {
        public ResultFeatureDto Feature { get; set; }
        public List<ResultSocialMediaDto> SocialMedias { get; set; }
        public List<ResultTestimonialDto> Testimonials { get; set; }
    }
}