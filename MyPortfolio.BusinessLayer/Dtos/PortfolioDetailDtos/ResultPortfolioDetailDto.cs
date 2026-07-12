using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolio.BusinessLayer.Dtos.PortfolioDetailDtos
{
    public class ResultPortfolioDetailDto
    {
        public required int Id { get; set; }
        public required int PortfolioId { get; set; }
        public required string DetailDescription { get; set; }
        public string? Technologies { get; set; }
        public string? VideoUrl { get; set; }
    }
}
