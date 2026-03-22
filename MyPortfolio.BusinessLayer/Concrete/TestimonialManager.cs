using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.TestimonialDtos;
using MyPortfolio.DataAccessLayer.Abstract;
using MyPortfolio.EntityLayer.Concrete;

namespace MyPortfolio.BusinessLayer.Concrete
{
    public class TestimonialManager : ITestimonialService
    {
        private readonly IGenericDal<Testimonial> _testimonialDal;

        public TestimonialManager(IGenericDal<Testimonial> testimonialDal)
        {
            _testimonialDal = testimonialDal;
        }

        public async Task<List<ResultTestimonialDto>> TGetTestimonialListAsync()
        {
            var values = await _testimonialDal.GetListAsync();
            return values.Where(x => x.IsActive).Select(x => new ResultTestimonialDto
            {
                TestimonialId = x.Id,
                NameSurname = x.NameSurname,
                Title = x.Title,
                Description = x.Description,
                ImageUrl = x.ImageUrl
            }).ToList();
        }
    }
}