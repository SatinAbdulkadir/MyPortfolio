using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolio.BusinessLayer.Dtos.ExperienceDtos
{
    public class UpdateExperienceDto
    {
        public required int Id { get; set; }
        public required string Head { get; set; }
        public required string Title { get; set; }
        public required string DatePeriod { get; set; }
        public required string Description { get; set; }
    }
}
