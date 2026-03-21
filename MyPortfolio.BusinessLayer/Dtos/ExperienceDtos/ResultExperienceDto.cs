using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolio.BusinessLayer.Dtos.ExperienceDtos
{
    public class ResultExperienceDto
    {
        public required int ExperienceId { get; set; }
        public required string Head { get; set; }
        public required string Title { get; set; }//burda daha çok yeteneği ünvanı tarzında
        public required string DatePeriod { get; set; }
        public required string Description { get; set; }
    }
}
