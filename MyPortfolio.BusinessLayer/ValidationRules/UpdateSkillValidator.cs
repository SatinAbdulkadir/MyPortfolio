using FluentValidation;
using MyPortfolio.BusinessLayer.Dtos.SkillDtos;

namespace MyPortfolio.BusinessLayer.ValidationRules
{
    public class UpdateSkillValidator : AbstractValidator<UpdateSkillDto>
    {
        public UpdateSkillValidator()
        {
           
        }
    }
}