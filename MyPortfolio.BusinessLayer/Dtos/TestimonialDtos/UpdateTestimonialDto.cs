using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolio.BusinessLayer.Dtos.TestimonialDtos
{
    public class UpdateTestimonialDto
    {
        public required int Id { get; set; }
        public required string NameSurname { get; set; }
        public required string Title { get; set; }//Unvanı
        public required string Description { get; set; }//Yorumları
        public string? ImageUrl { get; set; }
    }
}
