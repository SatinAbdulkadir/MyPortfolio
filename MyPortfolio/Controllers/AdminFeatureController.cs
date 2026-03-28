using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.FeatureDtos;

public class AdminFeatureController : Controller
{
    private readonly IFeatureService _featureService;
    private readonly IMapper _mapper;

    public AdminFeatureController(IFeatureService featureService, IMapper mapper)
    {
        _featureService = featureService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var value = await _featureService.GetFeatureForBannerAsync();
        // ResultDto'yu UpdateDto'ya çevirip View'a (Forma) basıyoruz
        return View(_mapper.Map<UpdateFeatureDto>(value));
    }

    [HttpPost]
    public async Task<IActionResult> Index(UpdateFeatureDto dto)
    {
        await _featureService.TUpdateFeatureAsync(dto);
        return RedirectToAction("Index");
    }
}