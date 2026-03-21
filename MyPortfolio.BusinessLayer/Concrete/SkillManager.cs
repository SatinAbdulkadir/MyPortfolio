using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.SkillDtos;
using MyPortfolio.DataAccessLayer.Abstract;
using MyPortfolio.EntityLayer.Concrete;

namespace MyPortfolio.BusinessLayer.Concrete
{
    public class SkillManager : ISkillService
    {
        private readonly IGenericDal<Skill> _skillDal;

        public SkillManager(IGenericDal<Skill> skillDal)
        {
            _skillDal = skillDal;
        }

        public async Task<List<ResultSkillDto>> TGetSkillListAsync()
        {
            var values = await _skillDal.GetListAsync();
            return values.Select(x => new ResultSkillDto
            {
                SkillId = x.Id,
                Title = x.Title,
                Value = x.Value
            }).ToList();
        }
    }
}   