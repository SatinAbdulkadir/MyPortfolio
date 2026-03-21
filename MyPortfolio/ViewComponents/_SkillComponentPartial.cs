using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;

namespace MyPortfolio.WebUI.ViewComponents
{
    public class _SkillComponentPartial : ViewComponent
    {
        private readonly ISkillService _skillService;

        public _SkillComponentPartial(ISkillService skillService)
        {
            _skillService = skillService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _skillService.TGetSkillListAsync();
            return View(values);
        }
    }
}