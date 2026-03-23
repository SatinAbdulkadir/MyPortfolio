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
    }
}