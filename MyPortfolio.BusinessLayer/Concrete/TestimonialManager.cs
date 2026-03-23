using AutoMapper;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.TestimonialDtos;
using MyPortfolio.DataAccessLayer.Abstract;
using MyPortfolio.EntityLayer.Concrete;

namespace MyPortfolio.BusinessLayer.Concrete
{
    public class TestimonialManager : ITestimonialService
    {
        private readonly IGenericDal<Testimonial> _testimonialDal;
        private readonly IMapper _mapper; // Mapper enjeksiyonu

        public TestimonialManager(IGenericDal<Testimonial> testimonialDal, IMapper mapper)
        {
            _testimonialDal = testimonialDal;
            _mapper = mapper;
        }

        public async Task<List<ResultTestimonialDto>> TGetTestimonialListAsync()
        {
            // Veritabanından sadece aktif olanları çekiyoruz
            var values = await _testimonialDal.GetListAsync();
            var activeValues = values.Where(x => x.IsActive).ToList();

            // Dönüşümü AutoMapper hallediyor
            return _mapper.Map<List<ResultTestimonialDto>>(activeValues);
        }
    }
}