using MyPortfolio.BusinessLayer.Dtos.FeatureDtos;

namespace MyPortfolio.BusinessLayer.Abstract
{
    public interface IFeatureService
    {
        // UI katmanı sadece bu metodu çağıracak
        Task<ResultFeatureDto> GetFeatureForBannerAsync();

        // Gerekirse diğer Business kuralları buraya gelecek
        Task TUpdateFeatureAsync(ResultFeatureDto resultFeatureDto);
    }
}