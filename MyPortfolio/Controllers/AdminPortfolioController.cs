using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.PortfolioDtos;
using MyPortfolio.BusinessLayer.Helpers;
 // FileImageHelper'ı kullanabilmek için ekle

[Authorize]
public class AdminPortfolioController : Controller
{
    private readonly IPortfolioService _portfolioService;
    private readonly IValidator<CreatePortfolioDto> _createValidator;
    private readonly IValidator<UpdatePortfolioDto> _updateValidator;
    private readonly FileImageHelper _fileImageHelper; 

    public AdminPortfolioController(IPortfolioService portfolioService,
                                    IValidator<CreatePortfolioDto> createValidator,
                                    IValidator<UpdatePortfolioDto> updateValidator,
                                    FileImageHelper fileImageHelper)
    {
        _portfolioService = portfolioService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _fileImageHelper = fileImageHelper;
    }

    public async Task<IActionResult> Index()
    {
        var values = await _portfolioService.TGetPortfolioListAsync();
        return View(values);
    }

    [HttpGet] public IActionResult CreatePortfolio() => View();

    [HttpPost]
    public async Task<IActionResult> CreatePortfolio(CreatePortfolioDto dto)
    {
        var result = await _createValidator.ValidateAsync(dto);
        if (!result.IsValid)
        {
            foreach (var item in result.Errors) { ModelState.AddModelError(item.PropertyName, item.ErrorMessage); }
            TempData["ValidationResult"] = "error";
            return View(dto);
        }

        
        if (dto.ImageFile != null)
        {
           
            dto.ImageUrl = await _fileImageHelper.UploadImageAsync(dto.ImageFile);
        }

        await _portfolioService.TCreatePortfolioAsync(dto);
        TempData["ValidationResult"] = "success";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> UpdatePortfolio(int id)
    {
        var value = await _portfolioService.TGetByIdAsync(id);
       
        return View(value);
    }

    [HttpPost]
    public async Task<IActionResult> UpdatePortfolio(UpdatePortfolioDto dto)
    {
        var result = await _updateValidator.ValidateAsync(dto);
        if (!result.IsValid)
        {
            foreach (var item in result.Errors) { ModelState.AddModelError(item.PropertyName, item.ErrorMessage); }
            TempData["ValidationResult"] = "error";
            return View(dto);
        }

  
        if (dto.ImageFile != null)
        {
           
            dto.ImageUrl = await _fileImageHelper.UploadImageAsync(dto.ImageFile);
        }
       

        await _portfolioService.TUpdatePortfolioAsync(dto);
        TempData["ValidationResult"] = "success";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DeletePortfolio(int id)
    {
        await _portfolioService.TDeletePortfolioAsync(id);
        TempData["ValidationResult"] = "success";
        return RedirectToAction("Index");
    }
}