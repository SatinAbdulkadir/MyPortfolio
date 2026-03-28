using AutoMapper;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.ExperienceDtos;
using MyPortfolio.DataAccessLayer.Abstract;
using MyPortfolio.EntityLayer.Concrete;

namespace MyPortfolio.BusinessLayer.Concrete
{
    public class ExperienceManager : IExperienceService
    {
        private readonly IGenericDal<Experience> _experienceDal;
        private readonly IMapper _mapper; // Mapper enjekte edildi

        public ExperienceManager(IGenericDal<Experience> experienceDal, IMapper mapper)
        {
            _experienceDal = experienceDal;
            _mapper = mapper;
        }

        public async Task<List<ResultExperienceDto>> TGetExperienceListAsync()
        {
            // Veritabanından ham veriyi alıyoruz
            var values = await _experienceDal.GetListAsync();

            // AutoMapper ile List<Entity> -> List<DTO> dönüşümünü tek satırda yapıyoruz
            return _mapper.Map<List<ResultExperienceDto>>(values);
        }



        public async Task TCreateExperienceAsync(CreateExperienceDto createExperienceDto)
        {
            var value = _mapper.Map<Experience>(createExperienceDto);
            await _experienceDal.InsertAsync(value);
        }

        public async Task TDeleteExperienceAsync(int id)
        {
            // Önce silinecek veriyi ID ile buluyoruz
            var value = await _experienceDal.GetByIdAsync(id);

            // Eğer veri varsa silme işlemini yapıyoruz
            if (value != null)
            {
                await _experienceDal.DeleteAsync(value);
            }
        }

        public async Task<UpdateExperienceDto> TGetByIdExperienceAsync(int id)
        {
            var value = await _experienceDal.GetByIdAsync(id);
            return _mapper.Map<UpdateExperienceDto>(value);
        }

        public async Task TUpdateExperienceAsync(UpdateExperienceDto updateExperienceDto)
        {
            var existingData = await _experienceDal.GetByIdAsync(updateExperienceDto.Id);
            if (existingData != null)
            {
                _mapper.Map(updateExperienceDto, existingData);
                await _experienceDal.UpdateAsync(existingData);
            }
        }
    }
}