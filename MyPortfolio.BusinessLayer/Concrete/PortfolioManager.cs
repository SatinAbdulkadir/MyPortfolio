using AutoMapper;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.PortfolioDtos;
using MyPortfolio.DataAccessLayer.Abstract;
using MyPortfolio.EntityLayer.Concrete;

namespace MyPortfolio.BusinessLayer.Concrete
{
    public class PortfolioManager : IPortfolioService
    {
        private readonly IGenericDal<Portfolio> _portfolioDal;
        private readonly IMapper _mapper; // AutoMapper silahını kuşanıyoruz

        public PortfolioManager(IGenericDal<Portfolio> portfolioDal, IMapper mapper)
        {
            _portfolioDal = portfolioDal;
            _mapper = mapper;
        }

        public async Task<List<ResultPortfolioDto>> TGetPortfolioListAsync()
        {
            // Veritabanından ham listeyi çekiyoruz
            var values = await _portfolioDal.GetListAsync();

            // ESKİ: Select ile tek tek atama ameleliği
            // YENİ: Tek satırda kurumsal liste dönüşümü
            return _mapper.Map<List<ResultPortfolioDto>>(values);
        }
    }
}