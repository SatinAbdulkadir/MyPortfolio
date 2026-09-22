using AutoMapper;
using MyPortfolio.BusinessLayer.Dtos.AboutDtos;
using MyPortfolio.BusinessLayer.Dtos.AppUserDtos;
using MyPortfolio.BusinessLayer.Dtos.CertificateCategoryDtos;
using MyPortfolio.BusinessLayer.Dtos.CertificateDtos;
using MyPortfolio.BusinessLayer.Dtos.ContactDtos;
using MyPortfolio.BusinessLayer.Dtos.ExperienceDtos;
using MyPortfolio.BusinessLayer.Dtos.FeatureDtos;
using MyPortfolio.BusinessLayer.Dtos.MessageDtos;
using MyPortfolio.BusinessLayer.Dtos.PortfolioDtos;
using MyPortfolio.BusinessLayer.Dtos.PortfolioDetailDtos;
using MyPortfolio.BusinessLayer.Dtos.PortfolioImageDtos;
using MyPortfolio.BusinessLayer.Dtos.SkillDtos;
using MyPortfolio.BusinessLayer.Dtos.SocialMediaDtos;
using MyPortfolio.BusinessLayer.Dtos.TestimonialDtos;
using MyPortfolio.EntityLayer.Concrete;

namespace MyPortfolio.BusinessLayer.Mappings
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping() 
        {
           
            CreateMap<About, ResultAboutDto>().ReverseMap();
            CreateMap<ResultAboutDto, UpdateAboutDto>().ReverseMap();
            CreateMap<About, UpdateAboutDto>().ReverseMap();


            // Sertifikalar: kategori adı/ikonu entity'de yok, manager ayrı sorgudan doldurur
            CreateMap<Certificate, ResultCertificateDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryIcon, opt => opt.Ignore());
            CreateMap<Certificate, CreateCertificateDto>().ReverseMap();
            CreateMap<Certificate, UpdateCertificateDto>().ReverseMap();

            // CertificateCount da aynı şekilde manager tarafından hesaplanır
            CreateMap<CertificateCategory, ResultCertificateCategoryDto>()
                .ForMember(dest => dest.CertificateCount, opt => opt.Ignore());
            CreateMap<CertificateCategory, CreateCertificateCategoryDto>().ReverseMap();
            CreateMap<CertificateCategory, UpdateCertificateCategoryDto>().ReverseMap();

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

            CreateMap<PortfolioDetail, ResultPortfolioDetailDto>().ReverseMap();
            CreateMap<PortfolioDetail, CreatePortfolioDetailDto>().ReverseMap();
            CreateMap<PortfolioDetail, UpdatePortfolioDetailDto>().ReverseMap();

            CreateMap<PortfolioImage, ResultPortfolioImageDto>().ReverseMap();
            CreateMap<PortfolioImage, CreatePortfolioImageDto>().ReverseMap();

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


            CreateMap<Message, CreateMessageDto>().ReverseMap();
            CreateMap<Message, ResultMessageDto>().ReverseMap();
            // İletişim Formu DTO eşleştirmeleri
            CreateMap<CreateMessageDto, MailRequestDto>().ReverseMap();



        }
    }
}