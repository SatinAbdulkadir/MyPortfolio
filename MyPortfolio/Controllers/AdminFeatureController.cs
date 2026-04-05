using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.FeatureDtos;

[Authorize]
public class AdminFeatureController : Controller
{
    private readonly IFeatureService _featureService;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdateFeatureDto> _validator; // Mimariyi bozmak yok!

    public AdminFeatureController(IFeatureService featureService, IMapper mapper, IValidator<UpdateFeatureDto> validator)
    {
        _featureService = featureService;
        _mapper = mapper;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var value = await _featureService.GetFeatureForBannerAsync();
        return View(_mapper.Map<UpdateFeatureDto>(value));
    }

    [HttpPost]
    public async Task<IActionResult> Index(UpdateFeatureDto dto)
    {
        var result = await _validator.ValidateAsync(dto);

        if (!result.IsValid)
        {
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
            }
            TempData["ValidationResult"] = "error"; // Sağ üstten fırlayacak
            return View(dto);
        }

        await _featureService.TUpdateFeatureAsync(dto);
        TempData["ValidationResult"] = "success"; // Başarı bildirimi
        return RedirectToAction("Index");
    }
}