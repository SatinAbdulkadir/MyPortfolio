using MyPortfolio.EntityLayer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolio.EntityLayer.Concrete
{
    public class Testimonial:BaseEntity
    {
        public required string NameSurname { get; set; }
        public required string Title { get; set; }//Unvanı
        public required string Description { get; set; }//Yorumları
        public  string? ImageUrl { get; set; }
    }
}
