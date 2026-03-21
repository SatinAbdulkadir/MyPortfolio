using MyPortfolio.BusinessLayer.Dtos.TestimonialDtos;

namespace MyPortfolio.BusinessLayer.Abstract
{
    public interface ITestimonialService
    {
        Task<List<ResultTestimonialDto>> TGetTestimonialListAsync();
    }
}