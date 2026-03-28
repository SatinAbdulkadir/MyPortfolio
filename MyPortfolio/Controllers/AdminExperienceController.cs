using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.ExperienceDtos;

public class AdminExperienceController : Controller
{
    private readonly IExperienceService _experienceService;
    public AdminExperienceController(IExperienceService experienceService)
        => _experienceService = experienceService;

    public async Task<IActionResult> Index()
        => View(await _experienceService.TGetExperienceListAsync());

    [HttpGet] public IActionResult CreateExperience() => View();

    [HttpPost]
    public async Task<IActionResult> CreateExperience(CreateExperienceDto dto)
    {
        await _experienceService.TCreateExperienceAsync(dto);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DeleteExperience(int id)
    {
        await _experienceService.TDeleteExperienceAsync(id);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> UpdateExperience(int id)
        => View(await _experienceService.TGetByIdExperienceAsync(id));

    [HttpPost]
    public async Task<IActionResult> UpdateExperience(UpdateExperienceDto dto)
    {
        await _experienceService.TUpdateExperienceAsync(dto);
        return RedirectToAction("Index");
    }
}