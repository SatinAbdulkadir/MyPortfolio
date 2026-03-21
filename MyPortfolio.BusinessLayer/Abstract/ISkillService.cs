using MyPortfolio.BusinessLayer.Dtos.SkillDtos;

namespace MyPortfolio.BusinessLayer.Abstract
{
    public interface ISkillService
    {
        Task<List<ResultSkillDto>> TGetSkillListAsync();
    }
}