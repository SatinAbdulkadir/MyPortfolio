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
        private readonly IMapper _mapper; 

        public PortfolioManager(IGenericDal<Portfolio> portfolioDal, IMapper mapper)
        {
            _portfolioDal = portfolioDal;
            _mapper = mapper;
        }

        public async Task<List<ResultPortfolioDto>> TGetPortfolioListAsync()
        {
           
            var values = await _portfolioDal.GetListAsync();

            
            return _mapper.Map<List<ResultPortfolioDto>>(values);
        }







        public async Task TCreatePortfolioAsync(CreatePortfolioDto createDto)
        {
            var value = _mapper.Map<Portfolio>(createDto);
            await _portfolioDal.InsertAsync(value);
        }

        public async Task TDeletePortfolioAsync(int id)
        {
            var value = await _portfolioDal.GetByIdAsync(id);
            if (value != null) await _portfolioDal.DeleteAsync(value);
        }

        public async Task<UpdatePortfolioDto> TGetByIdAsync(int id)
        {
            var value = await _portfolioDal.GetByIdAsync(id);
            return _mapper.Map<UpdatePortfolioDto>(value);
        }

        public async Task TUpdatePortfolioAsync(UpdatePortfolioDto updateDto)
        {
            var existing = await _portfolioDal.GetByIdAsync(updateDto.Id);
            if (existing != null)
            {
                _mapper.Map(updateDto, existing);
                await _portfolioDal.UpdateAsync(existing);
            }
        }
    }
}