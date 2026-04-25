using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;

public class _ExperienceComponentPartial : ViewComponent
{
    private readonly IExperienceService _experienceService;

    public _ExperienceComponentPartial(IExperienceService experienceService)
    {
        _experienceService = experienceService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        
        var values = await _experienceService.TGetExperienceListAsync();

       
        return View(values);
    }
}