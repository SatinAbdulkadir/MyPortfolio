using MyPortfolio.BusinessLayer.Dtos.SkillDtos;

namespace MyPortfolio.BusinessLayer.Abstract
{
    public interface ISkillService
    {
        Task<List<ResultSkillDto>> TGetSkillListAsync();
      
        Task TCreateSkillAsync(CreateSkillDto createSkillDto);
        Task TUpdateSkillAsync(UpdateSkillDto updateSkillDto);
        Task TDeleteSkillAsync(int id);
        Task<UpdateSkillDto> TGetByIdAsync(int id);
    }
}