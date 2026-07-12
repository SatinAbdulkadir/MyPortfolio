using AutoMapper;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.PortfolioDetailDtos;
using MyPortfolio.DataAccessLayer.Abstract;
using MyPortfolio.EntityLayer.Concrete;

namespace MyPortfolio.BusinessLayer.Concrete
{
    public class PortfolioDetailManager : IPortfolioDetailService
    {
        private readonly IGenericDal<PortfolioDetail> _portfolioDetailDal;
        private readonly IMapper _mapper;
        private readonly IPortfolioImageService _portfolioImageService;
        private readonly Helpers.FileImageHelper _fileImageHelper;

        public PortfolioDetailManager(IGenericDal<PortfolioDetail> portfolioDetailDal,
                                      IMapper mapper,
                                      IPortfolioImageService portfolioImageService,
                                      Helpers.FileImageHelper fileImageHelper)
        {
            _portfolioDetailDal = portfolioDetailDal;
            _mapper = mapper;
            _portfolioImageService = portfolioImageService;
            _fileImageHelper = fileImageHelper;
        }

        public async Task<List<ResultPortfolioDetailDto>> TGetPortfolioDetailListAsync()
        {
            var values = await _portfolioDetailDal.GetListAsync();
            return _mapper.Map<List<ResultPortfolioDetailDto>>(values);
        }

        public async Task<ResultPortfolioDetailDto?> TGetByPortfolioIdAsync(int portfolioId)
        {
            var values = await _portfolioDetailDal.GetByFilterAsync(x => x.PortfolioId == portfolioId);
            var value = values.FirstOrDefault();
            return value == null ? null : _mapper.Map<ResultPortfolioDetailDto>(value);
        }

        public async Task<UpdatePortfolioDetailDto?> TGetByIdAsync(int id)
        {
            var value = await _portfolioDetailDal.GetByIdAsync(id);
            return value == null ? null : _mapper.Map<UpdatePortfolioDetailDto>(value);
        }

        // Yeni kaydın Id'sini döner ki galeri resimleri bu detaya bağlanabilsin
        public async Task<int> TCreatePortfolioDetailAsync(CreatePortfolioDetailDto createDto)
        {
            var value = _mapper.Map<PortfolioDetail>(createDto);
            await _portfolioDetailDal.InsertAsync(value);
            return value.Id;
        }

        public async Task TUpdatePortfolioDetailAsync(UpdatePortfolioDetailDto updateDto)
        {
            var existing = await _portfolioDetailDal.GetByIdAsync(updateDto.Id);
            if (existing != null)
            {
                _mapper.Map(updateDto, existing);
                await _portfolioDetailDal.UpdateAsync(existing);
            }
        }

        // Kaskad silme: önce galeri (kayıtlar + dosyalar), sonra yerel video dosyası, en son detay kaydı.
        // Bu mantık serviste yaşar ki hangi controller çağırırsa çağırsın bütünlük korunur.
        public async Task TDeletePortfolioDetailAsync(int id)
        {
            var value = await _portfolioDetailDal.GetByIdAsync(id);
            if (value == null) return;

            await _portfolioImageService.TDeleteByDetailIdAsync(id);

            if (value.VideoUrl != null && value.VideoUrl.StartsWith("/"))
            {
                _fileImageHelper.DeleteFile(value.VideoUrl);
            }

            await _portfolioDetailDal.DeleteAsync(value);
        }

        // Portfolio silinirken çağrılır: projeye ait detay varsa onu da kaskad temizler
        public async Task TDeleteByPortfolioIdAsync(int portfolioId)
        {
            var values = await _portfolioDetailDal.GetByFilterAsync(x => x.PortfolioId == portfolioId);
            foreach (var detail in values)
            {
                await TDeletePortfolioDetailAsync(detail.Id);
            }
        }
    }
}
