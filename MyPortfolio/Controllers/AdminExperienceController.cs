using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.ExperienceDtos;

[Authorize]
public class AdminExperienceController : Controller
{
    private readonly IExperienceService _experienceService;
    private readonly IValidator<CreateExperienceDto> _createValidator;
    private readonly IValidator<UpdateExperienceDto> _updateValidator;

    public AdminExperienceController(IExperienceService experienceService,
                                     IValidator<CreateExperienceDto> createValidator,
                                     IValidator<UpdateExperienceDto> updateValidator)
    {
        _experienceService = experienceService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<IActionResult> Index()
        => View(await _experienceService.TGetExperienceListAsync());

    [HttpGet] public IActionResult CreateExperience() => View();

    [HttpPost]
    public async Task<IActionResult> CreateExperience(CreateExperienceDto dto)
    {
        var result = await _createValidator.ValidateAsync(dto);
        if (!result.IsValid)
        {
            foreach (var item in result.Errors) { ModelState.AddModelError(item.PropertyName, item.ErrorMessage); }
            TempData["ValidationResult"] = "error";
            return View(dto);
        }
        await _experienceService.TCreateExperienceAsync(dto);
        TempData["ValidationResult"] = "success";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> UpdateExperience(int id)
        => View(await _experienceService.TGetByIdExperienceAsync(id));

    [HttpPost]
    public async Task<IActionResult> UpdateExperience(UpdateExperienceDto dto)
    {
        var result = await _updateValidator.ValidateAsync(dto);
        if (!result.IsValid)
        {
            foreach (var item in result.Errors) { ModelState.AddModelError(item.PropertyName, item.ErrorMessage); }
            TempData["ValidationResult"] = "error";
            return View(dto);
        }
        await _experienceService.TUpdateExperienceAsync(dto);
        TempData["ValidationResult"] = "success";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DeleteExperience(int id)
    {
        await _experienceService.TDeleteExperienceAsync(id);
        TempData["ValidationResult"] = "success";
        return RedirectToAction("Index");
    }
}