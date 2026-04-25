using MyPortfolio.BusinessLayer.Dtos.FeatureDtos;

namespace MyPortfolio.BusinessLayer.Abstract
{
    public interface IFeatureService
    {
       
        Task<ResultFeatureDto> GetFeatureForBannerAsync();
        Task TUpdateFeatureAsync(UpdateFeatureDto updateFeatureDto);
    }
}