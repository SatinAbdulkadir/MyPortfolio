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




        public async Task TCreateSocialMediaAsync(CreateSocialMediaDto createSocialMediaDto)
        {
            var value = _mapper.Map<SocialMedia>(createSocialMediaDto);
            await _socialMediaDal.InsertAsync(value);
        }

        public async Task TDeleteSocialMediaAsync(int id)
        {
            var value = await _socialMediaDal.GetByIdAsync(id);
            if (value != null) await _socialMediaDal.DeleteAsync(value);
        }

        public async Task<UpdateSocialMediaDto> TGetByIdAsync(int id)
        {
            var value = await _socialMediaDal.GetByIdAsync(id);
            return _mapper.Map<UpdateSocialMediaDto>(value);
        }

        public async Task TUpdateSocialMediaAsync(UpdateSocialMediaDto updateSocialMediaDto)
        {
            var existingData = await _socialMediaDal.GetByIdAsync(updateSocialMediaDto.Id);
            if (existingData != null)
            {
                _mapper.Map(updateSocialMediaDto, existingData);
                await _socialMediaDal.UpdateAsync(existingData);
            }
        }
    }
}