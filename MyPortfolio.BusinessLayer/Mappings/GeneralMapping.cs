using AutoMapper;
using MyPortfolio.BusinessLayer.Dtos.AboutDtos;
using MyPortfolio.BusinessLayer.Dtos.AppUserDtos;
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
            CreateMap<ResultAboutDto, UpdateAboutDto>().ReverseMap();
            CreateMap<About, UpdateAboutDto>().ReverseMap();


            CreateMap<Contact, ResultContactDto>().ReverseMap();
            CreateMap<ResultContactDto, UpdateContactDto>().ReverseMap();
            CreateMap<Contact, UpdateContactDto>().ReverseMap();

            CreateMap<Experience, ResultExperienceDto>().ReverseMap();
            CreateMap<Experience, CreateExperienceDto>().ReverseMap();
            CreateMap<Experience, UpdateExperienceDto>().ReverseMap();


            CreateMap<Feature, ResultFeatureDto>().ReverseMap();
            CreateMap<Feature, UpdateFeatureDto>().ReverseMap();
            CreateMap<ResultFeatureDto, UpdateFeatureDto>().ReverseMap();
            CreateMap<UpdateFeatureDto, Feature>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());


            CreateMap<Portfolio, ResultPortfolioDto>().ReverseMap();
            CreateMap<Portfolio, CreatePortfolioDto>().ReverseMap();
            CreateMap<Portfolio, UpdatePortfolioDto>().ReverseMap();

            CreateMap<Skill, ResultSkillDto>().ReverseMap();
            CreateMap<Skill, CreateSkillDto>().ReverseMap();
            CreateMap<Skill, UpdateSkillDto>().ReverseMap();

            CreateMap<SocialMedia, ResultSocialMediaDto>().ReverseMap();
            CreateMap<SocialMedia, CreateSocialMediaDto>().ReverseMap();
            CreateMap<SocialMedia, UpdateSocialMediaDto>().ReverseMap();

            CreateMap<Testimonial, ResultTestimonialDto>().ReverseMap();
            CreateMap<Testimonial, CreateTestimonialDto>().ReverseMap();
            CreateMap<Testimonial, UpdateTestimonialDto>().ReverseMap();



            CreateMap<AppUser, EditProfileDto>().ReverseMap();


            // İleride Experience, Skill vb. buraya eklenecek.
        }
    }
}