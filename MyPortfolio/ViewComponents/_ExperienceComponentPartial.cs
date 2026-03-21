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
        // Business katmanından List<ResultExperienceDto> geliyor
        var values = await _experienceService.TGetExperienceListAsync();

        // Listeyi View'e paketleyip fırlatıyoruz
        return View(values);
    }
}