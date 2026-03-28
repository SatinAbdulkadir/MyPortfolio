using MyPortfolio.BusinessLayer.Dtos.FeatureDtos;

namespace MyPortfolio.BusinessLayer.Abstract
{
    public interface IFeatureService
    {
        // UI katmanı sadece bu metodu çağıracak
        Task<ResultFeatureDto> GetFeatureForBannerAsync();

       

       
        // Parametreyi UpdateDto yaptık ki ID ile eşleşsin
        Task TUpdateFeatureAsync(UpdateFeatureDto updateFeatureDto);
    }
}