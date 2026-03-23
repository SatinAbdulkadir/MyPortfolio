using AutoMapper;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.SkillDtos;
using MyPortfolio.DataAccessLayer.Abstract;
using MyPortfolio.EntityLayer.Concrete;

namespace MyPortfolio.BusinessLayer.Concrete
{
    public class SkillManager : ISkillService
    {
        private readonly IGenericDal<Skill> _skillDal;
        private readonly IMapper _mapper; // Mapper enjekte edildi

        public SkillManager(IGenericDal<Skill> skillDal, IMapper mapper)
        {
            _skillDal = skillDal;
            _mapper = mapper;
        }

        public async Task<List<ResultSkillDto>> TGetSkillListAsync()
        {
            // Veritabanından ham listeyi çekiyoruz
            var values = await _skillDal.GetListAsync();

            // ESKİ: Select ile tek tek atama ameleliği
            // YENİ: Tek satırda profesyonel liste dönüşümü
            return _mapper.Map<List<ResultSkillDto>>(values);
        }
    }
}