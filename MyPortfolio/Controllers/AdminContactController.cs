using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.ContactDtos;

namespace MyPortfolio.WebUI.Controllers
{
    public class AdminContactController : Controller
    {
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;

        public AdminContactController(IContactService contactService, IMapper mapper)
        {
            _contactService = contactService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // DB'den tekil veriyi çektik
            var value = await _contactService.TGetContactAsync();

            // View'a UpdateContactDto modeli lazım olduğu için dönüştürüyoruz
            var updateDto = _mapper.Map<UpdateContactDto>(value);

            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> Index(UpdateContactDto updateContactDto)
        {
            await _contactService.TUpdateContactAsync(updateContactDto);
            // Kaydettikten sonra aynı sayfaya taze veriyle geri döner
            return RedirectToAction("Index");
        }
    }
}