using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolio.BusinessLayer.Dtos.AboutDtos
{
    public class ResultAboutDto
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string SubDescription { get; set; }
        public required string Details { get; set; }
    }
}
