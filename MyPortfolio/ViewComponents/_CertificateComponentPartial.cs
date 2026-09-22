using Microsoft.AspNetCore.Mvc;
using MyPortfolio.BusinessLayer.Abstract;

public class _CertificateComponentPartial : ViewComponent
{
    // Ana sayfada yalnızca "öne çıkar" işaretli sertifikalar görünür;
    // tamamı /Certificate sayfasında listelenir.
    private const int FeaturedTake = 6;

    private readonly ICertificateService _certificateService;

    public _CertificateComponentPartial(ICertificateService certificateService)
    {
        _certificateService = certificateService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var values = await _certificateService.TGetFeaturedCertificateListAsync(FeaturedTake);
        return View(values);
    }
}
