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
        private readonly IMapper _mapper; 

        public TestimonialManager(IGenericDal<Testimonial> testimonialDal, IMapper mapper)
        {
            _testimonialDal = testimonialDal;
            _mapper = mapper;
        }

        public async Task<List<ResultTestimonialDto>> TGetTestimonialListAsync()
        {
            
            var values = await _testimonialDal.GetListAsync();
            var activeValues = values.Where(x => x.IsActive).ToList();

           
            return _mapper.Map<List<ResultTestimonialDto>>(activeValues);
        }




        public async Task TCreateTestimonialAsync(CreateTestimonialDto createDto)
        {
            var value = _mapper.Map<Testimonial>(createDto);
            await _testimonialDal.InsertAsync(value);
        }

        public async Task TDeleteTestimonialAsync(int id)
        {
            var value = await _testimonialDal.GetByIdAsync(id);
            if (value != null) await _testimonialDal.DeleteAsync(value);
        }

        public async Task<UpdateTestimonialDto> TGetByIdAsync(int id)
        {
            var value = await _testimonialDal.GetByIdAsync(id);
            return _mapper.Map<UpdateTestimonialDto>(value);
        }

        public async Task TUpdateTestimonialAsync(UpdateTestimonialDto updateDto)
        {
            var existingData = await _testimonialDal.GetByIdAsync(updateDto.Id);
            if (existingData != null)
            {
                _mapper.Map(updateDto, existingData);
                await _testimonialDal.UpdateAsync(existingData);
            }
        }
    }
}