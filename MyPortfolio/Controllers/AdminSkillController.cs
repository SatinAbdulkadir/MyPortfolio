using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.SkillDtos;

namespace MyPortfolio.WebUI.Controllers
{
    public class AdminSkillController : Controller
    {
        private readonly ISkillService _skillService;

        public AdminSkillController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        // 1. Yetenek Listesi
        public async Task<IActionResult> Index()
        {
            var values = await _skillService.TGetSkillListAsync();
            return View(values);
        }

        // 2. Yeni Yetenek Ekleme (Sayfa)
        [HttpGet]
        public IActionResult CreateSkill()
        {
            return View();
        }

        // 2. Yeni Yetenek Ekleme (İşlem)
        [HttpPost]
        public async Task<IActionResult> CreateSkill(CreateSkillDto createSkillDto)
        {
            await _skillService.TCreateSkillAsync(createSkillDto);
            return RedirectToAction("Index");
        }

        // 3. Yetenek Silme
        public async Task<IActionResult> DeleteSkill(int id)
        {
            await _skillService.TDeleteSkillAsync(id);
            return RedirectToAction("Index");
        }

        // 4. Yetenek Güncelleme (Sayfa - Mevcut veriyi getirir)
        [HttpGet]
        public async Task<IActionResult> UpdateSkill(int id)
        {
            var value = await _skillService.TGetByIdAsync(id);
            return View(value);
        }

        // 4. Yetenek Güncelleme (İşlem)
        [HttpPost]
        public async Task<IActionResult> UpdateSkill(UpdateSkillDto updateSkillDto)
        {
            await _skillService.TUpdateSkillAsync(updateSkillDto);
            return RedirectToAction("Index");
        }
    }
}