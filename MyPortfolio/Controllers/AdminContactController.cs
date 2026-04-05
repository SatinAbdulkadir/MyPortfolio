using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.ContactDtos;

namespace MyPortfolio.WebUI.Controllers
{
    [Authorize]
    public class AdminContactController : Controller
    {
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateContactDto> _validator; // Mimari tertemiz!

        public AdminContactController(IContactService contactService, IMapper mapper, IValidator<UpdateContactDto> validator)
        {
            _contactService = contactService;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var value = await _contactService.TGetContactAsync();
            var updateDto = _mapper.Map<UpdateContactDto>(value);
            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> Index(UpdateContactDto updateContactDto)
        {
            var result = await _validator.ValidateAsync(updateContactDto);

            if (!result.IsValid)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                TempData["ValidationResult"] = "error"; // Sağ üstten SweetAlert fırlatır
                return View(updateContactDto);
            }

            await _contactService.TUpdateContactAsync(updateContactDto);
            TempData["ValidationResult"] = "success"; // Başarı mesajı
            return RedirectToAction("Index");
        }
    }
}