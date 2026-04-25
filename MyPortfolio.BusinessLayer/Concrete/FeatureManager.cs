using AutoMapper;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.FeatureDtos;
using MyPortfolio.DataAccessLayer.Abstract;
using MyPortfolio.EntityLayer.Concrete;

namespace MyPortfolio.BusinessLayer.Concrete
{
    public class FeatureManager : IFeatureService
    {
        private readonly IGenericDal<Feature> _featureDal;
        private readonly IMapper _mapper; 

        public FeatureManager(IGenericDal<Feature> featureDal, IMapper mapper)
        {
            _featureDal = featureDal;
            _mapper = mapper;
        }

        public async Task<ResultFeatureDto> GetFeatureForBannerAsync()
        {
            var values = await _featureDal.GetListAsync();
            var data = values.FirstOrDefault();

            
           
            return _mapper.Map<ResultFeatureDto>(data);
        }

        public async Task TUpdateFeatureAsync(UpdateFeatureDto updateFeatureDto)
        {
            
            var values = await _featureDal.GetListAsync();
            var existingData = values.FirstOrDefault();

            if (existingData != null)
            {
                
                _mapper.Map(updateFeatureDto, existingData);
                await _featureDal.UpdateAsync(existingData);
            }
            else
            {
                
                var newFeature = _mapper.Map<Feature>(updateFeatureDto);
                await _featureDal.InsertAsync(newFeature);
            }
        }


    }
}