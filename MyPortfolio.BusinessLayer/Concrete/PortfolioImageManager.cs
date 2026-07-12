using AutoMapper;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.PortfolioImageDtos;
using MyPortfolio.BusinessLayer.Helpers;
using MyPortfolio.DataAccessLayer.Abstract;
using MyPortfolio.EntityLayer.Concrete;

namespace MyPortfolio.BusinessLayer.Concrete
{
    public class PortfolioImageManager : IPortfolioImageService
    {
        private readonly IGenericDal<PortfolioImage> _portfolioImageDal;
        private readonly IMapper _mapper;
        private readonly FileImageHelper _fileImageHelper;

        public PortfolioImageManager(IGenericDal<PortfolioImage> portfolioImageDal, IMapper mapper, FileImageHelper fileImageHelper)
        {
            _portfolioImageDal = portfolioImageDal;
            _mapper = mapper;
            _fileImageHelper = fileImageHelper;
        }

        public async Task<List<ResultPortfolioImageDto>> TGetListByDetailIdAsync(int portfolioDetailId)
        {
            var values = await _portfolioImageDal.GetByFilterAsync(x => x.PortfolioDetailId == portfolioDetailId);
            return _mapper.Map<List<ResultPortfolioImageDto>>(values);
        }

        public async Task TCreatePortfolioImageAsync(CreatePortfolioImageDto createDto)
        {
            var value = _mapper.Map<PortfolioImage>(createDto);
            await _portfolioImageDal.InsertAsync(value);
        }

        // Galeri yüklemesi: N resim tek veritabanı turunda eklenir
        public async Task TCreatePortfolioImagesAsync(List<CreatePortfolioImageDto> createDtos)
        {
            if (createDtos.Count == 0) return;
            var values = _mapper.Map<List<PortfolioImage>>(createDtos);
            await _portfolioImageDal.InsertRangeAsync(values);
        }

        // Kayıtla birlikte fiziksel dosya da silinir; yoksa wwwroot'ta yetim dosya birikir
        public async Task TDeletePortfolioImageAsync(int id)
        {
            var value = await _portfolioImageDal.GetByIdAsync(id);
            if (value == null) return;

            _fileImageHelper.DeleteFile(value.ImageUrl);
            await _portfolioImageDal.DeleteAsync(value);
        }

        // Detay silinirken tüm galeri tek turda temizlenir (kayıtlar + dosyalar)
        public async Task TDeleteByDetailIdAsync(int portfolioDetailId)
        {
            var values = await _portfolioImageDal.GetByFilterAsync(x => x.PortfolioDetailId == portfolioDetailId);
            if (values.Count == 0) return;

            foreach (var image in values)
            {
                _fileImageHelper.DeleteFile(image.ImageUrl);
            }

            await _portfolioImageDal.DeleteRangeAsync(values);
        }

        public async Task<ResultPortfolioImageDto?> TGetByIdAsync(int id)
        {
            var value = await _portfolioImageDal.GetByIdAsync(id);
            return value == null ? null : _mapper.Map<ResultPortfolioImageDto>(value);
        }
    }
}
