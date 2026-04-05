using MyPortfolio.BusinessLayer.Dtos.TestimonialDtos;

namespace MyPortfolio.BusinessLayer.Abstract
{
    public interface ITestimonialService
    {
        Task<List<ResultTestimonialDto>> TGetTestimonialListAsync();
        
        Task TCreateTestimonialAsync(CreateTestimonialDto createDto);
        Task TUpdateTestimonialAsync(UpdateTestimonialDto updateDto);
        Task TDeleteTestimonialAsync(int id);
        Task<UpdateTestimonialDto> TGetByIdAsync(int id);
    }
}