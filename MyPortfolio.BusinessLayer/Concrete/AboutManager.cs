using AutoMapper; // Kütüphaneyi ekle
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.AboutDtos;
using MyPortfolio.DataAccessLayer.Abstract;
using MyPortfolio.EntityLayer.Concrete;

namespace MyPortfolio.BusinessLayer.Concrete
{
    public class AboutManager : IAboutService
    {
        private readonly IGenericDal<About> _aboutDal;
        private readonly IMapper _mapper; // Mapper nesnemiz

        // Constructor Injection ile IMapper'ı içeri alıyoruz
        public AboutManager(IGenericDal<About> aboutDal, IMapper mapper)
        {
            _aboutDal = aboutDal;
            _mapper = mapper;
        }

        public async Task<ResultAboutDto> TGetAboutAsync()
        {
            var values = await _aboutDal.GetListAsync();
            var data = values.FirstOrDefault();

            // Manuel atama bitti, tek satırda profesyonel dönüşüm:
            return _mapper.Map<ResultAboutDto>(data);
        }

        public async Task TUpdateAboutAsync(UpdateAboutDto updateAboutDto)
        {
            var existingData = await _aboutDal.GetByIdAsync(updateAboutDto.AboutId);

            if (existingData != null)
            {
                // DTO'daki verileri mevcut Entity'ye yansıt:
                _mapper.Map(updateAboutDto, existingData);
                await _aboutDal.UpdateAsync(existingData);
            }
        }

        public async Task TChangeAboutStatusAsync(int id)
        {
            var data = await _aboutDal.GetByIdAsync(id);
            if (data != null)
            {
                data.IsActive = !data.IsActive;
                await _aboutDal.UpdateAsync(data);
            }
        }
    }
}