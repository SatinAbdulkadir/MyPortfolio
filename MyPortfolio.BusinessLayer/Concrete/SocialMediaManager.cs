using AutoMapper;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.SocialMediaDtos;
using MyPortfolio.DataAccessLayer.Abstract;
using MyPortfolio.EntityLayer.Concrete;

namespace MyPortfolio.BusinessLayer.Concrete
{
    public class SocialMediaManager : ISocialMediaService
    {
        private readonly IGenericDal<SocialMedia> _socialMediaDal;
        private readonly IMapper _mapper; // Mapper enjekte edildi

        public SocialMediaManager(IGenericDal<SocialMedia> socialMediaDal, IMapper mapper)
        {
            _socialMediaDal = socialMediaDal;
            _mapper = mapper;
        }

        public async Task<List<ResultSocialMediaDto>> TGetSocialMediaListAsync()
        {
            // Veritabanından ham listeyi çekiyoruz
            var values = await _socialMediaDal.GetListAsync();

            // ESKİ: Select ile manuel atama
            // YENİ: Tek satırda profesyonel liste dönüşümü
            return _mapper.Map<List<ResultSocialMediaDto>>(values);
        }
    }
}