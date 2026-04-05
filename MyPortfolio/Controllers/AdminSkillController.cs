using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.SkillDtos;

namespace MyPortfolio.WebUI.Controllers
{
    [Authorize]
    public class AdminSkillController : Controller
    {
        private readonly ISkillService _skillService;
        private readonly IValidator<CreateSkillDto> _createValidator;
        private readonly IValidator<UpdateSkillDto> _updateValidator;

        public AdminSkillController(ISkillService skillService,
                                    IValidator<CreateSkillDto> createValidator,
                                    IValidator<UpdateSkillDto> updateValidator)
        {
            _skillService = skillService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _skillService.TGetSkillListAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateSkill() => View();

        [HttpPost]
        public async Task<IActionResult> CreateSkill(CreateSkillDto createSkillDto)
        {
            var result = await _createValidator.ValidateAsync(createSkillDto);
            if (!result.IsValid)
            {
                foreach (var item in result.Errors) { ModelState.AddModelError(item.PropertyName, item.ErrorMessage); }
                TempData["ValidationResult"] = "error";
                return View(createSkillDto);
            }
            await _skillService.TCreateSkillAsync(createSkillDto);
            TempData["ValidationResult"] = "success";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteSkill(int id)
        {
            await _skillService.TDeleteSkillAsync(id);
            TempData["ValidationResult"] = "success";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateSkill(int id)
        {
            var value = await _skillService.TGetByIdAsync(id);
            // NOT: Servis muhtemelen ResultDto dönüyor, UpdateSkill sayfasında hata alırsan 
            // burada bir AutoMapper dönüşümü (ResultDto -> UpdateSkillDto) yapman gerekebilir.
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSkill(UpdateSkillDto updateSkillDto)
        {
            var result = await _updateValidator.ValidateAsync(updateSkillDto);
            if (!result.IsValid)
            {
                foreach (var item in result.Errors) { ModelState.AddModelError(item.PropertyName, item.ErrorMessage); }
                TempData["ValidationResult"] = "error";
                return View(updateSkillDto);
            }
            await _skillService.TUpdateSkillAsync(updateSkillDto);
            TempData["ValidationResult"] = "success";
            return RedirectToAction("Index");
        }
    }
}