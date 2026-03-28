using AutoMapper;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.ExperienceDtos;
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

        public async Task TCreateSkillAsync(CreateSkillDto createSkillDto)
        {
            var value = _mapper.Map<Skill>(createSkillDto);
            await _skillDal.InsertAsync(value);
        }

        public async Task TDeleteSkillAsync(int id)
        {
            var value = await _skillDal.GetByIdAsync(id);
            if (value != null) await _skillDal.DeleteAsync(value);
        }

        public async Task<UpdateSkillDto> TGetByIdAsync(int id)
        {
            // Veritabanından veriyi alıyoruz
            var value = await _skillDal.GetByIdAsync(id);

            // Update formuna basabilmek için UpdateDto'ya çevirip gönderiyoruz
            return _mapper.Map<UpdateSkillDto>(value);
        }

        public async Task TUpdateSkillAsync(UpdateSkillDto updateSkillDto)
        {
            // Önce veritabanındaki mevcut kaydı buluyoruz
            var existingData = await _skillDal.GetByIdAsync(updateSkillDto.Id);

            if (existingData != null)
            {
                // AutoMapper sihrini konuşturup gelen verileri mevcut verinin üzerine yazıyoruz
                _mapper.Map(updateSkillDto, existingData);

                // Veritabanını güncelliyoruz
                await _skillDal.UpdateAsync(existingData);
            }
        }

    }
}