using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.TestimonialDtos;

[Authorize]
public class AdminTestimonialController : Controller
{
    private readonly ITestimonialService _testimonialService;
    private readonly IValidator<CreateTestimonialDto> _createValidator;
    private readonly IValidator<UpdateTestimonialDto> _updateValidator;

    public AdminTestimonialController(ITestimonialService testimonialService,
                                      IValidator<CreateTestimonialDto> createValidator,
                                      IValidator<UpdateTestimonialDto> updateValidator)
    {
        _testimonialService = testimonialService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<IActionResult> Index()
    {
        var values = await _testimonialService.TGetTestimonialListAsync();
        return View(values);
    }

    [HttpGet]
    public IActionResult CreateTestimonial() => View();

    [HttpPost]
    public async Task<IActionResult> CreateTestimonial(CreateTestimonialDto dto)
    {
        var result = await _createValidator.ValidateAsync(dto);
        if (!result.IsValid)
        {
            foreach (var item in result.Errors) { ModelState.AddModelError(item.PropertyName, item.ErrorMessage); }
            TempData["ValidationResult"] = "error";
            return View(dto);
        }
        await _testimonialService.TCreateTestimonialAsync(dto);
        TempData["ValidationResult"] = "success";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DeleteTestimonial(int id)
    {
        await _testimonialService.TDeleteTestimonialAsync(id);
        TempData["ValidationResult"] = "success";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> UpdateTestimonial(int id)
    {
        var value = await _testimonialService.TGetByIdAsync(id);
        
        return View(value);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateTestimonial(UpdateTestimonialDto dto)
    {
        var result = await _updateValidator.ValidateAsync(dto);
        if (!result.IsValid)
        {
            foreach (var item in result.Errors) { ModelState.AddModelError(item.PropertyName, item.ErrorMessage); }
            TempData["ValidationResult"] = "error";
            return View(dto);
        }
        await _testimonialService.TUpdateTestimonialAsync(dto);
        TempData["ValidationResult"] = "success";
        return RedirectToAction("Index");
    }
}