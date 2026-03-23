using AutoMapper;
using MyPortfolio.BusinessLayer.Dtos.AboutDtos;
using MyPortfolio.BusinessLayer.Dtos.ContactDtos;
using MyPortfolio.BusinessLayer.Dtos.ExperienceDtos;
using MyPortfolio.BusinessLayer.Dtos.FeatureDtos;
using MyPortfolio.BusinessLayer.Dtos.PortfolioDtos;
using MyPortfolio.BusinessLayer.Dtos.SkillDtos;
using MyPortfolio.BusinessLayer.Dtos.SocialMediaDtos;
using MyPortfolio.BusinessLayer.Dtos.TestimonialDtos;
using MyPortfolio.EntityLayer.Concrete;

namespace MyPortfolio.BusinessLayer.Mappings
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping() // Sadece erişim belirleyici ve sınıf ismi!
        {
            // ReverseMap() sayesinde çift yönlü yol yapıyoruz.
            CreateMap<About, ResultAboutDto>().ReverseMap();
            CreateMap<About, UpdateAboutDto>().ReverseMap();

           
            CreateMap<Contact, ResultContactDto>().ReverseMap();

            CreateMap<Experience, ResultExperienceDto>().ReverseMap();
           
            CreateMap<Feature, ResultFeatureDto>().ReverseMap();

            CreateMap<Portfolio, ResultPortfolioDto>().ReverseMap();

            CreateMap<Skill, ResultSkillDto>().ReverseMap();

            CreateMap<SocialMedia, ResultSocialMediaDto>().ReverseMap();

            CreateMap<Testimonial, ResultTestimonialDto>().ReverseMap();
            // İleride Experience, Skill vb. buraya eklenecek.
        }
    }
}