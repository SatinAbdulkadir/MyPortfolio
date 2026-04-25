using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.AboutDtos;
using MyPortfolio.BusinessLayer.ValidationRules;

namespace MyPortfolio.WebUI.Controllers
{
    [Authorize]
    public class AdminAboutController : Controller
    {
        private readonly IAboutService _aboutService;
        private readonly IMapper _mapper; 
        private readonly IValidator<UpdateAboutDto> _validator;

        public AdminAboutController(IAboutService aboutService, IMapper mapper,IValidator<UpdateAboutDto> validator)
        {
            _aboutService = aboutService;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            
            var values = await _aboutService.TGetAboutAsync();

           
            var model = _mapper.Map<UpdateAboutDto>(values);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(UpdateAboutDto updateAboutDto)
        {
           

            var result = await _validator.ValidateAsync(updateAboutDto);

            if (!result.IsValid)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                

                TempData["ValidationResult"] = "error";
                return View(updateAboutDto);
            }

            await _aboutService.TUpdateAboutAsync(updateAboutDto);

            
            TempData["ValidationResult"] = "success";
            return RedirectToAction("Index");
        }
    }
}