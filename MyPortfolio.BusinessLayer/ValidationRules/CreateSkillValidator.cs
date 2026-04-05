using FluentValidation;
using MyPortfolio.BusinessLayer.Dtos.SkillDtos;

namespace MyPortfolio.BusinessLayer.ValidationRules
{
    public class CreateSkillValidator : AbstractValidator<CreateSkillDto>
    {
        public CreateSkillValidator()
        {
           
        }
    }
}